using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Infrastructure;
using ModelingConsoleApp.Model;

namespace ModelingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new TaskQueue<int>();
            //q.Enqueue(1);
            //q.Enqueue(2);
            //q.Enqueue(3);
            //q.Enqueue(4);

            //Console.WriteLine(q.Dequeue());
            //Console.WriteLine(q.Count);
            //Console.WriteLine(q.Peek());
            //Console.WriteLine(q.Count);
            q.DisplayQuery();


            Console.WriteLine(Generator.ExpDistribution(0.5));
            Console.WriteLine(Generator.UniformDistribution(4, 6));

            Console.ReadKey();

        }
    }
}
