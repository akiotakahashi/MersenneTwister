using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public class PreciseRandom : Random
    {
        private readonly Random rng;

        private int cachedMaxValue = -1;
        private int cachedValueMsb = -1;

        public PreciseRandom(Random baseRandom)
        {
            this.rng = baseRandom;
        }

        public override int Next()
        {
            return base.Next();
        }

        public override int Next(int maxValue)
        {
            if (maxValue <= 0) { throw new ArgumentOutOfRangeException(); }
            // determine the position of MSB
            int pos;
            if (maxValue == this.cachedMaxValue) {
                pos = this.cachedValueMsb;
            }
            else {
                this.cachedMaxValue = maxValue;
                pos = this.cachedValueMsb = BitScanner.MSB((ulong)maxValue);
            }
            //
            var val = (int)((1UL << pos) - 1);
            int num;
            do {
                num = this.rng.Next();
                num &= val;
            } while (num >= maxValue);
            return num;
        }

        public override int Next(int minValue, int maxValue)
        {
            return this.Next(maxValue - minValue) + minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
            this.rng.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            return this.rng.NextDouble();
        }

        protected override double Sample()
        {
            return this.rng.NextDouble();
        }
    }
}
