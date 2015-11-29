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
        public static int CountC;

        public TaskC()
        {
            GenCount++;
            Id = GenCount;
            Type = TaskTypes.ClassC;
            GenerateTime = Program.ModelingTime;
            Priority = InputValues.PriorityC;
            Weight = 3;
        }

        public static void ResetCountAndTimeLine()
        {
            GenCount = 0;
        }
    }

}
