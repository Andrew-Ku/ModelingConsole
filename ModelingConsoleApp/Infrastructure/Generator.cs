using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

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
                if (val > 1)
                    continue;
                return val;
            }
        }

        // Экспоненциальное распределение в зависимости от класса задачи назначения
        public static double ExpDistribution(string taskType, int target)
        {
            var beta=0.4; // Значение по умолчанию для всех одно

            if (target == GeneratorParametrs.Generation) // Если генерация для генерации новой задачи 
            {
                switch (taskType)
                {
                    case TaskTypes.ClassA:
                        beta = InputValues.BetaA;
                        break;

                    case TaskTypes.ClassB:
                        beta = InputValues.BetaB;
                        break;

                    case TaskTypes.ClassC:
                        beta = InputValues.BetaC;
                        break;
                }
            }
            if (target == GeneratorParametrs.Advance) // Если генерация для планирования времени обслуживания  задачи 
            {
                switch (taskType)
                {
                    case TaskTypes.ClassA:
                        beta = InputValues.BetaAdvanceA;
                        break;

                    case TaskTypes.ClassB:
                        beta = InputValues.BetaAdvanceB;
                        break;

                    case TaskTypes.ClassC:
                        beta = InputValues.BetaAdvanceC;
                        break;
                }
            }

            while (true)
            {
                var rand = NextRandomDouble();
                var val = -beta * Math.Log(Math.E, rand);
                Console.WriteLine(val);
                if (val > Koef(beta,1.08))
                    continue;
                return val;
            }
            //while (true)
            //{
            //    var rand = NextRandomDouble();
            //    var val = ((1/beta)*Math.Pow(Math.E, -(rand - 0)/beta));
            //    Console.WriteLine(val);
            //    if (val > 0.65)
            //        continue;
            //    return val;
            //}
           
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

        static double Koef(double beta, double k)
        {
            return Math.E*beta/k;
        }
    }
}
