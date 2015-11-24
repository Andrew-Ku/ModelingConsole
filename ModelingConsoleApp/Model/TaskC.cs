using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;
using ModelingConsoleApp.Infrastructure;

namespace ModelingConsoleApp.Model
{
    public class TaskC : TaskBase
    {
        public static int Count;
        public static double TimeLine;
      
        public TaskC()
        {
            Count++;
            Id = Count;
            Type = TaskTypes.ClassC;
            TimeLine += Generator.ExpDistribution(0.1);
            GenerateTime = TimeLine;
        }
    }
}
