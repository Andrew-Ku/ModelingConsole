using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Model
{
    public class TaskB : TaskBase
    {
        public static int count;

        public TaskB()
        {
            count++;
            Id = count;
            Type = TaskTypes.ClassB;
        }
    }
}
