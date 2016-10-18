using System;
using System.IO;
using System.Runtime.InteropServices;
using MersenneTwister.dSFMT;
using MersenneTwister.SFMT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class dsfmt_t_Test
    {
        [TestMethod]
        public void MersenneTwister_DSFMT()
        {
            new dsfmt_t_Test<dsfmt_t>().Test();
        }

        [TestMethod]
        public void MersenneTwister_DSFMT_opt()
        {
            new dsfmt_t_Test<dsfmt_opt_t>().Test();
        }

        [TestMethod]
        public void MersenneTwister_DSFMT_opt_gen()
        {
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_521>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_1279>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_2203>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_4253>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_11213>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_19937>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_44497>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_86243>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_132049>>().Test();
            new dsfmt_t_Test<dsfmt_opt_gen_t<dsfmt_params_216091>>().Test();
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct W64_T
    {
        [FieldOffset(0)]
        public uint64_t u;
        [FieldOffset(0)]
        public double d;
    }

    public class dsfmt_t_Test<T> : TestBase where T : Idsfmt, new()
    {
        private const int NUM_RANDS = 50000;
        private int DSFMT_N { get { return gv.DSFMT_N; } }

        private delegate double genrand_t();
        private delegate double st_genrand_t(T dsfmt);
        private unsafe delegate void fill_array_t(double* array, int size);
        private unsafe delegate void st_fill_array_t(T dsfmt, double* array, int size);

        private T gv = new T();

        unsafe void check(string range_str, genrand_t genrand, fill_array_t fill_array,
                  st_genrand_t st_genrand, st_fill_array_t st_fill_array,
                  uint32_t seed, int print_size)
        {
            int i;
            w128_t* little = stackalloc w128_t[DSFMT_N + 1];
            W64_T* plittle = (W64_T*)little;
            var dummy = new W64_T[(NUM_RANDS / 2 + 1) * 2];
            fixed (W64_T* array = dummy) {
                W64_T r = new W64_T();
                W64_T r_st = new W64_T();
                int lsize = DSFMT_N * 2 + 2;

                var dsfmt = new T();

                printf("generated randoms {0}\n", range_str);
                gv.dsfmt_init_gen_rand(seed);
                fill_array(&plittle[0].d, lsize);
                fill_array(&array[0].d, 5000);
                gv.dsfmt_init_gen_rand(seed);
                dsfmt.dsfmt_init_gen_rand(seed);
                for (i = 0; i < lsize; i++) {
                    r.d = genrand();
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r.u, r_st.u);
                    Assert.AreEqual(r.u, plittle[i].u);
                    if (i < print_size) {
                        printf("{0} ", d2s(plittle[i].d));
                        if (i % 4 == 3) {
                            printf("\n");
                        }
                    }
                }
                for (i = 0; i < 5000; i++) {
                    r.d = genrand();
                    Assert.AreEqual(r.u, array[i].u);
                    if (i + lsize < print_size) {
                        printf("{0} ", d2s(array[i].d));
                        if ((i + lsize) % 4 == 3) {
                            printf("\n");
                        }
                    }
                }

                dsfmt.dsfmt_init_gen_rand(seed);
                st_fill_array(dsfmt, &plittle[0].d, lsize);
                st_fill_array(dsfmt, &array[0].d, 5000);
                dsfmt.dsfmt_init_gen_rand(seed);
                for (i = 0; i < lsize; i++) {
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r_st.u, plittle[i].u);
                }
                for (i = 0; i < 5000; i++) {
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r_st.u, array[i].u);
                }
            }
        }

        unsafe void check_ar(string range_str, genrand_t genrand,
                     fill_array_t fill_array,
                     st_genrand_t st_genrand,
                     st_fill_array_t st_fill_array,
                     int print_size)
        {
            int i;
            w128_t* little = stackalloc w128_t[DSFMT_N + 1];
            W64_T* plittle = (W64_T*)little;
            var dummy = new W64_T[(NUM_RANDS / 2 + 1) * 2];
            fixed (W64_T* array = dummy) {
                W64_T r = new W64_T();
                W64_T r_st = new W64_T();
                int lsize = DSFMT_N * 2 + 2;
                uint32_t[] ar = new uint32_t[] { 1, 2, 3, 4 };

                var dsfmt = new T();

                printf("generated randoms {0}\n", range_str);
                gv.dsfmt_init_by_array(ar);
                fill_array(&plittle[0].d, lsize);
                fill_array(&array[0].d, 5000);
                gv.dsfmt_init_by_array(ar);
                dsfmt.dsfmt_init_by_array(ar);
                for (i = 0; i < lsize; i++) {
                    r.d = genrand();
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r.u, r_st.u);
                    Assert.AreEqual(r.u, plittle[i].u);
                    if (i < print_size) {
                        printf("{0} ", d2s(plittle[i].d));
                        if (i % 4 == 3) {
                            printf("\n");
                        }
                    }
                }
                for (i = 0; i < 5000; i++) {
                    r.d = genrand();
                    Assert.AreEqual(r.u, array[i].u);
                    if (i + lsize < print_size) {
                        printf("{0} ", d2s(array[i].d));
                        if ((i + lsize) % 4 == 3) {
                            printf("\n");
                        }
                    }
                }

                dsfmt.dsfmt_init_by_array(ar);
                st_fill_array(dsfmt, &plittle[0].d, lsize);
                st_fill_array(dsfmt, &array[0].d, 5000);
                dsfmt.dsfmt_init_by_array(ar);
                for (i = 0; i < lsize; i++) {
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r_st.u, plittle[i].u);
                }
                for (i = 0; i < 5000; i++) {
                    r_st.d = st_genrand(dsfmt);
                    Assert.AreEqual(r_st.u, array[i].u);
                }
            }
        }

        double s_genrand_close_open() { return gv.dsfmt_genrand_close_open(); }
        double s_genrand_open_close() { return gv.dsfmt_genrand_open_close(); }
        double s_genrand_open_open() { return gv.dsfmt_genrand_open_open(); }
        double s_genrand_close1_open2() { return gv.dsfmt_genrand_close1_open2(); }
        double sst_genrand_close_open(T dsfmt) { return dsfmt.dsfmt_genrand_close_open(); }
        double sst_genrand_open_close(T dsfmt) { return dsfmt.dsfmt_genrand_open_close(); }
        double sst_genrand_open_open(T dsfmt) { return dsfmt.dsfmt_genrand_open_open(); }
        double sst_genrand_close1_open2(T dsfmt) { return dsfmt.dsfmt_genrand_close1_open2(); }
        unsafe void s_fill_array_close_open(double[] array, int size) { gv.dsfmt_fill_array_close_open(array, size); }
        unsafe void s_fill_array_open_close(double[] array, int size) { gv.dsfmt_fill_array_open_close(array, size); }
        unsafe void s_fill_array_open_open(double[] array, int size) { gv.dsfmt_fill_array_open_open(array, size); }
        unsafe void s_fill_array_close1_open2(double[] array, int size) { gv.dsfmt_fill_array_close1_open2(array, size); }
        unsafe void sst_fill_array_close_open(T dsfmt, double[] array, int size) { dsfmt.dsfmt_fill_array_close_open(array, size); }
        unsafe void sst_fill_array_open_close(T dsfmt, double[] array, int size) { dsfmt.dsfmt_fill_array_open_close(array, size); }
        unsafe void sst_fill_array_open_open(T dsfmt, double[] array, int size) { dsfmt.dsfmt_fill_array_open_open(array, size); }
        unsafe void sst_fill_array_close1_open2(T dsfmt, double[] array, int size) { dsfmt.dsfmt_fill_array_close1_open2(array, size); }

        static unsafe fill_array_t conv(Action<double[], int> f)
        {
            return (arr, sz) => {
                var buf = new double[sz];
                f(buf, sz);
                for (var i = 0; i < sz; ++i) {
                    arr[i] = buf[i];
                }
            };
        }

        static unsafe st_fill_array_t conv(Action<T, double[], int> f)
        {
            return (dsfmt, arr, sz) => {
                var buf = new double[sz];
                f(dsfmt, buf, sz);
                for (var i = 0; i < sz; ++i) {
                    arr[i] = buf[i];
                }
            };
        }

        int main()
        {
            printf("{0}\n", gv.dsfmt_get_idstring());
            printf("init_gen_rand(0) ");
            check("[1, 2)", s_genrand_close1_open2, conv(s_fill_array_close1_open2), sst_genrand_close1_open2, conv(sst_fill_array_close1_open2), 0, 1000);
            for (uint i = 0; i < 20; i++) {
                printf("init_gen_rand({0}) ", i);
                switch (i % 4) {
                case 0:
                    check("[0, 1)", s_genrand_close_open, conv(s_fill_array_close_open), sst_genrand_close_open, conv(sst_fill_array_close_open), i, 12);
                    break;
                case 1:
                    check("(0, 1]", s_genrand_open_close, conv(s_fill_array_open_close), sst_genrand_open_close, conv(sst_fill_array_open_close), i, 12);
                    break;
                case 2:
                    check("(0, 1)", s_genrand_open_open, conv(s_fill_array_open_open), sst_genrand_open_open, conv(sst_fill_array_open_open), i, 12);
                    break;
                case 3:
                default:
                    check("[1, 2)", s_genrand_close1_open2,
                          conv(s_fill_array_close1_open2),
                          sst_genrand_close1_open2,
                          conv(sst_fill_array_close1_open2), i, 12);
                    break;
                }
            }
            printf("init_by_array {1, 2, 3, 4} ");
            check_ar("[1, 2)", s_genrand_close1_open2,
                 conv(s_fill_array_close1_open2),
                 sst_genrand_close1_open2,
                 conv(sst_fill_array_close1_open2),
                 1000);
            return 0;
        }

        public void Test()
        {
            main();
            var mexp = gv.MEXP;
            var expected = File.ReadAllText(Path.Combine("dSFMT", $"dSFMT.{mexp}.out.txt"));
            expected = expected.Replace("\r\n", "\n");
            var actual = this.GetOutput();
            AssertResults(expected, actual, 1e-14);
        }
    }
}
