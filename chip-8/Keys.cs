using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    [Flags]
    enum Keys
    {
        None,
        Key_0 = 0x00, Key_1 = 0x01, Key_2 = 0x02, Key_3 = 0x03, Key_4 = 0x04, Key_5 = 0x05, Key_6 = 0x06, Key_7 = 0x7,
        Key_8 = 0x08, Key_9 = 0x09, Key_A = 0x0A, Key_B = 0x0B, Key_C = 0x0C, Key_D = 0x0D, Key_E = 0x0E, Key_F = 0x0F
    }
}
