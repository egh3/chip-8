using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace chip_8
{
    [TestFixture]
    class CoreInstructionTests
    {
        [SetUp]
        public void Setup()
        {

        }

        //00E0 - CLS
        //Clear the display.
        [Test]
        public void TestCLS()
        {
            Assert.Fail();
        }

        //00EE - RET
        //Return from a subroutine.
        //
        //The interpreter sets the program counter to the address at the top of the stack,
        //then subtracts 1 from the stack pointer.
        [Test]
        public void TestRET()
        {
            State state = new State();
            Core test = new Core(state);
            state.stack[state.SP] = 0x08FF;
            state.SP += 1;
            var oldSP = state.SP;
            test.Load(new byte[] { 0x00, 0xEE });
            test.ExecuteCycle();

            Assert.AreEqual(0x8FF, state.PC);
            Assert.AreEqual(oldSP - 1, state.SP);
        }

        // Jump to address nnn
        [Test]
        public void Test1nnn()
        {
            State state = new State();
            Core test = new Core(state);
            test.Load(new byte[] { 0x1F, 0xF8 });
            test.ExecuteCycle();

            Assert.AreEqual(state.PC, 0x0FF8);
        }

        // 2nnn - CALL addr
        [Test]
        public void Test2nnn()
        {
            State state = new State();
            Core test = new Core(state);
            test.Load(new byte[] { 0x21, 0x23 });
            var oldSP = state.SP;
            test.ExecuteCycle();

            Assert.AreEqual(state.PC, 0x0123);
            Assert.AreEqual(1, state.SP);
        }

        //3xkk - SE Vx, byte
        //Skip next instruction if Vx = kk.
        //
        //The interpreter compares register Vx to kk, and if they are equal, increments the program counter by 2.
        [Test]
        public void Test3xkk()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[1] = 0xAA;
            test.Load(new byte[] { 0x31, 0xAA });
            var oldPC = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC + 4, state.PC);

            state.V[1] = 0x00;
            test.Load(new byte[] { 0x31, 0xAA });
            state.Reset();
            var oldPC2 = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC2 + 2, state.PC);
        }

        //4xkk - SNE Vx, byte
        //Skip next instruction if Vx != kk.
        //
        //The interpreter compares register Vx to kk, and if they are not equal, increments the program counter by 2.
        [Test]
        public void Test4xkk()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[1] = 0;
            test.Load(new byte[] { 0x41, 0x23 });
            var oldPC = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC + 4, state.PC);

            state.Reset();
            state.V[1] = 0x23;
            var oldPC2 = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC2 + 2, state.PC);
        }

        //5xy0 - SE Vx, Vy
        //Skip next instruction if Vx = Vy.
        //
        //The interpreter compares register Vx to register Vy, and if they are equal, increments the program counter by 2.
        [Test]
        public void Test5xkk()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0xAA;
            state.V[6] = 0xAA;
            test.Load(new byte[] { 0x53, 0x60 });
            var oldPC = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC + 4, state.PC);
        }

        //6xkk - LD Vx, byte
        //Set Vx = kk.
        //
        //The interpreter puts the value kk into register Vx.
        [Test]
        public void Test6xkk()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[0xA] = 0x00;
            test.Load(new byte[] { 0x6A, 0xAA });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0xA], 0xAA);
        }

        //7xkk - ADD Vx, byte
        //Set Vx = Vx + kk.
        //
        //Adds the value kk to the value of register Vx, then stores the result in Vx.
        [Test]
        public void Test7xkk()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 2;
            test.Load(new byte[] { 0x73, 0x02 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 4);
        }

        //8xy0 - LD Vx, Vy
        //Set Vx = Vy.

        //Stores the value of register Vy in register Vx.
        [Test]
        public void Test8xy0()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0;
            state.V[6] = 2;
            test.Load(new byte[] { 0x83, 0x60 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 2);
        }

        //8xy1 - OR Vx, Vy
        //Set Vx = Vx OR Vy.

        //Performs a bitwise OR on the values of Vx and Vy, then stores the result
        //in Vx. A bitwise OR compares the corrseponding bits from two values, and
        //if either bit is 1, then the same bit in the result is also 1.
        //Otherwise, it is 0. 
        [Test]
        public void Test8xy1()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0x01;
            state.V[6] = 0x01;
            test.Load(new byte[] { 0x83, 0x61 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01 | 0x01);

            state.Reset();
            state.V[3] = 0x01;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01 | 0x00);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x01;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 | 0x01);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 | 0x00);
        }

        //8xy2 - AND Vx, Vy
        //Set Vx = Vx AND Vy.
        //
        //Performs a bitwise AND on the values of Vx and Vy, then stores the result in Vx.
        //A bitwise AND compares the corrseponding bits from two values, and if both bits
        //are 1, then the same bit in the result is also 1. Otherwise, it is 0. 
        [Test]
        public void Test8xy2()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0x01;
            state.V[6] = 0x01;
            test.Load(new byte[] { 0x83, 0x62 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01 & 0x01);

            state.Reset();
            state.V[3] = 0x01;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01 & 0x00);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x01;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 & 0x01);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 & 0x00);
        }

        //8xy3 - XOR Vx, Vy
        //Set Vx = Vx XOR Vy.
        //
        //Performs a bitwise exclusive OR on the values of Vx and Vy, then stores the result in Vx.
        //An exclusive OR compares the corrseponding bits from two values, and if the bits are not
        //both the same, then the corresponding bit in the result is set to 1. Otherwise, it is 0. 
        [Test]
        public void Test8xy3()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0xF0;
            state.V[6] = 0x0F;
            test.Load(new byte[] { 0x83, 0x63 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0xF0 ^ 0x0F);

            state.Reset();
            state.V[3] = 0x01;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01 ^ 0x00);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x01;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 ^ 0x01);

            state.Reset();
            state.V[3] = 0x00;
            state.V[6] = 0x00;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00 ^ 0x00);
        }

        //8xy4 - ADD Vx, Vy
        //Set Vx = Vx + Vy, set VF = carry.
        //
        //The values of Vx and Vy are added together.If the result is greater than 8 bits(i.e., > 255,)
        //VF is set to 1, otherwise 0. Only the lowest 8 bits of the result are kept, and stored in Vx.
        [Test]
        public void Test8xy4()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0xFF;
            state.V[6] = 0x01;
            test.Load(new byte[] { 0x83, 0x64 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x0);
            Assert.AreEqual(state.V[0xF], 1);

            state.Reset();

            state.V[3] = 0x02;
            state.V[6] = 0x02;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x04);
            Assert.AreEqual(state.V[0xF], 0);
        }

        //8xy5 - SUB Vx, Vy
        //Set Vx = Vx - Vy, set VF = NOT borrow.
        //
        //If Vx > Vy, then VF is set to 1, otherwise 0. Then Vy is subtracted from Vx, and the results
        //stored in Vx
        [Test]
        public void Test8xy5()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0x01;
            state.V[6] = 0x02;
            test.Load(new byte[] { 0x83, 0x65 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0xFF);
            Assert.AreEqual(state.V[0xF], 0);

            state.Reset();

            state.V[3] = 0x02;
            state.V[6] = 0x02;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x00);
            Assert.AreEqual(state.V[0xF], 0);
        }

        //8xy6 - SHR Vx {, Vy}
        //Set Vx = Vx SHR 1.
        //
        //If the least-significant bit of Vx is 1, then VF is set to 1, otherwise 0. Then Vx is divided by 2.
        [Test]
        public void Test8xy6()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0x05;
            test.Load(new byte[] { 0x83, 0x66 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x2);
            Assert.AreEqual(state.V[0xF], 1);

            state.Reset();
            state.V[3] = 0x08;
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x4);
            Assert.AreEqual(state.V[0xF], 0);
        }

        //8xy7 - SUBN Vx, Vy
        //Set Vx = Vy - Vx, set VF = NOT borrow.
        //
        //If Vy > Vx, then VF is set to 1, otherwise 0. Then Vx is subtracted from Vy, and the results stored in Vx.
        [Test]
        public void Test8xy7()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[3] = 0x01;
            state.V[6] = 0x02;
            test.Load(new byte[] { 0x83, 0x67 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0x01);
            Assert.AreEqual(state.V[0xF], 1);

            state.Reset();
        }

        //8xyE - SHL Vx {, Vy }
        //Set Vx = Vx SHL 1.
        //
        //If the most-significant bit of Vx is 1, then VF is set to 1, otherwise to 0. Then Vx is multiplied by 2.
        [Test]
        public void Test8xyE()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[0xF] = 0;
            state.V[3] = 0x85;

            test.Load(new byte[] { 0x83, 0x6E });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[3], 10);
            Assert.AreEqual(state.V[0xF], 1);
        }

        //9xy0 - SNE Vx, Vy
        //Skip next instruction if Vx != Vy.
        //
        //The values of Vx and Vy are compared, and if they are not equal, the program counter is increased by 2.
        [Test]
        public void Test9xy0()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[0x3] = 10;
            state.V[0x6] = 10;

            test.Load(new byte[] { 0x93, 0x60 });
            var oldPC = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC + 2, state.PC);

            state.Reset();

            state.V[0x3] = 10;
            state.V[0x6] = 20;
            var oldPC2 = state.PC;
            test.ExecuteCycle();

            Assert.AreEqual(oldPC2 + 4, state.PC);
        }

        //Annn - LD I, addr
        //Set I = nnn.
        //
        //The value of register I is set to nnn.
        [Test]
        public void TestAnnn()
        {
            State state = new State();
            Core test = new Core(state);
            test.Load(new byte[] { 0xA3, 0x45 });
            state.I = 100;
            test.ExecuteCycle();

            Assert.AreEqual(state.I, 0x345);
        }

        //Bnnn - JP V0, addr
        //Jump to location nnn + V0.
        //
        //The program counter is set to nnn plus the value of V0.
        [Test]
        public void TestBnnn()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[0] = 8;
            test.Load(new byte[] { 0xB1, 0x04 });
            test.ExecuteCycle();

            Assert.AreEqual(0x104 + 8, state.PC);
        }

        //Cxkk - RND Vx, byte
        //Set Vx = random byte AND kk.
        //
        //The interpreter generates a random number from 0 to 255, which is then ANDed with the value kk.
        //The results are stored in Vx. See instruction 8xy2 for more information on AND.
        [Test]
        public void TestCxkk()
        {
            State state = new State();
            Core test = new Core(state, new Random(1));

            test.Load(new byte[] { 0xC1, 0xAA });
            test.ExecuteCycle();

            Assert.AreEqual(0xAA & 70, state.V[1]);
        }

        //Dxyn - DRW Vx, Vy, nibble
        //Display n-byte sprite starting at memory location I at(Vx, Vy), set VF = collision.
        //
        //The interpreter reads n bytes from memory, starting at the address stored in I.
        //These bytes are then displayed as sprites on screen at coordinates(Vx, Vy).
        //Sprites are XORed onto the existing screen.If this causes any pixels to be erased, VF is set to 1, 
        //otherwise it is set to 0. If the sprite is positioned so part of it is outside the coordinates of
        //the display, it wraps around to the opposite side of the screen.
        //See instruction 8xy3 for more information on XOR, and section 2.4, Display, for more information
        //on the Chip-8 screen and sprites.
        [Test]
        public void TestDxyn()
        {
            Assert.Fail();
        }

        //Ex9E - SKP Vx
        //Skip next instruction if key with the value of Vx is pressed.
        //
        //Checks the keyboard, and if the key corresponding to the value of Vx is currently in the
        //down position, PC is increased by 2.
        [Test]
        public void TestEx9E()
        {
            Assert.Fail();
        }

        //ExA1 - SKNP Vx
        //Skip next instruction if key with the value of Vx is not pressed.
        //
        //Checks the keyboard, and if the key corresponding to the value of Vx is currently in the
        //up position, PC is increased by 2.
        [Test]
        public void TestExA1()
        {
            Assert.Fail();
        }

        //Fx07 - LD Vx, DT
        //Set Vx = delay timer value.
        //
        //The value of DT is placed into Vx.
        [Test]
        public void TestFx07()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[7] = 0;
            state.DT = 70;

            test.Load(new byte[] { 0xF7, 0x07 });
            test.ExecuteCycle();

            Assert.AreEqual(70, state.V[7]);
        }

        //Fx0A - LD Vx, K
        //Wait for a key press, store the value of the key in Vx.
        //
        //All execution stops until a key is pressed, then the value of that key is stored in Vx.
        [Test]
        public void TestFx0A()
        {
            Assert.Fail();
        }

        //Fx15 - LD DT, Vx
        //Set delay timer = Vx.
        //
        //DT is set equal to the value of Vx.
        [Test]
        public void TestFx15()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[7] = 40;
            state.DT = 0;

            test.Load(new byte[] { 0xF7, 0x15 });
            test.ExecuteCycle();

            Assert.AreEqual(40, state.DT);
        }

        //Fx18 - LD ST, Vx
        //Set sound timer = Vx.
        //
        //ST is set equal to the value of Vx.
        [Test]
        public void TestFx18()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[7] = 40;
            state.DT = 0;

            test.Load(new byte[] { 0xF7, 0x18 });
            test.ExecuteCycle();

            Assert.AreEqual(40, state.ST);
        }

        //Fx1E - ADD I, Vx
        //Set I = I + Vx.
        //
        //The values of I and Vx are added, and the results are stored in I.
        [Test]
        public void TestFx1E()
        {
            State state = new State();
            Core test = new Core(state);
            state.I = 2;
            state.V[8] = 2;

            test.Load(new byte[] { 0xF8, 0x1E });
            test.ExecuteCycle();

            Assert.AreEqual(4, state.I);
        }

        //Fx29 - LD F, Vx
        //Set I = location of sprite for digit Vx.
        //
        //The value of I is set to the location for the hexadecimal sprite 
        //corresponding to the value of Vx.See section 2.4, Display, for more information
        //on the Chip-8 hexadecimal font.
        [Test]
        public void TestFx29()
        {
            Assert.Fail();
        }

        //Fx33 - LD B, Vx
        //Store BCD representation of Vx in memory locations I, I+1, and I+2.
        //
        //The interpreter takes the decimal value of Vx, and places the hundreds digit in memory
        //at location in I, the tens digit at location I+1, and the ones digit at location I+2.
        [Test]
        public void TestFx33()
        {
            State state = new State();
            Core test = new Core(state);
            state.I = 0x250;
            state.V[8] = 255;

            test.Load(new byte[] { 0xF8, 0x33 });
            test.ExecuteCycle();

            var bcd = new byte[] { state.memory[state.I], state.memory[state.I + 1], state.memory[state.I + 2] };

            Assert.AreEqual(new byte[] { 2, 5, 5 }, bcd);

            state.Reset();
            state.V[8] = 128;
            test.ExecuteCycle();

            var bcd2 = new byte[] { state.memory[state.I], state.memory[state.I + 1], state.memory[state.I + 2] };

            Assert.AreEqual(new byte[] { 1, 2, 8 }, bcd2);

            state.Reset();
            state.V[8] = 18;
            test.ExecuteCycle();

            var bcd3 = new byte[] { state.memory[state.I], state.memory[state.I + 1], state.memory[state.I + 2] };

            Assert.AreEqual(new byte[] {0, 1, 8 }, bcd3);

            state.Reset();
            state.V[8] = 5;
            test.ExecuteCycle();

            var bcd4 = new byte[] { state.memory[state.I], state.memory[state.I + 1], state.memory[state.I + 2] };

            Assert.AreEqual(new byte[] { 0, 0, 5 }, bcd4);
        }

        //Fx55 - LD[I], Vx
        //Store registers V0 through Vx in memory starting at location I.
        //
        //The interpreter copies the values of registers V0 through Vx into memory,
        //starting at the address in I.
        //TODO: Copy beyond 4096 bountry?
        [Test]
        public void TestFx55()
        {
            State state = new State();
            Core test = new Core(state);
            state.V[0] = 5;
            state.V[1] = 4;
            state.V[2] = 3;
            state.V[3] = 2;
            state.V[4] = 1;
            state.V[5] = 5;
            state.I = 0x250;

            test.Load(new byte[] { 0xF5, 0x55 });
            test.ExecuteCycle();

            var user_memory = new byte[5];
            Array.Copy(state.memory, state.I, user_memory, 0, 5);

            Assert.AreEqual(new byte[] { 5, 4, 3, 2, 1 }, user_memory);
        }

        //Fx65 - LD Vx, [I]
        //Read registers V0 through Vx from memory starting at location I.
        //
        //The interpreter reads values from memory starting at location I into registers V0 through Vx.
        [Test]
        public void TestFx65()
        {
            State state = new State();
            Core test = new Core(state);
            state.I = 0x250;
            state.V[5] = 5;
            Array.Copy(new byte[] { 1, 2, 3, 4, 5 }, 0, state.memory, state.I, 5);

            test.Load(new byte[] { 0xF5, 0x65 });
            test.ExecuteCycle();

            var temp = new byte[5];
            Array.Copy(state.V, 0, temp, 0, 5);

            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, temp);
        }
     }
}
