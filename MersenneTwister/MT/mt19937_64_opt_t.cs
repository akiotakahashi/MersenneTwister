﻿/* 
   A C-program for MT19937-64 (2014/2/23 version).
   Coded by Takuji Nishimura and Makoto Matsumoto.

   This is a 64-bit version of Mersenne Twister pseudorandom number
   generator.

   Before using, initialize the state by using init_genrand64(seed)  
   or init_by_array64(init_key, key_length).

   Copyright (C) 2004, 2014, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

     1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.

     2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

     3. The names of its contributors may not be used to endorse or promote 
        products derived from this software without specific prior written 
        permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

   References:
   T. Nishimura, ``Tables of 64-bit Mersenne Twisters''
     ACM Transactions on Modeling and 
     Computer Simulation 10. (2000) 348--357.
   M. Matsumoto and T. Nishimura,
     ``Mersenne Twister: a 623-dimensionally equidistributed
       uniform pseudorandom number generator''
     ACM Transactions on Modeling and 
     Computer Simulation 8. (Jan. 1998) 3--30.

   Any feedback is very welcome.
   http://www.math.hiroshima-u.ac.jp/~m-mat/MT/emt.html
   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove spaces)
*/

using System;
using System.Runtime.CompilerServices;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.MT
{
#if PUBLIC
    public
#else
    internal
#endif
    sealed class mt19937_64_opt_t : mt_base_t, Imt19937_64
    {
        private const int NN = 312;
        private const int MM = 156;
        private const uint64_t MATRIX_A = 0xB5026F5AA96619E9UL;
        private const uint64_t UM = 0xFFFFFFFF80000000UL; /* Most significant 33 bits */
        private const uint64_t LM = 0x7FFFFFFFUL; /* Least significant 31 bits */

        /* The array for the state vector */
        private readonly uint64_t[] mt = new uint64_t[NN];
        /* mti==NN+1 means mt[NN] is not initialized */
        private uint mti = NN + 1;

        /* initializes mt[NN] with a seed */
        public void init_genrand64(uint64_t seed)
        {
            var mt = this.mt;
            mt[0] = seed;
            for (mti = 1; mti < NN; mti++) {
                mt[mti] = (6364136223846793005UL * (mt[mti - 1] ^ (mt[mti - 1] >> 62)) + mti);
            }
        }

        /* initialize by an array with array-length */
        /* init_key is the array for initializing keys */
        /* key_length is its length */
        public void init_by_array64(uint64_t[] init_key, uint64_t key_length)
        {
            var mt = this.mt;
            uint i, j;
            uint64_t k;
            init_genrand64(19650218UL);
            i = 1;
            j = 0;
            k = (NN > key_length ? NN : key_length);
            for (; k != 0; k--) {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 62)) * 3935559000370003845UL)) + init_key[j] + j; /* non linear */
                i++;
                j++;
                if (i >= NN) { mt[0] = mt[NN - 1]; i = 1; }
                if (j >= key_length) {
                    j = 0;
                }
            }
            for (k = NN - 1; k != 0; k--) {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 62)) * 2862933555777941757UL)) - i; /* non linear */
                i++;
                if (i >= NN) { mt[0] = mt[NN - 1]; i = 1; }
            }

            mt[0] = 1UL << 63; /* MSB is 1; assuring non-zero initial array */
        }

        private static readonly uint64_t[] mag01 = new[] { 0UL, MATRIX_A };

        /* generates a random number on [0, 2^64-1]-interval */
        public uint64_t genrand64_int64()
        {
            var mt = this.mt;
            int i;
            uint64_t x;

            if (mti >= NN) { /* generate NN words at one time */
                for (i = 0; i < NN - MM; i += 4) {
                    x = (mt[i + 0] & UM) | (mt[i + 1] & LM);
                    mt[i + 0] = mt[i + (MM + 0)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 1] & UM) | (mt[i + 2] & LM);
                    mt[i + 1] = mt[i + (MM + 1)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 2] & UM) | (mt[i + 3] & LM);
                    mt[i + 2] = mt[i + (MM + 2)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 3] & UM) | (mt[i + 4] & LM);
                    mt[i + 3] = mt[i + (MM + 3)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                }
                for (; i < NN - 5; i += 4) {
                    x = (mt[i + 0] & UM) | (mt[i + 1] & LM);
                    mt[i + 0] = mt[i + (MM - NN + 0)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 1] & UM) | (mt[i + 2] & LM);
                    mt[i + 1] = mt[i + (MM - NN + 1)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 2] & UM) | (mt[i + 3] & LM);
                    mt[i + 2] = mt[i + (MM - NN + 2)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                    x = (mt[i + 3] & UM) | (mt[i + 4] & LM);
                    mt[i + 3] = mt[i + (MM - NN + 3)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                }
                x = (mt[i + 0] & UM) | (mt[i + 1] & LM);
                mt[i + 0] = mt[i + (MM - NN + 0)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                x = (mt[i + 1] & UM) | (mt[i + 2] & LM);
                mt[i + 1] = mt[i + (MM - NN + 1)] ^ (x >> 1) ^ mag01[(uint)x & 1];
                x = (mt[i + 2] & UM) | (mt[i + 3] & LM);
                mt[i + 2] = mt[i + (MM - NN + 2)] ^ (x >> 1) ^ mag01[(uint)x & 1];

                x = (mt[NN - 1] & UM) | (mt[0] & LM);
                mt[NN - 1] = mt[MM - 1] ^ (x >> 1) ^ mag01[(uint)x & 1];

                mti = 0;
            }

            x = mt[mti++];

            x ^= (x >> 29) & 0x5555555555555555UL;
            x ^= (x << 17) & 0x71D67FFFEDA60000UL;
            x ^= (x << 37) & 0xFFF7EEE000000000UL;
            x ^= (x >> 43);

            return x;
        }

        /* generates a random number on [0, 2^63-1]-interval */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long genrand64_int63()
        {
            return (long)(genrand64_int64() >> 1);
        }

        /* generates a random number on [0,1]-real-interval */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double genrand64_real1()
        {
            return (genrand64_int64() >> 11) * (1.0 / 9007199254740991.0);
        }

        /* generates a random number on [0,1)-real-interval */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double genrand64_real2()
        {
            return (genrand64_int64() >> 11) * (1.0 / 9007199254740992.0);
        }

        /* generates a random number on (0,1)-real-interval */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double genrand64_real3()
        {
            return ((genrand64_int64() >> 12) + 0.5) * (1.0 / 4503599627370496.0);
        }
    }
}
