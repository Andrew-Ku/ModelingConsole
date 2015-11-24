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

            AddTasksToGlobalList(taskSourceList, typeof (TaskA), InputValues.TaskCountForGlobalList);
            AddTasksToGlobalList(taskSourceList, typeof (TaskB), InputValues.TaskCountForGlobalList);
            AddTasksToGlobalList(taskSourceList, typeof (TaskC), InputValues.TaskCountForGlobalList);

            var sortedTaskSourceList = taskSourceList.OrderBy(t => t.GenerateTime).Take(InputValues.TaskCountForSystem);

            Console.WriteLine(sortedTaskSourceList.Count(x=>x.Type==TaskTypes.ClassA));
            Console.WriteLine(sortedTaskSourceList.Count(x=>x.Type==TaskTypes.ClassB));
            Console.WriteLine(sortedTaskSourceList.Count(x=>x.Type==TaskTypes.ClassC));
           


            Console.ReadKey();
        }

        private static void AddTasksToGlobalList(List<TaskBase> list, Type typeTask, int count )
        {
            for (var i = 0; i < count; i++)
            {
                list.Add((TaskBase)Activator.CreateInstance(typeTask));
            }
        }
    }
}
