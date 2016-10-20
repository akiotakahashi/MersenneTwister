using System;
using System.Runtime.CompilerServices;
using MersenneTwister.SFMT;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.dSFMT
{
    public sealed class dsfmt_t : dsfmt_t_params, Idsfmt
    {
        private w128_t[] status = new w128_t[DSFMT_N + 1];
        private int idx;

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range [0, 1).
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_c0o1(ref w128_t w)
        {
            w.d0 -= 1.0;
            w.d1 -= 1.0;
        }

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range (0, 1].
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_o0c1(ref w128_t w)
        {
            w.d0 = 2.0 - w.d0;
            w.d1 = 2.0 - w.d1;
        }

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range (0, 1).
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_o0o1(ref w128_t w)
        {
            w.u64_0 |= 1;
            w.u64_1 |= 1;
            w.d0 -= 1.0;
            w.d1 -= 1.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void do_recursion(ref w128_t r, ref w128_t a, ref w128_t b, ref w128_t lung)
        {
            uint64_t t0, t1, L0, L1;
            t0 = a.u64_0;
            t1 = a.u64_1;
            L0 = lung.u64_0;
            L1 = lung.u64_1;
            lung.u64_0 = (t0 << DSFMT_SL1) ^ (L1 >> 32) ^ (L1 << 32) ^ b.u64_0;
            lung.u64_1 = (t1 << DSFMT_SL1) ^ (L0 >> 32) ^ (L0 << 32) ^ b.u64_1;
            r.u64_0 = (lung.u64_0 >> DSFMT_SR) ^ (lung.u64_0 & DSFMT_MSK1) ^ t0;
            r.u64_1 = (lung.u64_1 >> DSFMT_SR) ^ (lung.u64_1 & DSFMT_MSK2) ^ t1;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_c1o2(w128_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            w128_t lung;
            lung = status[DSFMT_N];
            do_recursion(ref array[0], ref status[0], ref status[DSFMT_POS1], ref lung);
            for (i = 1; i < DSFMT_N - DSFMT_POS1; i++) {
                do_recursion(ref array[i], ref status[i], ref status[i + DSFMT_POS1], ref lung);
            }
            for (; i < DSFMT_N; i++) {
                do_recursion(ref array[i], ref status[i], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            for (; i < size - DSFMT_N; i++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            for (j = 0; j < 2 * DSFMT_N - size; j++) {
                status[j] = array[j + size - DSFMT_N];
            }
            for (; i < size; i++, j++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                status[j] = array[i];
            }
            status[DSFMT_N] = lung;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_c0o1(w128_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            w128_t lung;
            lung = status[DSFMT_N];
            do_recursion(ref array[0], ref status[0], ref status[DSFMT_POS1], ref lung);
            for (i = 1; i < DSFMT_N - DSFMT_POS1; i++) {
                do_recursion(ref array[i], ref status[i], ref status[i + DSFMT_POS1], ref lung);
            }
            for (; i < DSFMT_N; i++) {
                do_recursion(ref array[i], ref status[i], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            for (; i < size - DSFMT_N; i++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                convert_c0o1(ref array[i - DSFMT_N]);
            }
            for (j = 0; j < 2 * DSFMT_N - size; j++) {
                status[j] = array[j + size - DSFMT_N];
            }
            for (; i < size; i++, j++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                status[j] = array[i];
                convert_c0o1(ref array[i - DSFMT_N]);
            }
            for (i = size - DSFMT_N; i < size; i++) {
                convert_c0o1(ref array[i]);
            }
            status[DSFMT_N] = lung;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_o0o1(w128_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            w128_t lung;
            lung = status[DSFMT_N];
            do_recursion(ref array[0], ref status[0], ref status[DSFMT_POS1], ref lung);
            for (i = 1; i < DSFMT_N - DSFMT_POS1; i++) {
                do_recursion(ref array[i], ref status[i], ref status[i + DSFMT_POS1], ref lung);
            }
            for (; i < DSFMT_N; i++) {
                do_recursion(ref array[i], ref status[i], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            for (; i < size - DSFMT_N; i++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                convert_o0o1(ref array[i - DSFMT_N]);
            }
            for (j = 0; j < 2 * DSFMT_N - size; j++) {
                status[j] = array[j + size - DSFMT_N];
            }
            for (; i < size; i++, j++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                status[j] = array[i];
                convert_o0o1(ref array[i - DSFMT_N]);
            }
            for (i = size - DSFMT_N; i < size; i++) {
                convert_o0o1(ref array[i]);
            }
            status[DSFMT_N] = lung;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_o0c1(w128_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            w128_t lung;
            lung = status[DSFMT_N];
            do_recursion(ref array[0], ref status[0], ref status[DSFMT_POS1], ref lung);
            for (i = 1; i < DSFMT_N - DSFMT_POS1; i++) {
                do_recursion(ref array[i], ref status[i], ref status[i + DSFMT_POS1], ref lung);
            }
            for (; i < DSFMT_N; i++) {
                do_recursion(ref array[i], ref status[i], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            for (; i < size - DSFMT_N; i++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                convert_o0c1(ref array[i - DSFMT_N]);
            }
            for (j = 0; j < 2 * DSFMT_N - size; j++) {
                status[j] = array[j + size - DSFMT_N];
            }
            for (; i < size; i++, j++) {
                do_recursion(ref array[i], ref array[i - DSFMT_N], ref array[i + DSFMT_POS1 - DSFMT_N], ref lung);
                status[j] = array[i];
                convert_o0c1(ref array[i - DSFMT_N]);
            }
            for (i = size - DSFMT_N; i < size; i++) {
                convert_o0c1(ref array[i]);
            }
            status[DSFMT_N] = lung;
        }

        /**
		 * This function represents a function used in the initialization
		 * by init_by_array
		 * @param x 32-bit integer
		 * @return 32-bit integer
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint32_t ini_func1(uint32_t x)
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
        static uint32_t ini_func2(uint32_t x)
        {
            return (x ^ (x >> 27)) * 1566083941U;
        }

        /**
		 * This function initializes the internal state array to fit the IEEE
		 * 754 format.
		 * @param dsfmt dsfmt state vector.
		 */
        void initial_mask()
        {
            var status = this.status;
            for (uint i = 0; i < DSFMT_N * 2; i++) {
                set64(status, i, (get64(status, i) & DSFMT_LOW_MASK) | DSFMT_HIGH_CONST);
            }
        }

        private static readonly uint64_t[] pcv = new[] { DSFMT_PCV1, DSFMT_PCV2 };

        /**
		 * This function certificate the period of 2^{SFMT_MEXP}-1.
		 * @param dsfmt dsfmt state vector.
		 */
        void period_certification()
        {
            var status = this.status;
            uint64_t tmp0, tmp1;
            uint64_t inner;
            uint64_t work;
            tmp0 = (status[DSFMT_N].u64_0 ^ DSFMT_FIX1);
            tmp1 = (status[DSFMT_N].u64_1 ^ DSFMT_FIX2);

            inner = tmp0 & pcv[0];
            inner ^= tmp1 & pcv[1];
            for (int i = 32; i > 0; i >>= 1) {
                inner ^= inner >> i;
            }
            inner &= 1;
            /* check OK */
            if (inner == 1) {
                return;
            }
            /* check NG, and modification */
            if ((DSFMT_PCV2 & 1) == 1) {
                status[DSFMT_N].u64_1 ^= 1;
            }
            else {
#pragma warning disable 162
                for (int i = 1; i >= 0; i--) {
                    work = 1;
                    for (int j = 0; j < 64; j++) {
                        if ((work & pcv[i]) != 0) {
                            status[DSFMT_N].xor64((uint)i, work);
                            return;
                        }
                        work = work << 1;
                    }
                }
#pragma warning restore
            }
        }

        /*----------------
		  PUBLIC FUNCTIONS
		  ----------------*/

        int Idsfmt.MEXP {
            get {
                return DSFMT_MEXP;
            }
        }

        int Idsfmt.DSFMT_N {
            get {
                return DSFMT_N;
            }
        }

        /**
		 * This function returns the identification string.  The string shows
		 * the Mersenne exponent, and all parameters of this generator.
		 * @return id string.
		 */
        public string dsfmt_get_idstring()
        {
            return DSFMT_IDSTR;
        }

        /**
		 * This function returns the minimum size of array used for \b
		 * fill_array functions.
		 * @return minimum size of array used for fill_array functions.
		 */
        int dsfmt_get_min_array_size()
        {
            return DSFMT_N64;
        }

        /**
		 * This function fills the internal state array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 */
        void dsfmt_gen_rand_all()
        {
            var status = this.status;
            int i;
            w128_t lung;
            lung = status[DSFMT_N];
            do_recursion(ref status[0], ref status[0], ref status[DSFMT_POS1], ref lung);
            for (i = 1; i < DSFMT_N - DSFMT_POS1; i++) {
                do_recursion(ref status[i], ref status[i], ref status[i + DSFMT_POS1], ref lung);
            }
            for (; i < DSFMT_N; i++) {
                do_recursion(ref status[i], ref status[i], ref status[i + DSFMT_POS1 - DSFMT_N], ref lung);
            }
            status[DSFMT_N] = lung;
        }

        /**
		 * This function generates double precision floating point
		 * pseudorandom numbers which distribute in the range [1, 2) to the
		 * specified array[] by one call. The number of pseudorandom numbers
		 * is specified by the argument \b size, which must be at least (SFMT_MEXP
		 * / 128) * 2 and a multiple of two.  The function
		 * get_min_array_size() returns this minimum size.  The generation by
		 * this function is much faster than the following fill_array_xxx functions.
		 *
		 * For initialization, init_gen_rand() or init_by_array() must be called
		 * before the first call of this function. This function can not be
		 * used after calling genrand_xxx functions, without initialization.
		 *
		 * @param dsfmt dsfmt state vector.
		 * @param array an array where pseudorandom numbers are filled
		 * by this function.  The pointer to the array must be "aligned"
		 * (namely, must be a multiple of 16) in the SIMD version, since it
		 * refers to the address of a 128-bit integer.  In the standard C
		 * version, the pointer is arbitrary.
		 *
		 * @param size the number of 64-bit pseudorandom integers to be
		 * generated.  size must be a multiple of 2, and greater than or equal
		 * to (SFMT_MEXP / 128) * 2.
		 *
		 * @note \b memalign or \b posix_memalign is available to get aligned
		 * memory. Mac OSX doesn't have these functions, but \b malloc of OSX
		 * returns the pointer to the aligned memory block.
		 */
        public void dsfmt_fill_array_close1_open2(double[] array, int size)
        {
            assert(size % 2 == 0);
            assert(size >= DSFMT_N64);
            var buf = new w128_t[size / 2];
            gen_rand_array_c1o2(buf, buf.Length);
            copy64(buf, array);
        }

        /**
		 * This function generates double precision floating point
		 * pseudorandom numbers which distribute in the range (0, 1] to the
		 * specified array[] by one call. This function is the same as
		 * fill_array_close1_open2() except the distribution range.
		 *
		 * @param dsfmt dsfmt state vector.
		 * @param array an array where pseudorandom numbers are filled
		 * by this function.
		 * @param size the number of pseudorandom numbers to be generated.
		 * see also \sa fill_array_close1_open2()
		 */
        public void dsfmt_fill_array_open_close(double[] array, int size)
        {
            assert(size % 2 == 0);
            assert(size >= DSFMT_N64);
            var buf = new w128_t[size / 2];
            gen_rand_array_o0c1(buf, buf.Length);
            copy64(buf, array);
        }

        /**
		 * This function generates double precision floating point
		 * pseudorandom numbers which distribute in the range [0, 1) to the
		 * specified array[] by one call. This function is the same as
		 * fill_array_close1_open2() except the distribution range.
		 *
		 * @param array an array where pseudorandom numbers are filled
		 * by this function.
		 * @param dsfmt dsfmt state vector.
		 * @param size the number of pseudorandom numbers to be generated.
		 * see also \sa fill_array_close1_open2()
		 */
        public void dsfmt_fill_array_close_open(double[] array, int size)
        {
            assert(size % 2 == 0);
            assert(size >= DSFMT_N64);
            var buf = new w128_t[size / 2];
            gen_rand_array_c0o1(buf, buf.Length);
            copy64(buf, array);
        }


        /**
		 * This function generates double precision floating point
		 * pseudorandom numbers which distribute in the range (0, 1) to the
		 * specified array[] by one call. This function is the same as
		 * fill_array_close1_open2() except the distribution range.
		 *
		 * @param dsfmt dsfmt state vector.
		 * @param array an array where pseudorandom numbers are filled
		 * by this function.
		 * @param size the number of pseudorandom numbers to be generated.
		 * see also \sa fill_array_close1_open2()
		 */
        public void dsfmt_fill_array_open_open(double[] array, int size)
        {
            assert(size % 2 == 0);
            assert(size >= DSFMT_N64);
            var buf = new w128_t[size / 2];
            gen_rand_array_o0o1(buf, buf.Length);
            copy64(buf, array);
        }

        /**
		 * This function initializes the internal state array with a 32-bit
		 * integer seed.
		 * @param dsfmt dsfmt state vector.
		 * @param seed a 32-bit integer used as the seed.
		 * @param mexp caller's mersenne expornent
		 */
        void dsfmt_chk_init_gen_rand(uint32_t seed)
        {
            var status = this.status;
            var u0 = status[0].u32_0 = seed;
            var u1 = status[0].u32_1 = 1812433253U * (u0 ^ (u0 >> 30)) + 1;
            var u2 = status[0].u32_2 = 1812433253U * (u1 ^ (u1 >> 30)) + 2;
            var u3 = status[0].u32_3 = 1812433253U * (u2 ^ (u2 >> 30)) + 3;
            var j = 4u;
            for (uint i = 1; i < (DSFMT_N + 1); i++) {
                u0 = status[i].u32_0 = 1812433253U * (u3 ^ (u3 >> 30)) + j++;
                u1 = status[i].u32_1 = 1812433253U * (u0 ^ (u0 >> 30)) + j++;
                u2 = status[i].u32_2 = 1812433253U * (u1 ^ (u1 >> 30)) + j++;
                u3 = status[i].u32_3 = 1812433253U * (u2 ^ (u2 >> 30)) + j++;
            }
            initial_mask();
            period_certification();
            idx = DSFMT_N64;
        }

        /**
		 * This function initializes the internal state array,
		 * with an array of 32-bit integers used as the seeds
		 * @param dsfmt dsfmt state vector.
		 * @param init_key the array of 32-bit integers, used as a seed.
		 * @param key_length the length of init_key.
		 * @param mexp caller's mersenne expornent
		 */
        void dsfmt_chk_init_by_array(uint32_t[] init_key, uint key_length)
        {
            var status = this.status;
            uint i, j, count;
            uint32_t r;
            const int size = (DSFMT_N + 1) * 4;   /* pulmonary */
            const int lag =
                (size >= 623) ? 11 :
                (size >= 68) ? 7 :
                (size >= 39) ? 5 : 3;
            const int mid = (size - lag) / 2;
            //memset(status, 0x8b, sizeof(status));
            for (var idx = 0; idx <= DSFMT_N; ++idx) {
                status[idx].u64_0 = 0x8b8b8b8b8b8b8b8bUL;
                status[idx].u64_1 = 0x8b8b8b8b8b8b8b8bUL;
            }
            if (key_length + 1 > size) {
                count = key_length + 1;
            }
            else {
                count = size;
            }
            r = ini_func1(get32(status, 0) ^ get32(status, mid % size) ^ get32(status, (size - 1) % size));
            add32(status, mid % size, r);
            r += key_length;
            add32(status, (mid + lag) % size, r);
            set32(status, 0, r);
            count--;
            for (i = 1, j = 0; (j < count) && (j < key_length); j++) {
                r = ini_func1(get32(status, i) ^ get32(status, (i + mid) % size) ^ get32(status, (i + size - 1) % size));
                add32(status, (i + mid) % size, r);
                r += init_key[j] + i;
                add32(status, (i + mid + lag) % size, r);
                set32(status, i, r);
                i = (i + 1) % size;
            }
            for (; j < count; j++) {
                r = ini_func1(get32(status, i) ^ get32(status, (i + mid) % size) ^ get32(status, (i + size - 1) % size));
                add32(status, (i + mid) % size, r);
                r += i;
                add32(status, (i + mid + lag) % size, r);
                set32(status, i, r);
                i = (i + 1) % size;
            }
            for (j = 0; j < size; j++) {
                r = ini_func2(get32(status, i) + get32(status, (i + mid) % size) + get32(status, (i + size - 1) % size));
                xor32(status, (i + mid) % size, r);
                r -= i;
                xor32(status, (i + mid + lag) % size, r);
                set32(status, i, r);
                i = (i + 1) % size;
            }
            initial_mask();
            period_certification();
            idx = DSFMT_N64;
        }

        void dsfmt_chk_init_by_array(uint32_t[] init_key)
        {
            dsfmt_chk_init_by_array(init_key, (uint)init_key.Length);
        }

        /**
		* This function generates and returns unsigned 32-bit integer.
		* This is slower than SFMT, only for convenience usage.
		* dsfmt_init_gen_rand() or dsfmt_init_by_array() must be called
		* before this function.
		* @param dsfmt dsfmt internal state date
		* @return double precision floating point pseudorandom number
		*/
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint32_t dsfmt_genrand_uint32()
        {
            if (idx >= DSFMT_N64) {
                dsfmt_gen_rand_all();
                idx = 0;
            }
            return (uint)get64(this.status, (uint32_t)idx++);
        }

        /**
		 * This function generates and returns double precision pseudorandom
		 * number which distributes uniformly in the range [1, 2).  This is
		 * the primitive and faster than generating numbers in other ranges.
		 * dsfmt_init_gen_rand() or dsfmt_init_by_array() must be called
		 * before this function.
		 * @param dsfmt dsfmt internal state date
		 * @return double precision floating point pseudorandom number
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double dsfmt_genrand_close1_open2()
        {
            if (idx >= DSFMT_N64) {
                dsfmt_gen_rand_all();
                idx = 0;
            }
            return getd(status, (uint32_t)idx++);
        }

        /**
		 * This function generates and returns double precision pseudorandom
		 * number which distributes uniformly in the range [0, 1).
		 * dsfmt_init_gen_rand() or dsfmt_init_by_array() must be called
		 * before this function.
		 * @param dsfmt dsfmt internal state date
		 * @return double precision floating point pseudorandom number
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double dsfmt_genrand_close_open()
        {
            return dsfmt_genrand_close1_open2() - 1.0;
        }

        /**
		 * This function generates and returns double precision pseudorandom
		 * number which distributes uniformly in the range (0, 1].
		 * dsfmt_init_gen_rand() or dsfmt_init_by_array() must be called
		 * before this function.
		 * @param dsfmt dsfmt internal state date
		 * @return double precision floating point pseudorandom number
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double dsfmt_genrand_open_close()
        {
            return 2.0 - dsfmt_genrand_close1_open2();
        }

        /**
		 * This function generates and returns double precision pseudorandom
		 * number which distributes uniformly in the range (0, 1).
		 * dsfmt_init_gen_rand() or dsfmt_init_by_array() must be called
		 * before this function.
		 * @param dsfmt dsfmt internal state date
		 * @return double precision floating point pseudorandom number
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double dsfmt_genrand_open_open()
        {
            if (idx >= DSFMT_N64) {
                dsfmt_gen_rand_all();
                idx = 0;
            }
            uint64_t d = get64(this.status, (uint)idx++);
            d |= 1;
            return BitConverter.Int64BitsToDouble((long)d) - 1.0;
        }

        /**
		 * This function initializes the internal state array with a 32-bit
		 * integer seed.
		 * @param dsfmt dsfmt state vector.
		 * @param seed a 32-bit integer used as the seed.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void dsfmt_init_gen_rand(uint32_t seed)
        {
            dsfmt_chk_init_gen_rand(seed);
        }

        /**
		 * This function initializes the internal state array,
		 * with an array of 32-bit integers used as the seeds.
		 * @param dsfmt dsfmt state vector
		 * @param init_key the array of 32-bit integers, used as a seed.
		 * @param key_length the length of init_key.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void dsfmt_init_by_array(uint32_t[] init_key, uint key_length)
        {
            dsfmt_chk_init_by_array(init_key, key_length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void dsfmt_init_by_array(uint32_t[] init_key)
        {
            dsfmt_init_by_array(init_key, (uint)init_key.Length);
        }
    }
}
