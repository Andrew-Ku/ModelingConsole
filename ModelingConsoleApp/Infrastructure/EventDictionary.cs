using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Infrastructure
{
    public static class EventDictionary
    {
        public static string GetValue(int key)
        {
            switch (key)
            {
                case EventCode.TaskGen:
                    return "Поступление заявки";
                case EventCode.StopModeling:
                    return "Завершение моделирования";
                case EventCode.ReleaseChannel1:
                    return "Освобождения канала 1";
                case EventCode.ReleaseChannel2:
                    return "Освобождение канала 2";
                case EventCode.ReleaseChannelAll:
                    return "Освобожения всех каналов";
                case EventCode.GetTaskFromQueue:
                    return "Получение задачи из очереди";
                default:
                    return "Неизвестное событие!";
            }
        }

        public static void DisplayEvent(int eventCode)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine(GetValue(eventCode));
        }
    }
}
