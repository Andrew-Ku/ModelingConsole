using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    /// <summary>
    /// Собитие
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Код
        /// </summary>
        public int EventCode { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public double EventTime { get; set; }

        /// <summary>
        /// Тип задачи
        /// </summary>
        public string TaskType { get; set; }
    }
}
