using System;
using System.Runtime.CompilerServices;
using MersenneTwister.MT;

namespace MersenneTwister
{
    public static class MT64Random
    {
        public static Random Create()
        {
            return Create(MT64Edition.Opt_19937);
        }

        public static Random Create(int seed)
        {
            return Create(seed, MT64Edition.Opt_19937);
        }

        public static Random Create(ulong[] seed)
        {
            return Create(seed, MT64Edition.Opt_19937);
        }

        public static Random Create(MT64Edition edition)
        {
            switch (edition) {
            case MT64Edition.Original_19937:
                return new MT64Random<mt19937_64_t>();
            case MT64Edition.Opt_19937:
                return new MT64Random<mt19937_64_opt_t>();
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(int seed, MT64Edition edition)
        {
            switch (edition) {
            case MT64Edition.Original_19937:
                return new MT64Random<mt19937_64_t>(seed);
            case MT64Edition.Opt_19937:
                return new MT64Random<mt19937_64_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(ulong[] seed, MT64Edition edition)
        {
            switch (edition) {
            case MT64Edition.Original_19937:
                return new MT64Random<mt19937_64_t>(seed);
            case MT64Edition.Opt_19937:
                return new MT64Random<mt19937_64_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }
    }

#if PUBLIC
    public
#else
    internal
#endif
    sealed class MT64Random<T> : MTRandom where T : Imt19937_64, new()
    {
        private readonly T mt = new T();

        public MT64Random()
        {
            var seed = SeedUtil.GenerateSeed();
            var buf = new ulong[(seed.Length + 1) / 2];
            Buffer.BlockCopy(seed, 0, buf, 0, Buffer.ByteLength(seed));
            this.mt.init_by_array64(buf, (uint)buf.Length);
        }

        public MT64Random(int seed)
        {
            this.mt.init_genrand64((uint)seed);
        }

        public MT64Random(ulong[] seed)
        {
            this.mt.init_by_array64(seed, (uint)seed.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double gendouble()
        {
            return this.mt.genrand64_res53();
        }

        public override int Next()
        {
            return (int)(this.mt.genrand64_int64() >> 33);
        }

        public override int Next(int maxValue)
        {
            if (maxValue < 0) { throw new ArgumentOutOfRangeException(); }
            return (int)(this.gendouble() * maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue) { throw new ArgumentOutOfRangeException(); }
            var num = (long)maxValue - minValue;
            return (int)(this.gendouble() * num) + minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null) { throw new ArgumentNullException(); }
            var i = 7;
            for (; i < buffer.Length; i += 8) {
                var val = this.mt.genrand64_int64();
                buffer[i - 7] = (byte)val;
                buffer[i - 6] = (byte)(val >> 8);
                buffer[i - 5] = (byte)(val >> 16);
                buffer[i - 4] = (byte)(val >> 24);
                buffer[i - 3] = (byte)(val >> 32);
                buffer[i - 2] = (byte)(val >> 40);
                buffer[i - 1] = (byte)(val >> 48);
                buffer[i - 0] = (byte)(val >> 56);
            }
            i -= 7;
            if (i < buffer.Length) {
                var val = this.mt.genrand64_int64();
                for (; i < buffer.Length; ++i) {
                    buffer[i] = (byte)val;
                    val >>= 8;
                }
            }
        }

        public override double NextDouble()
        {
            return this.gendouble();
        }

        protected override double Sample()
        {
            return this.gendouble();
        }
    }
}
