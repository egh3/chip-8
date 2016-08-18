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
            test.ExecuteCycle();

            Assert.AreEqual(state.PC, 0x0123);
            Assert.AreEqual(state.SP, 0x100 - 2);
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
            test.Load(new byte[] { 0x21, 0x23 });
            test.ExecuteCycle();

            Assert.AreEqual(state.PC, 0x0123);
            Assert.AreEqual(state.SP, 0x100 - 2);
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
            state.V[3] = 0x01;
            state.V[6] = 0x02;
            test.Load(new byte[] { 0x83, 0x66 });
            test.ExecuteCycle();

            Assert.AreEqual(state.V[0x3], 0xFF);
            Assert.AreEqual(state.V[0xF], 0);

            state.Reset();
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

            Assert.AreEqual(state.V[0x3], 0xFF);
            Assert.AreEqual(state.V[0xF], 0);

            state.Reset();
        }
    }
}
