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
        public static int CountA;

        public TaskA()
        {
            GenCount++;
            Id = GenCount;
            Type = TaskTypes.ClassA;
            GenerateTime = Program.ModelingTime;
            Priority = InputValues.PriorityA;
            Weight = 1;
        }

        public static void ResetCountAndTimeLine()
        {
            GenCount = 0;
        }
    }
}
