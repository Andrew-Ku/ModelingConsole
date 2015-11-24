using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Infrastructure
{
    public static class Generator
    {
        static long _preX = DateTime.Now.Millisecond, _newX;

        // Равномерное распределение
        public static double UniformDistribution(int a, int b)
        {
            var rand = new Random();
            return a + (b - a) * rand.NextDouble();
        }

        // Экспоненциальное распределение
        public static double ExpDistribution(double beta)
        {
            while (true)
            {
                var rand = NextRandomDouble();
                var val = -beta * Math.Log(Math.E, rand); 
                if(val > 5)
                    continue;
                return val;
            }
        }
        static double NextRandomDouble()
        {
            var m = (long)Math.Pow(2, 32);
            const int a = 214013;
            const long c = 2531011;
            _newX = (a * _preX + c) % m;
            _preX = _newX;

            var result = _newX / (double)m; //От нуля до единицы
            return result;
        }
    }
}
