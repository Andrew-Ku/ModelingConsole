using System;
using System.Collections.Generic;
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

        public static List<TaskBase> TestQuery = new List<TaskBase>(); // Не объектная очередь для теста


        private static void Main(string[] args)
        {
            var modeling = true; // Флаг работы модели

            InitializationEvent(); // Инициализирующее событие

            var iter = 0; // Для теста

            while (iter < 10) // Основной цикл моделирования 
            //   while (modeling) // Основной цикл моделирования 
            {
                var currentEvent = GetEvent();
                EventDictionary.DisplayEvent(currentEvent.EventCode);
                switch (currentEvent.EventCode)
                {
                    case EventCode.TaskGen:
                        Console.WriteLine(currentEvent.TaskType);
                        TaskGen_EventHandler(currentEvent);
                        break;

                    case EventCode.SeizeChannel1:
                        SeizeChannel1_EventHandler(currentEvent);
                        break;
                    case EventCode.SeizeChannel2:
                        SeizeChannel2_EventHandler(currentEvent);
                        break;
                    case EventCode.SeizeChannelAll:
                        SeizeChannelAll_EventHandler(currentEvent);
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

                iter++;

                //modeling = false; // Для теста

            }


            Console.ReadKey();
        }

        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void TaskGen_EventHandler(Event e)
        {
            if (TaskTypes.ParallelCkasses.Contains(e.TaskType)) // Задачи класса A и B
            {
                if (IsFreeChannel1)
                {
                    IsFreeChannel1 = false;
                    PlanEvent(EventCode.ReleaseChannel1, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                }
                else if (IsFreeChannel2)
                {
                    IsFreeChannel2 = false;
                    PlanEvent(EventCode.ReleaseChannel2, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                }
                else
                {
                    switch (e.TaskType)
                    {
                        case TaskTypes.ClassA: TestQuery.Add(new TaskA()); break;
                        case TaskTypes.ClassB: TestQuery.Add(new TaskB()); break;
                    }
                  
                    Console.WriteLine("В очереди: {0}", TestQuery.Count);
                    // PlanEvent(EventCode.GetTaskFromQueue, TestQuery.Sum(x => x.GenerateTime), e.TaskType);  // !!! - Решить вопрос со временем
                }
            }
            else // Задачи класса C
            {
                if (IsFreeChannel1 && IsFreeChannel2)
                {
                    IsFreeChannel1 = false;
                    IsFreeChannel2 = false;

                    PlanEvent(EventCode.ReleaseChannelAll, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Advance) + ModelingTime, e.TaskType);
                }
                else  /// !!! - подумать что делать, если занят хотябы один канал
                {
                    TestQuery.Add(new TaskC());
                }
            }

            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(e.TaskType, GeneratorParametrs.Generation) + ModelingTime, e.TaskType);
        }

        /// <summary>
        /// Обработка Занять канал 1
        /// </summary>
        public static void SeizeChannel1_EventHandler(Event e)
        {

        }
        /// <summary>
        /// Обработка Занять канал 2
        /// </summary>
        public static void SeizeChannel2_EventHandler(Event e)
        {

        }
        /// <summary>
        /// Обработка Занять оба канала
        /// </summary>
        public static void SeizeChannelAll_EventHandler(Event e)
        {

        }
        /// <summary>
        /// Обработка Освободить канал 1
        /// </summary>
        public static void ReleaseChannel1_EventHandler(Event e)  // !!! - Объединить два обработчика 
        {
            TestQuery = TestQuery.OrderByDescending(t => t.Priority).ToList(); // Без  этого лучше работает (Не сортируем по приоритетам) 
            var firstTask = TestQuery.FirstOrDefault();

            if (firstTask == null)
            {
                IsFreeChannel1 = true;
                return;
            }
            TestQuery.RemoveAt(0);
            Console.WriteLine("Берем из очереди");
            PlanEvent(EventCode.ReleaseChannel1, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);
            IsFreeChannel1 = false; // Скорее всего излишнее , т.к. мы не освобождали канал
        }

        /// <summary>
        /// Обработка Освободить канал 2
        /// </summary>
        public static void ReleaseChannel2_EventHandler(Event e)
        {
            TestQuery = TestQuery.OrderByDescending(t => t.Priority).ToList(); // Без  этого лучше работает (Не сортируем по приоритетам) 
            var firstTask = TestQuery.FirstOrDefault();

            if (firstTask == null)
            {
                IsFreeChannel2 = true;
                return;
            }
            TestQuery.RemoveAt(0);
            Console.WriteLine("Берем из очереди");
            PlanEvent(EventCode.ReleaseChannel2, Generator.ExpDistribution(firstTask.Type, GeneratorParametrs.Advance) + ModelingTime, firstTask.Type);
            IsFreeChannel2 = false; // Скорее всего излишнее , т.к. мы не освобождали канал
        }
        /// <summary>
        /// Обработка Освободить оба канала
        /// </summary>
        public static void ReleaseChannelAll_EventHandler(Event e)
        {
            IsFreeChannel1 = true;
            IsFreeChannel2 = true;
        }
        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void StopModeling_EventHandler(Event e)
        {

        }


        /// <summary>
        ///  Инициализирующее событие
        /// </summary>
        public static void InitializationEvent()
        {
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassA, GeneratorParametrs.Generation), TaskTypes.ClassA);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassB, GeneratorParametrs.Generation), TaskTypes.ClassB);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(TaskTypes.ClassC, GeneratorParametrs.Generation), TaskTypes.ClassC);

            EventList.OrderBy(e => e.EventTime);
            ModelingTime = EventList.Last().EventTime; // Задаем модельное время последним событием (Значит обязательно выполнятся первые три события. Это не очень правильно)
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
        public static void PlanEvent(int eventCode, double eventTime, string taskType)
        {
            EventList.Add(new Event
            {
                EventCode = eventCode,
                EventTime = eventTime,
                TaskType = taskType
            });
        }
    }
}
