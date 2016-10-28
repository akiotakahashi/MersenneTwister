using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public sealed class AccurateRandom : Random
    {
        private const int BUFFER_SIZE = sizeof(ulong);

        private readonly Random rng;
        private readonly byte[] buffer = new byte[BUFFER_SIZE];

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

        private uint NextUInt32()
        {
            this.rng.NextBytes(this.buffer);
            return BitConverter.ToUInt32(this.buffer, 0);
        }

        public override int Next()
        {
            return this.Next(int.MaxValue);
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
            do {
                num = this.NextUInt32();
                num &= mask;
            } while (num >= maxValue);
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
            return FullPrecisionDouble_c0o1(this.rng, this.buffer);
        }

        protected override double Sample()
        {
            return FullPrecisionDouble_c0o1(this.rng, this.buffer);
        }

        private static double FullPrecisionDouble_c0o1(Random rng, byte[] buffer)
        {
            const int EffectiveBits = BUFFER_SIZE * 8;
            // exponent
            var e = -1;
            do {
                rng.NextBytes(buffer);
                var r = BitConverter.ToUInt64(buffer, 0);
                var ntz = BitScanner.NumberOfTrailingZeros64(r);
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
            rng.NextBytes(buffer);
            var f = (long)(BitConverter.ToUInt64(buffer, 0) >> 12);
            // IEEE-754
            return BitConverter.Int64BitsToDouble(((long)(e + 1023) << 52) | f);
        }

#if PUBLIC
        public static double FullPrecisionDouble_c0o1(Random rng)
        {
            return FullPrecisionDouble_c0o1(rng, new byte[BUFFER_SIZE]);
        }
#endif
    }
}
