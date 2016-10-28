using System;
using System.Runtime.CompilerServices;
using MersenneTwister.MT;

namespace MersenneTwister
{
    public static class MT64Random
    {
        public static Random Create()
        {
            return Create(MT64Edition.Original_19937);
        }

        public static Random Create(int seed)
        {
            return Create(seed, MT64Edition.Original_19937);
        }

        public static Random Create(ulong[] seed)
        {
            return Create(seed, MT64Edition.Original_19937);
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
    sealed class MT64Random<T> : RandomBase64 where T : Imt19937_64, new()
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

        protected override ulong GenerateUInt64()
        {
            return this.mt.genrand64_int64();
        }
    }
}
