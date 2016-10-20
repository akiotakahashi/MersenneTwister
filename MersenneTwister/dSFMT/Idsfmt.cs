using System;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.dSFMT
{
    public interface Idsfmt
    {
        int MEXP { get; }
        int DSFMT_N { get; }
        string dsfmt_get_idstring();
        void dsfmt_init_gen_rand(uint32_t seed);
        void dsfmt_init_by_array(uint32_t[] init_key);
        uint32_t dsfmt_genrand_uint32();
        double dsfmt_genrand_open_open();
        double dsfmt_genrand_close_open();
        double dsfmt_genrand_open_close();
        double dsfmt_genrand_close1_open2();
        void dsfmt_fill_array_open_open(double[] array, int size);
        void dsfmt_fill_array_close_open(double[] array, int size);
        void dsfmt_fill_array_open_close(double[] array, int size);
        void dsfmt_fill_array_close1_open2(double[] array, int size);
    }
}
