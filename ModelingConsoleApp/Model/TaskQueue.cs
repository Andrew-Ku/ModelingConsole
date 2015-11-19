using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Console.WriteLine("Содержимое очериди");

            foreach (var item  in this )  // EXception
            {
                Console.WriteLine((item as TaskBase).Type);
            }
        }
        
        #endregion

       
    }
}
