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
        Runner test = new Runner();
        public Form1()
        {
            InitializeComponent();

            test.Run();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render rend = new Render(test._core._screen, this);
        }
    }
}
