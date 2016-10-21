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
    sealed class DsfmtRandom<T> : MTRandom where T : Idsfmt, new()
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

        public override int Next()
        {
            return (int)(this.dsfmt.dsfmt_genrand_uint32() >> 1);
        }

        public override int Next(int maxValue)
        {
            if (maxValue < 0) { throw new ArgumentOutOfRangeException(); }
            var r = this.dsfmt.dsfmt_genrand_uint32();
            return (int)(((ulong)maxValue * r) >> 32);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue) { throw new ArgumentOutOfRangeException(); }
            var num = (ulong)((long)maxValue - minValue);
            var r = this.dsfmt.dsfmt_genrand_uint32();
            return (int)((num * r) >> 32) + minValue;
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null) { throw new ArgumentNullException(); }
            var i = 3;
            for (; i < buffer.Length; i += 4) {
                var val = this.dsfmt.dsfmt_genrand_uint32();
                buffer[i - 3] = (byte)val;
                buffer[i - 2] = (byte)(val >> 8);
                buffer[i - 1] = (byte)(val >> 16);
                buffer[i - 0] = (byte)(val >> 24);
            }
            i -= 3;
            if (i < buffer.Length) {
                var val = this.dsfmt.dsfmt_genrand_uint32();
                for (; i < buffer.Length; ++i) {
                    buffer[i] = (byte)val;
                    val >>= 8;
                }
            }
        }

        public override double NextDouble()
        {
            return this.dsfmt.dsfmt_genrand_close_open();
        }

        protected override double Sample()
        {
            return this.dsfmt.dsfmt_genrand_close_open();
        }
    }
}
