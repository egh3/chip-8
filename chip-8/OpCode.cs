using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class OpCode
    {
        private ushort _opcode;
        public OpCode(ushort opcode)
        {
            _opcode = opcode;
        }

        public void setOpCode(ushort opcode)
        {
            _opcode = opcode;
        }

        public byte opcode
        {
            get { return (byte)((_opcode & 0xF000) >> 12); }
        }

        public ushort nnn
        {
            get { return (ushort)(_opcode & 0x0FFF); }
        }

        public byte kk
        {
            get { return (byte)(_opcode & 0x00FF); }
        }

        public byte x
        {
            get { return (byte)((_opcode & 0x0F00) >> 8); }
        }

        public byte y
        {
            get { return (byte)((_opcode & 0x00F0) >> 4 ); }
        }

        public byte op
        {
            get { return (byte)(_opcode & 0x000F); }
        }
    }
}
