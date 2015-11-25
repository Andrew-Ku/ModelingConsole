using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Const
{
    public static  class InputValues
    {
        /// <summary>
        /// Количество сгенерированных задач каждого класса
        /// </summary>
        public static int TaskCountForGlobalList = 10000;

        /// <summary>
        /// Количество обслужеваемых системой задач
        /// </summary>
        public static int TaskCountForSystem = 10000;

        /// <summary>
        /// Время моделирования
        /// </summary>
        public static double ModelingTimeForSystem = 2000;
    }
}
