using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;
using ModelingConsoleApp.Infrastructure;

namespace ModelingConsoleApp.Model
{
    public class TaskA : TaskBase
    {
        public static int Count;

        public static double TimeLine;

        public TaskA()
        {
            Count++;
            Id = Count;
            Type = TaskTypes.ClassA;
            TimeLine += Generator.ExpDistribution(0.6);
            GenerateTime = TimeLine;
        }
    }
}
