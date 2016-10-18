using System;
using System.Runtime.CompilerServices;
using MersenneTwister.MT;

namespace MersenneTwister
{
    public abstract class MTRandom : Random
    {
        public static Random Create()
        {
            return Create(MTEdition.CokOpt_19937);
        }

        public static Random Create(int seed)
        {
            return Create(seed, MTEdition.CokOpt_19937);
        }

        public static Random Create(uint[] seed)
        {
            return Create(seed, MTEdition.CokOpt_19937);
        }

        public static Random Create(MTEdition edition)
        {
            switch (edition) {
            case MTEdition.Original_19937:
                return new MTRandom<mt19937ar_t>();
            case MTEdition.Cok_19937:
                return new MTRandom<mt19937ar_cok_t>();
            case MTEdition.CokOpt_19937:
                return new MTRandom<mt19937ar_cok_opt_t>();
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(int seed, MTEdition edition)
        {
            switch (edition) {
            case MTEdition.Original_19937:
                return new MTRandom<mt19937ar_t>(seed);
            case MTEdition.Cok_19937:
                return new MTRandom<mt19937ar_cok_t>(seed);
            case MTEdition.CokOpt_19937:
                return new MTRandom<mt19937ar_cok_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(uint[] seed, MTEdition edition)
        {
            switch (edition) {
            case MTEdition.Original_19937:
                return new MTRandom<mt19937ar_t>(seed);
            case MTEdition.Cok_19937:
                return new MTRandom<mt19937ar_cok_t>(seed);
            case MTEdition.CokOpt_19937:
                return new MTRandom<mt19937ar_cok_opt_t>(seed);
            default:
                throw new ArgumentException();
            }
        }
    }

    internal sealed class MTRandom<T> : MTRandom where T : Imt19937, new()
    {
        private readonly T mt = new T();

        public MTRandom()
        {
            var seed = SeedUtil.GenerateSeed();
            this.mt.init_by_array(seed, seed.Length);
        }

        public MTRandom(int seed)
        {
            this.mt.init_genrand((uint)seed);
        }

        public MTRandom(uint[] seed)
        {
            this.mt.init_by_array(seed, seed.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double gendouble()
        {
            return this.mt.genrand_res53();
        }

        public override int Next()
        {
            return this.mt.genrand_int31();
        }

        public override int Next(int maxValue)
        {
            return (int)(this.gendouble() * maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            var num = (long)maxValue - minValue;
            return (int)(this.gendouble() * num) + minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
            var i = 3;
            for (; i < buffer.Length; i += 4) {
                var val = this.mt.genrand_int32();
                buffer[i - 3] = (byte)val;
                buffer[i - 2] = (byte)(val >> 8);
                buffer[i - 1] = (byte)(val >> 16);
                buffer[i - 0] = (byte)(val >> 24);
            }
            i -= 3;
            if (i < buffer.Length) {
                var val = this.mt.genrand_int32();
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
