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
        public static Device Device= new Device("Dev1");   // Время моделирования


        private static void Main(string[] args)
        {
            var modeling = true; // Флаг работы модели

            InitializationEvent(); // Инициализирующее событие

            while (modeling)
            {
                var currentEvent = GetEvent();
                switch (currentEvent.EventCode)
                {
                    case EventCode.TaskGen:
                        TaskGenEventHandler(currentEvent);
                        break;

                    case EventCode.StopModeling:
                        modeling = false;
                        break;
                }


                modeling = false; // Для теста

            }


            Console.ReadKey();
        }

        /// <summary>
        /// Обработка поступления задачи
        /// </summary>
        public static void TaskGenEventHandler(Event e)
        {

        }


        /// <summary>
        ///  Инициализирующее событие
        /// </summary>
        public static void InitializationEvent()
        {
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(0.2), TaskTypes.ClassA);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(0.4), TaskTypes.ClassB);
            PlanEvent(EventCode.TaskGen, Generator.ExpDistribution(0.6), TaskTypes.ClassC);

            EventList.OrderBy(e => e.EventTime);
        }

        /// <summary>
        /// Получение кода первого события в списке
        /// </summary>
        static public Event GetEvent()
        {
            return EventList.FirstOrDefault();
        }

        /// <summary>
        /// Добавление события в список (планирование)
        /// </summary>
        public static void PlanEvent(int eventCode, double eventTime, string taskType)
        {
            EventList.Add(new Event()
            {
                EventCode = eventCode,
                EventTime = eventTime,
                TaskType = taskType
            });
        }
    }
}
