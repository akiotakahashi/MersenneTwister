using System;
using System.Runtime.CompilerServices;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.dSFMT
{
#if PUBLIC
    public
#else
    internal
#endif
    sealed class dsfmt_opt_t : dsfmt_t_params, Idsfmt
    {
        private const int DSFMT_N_2 = DSFMT_N * 2;
        private const int DSFMT_POS1_2 = DSFMT_POS1 * 2;
        private const int DSFMT_POS1_2_sub_DSFMT_N_2 = DSFMT_POS1_2 - DSFMT_N_2;

        private uint64_t[] status = new uint64_t[(DSFMT_N + 1) * 2];
        private int idx;

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range [0, 1).
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_c0o1(ref uint64_t w)
        {
            var d = BitConverter.Int64BitsToDouble((long)w);
            w = (uint64_t)BitConverter.DoubleToInt64Bits(d - 1.0);
        }

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range (0, 1].
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_o0c1(ref uint64_t w)
        {
            var d = BitConverter.Int64BitsToDouble((long)w);
            w = (uint64_t)BitConverter.DoubleToInt64Bits(2.0 - d);
        }

        /**
		 * This function converts the double precision floating point numbers which
		 * distribute uniformly in the range [1, 2) to those which distribute uniformly
		 * in the range (0, 1).
		 * @param w 128bit stracture of double precision floating point numbers (I/O)
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void convert_o0o1(ref uint64_t w)
        {
            var d = BitConverter.Int64BitsToDouble((long)(w | 1));
            w = (uint64_t)BitConverter.DoubleToInt64Bits(d - 1.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void do_recursion(uint64_t[] r, int ri, uint64_t[] a, int ai, uint64_t[] b, int bi, ref uint64_t lung0, ref uint64_t lung1)
        {
            var t0 = a[ai];
            var t1 = a[ai + 1];
            var L0 = lung0;
            var L1 = lung1;
            lung0 = (t0 << DSFMT_SL1) ^ (L1 >> 32) ^ (L1 << 32) ^ b[bi];
            lung1 = (t1 << DSFMT_SL1) ^ (L0 >> 32) ^ (L0 << 32) ^ b[bi + 1];
            r[ri] = (lung0 >> DSFMT_SR) ^ (lung0 & DSFMT_MSK1) ^ t0;
            r[ri + 1] = (lung1 >> DSFMT_SR) ^ (lung1 & DSFMT_MSK2) ^ t1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int do_recursion(uint64_t[] r, int ai, int bi, ref uint64_t lung0, ref uint64_t lung1)
        {
            uint64_t t0;
            uint64_t t1;
            uint64_t L0;
            uint64_t L1;
            L0 = lung0;
            L1 = lung1;
            lung0 = ((t0 = r[ai + 0]) << DSFMT_SL1) ^ (L1 >> 32) ^ (L1 << 32) ^ r[ai + (bi + (0 + DSFMT_POS1_2))];
            lung1 = ((t1 = r[ai + 1]) << DSFMT_SL1) ^ (L0 >> 32) ^ (L0 << 32) ^ r[ai + (bi + (1 + DSFMT_POS1_2))];
            r[ai + 0] = (lung0 >> DSFMT_SR) ^ (lung0 & DSFMT_MSK1) ^ t0;
            r[ai + 1] = (lung1 >> DSFMT_SR) ^ (lung1 & DSFMT_MSK2) ^ t1;
            L0 = lung0;
            L1 = lung1;
            lung0 = ((t0 = r[ai + 2]) << DSFMT_SL1) ^ (L1 >> 32) ^ (L1 << 32) ^ r[ai + (bi + (2 + DSFMT_POS1_2))];
            lung1 = ((t1 = r[ai + 3]) << DSFMT_SL1) ^ (L0 >> 32) ^ (L0 << 32) ^ r[ai + (bi + (3 + DSFMT_POS1_2))];
            r[ai + 2] = (lung0 >> DSFMT_SR) ^ (lung0 & DSFMT_MSK1) ^ t0;
            r[ai + 3] = (lung1 >> DSFMT_SR) ^ (lung1 & DSFMT_MSK2) ^ t1;
            return 4;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_c1o2(uint64_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            var lung0 = status[DSFMT_N_2 + 0];
            var lung1 = status[DSFMT_N_2 + 1];
            do_recursion(array, 0, status, 0, status, DSFMT_POS1_2, ref lung0, ref lung1);
            for (i = 2; i < (DSFMT_N - DSFMT_POS1) * 2; i += 2) {
                do_recursion(array, i, status, i, status, i + DSFMT_POS1_2, ref lung0, ref lung1);
            }
            for (; i < DSFMT_N_2; i += 2) {
                do_recursion(array, i, status, i, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
            }
            for (; i < size - DSFMT_N_2; i += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
            }
            for (j = 0; j < (2 * DSFMT_N) * 2 - size; j += 2) {
                status[j + 0] = array[j + 0 + size - DSFMT_N_2];
                status[j + 1] = array[j + 1 + size - DSFMT_N_2];
            }
            for (; i < size; i += 2, j += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                status[j + 0] = array[i + 0];
                status[j + 1] = array[i + 1];
            }
            status[DSFMT_N_2 + 0] = lung0;
            status[DSFMT_N_2 + 1] = lung1;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_c0o1(uint64_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            var lung0 = status[DSFMT_N_2 + 0];
            var lung1 = status[DSFMT_N_2 + 1];
            do_recursion(array, 0, status, 0, status, DSFMT_POS1_2, ref lung0, ref lung1);
            for (i = 2; i < (DSFMT_N - DSFMT_POS1) * 2; i += 2) {
                do_recursion(array, i, status, i, status, i + DSFMT_POS1_2, ref lung0, ref lung1);
            }
            for (; i < DSFMT_N_2; i += 2) {
                do_recursion(array, i, status, i, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
            }
            for (; i < size - DSFMT_N_2; i += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                convert_c0o1(ref array[i - DSFMT_N_2 + 0]);
                convert_c0o1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (j = 0; j < (2 * DSFMT_N) * 2 - size; j += 2) {
                status[j + 0] = array[j + 0 + size - DSFMT_N_2];
                status[j + 1] = array[j + 1 + size - DSFMT_N_2];
            }
            for (; i < size; i += 2, j += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                status[j + 0] = array[i + 0];
                status[j + 1] = array[i + 1];
                convert_c0o1(ref array[i - DSFMT_N_2 + 0]);
                convert_c0o1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (i = size - DSFMT_N_2; i < size; i += 2) {
                convert_c0o1(ref array[i + 0]);
                convert_c0o1(ref array[i + 1]);
            }
            status[DSFMT_N_2 + 0] = lung0;
            status[DSFMT_N_2 + 1] = lung1;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_o0o1(uint64_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            var lung0 = status[DSFMT_N_2 + 0];
            var lung1 = status[DSFMT_N_2 + 1];
            do_recursion(array, 0, status, 0, status, DSFMT_POS1_2, ref lung0, ref lung1);
            for (i = 2; i < (DSFMT_N - DSFMT_POS1) * 2; i += 2) {
                do_recursion(array, i, status, i, status, i + DSFMT_POS1_2, ref lung0, ref lung1);
            }
            for (; i < DSFMT_N_2; i += 2) {
                do_recursion(array, i, status, i, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
            }
            for (; i < size - DSFMT_N_2; i += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                convert_o0o1(ref array[i - DSFMT_N_2 + 0]);
                convert_o0o1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (j = 0; j < (2 * DSFMT_N) * 2 - size; j += 2) {
                status[j + 0] = array[j + 0 + size - DSFMT_N_2];
                status[j + 1] = array[j + 1 + size - DSFMT_N_2];
            }
            for (; i < size; i += 2, j += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                status[j + 0] = array[i + 0];
                status[j + 1] = array[i + 1];
                convert_o0o1(ref array[i - DSFMT_N_2 + 0]);
                convert_o0o1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (i = size - DSFMT_N_2; i < size; i += 2) {
                convert_o0o1(ref array[i + 0]);
                convert_o0o1(ref array[i + 1]);
            }
            status[DSFMT_N_2 + 0] = lung0;
            status[DSFMT_N_2 + 1] = lung1;
        }

        /**
		 * This function fills the user-specified array with double precision
		 * floating point pseudorandom numbers of the IEEE 754 format.
		 * @param dsfmt dsfmt state vector.
		 * @param array an 128-bit array to be filled by pseudorandom numbers.
		 * @param size number of 128-bit pseudorandom numbers to be generated.
		 */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void gen_rand_array_o0c1(uint64_t[] array, int size)
        {
            var status = this.status;
            int i, j;
            var lung0 = status[DSFMT_N_2 + 0];
            var lung1 = status[DSFMT_N_2 + 1];
            do_recursion(array, 0, status, 0, status, DSFMT_POS1_2, ref lung0, ref lung1);
            for (i = 2; i < (DSFMT_N - DSFMT_POS1) * 2; i += 2) {
                do_recursion(array, i, status, i, status, i + DSFMT_POS1_2, ref lung0, ref lung1);
            }
            for (; i < DSFMT_N_2; i += 2) {
                do_recursion(array, i, status, i, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
            }
            for (; i < size - DSFMT_N_2; i += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                convert_o0c1(ref array[i - DSFMT_N_2 + 0]);
                convert_o0c1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (j = 0; j < (2 * DSFMT_N) * 2 - size; j += 2) {
                status[j + 0] = array[j + 0 + size - DSFMT_N_2];
                status[j + 1] = array[j + 1 + size - DSFMT_N_2];
            }
            for (; i < size; i += 2, j += 2) {
                do_recursion(array, i, array, i - DSFMT_N_2, array, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                status[j + 0] = array[i + 0];
                status[j + 1] = array[i + 1];
                convert_o0c1(ref array[i - DSFMT_N_2 + 0]);
                convert_o0c1(ref array[i - DSFMT_N_2 + 1]);
            }
            for (i = size - DSFMT_N_2; i < size; i += 2) {
                convert_o0c1(ref array[i + 0]);
                convert_o0c1(ref array[i + 1]);
            }
            status[DSFMT_N_2 + 0] = lung0;
            status[DSFMT_N_2 + 1] = lung1;
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
            tmp0 = (status[DSFMT_N_2] ^ DSFMT_FIX1);
            tmp1 = (status[DSFMT_N_2 + 1] ^ DSFMT_FIX2);

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
                status[DSFMT_N_2 + 1] ^= 1;
            }
            else {
#pragma warning disable 162
                for (int i = 1; i >= 0; i--) {
                    work = 1;
                    for (int j = 0; j < 64; j++) {
                        if ((work & pcv[i]) != 0) {
#if false
                            status[DSFMT_N].xor64((uint)i, work);
                            return;
#else
                            throw new NotImplementedException();
#endif
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return DSFMT_MEXP;
            }
        }

        int Idsfmt.DSFMT_N {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var lung0 = status[DSFMT_N_2 + 0];
            var lung1 = status[DSFMT_N_2 + 1];
            int i = 0;
            for (; i < ((DSFMT_N_2 - DSFMT_POS1_2) & ~3);) {
                i += do_recursion(status, i, 0, ref lung0, ref lung1);
            }
            for (; i < (DSFMT_N_2 & ~3);) {
                i += do_recursion(status, i, -DSFMT_N_2, ref lung0, ref lung1);
            }
            if ((DSFMT_N_2 & 2) != 0) {
                do_recursion(status, i, status, i, status, i + DSFMT_POS1_2_sub_DSFMT_N_2, ref lung0, ref lung1);
                i += 2;
            }
            status[DSFMT_N_2 + 0] = lung0;
            status[DSFMT_N_2 + 1] = lung1;
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
            var buf = new uint64_t[size];
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
            var buf = new uint64_t[size];
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
            var buf = new uint64_t[size];
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
            var buf = new uint64_t[size];
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
            uint32_t u;
            set32(status, 0, u = seed);
            for (uint i = 1; i < (DSFMT_N + 1) * 4; i++) {
                set32(status, i, u = 1812433253U * (u ^ (u >> 30)) + i);
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
            const uint size = (DSFMT_N + 1) * 4;   /* pulmonary */
            const uint lag =
                (size >= 623) ? 11 :
                (size >= 68) ? 7 :
                (size >= 39) ? 5 : 3;
            const uint mid = (size - lag) / 2;
            //memset(status, 0x8b, sizeof(status));
            for (var idx = 0; idx < (DSFMT_N + 1) * 2; ++idx) {
                status[idx] = 0x8b8b8b8b8b8b8b8bUL;
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
