using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Model
{
    public class TaskQueue<T> : Queue<T>
    {
        public TaskQueue() { }
        public TaskQueue(int capasity) : base(capasity) { }
        public TaskQueue(IEnumerable<T> collection) : base(collection) { }

        #region Свойства

        public QueryInfo QueryInfo { get; set; }

        #endregion

        #region Методы

        public void DisplayQuery()
        {
            if (!this.Any()) Console.WriteLine("Очередь пуста");

            Console.WriteLine("Содержимое очереди");

            foreach (var item in this)  // EXception иногда
            {
                Console.WriteLine((item as TaskBase).Type);
            }

            Console.WriteLine("Количество задач A: " + this.Count(i => (i as TaskBase).Type == TaskTypes.ClassA));
            Console.WriteLine("Количество задач B: " + this.Count(i => (i as TaskBase).Type == TaskTypes.ClassB));
            Console.WriteLine("Количество задач C: " + this.Count(i => (i as TaskBase).Type == TaskTypes.ClassC));
        }

        #endregion


    }
}
