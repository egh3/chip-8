using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    interface IScreen
    {
        /// <summary>
        /// Draws sprite on screen
        /// </summary>
        /// <param name="x">X-Coord of location to draw. (0-63)</param>
        /// <param name="y">Y-Coord of location to draw. (0-31)</param>
        /// <param name="address"></param>
        /// <param name="length">Amount in bytes to draw</param>
        /// <returns>True of draw operation causes any bit to clear. Otherwise False.</returns>
        bool DrawSprite(int x, int y, ushort address, int length);

        /// <summary>
        /// Clears the screen
        /// </summary>
        void ClearScreen();
    }
}
