using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Model;

namespace ModelingConsoleApp.Infrastructure
{
    public static class Extensions
    {
        /// <summary>
        /// Добавление в список со временем моделирования
        /// </summary>
        /// <param name="list"></param>
        /// <param name="val"></param>
        public static void AddWithTime(this List<string> list, string val)
        {
            list.Add(Program.ModelingTime.ToString("F3").Replace(",",".") + ": " + val);
        }

        /// <summary>
        /// Добавление задачи в очередь с отметкой времени добавления
        /// </summary>
        /// <param name="list"></param>
        /// <param name="val"></param>
        public static void AddTask(this List<TaskBase> list, TaskBase val)
        {
            val.PushQueueTime = Program.ModelingTime;
            list.Add(val);
        }
       
        /// <summary>
        /// Удаления из оереди первого элемента со счетом
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveFirstTask(this List<TaskBase> list)
        {
            TaskBase.OutQueueCount++;
            var task = list.First();
            TaskBase.OutQueueWeightCount+=task.Weight;

            list.RemoveAt(0);
        }
    }
}
