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
        Runner test;
        IKeyboard keyboard;
        public Form1()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            keyboard = new Chip8Keyboard();
            test = new Runner(this, keyboard);

            Task.Factory.StartNew(() =>
            {
                Task.Delay(1000);
                test.Run();
            });
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render rend = new Render(test._core._screen, this);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.A:
                    keyboard.KeyDown(Keys.Key_A);
                    break;
                case System.Windows.Forms.Keys.B:
                    keyboard.KeyDown(Keys.Key_B);
                    break;
                case System.Windows.Forms.Keys.C:
                    keyboard.KeyDown(Keys.Key_C);
                    break;
                case System.Windows.Forms.Keys.D:
                    keyboard.KeyDown(Keys.Key_D);
                    break;
                case System.Windows.Forms.Keys.E:
                    keyboard.KeyDown(Keys.Key_E);
                    break;
                case System.Windows.Forms.Keys.F:
                    keyboard.KeyDown(Keys.Key_F);
                    break;
                case System.Windows.Forms.Keys.NumPad0:
                    keyboard.KeyDown(Keys.Key_0);
                    break;
                case System.Windows.Forms.Keys.NumPad1:
                    keyboard.KeyDown(Keys.Key_1);
                    break;
                case System.Windows.Forms.Keys.NumPad2:
                    keyboard.KeyDown(Keys.Key_2);
                    break;
                case System.Windows.Forms.Keys.NumPad3:
                    keyboard.KeyDown(Keys.Key_3);
                    break;
                case System.Windows.Forms.Keys.NumPad4:
                    keyboard.KeyDown(Keys.Key_4);
                    break;
                case System.Windows.Forms.Keys.NumPad5:
                    keyboard.KeyDown(Keys.Key_5);
                    break;
                case System.Windows.Forms.Keys.NumPad6:
                    keyboard.KeyDown(Keys.Key_6);
                    break;
                case System.Windows.Forms.Keys.NumPad7:
                    keyboard.KeyDown(Keys.Key_7);
                    break;
                case System.Windows.Forms.Keys.NumPad8:
                    keyboard.KeyDown(Keys.Key_8);
                    break;
                case System.Windows.Forms.Keys.NumPad9:
                    keyboard.KeyDown(Keys.Key_9);
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.A:
                    keyboard.KeyUp(Keys.Key_A);
                    break;
                case System.Windows.Forms.Keys.B:
                    keyboard.KeyUp(Keys.Key_B);
                    break;
                case System.Windows.Forms.Keys.C:
                    keyboard.KeyUp(Keys.Key_C);
                    break;
                case System.Windows.Forms.Keys.D:
                    keyboard.KeyUp(Keys.Key_D);
                    break;
                case System.Windows.Forms.Keys.E:
                    keyboard.KeyUp(Keys.Key_E);
                    break;
                case System.Windows.Forms.Keys.F:
                    keyboard.KeyUp(Keys.Key_F);
                    break;
                case System.Windows.Forms.Keys.NumPad0:
                    keyboard.KeyUp(Keys.Key_0);
                    break;
                case System.Windows.Forms.Keys.NumPad1:
                    keyboard.KeyUp(Keys.Key_1);
                    break;
                case System.Windows.Forms.Keys.NumPad2:
                    keyboard.KeyUp(Keys.Key_2);
                    break;
                case System.Windows.Forms.Keys.NumPad3:
                    keyboard.KeyUp(Keys.Key_3);
                    break;
                case System.Windows.Forms.Keys.NumPad4:
                    keyboard.KeyUp(Keys.Key_4);
                    break;
                case System.Windows.Forms.Keys.NumPad5:
                    keyboard.KeyUp(Keys.Key_5);
                    break;
                case System.Windows.Forms.Keys.NumPad6:
                    keyboard.KeyUp(Keys.Key_6);
                    break;
                case System.Windows.Forms.Keys.NumPad7:
                    keyboard.KeyUp(Keys.Key_7);
                    break;
                case System.Windows.Forms.Keys.NumPad8:
                    keyboard.KeyUp(Keys.Key_8);
                    break;
                case System.Windows.Forms.Keys.NumPad9:
                    keyboard.KeyUp(Keys.Key_9);
                    break;
                default:
                    break;
            }
        }
    }
}
