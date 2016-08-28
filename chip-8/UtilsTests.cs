using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace chip_8
{
    [TestFixture]
    class UtilsTests
    {
        [Test]
        public void TestNoClearWithOrignalSetSpriteClear()
        {
            ulong line = 0xAAAAAAAAAAAAAAAA;
            ulong sprite = 0x0000000000000000;

            Assert.IsFalse(Utils.checkForClear(line, line ^ sprite));
        }

        [Test]
        public void TestNoClearWithOrignalSetSpriteSet()
        {
            ulong line = 0xAAAAAAAAAAAAAAAA;
            ulong sprite = 0x5555555555555555;

            Assert.IsFalse(Utils.checkForClear(line, line ^ sprite));
        }

        [Test]
        public void TestClearWithOrignalSetSpriteSet()
        {
            ulong line = 0xAAAAAAAAAAAAAAAA;
            ulong sprite = 0xAA55555555555555;

            Assert.IsTrue(Utils.checkForClear(line, line ^ sprite));
        }
    }
}
