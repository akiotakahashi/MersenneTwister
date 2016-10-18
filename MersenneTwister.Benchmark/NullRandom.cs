using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister.Benchmark
{
    public sealed class NullRandom : Random
    {
        public static readonly Random Defalut = new NullRandom();

        private NullRandom()
        {
        }

        public override int Next()
        {
            return 0;
        }

        public override int Next(int maxValue)
        {
            return 0;
        }

        public override int Next(int minValue, int maxValue)
        {
            return minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
        }

        public override double NextDouble()
        {
            return 0;
        }

        protected override double Sample()
        {
            return 0;
        }
    }
}
