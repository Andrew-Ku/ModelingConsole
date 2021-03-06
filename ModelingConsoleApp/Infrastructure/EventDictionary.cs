﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Infrastructure
{
    public static class EventDictionary
    {
        /// <summary>
        /// Получение названия события
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(int key)
        {
            switch (key)
            {
                case EventCode.TaskGen:
                    return "Поступление задачи";
                case EventCode.StopModeling:
                    return "Завершение моделирования";
                case EventCode.ReleaseChannel1:
                    return "Освобождение канала 1";
                case EventCode.ReleaseChannel2:
                    return "Освобождение канала 2";
                case EventCode.ReleaseChannelAll:
                    return "Освобожения всех каналов";
                default:
                    return "Неизвестное событие!";
            }
        }

        /// <summary>
        /// Вывод названия события 
        /// </summary>
        /// <param name="eventCode"></param>
        public static void DisplayEvent(int eventCode)
        {
            Console.WriteLine(GetValue(eventCode));
        }
    }
}
