using System;
using System.Runtime.CompilerServices;
using MersenneTwister.dSFMT;

namespace MersenneTwister
{
    public static class DsfmtRandom
    {
        public static Random Create()
        {
            return Create(DsfmtEdition.Opt_19937);
        }

        public static Random Create(int seed)
        {
            return Create(seed, DsfmtEdition.Opt_19937);
        }

        public static Random Create(uint[] seed)
        {
            return Create(seed, DsfmtEdition.Opt_19937);
        }

        public static Random Create(DsfmtEdition edition)
        {
            switch (edition) {
            case DsfmtEdition.Original_19937:
                return new DsfmtRandom<dsfmt_t>();
            case DsfmtEdition.Opt_19937:
                return new DsfmtRandom<dsfmt_opt_t>();
            case DsfmtEdition.OptGen_521:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_521>>();
            case DsfmtEdition.OptGen_1279:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_1279>>();
            case DsfmtEdition.OptGen_2203:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_2203>>();
            case DsfmtEdition.OptGen_4253:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_4253>>();
            case DsfmtEdition.OptGen_11213:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_11213>>();
            case DsfmtEdition.OptGen_19937:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_19937>>();
            case DsfmtEdition.OptGen_44497:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_44497>>();
            case DsfmtEdition.OptGen_86243:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_86243>>();
            case DsfmtEdition.OptGen_132049:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_132049>>();
            case DsfmtEdition.OptGen_216091:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_216091>>();
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(int seed, DsfmtEdition edition)
        {
            switch (edition) {
            case DsfmtEdition.Original_19937:
                return new DsfmtRandom<dsfmt_t>(seed);
            case DsfmtEdition.Opt_19937:
                return new DsfmtRandom<dsfmt_opt_t>(seed);
            case DsfmtEdition.OptGen_521:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_521>>(seed);
            case DsfmtEdition.OptGen_1279:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_1279>>(seed);
            case DsfmtEdition.OptGen_2203:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_2203>>(seed);
            case DsfmtEdition.OptGen_4253:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_4253>>(seed);
            case DsfmtEdition.OptGen_11213:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_11213>>(seed);
            case DsfmtEdition.OptGen_19937:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_19937>>(seed);
            case DsfmtEdition.OptGen_44497:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_44497>>(seed);
            case DsfmtEdition.OptGen_86243:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_86243>>(seed);
            case DsfmtEdition.OptGen_132049:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_132049>>(seed);
            case DsfmtEdition.OptGen_216091:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_216091>>(seed);
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(uint[] seed, DsfmtEdition edition)
        {
            switch (edition) {
            case DsfmtEdition.Original_19937:
                return new DsfmtRandom<dsfmt_t>(seed);
            case DsfmtEdition.Opt_19937:
                return new DsfmtRandom<dsfmt_opt_t>(seed);
            case DsfmtEdition.OptGen_521:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_521>>(seed);
            case DsfmtEdition.OptGen_1279:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_1279>>(seed);
            case DsfmtEdition.OptGen_2203:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_2203>>(seed);
            case DsfmtEdition.OptGen_4253:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_4253>>(seed);
            case DsfmtEdition.OptGen_11213:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_11213>>(seed);
            case DsfmtEdition.OptGen_19937:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_19937>>(seed);
            case DsfmtEdition.OptGen_44497:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_44497>>(seed);
            case DsfmtEdition.OptGen_86243:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_86243>>(seed);
            case DsfmtEdition.OptGen_132049:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_132049>>(seed);
            case DsfmtEdition.OptGen_216091:
                return new DsfmtRandom<dsfmt_opt_gen_t<dsfmt_params_216091>>(seed);
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
    sealed class DsfmtRandom<T> : RandomBase32 where T : Idsfmt, new()
    {
        private readonly T dsfmt = new T();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DsfmtRandom()
        {
            var seed = SeedUtil.GenerateSeed();
            this.dsfmt.dsfmt_init_by_array(seed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DsfmtRandom(int seed)
        {
            this.dsfmt.dsfmt_init_gen_rand((uint)seed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DsfmtRandom(uint[] seed)
        {
            this.dsfmt.dsfmt_init_by_array(seed);
        }

        public override uint GenerateUInt32()
        {
            return this.dsfmt.dsfmt_genrand_uint32();
        }

        public override double GenerateDouble()
        {
            return this.dsfmt.dsfmt_genrand_close_open();
        }
    }
}
