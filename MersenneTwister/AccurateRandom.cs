using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public sealed class AccurateRandom : Random
    {
        private readonly Random rng;

        private uint cachedMaxValue = 0;
        private uint cachedValueMask = 0;

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

        private uint NextUInt32(uint maxValue)
        {
            if (maxValue == 0) { return 0; }
            // determine the position of MSB
            uint mask;
            if (maxValue == this.cachedMaxValue) {
                mask = this.cachedValueMask;
            }
            else {
                this.cachedMaxValue = maxValue;
                mask = this.cachedValueMask = BitScanner.Mask(maxValue - 1);
            }
            //
            uint num;
            if (maxValue < (1 << 30)) {
                do {
                    num = (uint)this.rng.Next() >> 1;
                    num &= mask;
                } while (num >= maxValue);
            }
            else {
                do {
                    var a = (uint)this.rng.Next() >> 1;
                    var b = (uint)this.rng.Next() >> 1;
                    num = a | (b << 30);
                    num &= mask;
                } while (num >= maxValue);
            }
            return num;
        }

        public override int Next(int maxValue)
        {
            if (maxValue < 0) { throw new ArgumentOutOfRangeException(); }
            return (int)this.NextUInt32((uint)maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue) { throw new ArgumentOutOfRangeException(); }
            return (int)this.NextUInt32((uint)((long)maxValue - minValue)) + minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
            this.rng.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            return FullPrecisionDouble_c0o1(this.rng);
        }

        protected override double Sample()
        {
            return FullPrecisionDouble_c0o1(this.rng);
        }

        public static double FullPrecisionDouble_c0o1(Random rng)
        {
            const int EffectiveBits = 30;
            // exponent
            var e = -1;
            do {
                var ntz = BitScanner.NumberOfTrailingZeros32((uint)(rng.Next() >> 1));
                if (ntz < EffectiveBits) {
                    e -= ntz;
                    break;
                }
                e -= EffectiveBits;
            } while (e > -1024);
            //
            if (e <= -1024) {
                return 0;
            }
            // fraction
            var a = rng.Next() >> 1;
            var b = rng.Next() >> 1;
            var f = ((long)a << 22) ^ b;
            // IEEE-754
            return BitConverter.Int64BitsToDouble(((long)(e + 1023) << 52) | f);
        }
    }
}
