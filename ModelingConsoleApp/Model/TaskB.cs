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
        public static int CountB;

        public TaskB()
        {
            GenCount++;
            Id = GenCount;
            Type = TaskTypes.ClassB;
            GenerateTime = Program.ModelingTime;
            Priority = InputValues.PriorityB;
            Weight = 2;
        }

        public static void ResetCountAndTimeLine()
        {
            GenCount = 0;
        }
    }

}
