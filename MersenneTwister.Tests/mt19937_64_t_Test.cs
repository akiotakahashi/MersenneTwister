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
    public class mt19937_64_t_Test : TestBase
    {
        [TestMethod]
        public void MersenneTwister_MT19937_64()
        {
            Test(new mt19937_64_t());
        }

        private void Test<T>(T mt) where T : Imt19937_64
        {
            var init = new[] { 0x12345UL, 0x23456UL, 0x34567UL, 0x45678UL };
            mt.init_by_array64(init, (uint)init.Length);
            printf("1000 outputs of genrand64_int64()\n");
            for (var i = 0; i < 1000; i++) {
                printf("{0,20} ", mt.genrand64_int64());
                if (i % 5 == 4) {
                    printf("\n");
                }
            }
            printf("\n1000 outputs of genrand64_real2()\n");
            for (var i = 0; i < 1000; i++) {
                printf("{0,10} ", d2s(mt.genrand64_real2(), 8));
                if (i % 5 == 4) {
                    printf("\n");
                }
            }
            //
            var expected = File.ReadAllText(Path.Combine("MT", "mt19937-64.out.txt"));
            expected = expected.Replace("\r\n", "\n");
            var actual = this.GetOutput();
            AssertResults(expected, actual, 1e-7);
        }
    }
}
