using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    public abstract class TaskBase
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Type { get; set; }
        public string Name { get; set; } 

        public TaskInfo TaskInfo { get; set; }
    }
}
