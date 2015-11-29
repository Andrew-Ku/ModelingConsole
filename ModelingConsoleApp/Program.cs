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

        public static bool IsFreeChannel1 = true; // Свободен ли канал 1
        public static bool IsFreeChannel2 = true;

        public static List<string> FileLines = new List<string>();
        private const string FilePath = "Log.txt";

        public static List<TaskBase> TestQuery = new List<TaskBase>(); // Не объектная очередь для теста


        private static void Main(string[] args)
        {
            var modeling = true; // Флаг работы модели
            InitializationEvent(); // Инициализирующее событие

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

            if (!File.Exists(FilePath))
            {
                using (File.Create(FilePath)) { }

            }


            Console.WriteLine("Задач А: {0}", TaskA.Count);
            Console.WriteLine("Задач B: {0}", TaskB.Count);
            Console.WriteLine("Задач C: {0}", TaskC.Count);

            File.WriteAllLines(FilePath, FileLines);
            Process.Start(FilePath);


        }

        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void TaskGen_EventHandler(Event e)
        {
            var task = GetTaskInstanceByType(e.TaskType);

            var firsTaskQueue = TestQuery.OrderByDescending(t => t.Priority).FirstOrDefault();
            var IsFirstTaskC = false;
            if (firsTaskQueue != null)
            {
                IsFirstTaskC = firsTaskQueue.Type == TaskTypes.ClassC;
            }

            if (TaskTypes.ParallelCkasses.Contains(e.TaskType)) // Задачи класса A и B
            {
                if (IsFirstTaskC || (!IsFreeChannel1 && !IsFreeChannel2))
                {
                    switch (e.TaskType)
                    {
                        case TaskTypes.ClassA:
                            TestQuery.Add(task);
                            break;
                        case TaskTypes.ClassB:
                            TestQuery.Add(task);
                            break;
                    }

                    FileLines.AddWithTime(string.Format("Задача {0} добавлена в очередь. В очереди {1} елемент(ов)",
                        e.TaskType, TestQuery.Count));
                }
                else
                {
                    if (IsFreeChannel1)
                    {
                        IsFreeChannel1 = false;
                        FileLines.AddWithTime("Занять канал 1 задачей " + e.TaskType);
                        PlanEvent(EventCode.ReleaseChannel1, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                    }
                    else if (IsFreeChannel2)
                    {
                        IsFreeChannel2 = false;
                        FileLines.AddWithTime("Занять канал 2 задачей " + e.TaskType);
                        PlanEvent(EventCode.ReleaseChannel2, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                    }
                }
            }
            else // Задачи класса C
            {
                if (IsFreeChannel1 && IsFreeChannel2)
                {
                    IsFreeChannel1 = false;
                    IsFreeChannel2 = false;
                    FileLines.AddWithTime("Занять оба канала задачей " + e.TaskType);

                    PlanEvent(EventCode.ReleaseChannelAll, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                }
                else  
                {
                    TestQuery.Add(task);
                    FileLines.AddWithTime(string.Format("Задача {0} добавлена в очередь. В очереди {1} елемент(ов)", e.TaskType, TestQuery.Count));
                }
            }

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
            TestQuery = TestQuery.OrderByDescending(t => t.Priority).ToList(); 
            var firstTask = TestQuery.FirstOrDefault();

            if (firstTask == null)
            {
                if (channelNum == Channels.Channel2)
                    IsFreeChannel2 = true;
                else
                    IsFreeChannel1 = true;
                return;
            }

            if (firstTask.Type == TaskTypes.ClassC)
            {
                bool IsFreeChannel;

                if (channelNum == Channels.Channel2)
                {
                    IsFreeChannel2 = true;
                    IsFreeChannel = IsFreeChannel1;
                }
                else
                {
                    IsFreeChannel1 = true;
                    IsFreeChannel = IsFreeChannel2;
                }

                if (IsFreeChannel)
                {
                    TestQuery.RemoveAt(0);
                    FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем оба канала");

                    IsFreeChannel1 = false;
                    IsFreeChannel2 = false;

                    PlanEvent(EventCode.ReleaseChannelAll, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);
                    return;
                }

                FileLines.AddWithTime(String.Format("Задача С первая в ожидании. Канал 1 - {0}. Канал 2 - {1}", IsFreeChannel1 ? "Свободен" : "Занят", IsFreeChannel2 ? "Свободен" : "Занят"));
                return;
            }

            TestQuery.RemoveAt(0);

            FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем канал " + channelNum);
            var eventCode = channelNum == Channels.Channel2 ? EventCode.ReleaseChannel2 : EventCode.ReleaseChannel1;
            PlanEvent(eventCode, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);

        }

        /// <summary>
        /// Обработка Освободить оба канала
        /// </summary>
        public static void ReleaseChannelAll_EventHandler(Event e)
        {
            TestQuery = TestQuery.OrderByDescending(t => t.Priority).ToList(); // Без  этого лучше работает (Не сортируем по приоритетам) 
            var firstTask = TestQuery.FirstOrDefault();

            if (firstTask == null) // В очереди нет задач
            {
                IsFreeChannel1 = true;
                IsFreeChannel2 = true;
                return;
            }

            if (firstTask.Type != TaskTypes.ClassC) // В очереди задача класса А или Б
            {
                TestQuery.RemoveAt(0);
                FileLines.AddWithTime("Берем из очереди задачу " + firstTask.Type + ". Занимаем канал 1");
                PlanEvent(EventCode.ReleaseChannel1,
                    Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);

                var secondTask = TestQuery.FirstOrDefault(); // Берем вторую задачу из очереди

                if (secondTask == null)
                {
                    IsFreeChannel2 = true;
                    return;
                }

                if (secondTask.Type == TaskTypes.ClassC)
                {
                    FileLines.AddWithTime("Нехватает каналов для задачи класса С");
                    return;
                }

                TestQuery.RemoveAt(0);
                FileLines.AddWithTime("Берем из очереди задачу " + secondTask.Type + ". Занимаем канал 2");
                PlanEvent(EventCode.ReleaseChannel2,
                    Generator.ExpDistribution(secondTask.Type, GeneratorParametrs.Advance) + ModelingTime,
                    secondTask.Type);
            }
            else
            {
                TestQuery.RemoveAt(0);
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
            Console.WriteLine("Конец моделирования");
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

        private static TaskBase GetTaskInstanceByType(string taskType)
        {
            switch (taskType)
            {
                case TaskTypes.ClassA: return new TaskA();
                case TaskTypes.ClassB: return new TaskB();
                case TaskTypes.ClassC: return new TaskC();
            }
           throw new NotImplementedException();
        }
    }
}
