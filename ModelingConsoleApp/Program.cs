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
        private static void Main(string[] args)
        {
            var taskSourceList = new List<TaskBase>();
            var taskSourceListByTime = new List<TaskBase>();

            AddTasksByCountToGlobalList(taskSourceList, typeof(TaskA), InputValues.TaskCountForGlobalList);
            AddTasksByCountToGlobalList(taskSourceList, typeof(TaskB), InputValues.TaskCountForGlobalList);
            AddTasksByCountToGlobalList(taskSourceList, typeof(TaskC), InputValues.TaskCountForGlobalList);

            var sortedTaskSourceList = taskSourceList.OrderBy(t => t.GenerateTime).Take(InputValues.TaskCountForSystem);

            Console.WriteLine("По количеству");
            Console.WriteLine(sortedTaskSourceList.Count(x => x.Type == TaskTypes.ClassA));
            Console.WriteLine(sortedTaskSourceList.Count(x => x.Type == TaskTypes.ClassB));
            Console.WriteLine(sortedTaskSourceList.Count(x => x.Type == TaskTypes.ClassC));

            TaskA.ResetCountAndTimeLine();
            TaskB.ResetCountAndTimeLine();
            TaskC.ResetCountAndTimeLine();

            AddTasksByTimeToGlobalList(taskSourceListByTime, typeof(TaskA), InputValues.ModelingTimeForSystem);
            AddTasksByTimeToGlobalList(taskSourceListByTime, typeof(TaskB), InputValues.ModelingTimeForSystem);
            AddTasksByTimeToGlobalList(taskSourceListByTime, typeof(TaskC), InputValues.ModelingTimeForSystem);

            var sortedTaskSourceListByTime = taskSourceListByTime.OrderBy(t => t.GenerateTime);

            Console.WriteLine("По времени");
            Console.WriteLine(sortedTaskSourceListByTime.Count(x => x.Type == TaskTypes.ClassA));
            Console.WriteLine(sortedTaskSourceListByTime.Count(x => x.Type == TaskTypes.ClassB));
            Console.WriteLine(sortedTaskSourceListByTime.Count(x => x.Type == TaskTypes.ClassC));

            Console.ReadKey();
        }


        /// <summary>
        /// Генерация определенного количтва задач
        /// </summary>
        private static void AddTasksByCountToGlobalList(List<TaskBase> list, Type typeTask, int count)
        {
            for (var i = 0; i < count; i++)
            {
                list.Add((TaskBase)Activator.CreateInstance(typeTask));
            }
        }


        /// <summary>
        /// Генерация задач в зависимости от модельного мремени
        /// </summary>
        private static void AddTasksByTimeToGlobalList(List<TaskBase> list, Type typeTask, double time)
        {
            while (true)
            {
                list.Add((TaskBase)Activator.CreateInstance(typeTask));

                if (list.Last().GenerateTime >= time)
                    break;
            }
        }
    }
}
