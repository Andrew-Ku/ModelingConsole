using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Model
{
    /// <summary>
    /// Базовый класс для задач
    /// </summary>
    public abstract class TaskBase
    {
        public int Id { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Тип (класс) задачи
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Время поступления задачи в систему
        /// </summary>
        public double GenerateTime { get; set; }

        /// <summary>
        /// Время поступления в очередь
        /// </summary>
        public double PushQueueTime { get; set; }

        /// <summary>
        /// Время выхода из очереди
        /// </summary>
        public double PopQueueTime { get; set; }

        /// <summary>
        /// Вес задачи (для расчета средневзвешенного)
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Количество поступивших в систему задач
        /// </summary>
        public static int GenCount;

        /// <summary>
        /// Количество покинувших  систему задач
        /// </summary>
        public static int TerCount;

        /// <summary>
        /// Количество покинувших очередь задач
        /// </summary>
        public static int OutQueueCount;

        /// <summary>
        /// Суммарные весовые коэффциенты покинувших очередб задач
        /// </summary>
        public static int OutQueueWeightCount;
    }
}
