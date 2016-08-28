using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip_8
{
    class Render
    {
        private Size _pixelSize = new Size(15,15);
        public Render(ulong[] screen, Form formToRenderOn)
        {
            var g = formToRenderOn.CreateGraphics();
            g.DrawRectangle(Pens.Black, 0, 0, _pixelSize.Width * 64, _pixelSize.Height * 32);

            int rowi = 0;
            for(var row = 0; row < 32 * _pixelSize.Height; row+=_pixelSize.Height)
            {
                ulong mask = 0x8000000000000000;
                for(var col = 0; col < 64 * _pixelSize.Width; col+=_pixelSize.Width)
                {
                    if((screen[rowi] & mask) != 0)
                        g.FillRectangle(Brushes.Black, new Rectangle(col, row, _pixelSize.Width, _pixelSize.Height));
                    mask = mask >> 1;
                }
                rowi++;
            }
        }
    }
}
