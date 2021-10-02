using System;
using System.Runtime.CompilerServices;
using MersenneTwister.MT;

namespace MersenneTwister
{
    public static class MTRandom
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

#if PUBLIC
    public
#else
    internal
#endif
    sealed class MTRandom<T> : RandomBase32 where T : Imt19937, new()
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

        public override uint GenerateUInt32()
        {
            return this.mt.genrand_int32();
        }
    }
}
