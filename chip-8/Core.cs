using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace chip_8
{
    class Core
    {
        private State _s;
        private readonly Random _rng;
        private readonly IKeyboard keyboard;
        public ulong[] _screen = new ulong[32];
        private System.Threading.Timer _SixtyHzTimer;

        public Core(State state)
        {
            _s = state;
            _rng = new Random();
            LoadFonts(0x000);
            keyboard = new MockKeyboard();
            _SixtyHzTimer = new System.Threading.Timer(Tick60Hz, null, 0, 16);
        }

        private void Tick60Hz(object state)
        {
            if(_s.DT > 0)
                _s.DT--;
        }

        public Core(State state, Random rng)
        {
            _s = state;
            _rng = rng;
            LoadFonts(0x000);
            keyboard = new MockKeyboard();
        }

        public void ExecuteCycle()
        {
            var newInstruction = (ushort)(_s.memory[_s.PC++] << 8 | _s.memory[_s.PC++]);
            var instruction = new OpCode(newInstruction);
            ushort opCode = (ushort)((newInstruction & 0xF000) >> 12);

            switch(instruction.opcode)
            {
                case 0x0:
                    if(instruction.kk == 0xE0) //CLS
                    {
                        for(int i = 0; i < _screen.Length; i++)
                        {
                            _screen[i] = 0;
                        }
                    }
                    else if(instruction.kk == 0xEE) //RET
                    {
                        _s.PC = popStack();
                    }
                    break;
                case 0x1:
                    _s.PC = instruction.nnn;
                    break;
                case 0x2:
                    pushStack(_s.PC);
                    _s.PC = instruction.nnn;
                    break;
                case 0x3:
                    {
                        var x = _s.V[instruction.x];
                        var param = instruction.kk;
                        if (x == param) _s.PC += 2;
                    }
                    break;
                case 0x4:
                    {
                        var x = _s.V[instruction.x];
                        var param = instruction.kk;
                        if (x != param) _s.PC += 2;
                    }
                    break;
                case 0x5:
                    {
                        var x = _s.V[instruction.x];
                        var y = _s.V[instruction.y];
                        if (x == y) _s.PC += 2;
                    }
                    break;
                case 0x6:
                    {
                        _s.V[instruction.x] = instruction.kk;
                    }
                    break;
                case 0x7:
                    {
                        _s.V[instruction.x] += instruction.kk;
                    }
                    break;
                case 0x8:
                    {
                        switch(instruction.op)
                        {
                            case 0x0:
                                {
                                    _s.V[instruction.x] = _s.V[instruction.y];
                                }
                                break;
                            case 0x1:
                                {
                                    _s.V[instruction.x] = (byte)(_s.V[instruction.x] | _s.V[instruction.y]);
                                }
                                break;
                            case 0x2:
                                {
                                    _s.V[instruction.x] = (byte)(_s.V[instruction.x] & _s.V[instruction.y]);
                                }
                                break;
                            case 0x3:
                                {
                                    _s.V[instruction.x] = (byte)(_s.V[instruction.x] ^ _s.V[instruction.y]);
                                }
                                break;
                            case 0x4:
                                {
                                    _s.V[0xF] = 0;
                                    int temp = _s.V[instruction.x] + _s.V[instruction.y];
                                    if(temp > 255)
                                    {
                                        _s.V[0xF] = 1;
                                    }
                                    _s.V[instruction.x] = (byte)temp;
                                }
                                break;
                            case 0x5: //BUG?
                                {
                                    _s.V[0xF] = 0;
                                    if(_s.V[instruction.x] > _s.V[instruction.y]) _s.V[0xF] = 1;
                                    _s.V[instruction.x] = (byte)(_s.V[instruction.x] - _s.V[instruction.y]);
                                }
                                break;
                            case 0x6:
                                _s.V[0xF] = (byte)((_s.V[instruction.x] & 0x1) != 0 ? 1 : 0);
                                _s.V[instruction.x] /= 2;
                                break;
                            case 0x7:
                                {
                                    _s.V[0xF] = 0;
                                    if (_s.V[instruction.y] > _s.V[instruction.x]) _s.V[0xF] = 1;
                                    _s.V[instruction.x] = (byte)(_s.V[instruction.y] - _s.V[instruction.x]);
                                }
                                break;
                            case 0xE:
                                _s.V[0xF] = (byte)((_s.V[instruction.x] & 0x80) != 0 ? 1 : 0);
                                _s.V[instruction.x] *= 2;
                                break;
                        }
                    }
                    break;
                case 0x9:
                    {
                        if(_s.V[instruction.x] != _s.V[instruction.y])
                        {
                            _s.PC += 2;
                        }
                    }
                    break;
                case 0xA:
                    _s.I = instruction.nnn;
                    break;
                case 0xB:
                    _s.PC = (ushort)(instruction.nnn + _s.V[0]);
                    break;
                case 0xC:
                    var ba = new byte[1];
                    _rng.NextBytes(ba);
                    _s.V[instruction.x] = (byte)(ba[0] & instruction.kk);
                    break;
                case 0xD:
                    _s.V[0xF] = 0;
                    for(var i = 0; i<instruction.op; i++)
                    {
                        ulong orignalLine = _screen[(_s.V[instruction.y] + i) % 32];
                        var sprite = (ulong)(_s.memory[_s.I + i]) << (64 - 8);
                        var newLine = Utils.RoR(sprite, _s.V[instruction.x]);
                        _screen[(_s.V[instruction.y] + i) % 32] ^= newLine;
                        if(Utils.CheckIfAnyBitCleared(orignalLine, _screen[(_s.V[instruction.y] + 1) % 32]))
                            _s.V[0xF] = 1;
                    }
                    break;
                case 0xE:
                    switch(instruction.kk)
                    {
                        case 0x9E:
                            if(keyboard.ReadKey().HasFlag(Keys.Key_4))
                            {
                                _s.PC += 2;
                            }
                            break;
                    }
                    break;
                case 0xF:
                    {
                        switch(instruction.kk)
                        {
                            case 0x07:
                                _s.V[instruction.x] = _s.DT;
                                break;
                            case 0x15:
                                _s.DT = _s.V[instruction.x];
                                break;
                            case 0x18:
                                _s.ST = _s.V[instruction.x];
                                break;
                            case 0x29:
                                _s.I = (ushort)(_s.V[instruction.x] * 5);
                                break;
                            case 0x1E:
                                _s.I += _s.V[instruction.x];
                                break;
                            case 0x33:
                                var temp = _s.V[instruction.x];
                                _s.memory[_s.I] = (byte)(temp/100);
                                temp %= 100;
                                _s.memory[_s.I+1] = (byte)(temp/10);
                                temp %= 10;
                                _s.memory[_s.I+2] = temp;
                                break;
                            case 0x55:
                                Array.Copy(_s.V, 0, _s.memory, _s.I, _s.V[instruction.x]);
                                break;
                            case 0x65:
                                Array.Copy(_s.memory, _s.I, _s.V, 0, _s.V[instruction.x]);
                                break;
                            case 0xFF:
                                throw new Exception("Invalid Opcode");
                                break;
                        }
                    }
                    break;
                default:
                    throw new Exception("Invalid Opcode");
            }
        }

        public void Load(byte[] code)
        {
            Array.Copy(code, 0, _s.memory, 0x200, code.Length);
        }

        private void pushStack(ushort item)
        {
            _s.stack[_s.SP++] = item;
        }

        private ushort popStack()
        {
            return _s.stack[--_s.SP];
        }

        private void LoadFonts(ushort baseAddress)
        {
            ushort startload = baseAddress;
            for(var i = 0; i <= 0xF; i++)
            {
                for(var j = 0; j < 5; j++)
                {
                    _s.memory[startload++] = Font.fonts[i, j];
                }
            }
        }
    }
}
