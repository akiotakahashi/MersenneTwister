using System;
using System.Runtime.CompilerServices;
using MersenneTwister.SFMT;

namespace MersenneTwister
{
    public static class SfmtRandom
    {
        public static Random Create()
        {
            return Create(SfmtEdition.Opt_19937);
        }

        public static Random Create(int seed)
        {
            return Create(seed, SfmtEdition.Opt_19937);
        }

        public static Random Create(uint[] seed)
        {
            return Create(seed, SfmtEdition.Opt_19937);
        }

        public static Random Create(SfmtEdition edition)
        {
            switch (edition) {
            case SfmtEdition.Original_19937:
                return new SfmtRandom<sfmt_t>();
            case SfmtEdition.Opt_19937:
                return new SfmtRandom<sfmt_opt_t>();
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(int seed, SfmtEdition edition)
        {
            switch (edition) {
            case SfmtEdition.Original_19937:
                return new SfmtRandom<sfmt_t>(seed);
            case SfmtEdition.Opt_19937:
                return new SfmtRandom<sfmt_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(uint[] seed, SfmtEdition edition)
        {
            switch (edition) {
            case SfmtEdition.Original_19937:
                return new SfmtRandom<sfmt_t>(seed);
            case SfmtEdition.Opt_19937:
                return new SfmtRandom<sfmt_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }
    }

    internal sealed class SfmtRandom<T> : Random where T : Isfmt, new()
    {
        private readonly T sfmt = new T();

        public SfmtRandom()
        {
            var seed = SeedUtil.GenerateSeed();
            this.sfmt.sfmt_init_by_array(seed);
        }

        public SfmtRandom(int seed)
        {
            this.sfmt.sfmt_init_gen_rand((uint)seed);
        }

        public SfmtRandom(uint[] seed)
        {
            this.sfmt.sfmt_init_by_array(seed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double gendouble()
        {
            var x = this.sfmt.sfmt_genrand_uint64();
            var a = ((uint)x) >> 5;
            var b = (uint)(x >> 38);
            return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
        }

        public override int Next()
        {
            return (int)(this.sfmt.sfmt_genrand_uint32() >> 1);
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
                var val = this.sfmt.sfmt_genrand_uint64();
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
                var val = this.sfmt.sfmt_genrand_uint64();
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
