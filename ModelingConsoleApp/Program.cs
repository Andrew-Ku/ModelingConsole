using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModelingConsoleApp.Infrastructure;
using ModelingConsoleApp.Model;

namespace ModelingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dev = new Device("Dev1");
            dev.Channel1=new TaskA();

            var s = dev.Channel1;

            return;
            #region Генерация задач в Main

            //var task1 = new Task(() =>
            //{
            //    while (true)
            //    {
            //        var s = (Generator.UniformDistribution(1, 4));
            //        Thread.Sleep(Convert.ToInt32(s) * 100);
            //        var task = new TaskA();
            //        Console.Write(task.Type + "-> ");

            //        dev.TaskQueue.Enqueue(task);
            //    }
            //});
            //task1.Start();

            //var task2 = new Task(() =>
            //{
            //    while (true)
            //    {
            //        var s = (Generator.UniformDistribution(1, 3));
            //        Thread.Sleep(Convert.ToInt32(s) * 100);
            //        var task = new TaskB();
            //        Console.Write(task.Type + "-> ");

            //        dev.TaskQueue.Enqueue(task);
            //    }
            //});
            //task2.Start();

            #endregion

            GenerateTask(dev, 1, 4, 100, typeof(TaskA));
            GenerateTask(dev, 1, 3, 100, typeof(TaskB));
            GenerateTask(dev, 1, 2, 100, typeof(TaskC));



            Thread.Sleep(10000);

            dev.TaskQueue.DisplayQuery();

        }

        // Создание источника генерации задачи определенного класса
        private static void GenerateTask(Device dev, int a, int b, int multiplier, Type typeTask)
        {
            if (typeTask.BaseType.Name != "TaskBase")
                throw new ArgumentException(typeTask.ToString());

            var task = new Task(() =>
            {
                while (true)
                {
                    var s = (Generator.UniformDistribution(a, b));
                    Thread.Sleep(Convert.ToInt32(s) * multiplier);
                    var newTask = Activator.CreateInstance(typeTask);
                    Console.Write((newTask as TaskBase).Type + "-> ");

                    dev.TaskQueue.Enqueue(newTask as TaskBase);
                }
            });
            task.Start();


        }
    }
}
