using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip_8
{
    class Runner
    {
        private bool _running = false;
        public Core _core;
        private State _state;

        public Runner()
        {
            _core = new Core(new State());
        }

        public void Run()
        {
           _running = true;
            _core.Load(new byte[]
                {
                    0xA0, 0x00,
                    0x64, 0x01,
                    0x63, 0x0C,
                    0x62, 0x05,
                    0x60, 0x01,
                    0x61, 0x0A,
                    0xD0, 0x15,
                    0xF2, 0x1E,
                    0x83, 0x45,
                    0x70, 0x05,
                    0x33, 0x00,
                    0x12, 0x0C,
                    0xFF, 0xFF
                });

            int i = 0;

            try
            {
                while (_running)
                {
                    _core.ExecuteCycle();
                    i++;
                    System.Threading.Thread.Sleep(50);
                }
            }
            catch
            {

            }
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
