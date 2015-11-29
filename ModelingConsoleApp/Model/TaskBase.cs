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
        
        /// <summary>
        /// Время поступления задачи в систему
        /// </summary>
        public double GenerateTime { get; set; }
        public double PushQueueTime { get; set; }
        public double PopQueueTime { get; set; }

        public int Weight { get; set; }

        public static int GenCount;
        public static int TerCount;
        public static int OutQueueCount;

        public TaskInfo TaskInfo { get; set; }
    }
}
