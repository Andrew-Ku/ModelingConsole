using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Model
{
    public class TaskC : TaskBase
    {
        public static int count;

        public TaskC()
        {
            count++;
            Id = count;
            Type = TaskTypes.ClassC;
        }
    }
}
