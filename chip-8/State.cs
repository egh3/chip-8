using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class State
    {
        public byte[] V = new byte[16];
        public byte[] Y = new byte[16];
        public ushort I;
        public ushort PC = 0x200;
        public ushort SP;
        public byte DT;
        public byte ST;

        public byte[] memory = new byte[0x1000];

        public State()
        {
            Reset();
        }

        public void Reset()
        {
            PC = 0x200;
            SP = 0x100;
            I = 0;
        }
    }
}
