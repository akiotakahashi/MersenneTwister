/**
 * @file  SFMT.c
 * @brief SIMD oriented Fast Mersenne Twister(SFMT)
 *
 * @author Mutsuo Saito (Hiroshima University)
 * @author Makoto Matsumoto (Hiroshima University)
 *
 * Copyright (C) 2006, 2007 Mutsuo Saito, Makoto Matsumoto and Hiroshima
 * University.
 * Copyright (C) 2012 Mutsuo Saito, Makoto Matsumoto, Hiroshima
 * University and The University of Tokyo.
 * Copyright (C) 2013 Mutsuo Saito, Makoto Matsumoto and Hiroshima
 * University.
 * All rights reserved.
 *
 * The 3-clause BSD License is applied to this software, see
 * LICENSE.txt
 */

using System;
using System.Runtime.CompilerServices;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.SFMT
{
#if PUBLIC
    public
#else
    internal
#endif
    sealed class sfmt_opt_t : sfmt_t_params, Isfmt
    {
        private static readonly uint32_t[] parity = new[] { SFMT_PARITY1, SFMT_PARITY2, SFMT_PARITY3, SFMT_PARITY4 };

        /*--------------------
		 * SFMT internal state
		  --------------------*/

        /* the 128-bit internal state array */
        private w128_t[] state = new w128_t[SFMT_N];
        /* index counter to the 32-bit internal state array */
        private uint idx;

        /*----------------
		  STATIC FUNCTIONS
		  ----------------*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void rshift128(ref w128_t dst, ref w128_t src, int shift)
        {
            uint64_t th, tl, oh, ol;
            th = src.u64_1;
            tl = src.u64_0;
            oh = th >> (shift * 8);
            ol = tl >> (shift * 8);
            ol |= th << (64 - shift * 8);
            dst.u64_0 = ol;
            dst.u64_1 = oh;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void lshift128(ref w128_t dst, ref w128_t src, int shift)
        {
            uint64_t th, tl, oh, ol;
            th = src.u64_1;
            tl = src.u64_0;
            oh = th << (shift * 8);
            ol = tl << (shift * 8);
            oh |= tl >> (64 - shift * 8);
            dst.u64_0 = ol;
            dst.u64_1 = oh;
        }

        private static w128_t x, y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void do_recursion(ref w128_t r, ref w128_t a, ref w128_t b, ref w128_t c, ref w128_t d)
        {
            lshift128(ref x, ref a, SFMT_SL2);
            rshift128(ref y, ref c, SFMT_SR2);
            r.u32_0 = a.u32_0 ^ x.u32_0 ^ ((b.u32_0 >> SFMT_SR1) & SFMT_MSK1) ^ y.u32_0 ^ (d.u32_0 << SFMT_SL1);
            r.u32_1 = a.u32_1 ^ x.u32_1 ^ ((b.u32_1 >> SFMT_SR1) & SFMT_MSK2) ^ y.u32_1 ^ (d.u32_1 << SFMT_SL1);
            r.u32_2 = a.u32_2 ^ x.u32_2 ^ ((b.u32_2 >> SFMT_SR1) & SFMT_MSK3) ^ y.u32_2 ^ (d.u32_2 << SFMT_SL1);
            r.u32_3 = a.u32_3 ^ x.u32_3 ^ ((b.u32_3 >> SFMT_SR1) & SFMT_MSK4) ^ y.u32_3 ^ (d.u32_3 << SFMT_SL1);
        }

        /**
		 * This function fills the user-specified array with pseudorandom
		 * integers.
		 *
		 * @param sfmt SFMT internal state
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array(w128_t[] array, int size)
        {
            var state = this.state;
            var a1 = state;
            var a2 = state;
            var r1 = SFMT_N - 2;
            var r2 = SFMT_N - 1;
            int i;
            for (i = 0; i < SFMT_N - SFMT_POS1; i++) {
                do_recursion(ref array[i], ref state[i], ref state[i + SFMT_POS1], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = array;
                r2 = i;
            }
            for (; i < SFMT_N; i++) {
                do_recursion(ref array[i], ref state[i], ref array[i + SFMT_POS1 - SFMT_N], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = array;
                r2 = i;
            }
            for (; i < size - SFMT_N; i++) {
                do_recursion(ref array[i], ref array[i - SFMT_N], ref array[i + SFMT_POS1 - SFMT_N], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = array;
                r2 = i;
            }
            int j;
            for (j = 0; j < 2 * SFMT_N - size; j++) {
                state[j] = array[j + size - SFMT_N];
            }
            for (; i < size; i++, j++) {
                do_recursion(ref array[i], ref array[i - SFMT_N], ref array[i + SFMT_POS1 - SFMT_N], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = array;
                r2 = i;
                state[j] = array[i];
            }
        }

        /**
		 * This function represents a function used in the initialization
		 * by init_by_array
		 * @param x 32-bit integer
		 * @return 32-bit integer
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint32_t func1(uint32_t x)
        {
            return (x ^ (x >> 27)) * 1664525U;
        }

        /**
		 * This function represents a function used in the initialization
		 * by init_by_array
		 * @param x 32-bit integer
		 * @return 32-bit integer
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint32_t func2(uint32_t x)
        {
            return (x ^ (x >> 27)) * 1566083941U;
        }

        /**
		 * This function certificate the period of 2^{MEXP}
		 * @param sfmt SFMT internal state
		 */
        void period_certification()
        {
            var state0 = this.state[0];

            uint inner = 0;
            inner ^= state0.u32_0 & parity[0];
            inner ^= state0.u32_1 & parity[1];
            inner ^= state0.u32_2 & parity[2];
            inner ^= state0.u32_3 & parity[3];
            for (int i = 16; i > 0; i >>= 1) {
                inner ^= inner >> i;
            }
            inner &= 1;
            /* check OK */
            if (inner == 1) {
                return;
            }
            /* check NG, and modification */
            if (modify(ref state[0].u32_0, 0)) { return; }
            if (modify(ref state[0].u32_0, 1)) { return; }
            if (modify(ref state[0].u32_0, 2)) { return; }
            if (modify(ref state[0].u32_0, 3)) { return; }
        }

        private static bool modify(ref uint32_t u, int i)
        {
            uint32_t work = 1;
            for (int j = 0; j < 32; j++) {
                if ((work & parity[i]) != 0) {
                    u ^= work;
                    return true;
                }
                work = work << 1;
            }
            return false;
        }

        /*----------------
		  PUBLIC FUNCTIONS
		  ----------------*/

        /**
		 * This function returns the identification string.
		 * The string shows the word size, the Mersenne exponent,
		 * and all parameters of this generator.
		 * @param sfmt SFMT internal state
		 */
        public string sfmt_get_idstring()
        {
            return SFMT_IDSTR;
        }

        /**
		 * This function returns the minimum size of array used for \b
		 * fill_array32() function.
		 * @param sfmt SFMT internal state
		 * @return minimum size of array used for fill_array32() function.
		 */
        public int sfmt_get_min_array_size32()
        {
            return SFMT_N32;
        }

        /**
		 * This function returns the minimum size of array used for \b
		 * fill_array64() function.
		 * @param sfmt SFMT internal state
		 * @return minimum size of array used for fill_array64() function.
		 */
        public int sfmt_get_min_array_size64()
        {
            return SFMT_N64;
        }

        /**
		 * This function fills the internal state array with pseudorandom
		 * integers.
		 * @param sfmt SFMT internal state
		 */
        void sfmt_gen_rand_all()
        {
            var state = this.state;
            var a1 = state;
            var a2 = state;
            var r1 = SFMT_N - 2;
            var r2 = SFMT_N - 1;
            int i;
            for (i = 0; i < SFMT_N - SFMT_POS1; i++) {
                do_recursion(ref state[i], ref state[i], ref state[i + SFMT_POS1], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = state;
                r2 = i;
            }
            for (; i < SFMT_N; i++) {
                do_recursion(ref state[i], ref state[i], ref state[i + SFMT_POS1 - SFMT_N], ref a1[r1], ref a2[r2]);
                a1 = a2;
                r1 = r2;
                a2 = state;
                r2 = i;
            }
        }

        /**
		 * This function generates pseudorandom 32-bit integers in the
		 * specified array[] by one call. The number of pseudorandom integers
		 * is specified by the argument size, which must be at least 624 and a
		 * multiple of four.  The generation by this function is much faster
		 * than the following gen_rand function.
		 *
		 * For initialization, init_gen_rand or init_by_array must be called
		 * before the first call of this function. This function can not be
		 * used after calling gen_rand function, without initialization.
		 *
		 * @param sfmt SFMT internal state
		 * @param array an array where pseudorandom 32-bit integers are filled
		 * by this function.  The pointer to the array must be \b "aligned"
		 * (namely, must be a multiple of 16) in the SIMD version, since it
		 * refers to the address of a 128-bit integer.  In the standard C
		 * version, the pointer is arbitrary.
		 *
		 * @param size the number of 32-bit pseudorandom integers to be
		 * generated.  size must be a multiple of 4, and greater than or equal
		 * to (MEXP / 128 + 1) * 4.
		 *
		 * @note \b memalign or \b posix_memalign is available to get aligned
		 * memory. Mac OSX doesn't have these functions, but \b malloc of OSX
		 * returns the pointer to the aligned memory block.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void sfmt_fill_array32(uint32_t[] array, int size)
        {
            assert(idx == SFMT_N32);
            assert(size % 4 == 0);
            assert(size >= SFMT_N32);
            var buf = new w128_t[size / 4];
            gen_rand_array(buf, buf.Length);
            copy32(buf, array);
            idx = SFMT_N32;
        }

        /**
		 * This function generates pseudorandom 64-bit integers in the
		 * specified array[] by one call. The number of pseudorandom integers
		 * is specified by the argument size, which must be at least 312 and a
		 * multiple of two.  The generation by this function is much faster
		 * than the following gen_rand function.
		 *
		 * @param sfmt SFMT internal state
		 * For initialization, init_gen_rand or init_by_array must be called
		 * before the first call of this function. This function can not be
		 * used after calling gen_rand function, without initialization.
		 *
		 * @param array an array where pseudorandom 64-bit integers are filled
		 * by this function.  The pointer to the array must be "aligned"
		 * (namely, must be a multiple of 16) in the SIMD version, since it
		 * refers to the address of a 128-bit integer.  In the standard C
		 * version, the pointer is arbitrary.
		 *
		 * @param size the number of 64-bit pseudorandom integers to be
		 * generated.  size must be a multiple of 2, and greater than or equal
		 * to (MEXP / 128 + 1) * 2
		 *
		 * @note \b memalign or \b posix_memalign is available to get aligned
		 * memory. Mac OSX doesn't have these functions, but \b malloc of OSX
		 * returns the pointer to the aligned memory block.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void sfmt_fill_array64(uint64_t[] array, int size)
        {
            assert(idx == SFMT_N32);
            assert(size % 2 == 0);
            assert(size >= SFMT_N64);
            var buf = new w128_t[size / 2];
            gen_rand_array(buf, buf.Length);
            copy64(buf, array);
            idx = SFMT_N32;
        }

        /**
		 * This function initializes the internal state array with a 32-bit
		 * integer seed.
		 *
		 * @param sfmt SFMT internal state
		 * @param seed a 32-bit integer used as the seed.
		 */
        public void sfmt_init_gen_rand(uint32_t seed)
        {
            var state = this.state;
            var u0 = state[0].u32_0 = seed;
            var u1 = state[0].u32_1 = 1812433253U * (u0 ^ (u0 >> 30)) + 1;
            var u2 = state[0].u32_2 = 1812433253U * (u1 ^ (u1 >> 30)) + 2;
            var u3 = state[0].u32_3 = 1812433253U * (u2 ^ (u2 >> 30)) + 3;
            var j = 4u;
            for (uint i = 1; i < SFMT_N; i++) {
                u0 = state[i].u32_0 = 1812433253U * (u3 ^ (u3 >> 30)) + j++;
                u1 = state[i].u32_1 = 1812433253U * (u0 ^ (u0 >> 30)) + j++;
                u2 = state[i].u32_2 = 1812433253U * (u1 ^ (u1 >> 30)) + j++;
                u3 = state[i].u32_3 = 1812433253U * (u2 ^ (u2 >> 30)) + j++;
            }
            idx = SFMT_N32;
            period_certification();
        }

        /**
		 * This function initializes the internal state array,
		 * with an array of 32-bit integers used as the seeds
		 * @param sfmt SFMT internal state
		 * @param init_key the array of 32-bit integers, used as a seed.
		 * @param key_length the length of init_key.
		 */
        public void sfmt_init_by_array(uint32_t[] init_key, uint key_length)
        {
            var state = this.state;
            const int size = SFMT_N * 4;
            const int lag =
                (size >= 623) ? 11 :
                (size >= 68) ? 7 :
                (size >= 39) ? 5 : 3;
            const int mid = (size - lag) / 2;

            //memset(psfmt32, 0x8b, sizeof(sfmt_t));
            for (var idx = 0; idx < SFMT_N; ++idx) {
                state[idx].u32_0 = 0x8b8b8b8bU;
                state[idx].u32_1 = 0x8b8b8b8bU;
                state[idx].u32_2 = 0x8b8b8b8bU;
                state[idx].u32_3 = 0x8b8b8b8bU;
            }

            uint count;
            if (key_length + 1 > SFMT_N32) {
                count = key_length + 1;
            }
            else {
                count = SFMT_N32;
            }

            uint32_t r;
            r = func1(get32(state, 0) ^ get32(state, mid) ^ get32(state, SFMT_N32 - 1));
            add32(state, mid, r);
            r += key_length;
            add32(state, mid + lag, r);
            set32(state, 0, r);

            count--;
            uint i, j;
            for (i = 1, j = 0; (j < count) && (j < key_length); j++) {
                r = func1(get32(state, i) ^ get32(state, (i + mid) % SFMT_N32) ^ get32(state, (i + SFMT_N32 - 1) % SFMT_N32));
                add32(state, (i + mid) % SFMT_N32, r);
                r += init_key[j] + i;
                add32(state, (i + mid + lag) % SFMT_N32, r);
                set32(state, i, r);
                i = (i + 1) % SFMT_N32;
            }
            for (; j < count; j++) {
                r = func1(get32(state, i) ^ get32(state, (i + mid) % SFMT_N32) ^ get32(state, (i + SFMT_N32 - 1) % SFMT_N32));
                add32(state, (i + mid) % SFMT_N32, r);
                r += i;
                add32(state, (i + mid + lag) % SFMT_N32, r);
                set32(state, i, r);
                i = (i + 1) % SFMT_N32;
            }
            for (j = 0; j < SFMT_N32; j++) {
                r = func2(get32(state, i) + get32(state, (i + mid) % SFMT_N32) + get32(state, (i + SFMT_N32 - 1) % SFMT_N32));
                xor32(state, (i + mid) % SFMT_N32, r);
                r -= i;
                xor32(state, (i + mid + lag) % SFMT_N32, r);
                set32(state, i, r);
                i = (i + 1) % SFMT_N32;
            }
            idx = SFMT_N32;
            period_certification();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void sfmt_init_by_array(uint32_t[] init_key)
        {
            sfmt_init_by_array(init_key, (uint)init_key.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint32_t sfmt_genrand_uint32()
        {
            if (idx >= SFMT_N32) {
                sfmt_gen_rand_all();
                idx = 0;
            }
            return get32(this.state, idx++);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint64_t sfmt_genrand_uint64()
        {
            if ((idx & 1) != 0) {
                ++idx;
            }
            if (idx >= SFMT_N32) {
                sfmt_gen_rand_all();
                idx = 0;
            }
            var r = get64(this.state, idx >> 1);
            idx += 2;
            return r;
        }
    }
}
