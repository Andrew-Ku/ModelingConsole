using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    public class TaskBase
    {
        public int Priority { get; set; }
        public int Type { get; set; }
        public string Name { get; set; } 

        public TaskInfo TaskInfo { get; set; }
    }
}
