using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MersenneTwister.MT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class mt19937ar_t_Test : TestBase
    {
        [TestMethod]
        public void MersenneTwister_MT19937ar()
        {
            var mt = new mt19937ar_t();
            TestMT19937(mt);
        }

        [TestMethod]
        public void MersenneTwister_MT19937ar_cok()
        {
            var mt = new mt19937ar_cok_t();
            TestMT19937(mt);
        }

        [TestMethod]
        public void MersenneTwister_MT19937ar_cok_opt()
        {
            var mt = new mt19937ar_cok_opt_t();
            TestMT19937(mt);
        }

        private void TestMT19937<T>(T mt) where T : Imt19937
        {
            var init = new uint[] { 0x123, 0x234, 0x345, 0x456 };
            mt.init_by_array(init, init.Length);
            printf("1000 outputs of genrand_int32()\n");
            for (var i = 0; i < 1000; i++) {
                printf("{0,10} ", mt.genrand_int32());
                if (i % 5 == 4) {
                    printf("\n");
                }
            }
            printf("\n1000 outputs of genrand_real2()\n");
            for (var i = 0; i < 1000; i++) {
                printf("{0,10} ", d2s(mt.genrand_real2(), 8));
                if (i % 5 == 4) {
                    printf("\n");
                }
            }
            //
            var expected = File.ReadAllText(Path.Combine("MT", "mt19937ar.out.txt"));
            expected = expected.Replace("\r\n", "\n");
            var actual = this.GetOutput();
            AssertResults(expected, actual, 1e-7);
        }
    }
}
