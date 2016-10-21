using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public class PreciseRandom : Random
    {
        private readonly Random rng;

        private int cachedMaxValue = -1;
        private int cachedValueMask = -1;

        public PreciseRandom(Random baseRandom)
        {
            this.rng = baseRandom;
        }

        public PreciseRandom() : this(MT64Random.Create())
        {
        }

        public PreciseRandom(int seed) : this(MT64Random.Create(seed))
        {
        }

        public PreciseRandom(ulong[] seed) : this(MT64Random.Create(seed))
        {
        }

        public override int Next()
        {
            return base.Next();
        }

        public override int Next(int maxValue)
        {
            if (maxValue <= 0) {
                if (maxValue == 0) { return 0; }
                throw new ArgumentOutOfRangeException();
            }
            // determine the position of MSB
            int mask;
            if (maxValue == this.cachedMaxValue) {
                mask = this.cachedValueMask;
            }
            else {
                this.cachedMaxValue = maxValue;
                mask = this.cachedValueMask = (int)BitScanner.Mask((uint)maxValue);
            }
            //
            int num;
            do {
                num = this.rng.Next();
                num &= mask;
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
