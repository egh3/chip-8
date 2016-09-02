using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class Chip8Keyboard : IKeyboard
    {
        private Keys _keys;
        public void KeyDown(Keys key)
        {
            _keys |= key;
        }

        public void KeyUp(Keys key)
        {
            _keys ^= key;
        }

        public Keys ReadKey()
        {
            return _keys;
        }
    }
}
