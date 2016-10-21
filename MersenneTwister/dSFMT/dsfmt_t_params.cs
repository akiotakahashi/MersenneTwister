#define DSFMT_MEXP_19937
using System;

namespace MersenneTwister.dSFMT
{
#if PUBLIC
    public
#else
    internal
#endif
    abstract class dsfmt_t_params : SFMT.sfmt_base_t
    {
        protected const ulong DSFMT_LOW_MASK = 0x000FFFFFFFFFFFFFUL;
        protected const ulong DSFMT_HIGH_CONST = 0x3FF0000000000000UL;
        protected const int DSFMT_SR = 12;

        /** DSFMT generator has an internal state array of 128-bit integers,
		 * and N is its size. */
        protected const int DSFMT_N = ((DSFMT_MEXP - 128) / 104 + 1);
        /** N32 is the size of internal state array when regarded as an array
		 * of 32-bit integers.*/
        protected const int DSFMT_N32 = (DSFMT_N * 4);
        /** N64 is the size of internal state array when regarded as an array
		 * of 64-bit integers.*/
        protected const int DSFMT_N64 = (DSFMT_N * 2);

#if DSFMT_MEXP_19937
        /* Mersenne Exponent. The period of the sequence
		 *  is a multiple of 2^DSFMT_MEXP-1. */
        protected const int DSFMT_MEXP = 19937;
        protected const int DSFMT_POS1 = 117;
        protected const int DSFMT_SL1 = 19;
        protected const ulong DSFMT_MSK1 = 0x000ffafffffffb3fUL;
        protected const ulong DSFMT_MSK2 = 0x000ffdfffc90fffdUL;
        protected const uint DSFMT_MSK32_1 = 0x000ffaffU;
        protected const uint DSFMT_MSK32_2 = 0xfffffb3fU;
        protected const uint DSFMT_MSK32_3 = 0x000ffdffU;
        protected const uint DSFMT_MSK32_4 = 0xfc90fffdU;
        protected const ulong DSFMT_FIX1 = 0x90014964b32f4329UL;
        protected const ulong DSFMT_FIX2 = 0x3b8d12ac548a7c7aUL;
        protected const ulong DSFMT_PCV1 = 0x3d84e1ac0dc82880UL;
        protected const ulong DSFMT_PCV2 = 0x0000000000000001UL;
        protected const string DSFMT_IDSTR = "dSFMT2-19937:117-19:ffafffffffb3f-ffdfffc90fffd";
#else
#error "DSFMT_MEXP is not valid."
#endif
    }
}
