using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;
using ModelingConsoleApp.Infrastructure;
using ModelingConsoleApp.Model;

namespace ModelingConsoleApp
{
    class Program
    {
        public static List<Event> EventList = new List<Event>(); // Список событий 
        public static double ModelingTime;   // Время моделирования
        public static Device Device = new Device("Dev1");   // Время моделирования

        public static List<string> FileLines = new List<string>();
        private const string FilePath = "Log.txt";
        private const string CsvResFilePath = "Res.csv";

        private static void Main(string[] args)
        {
            var modeling = true; // Флаг работы модели
            InitializationEvent(); // Инициализирующее событие

            Console.WriteLine("Идет процесс моделирования...");
            while (modeling) // Основной цикл моделирования 
            {
                var currentEvent = GetEvent();
                ModelingTime = currentEvent.EventTime;
                Console.WriteLine(ModelingTime);
                FileLines.AddWithTime(EventDictionary.GetValue(currentEvent.EventCode) + " - " + currentEvent.TaskType);
                switch (currentEvent.EventCode)
                {
                    case EventCode.TaskGen:
                        TaskGen_EventHandler(currentEvent);
                        break;
                    case EventCode.ReleaseChannel1:
                        ReleaseChannel1_EventHandler(currentEvent);
                        break;
                    case EventCode.ReleaseChannel2:
                        ReleaseChannel2_EventHandler(currentEvent);
                        break;
                    case EventCode.ReleaseChannelAll:
                        ReleaseChannelAll_EventHandler(currentEvent);
                        break;
                    case EventCode.StopModeling:
                        StopModeling_EventHandler(currentEvent);
                        modeling = false;
                        break;
                }
            }

            //   StatisticsDisplay();
            StatisticsDisplayColor(ConsoleColor.Green);
           
            if (!File.Exists(FilePath))
            {
                using (File.Create(FilePath)) { }
            }
            File.WriteAllLines(FilePath, FileLines);
           // Process.Start(FilePath);

            ResToFile(CsvResFilePath);
            Process.Start(CsvResFilePath);

            Console.ReadKey();

        }

        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void TaskGen_EventHandler(Event e)
        {
            var task = GetTaskInstanceByType(e.TaskType);

            var firsTaskQueue = Device.TaskQueue.OrderByDescending(t => t.Priority).FirstOrDefault();
            var IsFirstTaskC = false;
            if (firsTaskQueue != null)
            {
                IsFirstTaskC = firsTaskQueue.Type == TaskTypes.ClassC;
            }

            if (TaskTypes.ParallelCkasses.Contains(e.TaskType)) // Задачи класса A и B
            {
                if (IsFirstTaskC || (!Device.IsChannelAvailable(Channels.Channel1) && !Device.IsChannelAvailable(Channels.Channel2)))
                {
                    switch (e.TaskType)
                    {
                        case TaskTypes.ClassA:
                            Device.TaskQueue.AddTask(task);
                            break;
                        case TaskTypes.ClassB:
                            Device.TaskQueue.AddTask(task);
                            break;
                    }

                    FileLines.AddWithTime(string.Format("Задача {0} добавлена в очередь. В очереди {1} елемент(ов)",
                        e.TaskType, Device.TaskQueue.Count));
                }
                else
                {
                    if (Device.IsChannelAvailable(Channels.Channel1))
                    {
                        Device.Seize(task, Channels.Channel1);
                        FileLines.AddWithTime("Занять канал 1 задачей " + e.TaskType);
                        PlanEvent(EventCode.ReleaseChannel1, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                    }
                    else if (Device.IsChannelAvailable(Channels.Channel2))
                    {
                        Device.Seize(task, Channels.Channel2);
                        FileLines.AddWithTime("Занять канал 2 задачей " + e.TaskType);
                        PlanEvent(EventCode.ReleaseChannel2, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                    }
                }
            }
            else // Задачи класса C
            {
                if (Device.IsChannelAvailable(Channels.Channel1) && Device.IsChannelAvailable(Channels.Channel2))
                {
                    Device.Seize(task, Channels.AllChannels);
                    FileLines.AddWithTime("Занять оба канала задачей " + e.TaskType);

                    PlanEvent(EventCode.ReleaseChannelAll, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                }
                else
                {
                    Device.TaskQueue.AddTask(task);
                    FileLines.AddWithTime(string.Format("Задача {0} добавлена в очередь. В очереди {1} елемент(ов)", e.TaskType, Device.TaskQueue.Count));
                }
            }

            Device.QueueLengthSum += Device.TaskQueue.Count; // Для расчета средней длины очереди
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Generation) + ModelingTime, e.TaskType);
        }


        /// <summary>
        /// Обработка Освободить канал 1
        /// </summary>
        public static void ReleaseChannel1_EventHandler(Event e)
        {
            ReleaseChannel(Channels.Channel1);
        }

        /// <summary>
        /// Обработка Освободить канал 2
        /// </summary>
        public static void ReleaseChannel2_EventHandler(Event e)
        {
            ReleaseChannel(Channels.Channel2);
        }

        private static void ReleaseChannel(int channelNum)
        {
            Device.Release(channelNum);

            Device.TaskQueue = Device.TaskQueue.OrderByDescending(t => t.Priority).ToList();
            var firstTask = Device.TaskQueue.FirstOrDefault();

            if (firstTask == null)
            {
                return;
            }

            if (firstTask.Type == TaskTypes.ClassC)
            {
                Device.IsChannelAvailable(channelNum);
                var isFreeAnotherChannel = channelNum == Channels.Channel2 ? Device.IsChannelAvailable(Channels.Channel1)
                    : Device.IsChannelAvailable(Channels.Channel2);

                if (isFreeAnotherChannel)
                {
                    Device.TaskQueue.RemoveFirstTask();
                    FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем оба канала");

                    firstTask.PopQueueTime = ModelingTime; //Время извлечения задачи из очереди
                    Device.Seize(firstTask, Channels.AllChannels);

                    PlanEvent(EventCode.ReleaseChannelAll, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);
                    return;
                }

                FileLines.AddWithTime(String.Format("Задача С первая в ожидании. Канал 1 - {0}. Канал 2 - {1}", Device.IsChannelAvailable(Channels.Channel1) ? "Свободен" : "Занят", Device.IsChannelAvailable(Channels.Channel2) ? "Свободен" : "Занят"));
                return;
            }

            Device.TaskQueue.RemoveFirstTask();

            FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем канал " + channelNum);

            firstTask.PopQueueTime = ModelingTime; //Время извлечения задачи из очереди
            Device.Seize(firstTask, channelNum);

            var eventCode = channelNum == Channels.Channel2 ? EventCode.ReleaseChannel2 : EventCode.ReleaseChannel1;
            PlanEvent(eventCode, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);

        }

        /// <summary>
        /// Обработка Освободить оба канала
        /// </summary>
        public static void ReleaseChannelAll_EventHandler(Event e)
        {
            Device.Release(Channels.AllChannels);

            Device.TaskQueue = Device.TaskQueue.OrderByDescending(t => t.Priority).ToList(); // Без  этого лучше работает (Не сортируем по приоритетам) 
            var firstTask = Device.TaskQueue.FirstOrDefault();

            if (firstTask == null) // В очереди нет задач
            {
                return;
            }

            if (firstTask.Type != TaskTypes.ClassC) // В очереди задача класса А или Б
            {
                Device.TaskQueue.RemoveFirstTask();
                firstTask.PopQueueTime = ModelingTime; //Время извлечения задачи из очереди
                Device.Seize(firstTask, Channels.Channel1);
                FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем канал 1");
                PlanEvent(EventCode.ReleaseChannel1,
                    Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);

                var secondTask = Device.TaskQueue.FirstOrDefault(); // Берем вторую задачу из очереди

                if (secondTask == null)
                {
                    return;
                }

                if (secondTask.Type == TaskTypes.ClassC)
                {
                    FileLines.AddWithTime("Нехватает каналов для задачи класса С");
                    return;
                }

                Device.TaskQueue.RemoveFirstTask();
                secondTask.PopQueueTime = ModelingTime; //Время извлечения задачи из очереди
                Device.Seize(secondTask, Channels.Channel2);
                FileLines.AddWithTime("Берем из очереди задачу " + secondTask.Type + ". Занимаем канал 2");
                PlanEvent(EventCode.ReleaseChannel2,
                    Generator.ExpDistribution(secondTask.Type, GeneratorParametrs.Advance) + ModelingTime,
                    secondTask.Type);
            }
            else
            {
                Device.TaskQueue.RemoveFirstTask();
                firstTask.PopQueueTime = ModelingTime; //Время извлечения задачи из очереди
                Device.Seize(firstTask, Channels.AllChannels);
                FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем оба канала");
                PlanEvent(EventCode.ReleaseChannelAll,
                    Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime,
                    firstTask.Type);
            }
        }

        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void StopModeling_EventHandler(Event e)
        {
            Console.WriteLine("Моделирование завершено");
            Console.WriteLine("Время моделирования: " + ModelingTime);
        }


        /// <summary>
        ///  Инициализирующее событие
        /// </summary>
        public static void InitializationEvent()
        {
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassA, GeneratorParametrs.Generation), TaskTypes.ClassA);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassB, GeneratorParametrs.Generation), TaskTypes.ClassB);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassC, GeneratorParametrs.Generation), TaskTypes.ClassC);
            PlanEvent(EventCode.StopModeling, InputValues.ModelingTimeForSystem);

            EventList.OrderBy(e => e.EventTime);
            //   ModelingTime = EventList.Last().EventTime; // Задаем модельное время последним событием (Значит обязательно выполнятся первые три события. Это не очень правильно)
        }

        /// <summary>
        /// Получение кода первого события в списке
        /// </summary>
        static public Event GetEvent()
        {
            EventList = EventList.OrderBy(e => e.EventTime).ToList();
            var firstEvent = EventList.FirstOrDefault();
            if (firstEvent != null) { }
            EventList.RemoveAt(0);
            return firstEvent;
        }

        /// <summary>
        /// Добавление события в список (планирование)
        /// </summary>
        public static void PlanEvent(int eventCode, double eventTime, string taskType = "")
        {
            EventList.Add(new Event
            {
                EventCode = eventCode,
                EventTime = eventTime,
                TaskType = taskType
            });
        }

        /// <summary>
        /// Получение экземпляра события соответсвующего типа
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        private static TaskBase GetTaskInstanceByType(string taskType)
        {
            switch (taskType)
            {
                case TaskTypes.ClassA: return new TaskA();
                case TaskTypes.ClassB: return new TaskB();
                case TaskTypes.ClassC: return new TaskC();
            }
            throw new ArgumentException("Невозможно создать задачу типа " + taskType);
        }

        /// <summary>
        /// Вывод статистики
        /// </summary>
        private static void StatisticsDisplay()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Задач сгенерированно: {0}", TaskBase.GenCount);
            Console.WriteLine("Задач прошло через систему: {0}", Device.TaskReleaseCount);
            Console.WriteLine("Средняя длительность прохождения задач через систему: {0}", Device.TasksAverageSystemTime);

            Console.WriteLine("Очередь устройтсва: {0}", Device.TaskQueue.Count);
            Console.WriteLine("Средняя длина очереди устройтсва: {0}", Device.QueueLengthAverage);
            Console.WriteLine("Среднее время ожидания в очереди: {0}", Device.QueueTimeAverage);
            Console.WriteLine("Средневзвешенное время ожидания в очереди: {0}", Device.QueueWeightTimeAverage);
        }

        private static void StatisticsDisplayColor(ConsoleColor color)
        {
            Console.WriteLine("------------------------------");
            DipslayStatisricItem("Задач сгенерированно", TaskBase.GenCount, color);
            DipslayStatisricItem("Задач прошло через систему", Device.TaskReleaseCount, color);
            DipslayStatisricItem("Средняя длительность прохождения задач через систему", Device.TasksAverageSystemTime, color);
            DipslayStatisricItem("Очередь устройтсва", Device.TaskQueue.Count, color);
            DipslayStatisricItem("Средняя длина очереди устройтсва", Device.QueueLengthAverage, color);
            DipslayStatisricItem("Среднее время ожидания в очереди", Device.QueueTimeAverage, color);
            DipslayStatisricItem("Средневзвешенное время ожидания в очереди", Device.QueueWeightTimeAverage, color);

        }

        private static void ResToFile(string path)
        {
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
            var resParamList = new List<string>
            {
                Device.TasksAverageSystemTime.ToString("F3"),
                Device.QueueLengthAverage.ToString("F3"),
                Device.QueueTimeAverage.ToString("F3"),
                Device.QueueWeightTimeAverage.ToString("F3")
            };
            File.WriteAllLines(CsvResFilePath, resParamList);
        } 


        private static void DipslayStatisricItem(string str, object paramert, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(str + ": ");
            Console.ForegroundColor = color;
            Console.WriteLine(paramert);
        }

    }
}
