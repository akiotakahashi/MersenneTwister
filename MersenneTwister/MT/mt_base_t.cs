using System;

namespace MersenneTwister.MT
{
#if PUBLIC
    public
#else
    internal
#endif
    abstract class mt_base_t
    {
        static mt_base_t()
        {
            if (!BitConverter.IsLittleEndian) {
                throw new PlatformNotSupportedException("MersenneTwister does not support Big Endian platforms");
            }
        }
    }
}
