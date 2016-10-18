using System;
using System.IO;
using System.Linq;
using System.Text;
using MersenneTwister.SFMT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class sfmt_t_Test : TestBase
    {
        /**
		 * @file  test.c
		 * @brief test program for 32-bit and 64-bit output of SFMT.
		 *
		 * @author Mutsuo Saito (Hiroshima-univ)
		 *
		 * Copyright (C) 2012 Mutsuo Saito, Makoto Matsumoto, Hiroshima
		 * University and The University of Tokyo.
		 * All rights reserved.
		 *
		 * The new BSD License is applied to this software, see LICENSE.txt
		 */

        const int BLOCK_SIZE = 100000;
        const int BLOCK_SIZE64 = 50000;
        const int COUNT = 1000;

        w128_t[] array1 = new w128_t[BLOCK_SIZE / 4];
        w128_t[] array2 = new w128_t[10000 / 4];

        unsafe void check32<T>(T sfmt) where T : Isfmt
        {
            var array32 = new uint32_t[10000];
            var array32_2 = new uint32_t[10000];

            uint32_t r32;
            uint32_t[] ini = new uint32_t[] { 0x1234, 0x5678, 0x9abc, 0xdef0 };

            Assert.IsFalse(sfmt.sfmt_get_min_array_size32() > 10000);

            printf("{0}\n32 bit generated randoms\n", sfmt.sfmt_get_idstring());
            printf("init_gen_rand__________\n");
            /* 32 bit generation */
            sfmt.sfmt_init_gen_rand(1234);
            sfmt.sfmt_fill_array32(array32, 10000);
            sfmt.sfmt_fill_array32(array32_2, 10000);
            sfmt.sfmt_init_gen_rand(1234);
            for (var i = 0; i < 10000; i++) {
                if (i < 1000) {
                    printf("{0,10} ", array32[i]);
                    if (i % 5 == 4) {
                        printf("\n");
                    }
                }
                r32 = sfmt.sfmt_genrand_uint32();
                Assert.AreEqual(array32[i], r32);
            }
            for (var i = 0; i < 700; i++) {
                r32 = sfmt.sfmt_genrand_uint32();
                Assert.AreEqual(array32_2[i], r32);
            }
            printf("\n");
            sfmt.sfmt_init_by_array(ini);
            printf("init_by_array__________\n");
            sfmt.sfmt_fill_array32(array32, 10000);
            sfmt.sfmt_fill_array32(array32_2, 10000);
            sfmt.sfmt_init_by_array(ini);
            for (var i = 0; i < 10000; i++) {
                if (i < 1000) {
                    printf("{0,10} ", array32[i]);
                    if (i % 5 == 4) {
                        printf("\n");
                    }
                }
                r32 = sfmt.sfmt_genrand_uint32();
                Assert.AreEqual(array32[i], r32);
            }
            for (var i = 0; i < 700; i++) {
                r32 = sfmt.sfmt_genrand_uint32();
                Assert.AreEqual(array32_2[i], r32);
            }
        }

        unsafe void check64<T>(T sfmt) where T : Isfmt
        {
            var array64 = new uint64_t[5000];
            var array64_2 = new uint64_t[5000];

            uint64_t r64;
            uint32_t[] ini = new uint32_t[] { 5, 4, 3, 2, 1 };

            Assert.IsFalse(sfmt.sfmt_get_min_array_size64() > 5000);

            printf("%s\n64 bit generated randoms\n", sfmt.sfmt_get_idstring());
            printf("init_gen_rand__________\n");
            /* 64 bit generation */
            sfmt.sfmt_init_gen_rand(4321);
            sfmt.sfmt_fill_array64(array64, 5000);
            sfmt.sfmt_fill_array64(array64_2, 5000);
            sfmt.sfmt_init_gen_rand(4321);
            for (var i = 0; i < 5000; i++) {
                if (i < 1000) {
                    printf("{0:X16}", array64[i]);
                    if (i % 3 == 2) {
                        printf("\n");
                    }
                }
                r64 = sfmt.sfmt_genrand_uint64();
                Assert.AreEqual(array64[i], r64);
            }
            printf("\n");
            for (var i = 0; i < 700; i++) {
                r64 = sfmt.sfmt_genrand_uint64();
                Assert.AreEqual(array64_2[i], r64);
            }
            printf("init_by_array__________\n");
            /* 64 bit generation */
            sfmt.sfmt_init_by_array(ini);
            sfmt.sfmt_fill_array64(array64, 5000);
            sfmt.sfmt_fill_array64(array64_2, 5000);
            sfmt.sfmt_init_by_array(ini);
            for (var i = 0; i < 5000; i++) {
                if (i < 1000) {
                    printf("{0:X16}", array64[i]);
                    if (i % 3 == 2) {
                        printf("\n");
                    }
                }
                r64 = sfmt.sfmt_genrand_uint64();
                Assert.AreEqual(array64[i], r64);
            }
            printf("\n");
            for (var i = 0; i < 700; i++) {
                r64 = sfmt.sfmt_genrand_uint64();
                Assert.AreEqual(array64_2[i], r64);
            }
        }

        private void MersenneTwister_SFMT_32<T>(T sfmt) where T : Isfmt
        {
            check32(sfmt);
            var expected = File.ReadAllText(Path.Combine("SFMT", "SFMT.19937.out.txt"));
            expected = expected.Replace("\r\n", "\n");
            var actual = this.GetOutput();
            AssertResults(expected, actual, 1e-14);
        }

        private void MersenneTwister_SFMT_64<T>(T sfmt) where T : Isfmt
        {
            check64(sfmt);
        }

        [TestMethod]
        public void MersenneTwister_SFMT_32()
        {
            MersenneTwister_SFMT_32(new sfmt_t());
        }

        [TestMethod]
        public void MersenneTwister_SFMT_64()
        {
            MersenneTwister_SFMT_64(new sfmt_t());
        }

        [TestMethod]
        public void MersenneTwister_SFMT_opt_32() 
        {
            MersenneTwister_SFMT_32(new sfmt_opt_t());
        }

        [TestMethod]
        public void MersenneTwister_SFMT_opt_64() 
        {
            MersenneTwister_SFMT_64(new sfmt_opt_t());
        }
    }
}
