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

            var q = new TaskQueue<TaskBase>();

            var task1 = new Task(() =>
            {
                while (true)
                {
                    var s = (Generator.UniformDistribution(1,4));
                    Thread.Sleep(Convert.ToInt32(s)*100);
                    var task = new TaskA();
                    Console.WriteLine(task.Type);

                    q.Enqueue(task);
                }
            });
            task1.Start();

            var task2 = new Task(() =>
            {
                while (true)
                {
                    var s = (Generator.UniformDistribution(1, 4));
                    Thread.Sleep(Convert.ToInt32(s) * 100);
                    var task = new TaskB();
                    Console.WriteLine(task.Type);

                    q.Enqueue(task);
                }
            });
            task2.Start();


            //var i = 0;
            //while (i < 1000)
            //{

            //    var s = (Generator.ExpDistribution(0.9));
            //    Thread.Sleep(500);

            //    Console.WriteLine(i + " S: " + s);
            //    i++;
            //}

            Thread.Sleep(10000);

          q.DisplayQuery();

        }

        private async Task GetURLContentsAsync()
        {
           
            Console.WriteLine("Task");

        }
    }
}
