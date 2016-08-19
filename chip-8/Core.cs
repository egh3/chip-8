using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class Core
    {
        private State _s;
        private readonly Random _rng;

        public Core(State state)
        {
            _s = state;
            _rng = new Random();
        }

        public Core(State state, Random rng)
        {
            _s = state;
            _rng = rng;
        }

        public void ExecuteCycle()
        {
            var newInstruction = (ushort)(_s.memory[_s.PC++] << 8 | _s.memory[_s.PC++]);
            var instruction = new OpCode(newInstruction);
            ushort opCode = (ushort)((newInstruction & 0xF000) >> 12);

            switch(instruction.opcode)
            {
                case 0x1:
                    _s.PC = instruction.nnn;
                    break;
                case 0x2:
                    pushStack(_s.PC);
                    _s.PC = instruction.nnn;
                    break;
                case 0x3:
                    {
                        byte xIndex = instruction.x;
                        var x = _s.V[xIndex];
                        var param = instruction.kk;
                        if (x == param) _s.PC += 2;
                    }
                    break;
                case 0x4:
                    {
                        byte xIndex = instruction.x;
                        var x = _s.V[xIndex];
                        var param = instruction.kk;
                        if (x != param) _s.PC += 2;
                    }
                    break;
                case 0x5:
                    {
                        byte xIndex = instruction.x;
                        byte yIndex = instruction.y;
                        var x = _s.V[xIndex];
                        var y = _s.V[yIndex];
                        if (x == y) _s.PC += 2;
                    }
                    break;
                case 0x6:
                    {
                        byte xIndex = instruction.x;
                        var param = instruction.kk;
                        _s.V[xIndex] = param;
                    }
                    break;
                case 0x7:
                    {
                        byte xIndex = instruction.x;
                        var param = instruction.kk;
                        _s.V[xIndex] += param;
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
                    break;
                case 0xE:
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
            _s.memory[_s.SP--] = (byte)(item >> 8);
            _s.memory[_s.SP--] = (byte)(item & 0x00FF);
        }

        private void popStack()
        {

        }
    }
}
