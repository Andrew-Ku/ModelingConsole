using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    /// <summary>
    /// Канал
    /// </summary>
    public class Channel
    {
        public Channel()
        {
            CurrentTask = null;
            IsAvailable = true;
        }

        /// <summary>
        /// Текущая задача в канале
        /// </summary>
        public TaskBase CurrentTask { get; set; }

        /// <summary>
        /// Свободен ли канал
        /// </summary>
        public bool IsAvailable { get; set; }

    }
}
