using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public class AccurateRandom : Random
    {
        private readonly Random rng;

        private int cachedMaxValue = -1;
        private int cachedValueMask = -1;

        public AccurateRandom(Random baseRandom)
        {
            this.rng = baseRandom;
        }

        public AccurateRandom() : this(MT64Random.Create())
        {
        }

        public AccurateRandom(int seed) : this(MT64Random.Create(seed))
        {
        }

        public AccurateRandom(ulong[] seed) : this(MT64Random.Create(seed))
        {
        }

        public override int Next()
        {
            return this.rng.Next();
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

        private readonly byte[] buffer64 = new byte[8];

        private double nextDouble()
        {
            this.rng.NextBytes(this.buffer64);
            var x = (ulong)BitConverter.ToInt64(this.buffer64, 0);
            if (x == 0) { return 0; }
            var msb = BitScanner.PositionOfMSB(x);
            var exp = msb - 64 - 1 + 1023;
            return BitConverter.Int64BitsToDouble(((long)exp << 52) | (long)(x & ((1UL << 52) - 1)));
        }

        public override double NextDouble()
        {
            return this.nextDouble();
        }

        protected override double Sample()
        {
            return this.nextDouble();
        }
    }
}
