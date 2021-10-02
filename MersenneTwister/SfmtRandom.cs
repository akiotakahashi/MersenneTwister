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

#if PUBLIC
    public
#else
    internal
#endif
    sealed class SfmtRandom<T> : RandomBase64 where T : Isfmt, new()
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

        public override ulong GenerateUInt64()
        {
            return this.sfmt.sfmt_genrand_uint64();
        }
    }
}
