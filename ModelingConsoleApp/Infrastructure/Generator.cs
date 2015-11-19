using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingConsoleApp.Infrastructure
{
    public static class Generator
    {
        // Равномерное распределение
        public static double UniformDistribution(int a, int b)
        {
            var rand = new Random();
            return a + (b - a) * rand.NextDouble();
        }

        // Экспоненциальное распределение
        public static double ExpDistribution(double beta)
        {
            var rand = new Random();
            return -beta * Math.Log(Math.E, rand.NextDouble());
        }
    }
}
