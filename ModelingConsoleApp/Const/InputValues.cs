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
        public static double ModelingTimeForSystem = 6.4;

        /// <summary>
        /// Параметр бета для экспоненциального распределения задач класса A
        /// </summary>
        public static double BetaA = 0.2;

        /// <summary>
        /// Параметр бета для экспоненциального распределения задач класса B
        /// </summary>
        public static double BetaB = 0.125;

        /// <summary>
        /// Параметр бета для экспоненциального распределения задач класса C
        /// </summary>
        public static double BetaC = 0.066;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса A
        /// </summary>
        public static double BetaAdvanceA = 0.0;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса B
        /// </summary>
        public static double BetaAdvanceB = 0.16;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса C
        /// </summary>
        public static double BetaAdvanceC = 0.08;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса A
        /// </summary>
        public static int PriorityA = 1;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса B
        /// </summary>
        public static int PriorityB = 2;

        /// <summary>
        /// Параметр бета для времени обслуживания задач класса C
        /// </summary>
        public static int PriorityC = 3;

    }
}
