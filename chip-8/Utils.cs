using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class Utils
    {
        public static ulong RoR(ulong value, int shift)
        {
            ulong mask = 0xFFFFFFFFFFFFFFFF >> (64 - shift);
            var shiftv = (value & mask) << (64 - shift);
            var ror = (value >> shift) | shiftv;

            return ror;
        }

        public static bool checkForClear(ulong orignalLine, ulong newLine)
        {
            return (orignalLine & ~newLine) != 0;
        }
    }
}
