using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    interface IKeyboard
    {
        Keys WaitForKey();
        Keys ReadKey(); 
    }
}
