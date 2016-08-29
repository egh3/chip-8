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

        public static ulong RoR_New(ulong value, int shift)
        {
            return (value >> shift) | (value << (sizeof(long) * 8 - shift));
        }

        public static bool CheckIfAnyBitCleared(ulong orignalLine, ulong newLine)
        {
            return (orignalLine & ~newLine) != 0;
        }
    }
}
