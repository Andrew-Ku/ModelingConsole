using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    public class Channel
    {
        public Channel()
        {
            CurrentTask = null;
            IsAvailable = true;
        }

        public TaskBase CurrentTask { get; set; }
        public bool IsAvailable { get; set; }

    }
}
