﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class MockKeyboard : IKeyboard
    {
        public Keys[] pressed = new Keys[] { Keys.Key_4 | Keys.Key_0, Keys.Key_0 };
        public int index = 0;

        public Keys ReadKey()
        {
            return pressed[index++%pressed.Length];
        }

        public void KeyUp(Keys key)
        {
            throw new NotImplementedException();
        }

        public void KeyDown(Keys key)
        {
            throw new NotImplementedException();
        }
    }
}
