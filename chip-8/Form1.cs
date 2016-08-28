using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip_8
{
    public partial class Form1 : Form
    {
        private State state;
        private Core test;
        public Form1()
        {
            InitializeComponent();

            state = new State();
            test = new Core(state);

            test.Load(new byte[] { 0xD4, 0x05, 0x12, 0x00 });

            ulong line = 0xAAAA;
            ulong chan = 0x8000 >> 1;

            var clear = Utils.checkForClear(line, (ulong)(line ^ chan));


            for (byte i = 0; i < 30; i += 5)
            {
                state.V[4] = (byte)(i + 62);
                state.V[5] = 0;
                state.I = i;
                test.ExecuteCycle();
                test.ExecuteCycle();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var screen = new ulong[32];
            screen[0] = 0xAAAAAAAAAAAAAAAA;
            screen[1] = 0xAAAAAAAAAAAAAAAA >> 1;
            screen[2] = 0xAAAAAAAAAAAAAAAA;
            screen[3] = 0xAAAAAAAAAAAAAAAA >> 1;
            //Render rend = new Render(screen, this);
            Render rend = new Render(test._screen, this);
        }
    }
}
