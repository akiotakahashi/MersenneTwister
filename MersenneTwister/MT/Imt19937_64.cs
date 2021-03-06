﻿using System;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.MT
{
#if PUBLIC
    public
#else
    internal
#endif
    interface Imt19937_64
    {
        void init_genrand64(ulong s);
        void init_by_array64(uint64_t[] init_key, uint64_t key_length);
        uint64_t genrand64_int64();
#if PUBLIC
        double genrand64_real2();
#endif
    }
}
