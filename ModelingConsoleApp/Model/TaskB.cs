using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;
using ModelingConsoleApp.Infrastructure;

namespace ModelingConsoleApp.Model
{
    public class TaskB : TaskBase
    {
        public static int Count;
        public static double TimeLine;

        public TaskB()
        {
            Count++;
            Id = Count;
            Type = TaskTypes.ClassB;
            TimeLine += Generator.ExpDistribution(0.4);
            GenerateTime = TimeLine;
        }

        public static void ResetCountAndTimeLine()
        {
            Count = 0;
            TimeLine = 0;
        }
    }

}
