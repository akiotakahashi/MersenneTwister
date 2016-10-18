#define SFMT_MEXP_19937
using System;

namespace MersenneTwister.SFMT
{
    public abstract class sfmt_t_params : sfmt_base_t
    {
        /** SFMT generator has an internal state array of 128-bit integers,
		 * and N is its size. */
        protected const int SFMT_N = (SFMT_MEXP / 128 + 1);

        /** N32 is the size of internal state array when regarded as an array
		 * of 32-bit integers.*/
        protected const int SFMT_N32 = (SFMT_N * 4);

        /** N64 is the size of internal state array when regarded as an array
		 * of 64-bit integers.*/
        protected const int SFMT_N64 = (SFMT_N * 2);

        /*----------------------
		  the parameters of SFMT
		  ----------------------*/

#if SFMT_MEXP_19937
        protected const int SFMT_MEXP = 19937;
        protected const int SFMT_POS1 = 122;
        protected const int SFMT_SL1 = 18;
        protected const int SFMT_SL2 = 1;
        protected const int SFMT_SR1 = 11;
        protected const int SFMT_SR2 = 1;
        protected const uint SFMT_MSK1 = 0xdfffffefU;
        protected const uint SFMT_MSK2 = 0xddfecb7fU;
        protected const uint SFMT_MSK3 = 0xbffaffffU;
        protected const uint SFMT_MSK4 = 0xbffffff6U;
        protected const uint SFMT_PARITY1 = 0x00000001U;
        protected const uint SFMT_PARITY2 = 0x00000000U;
        protected const uint SFMT_PARITY3 = 0x00000000U;
        protected const uint SFMT_PARITY4 = 0x13c9e684U;
        protected const string SFMT_IDSTR = "SFMT-19937:122-18-1-11-1:dfffffef-ddfecb7f-bffaffff-bffffff6";
#else
#error "SFMT_MEXP is not valid."
#endif
    }
}
