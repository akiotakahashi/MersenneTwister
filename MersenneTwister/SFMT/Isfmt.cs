using System;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.SFMT
{
    public interface Isfmt
    {
        string sfmt_get_idstring();
        int sfmt_get_min_array_size32();
        int sfmt_get_min_array_size64();
        void sfmt_fill_array32(uint32_t[] array, int size);
        void sfmt_fill_array64(uint64_t[] array, int size);
        void sfmt_init_gen_rand(uint32_t seed);
        void sfmt_init_by_array(uint32_t[] init_key, uint key_length);
        void sfmt_init_by_array(uint32_t[] init_key);
        uint32_t sfmt_genrand_uint32();
        uint64_t sfmt_genrand_uint64();
    }
}
