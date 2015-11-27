using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Const
{
    public static class EventCode
    {
        /// <summary>
        /// Поступление заявки
        /// </summary>
        public const int TaskGen = 1;

        /// <summary>
        /// Занять канал 1
        /// </summary>
        public const int SeizeChannel1 = 2;

        /// <summary>
        /// Занять канал 2
        /// </summary>
        public const int SeizeChannel2 = 3;

        /// <summary>
        /// Занятьвсе каналы
        /// </summary>
        public const int SeizeChannelAll = 4;

        /// <summary>
        /// Освободить канал 1
        /// </summary>
        public const int ReleaseChannel1 = 5;

        /// <summary>
        /// Освободить канал 2
        /// </summary>
        public const int ReleaseChannel2 = 6;

        /// <summary>
        /// Освободить все каналы
        /// </summary>
        public const int ReleaseChannelAll = 7;

        /// <summary>
        /// Добавить задачу в очередь
        /// </summary>
        public const int GetTaskFromQueue = 8;


        /// <summary>
        /// Окончание моделирования
        /// </summary>
        public const int StopModeling = 100;
    }
}
