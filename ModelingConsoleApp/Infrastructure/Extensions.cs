using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Infrastructure
{
    public static class Extensions
    {
        /// <summary>
        /// Добавление в список со временем моделирования
        /// </summary>
        /// <param name="list"></param>
        /// <param name="val"></param>
        public static void AddWithTime(this List<string> list, string val)
        {
            list.Add(Program.ModelingTime.ToString("F3").Replace(",",".") + ": " + val);
        }
    }
}
