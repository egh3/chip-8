using System.IO;

namespace chip_8
{
    class Runner
    {
        private bool _running = false;
        public Core _core;
        private Form1 _form1;

        public Runner(Form1 form, IKeyboard keyboard)
        {
            _core = new Core(new State(), keyboard);
            _form1 = form;
        }

        public void Run()
        {
           _running = true;
            //_core.Load(new byte[]
            //    {
            //        0xA0, 0x00,
            //        0x64, 0x01,
            //        0x63, 0x0C,
            //        0x62, 0x05,
            //        0x60, 0x01,
            //        0x61, 0x0A,
            //        0xD0, 0x15,
            //        0xF2, 0x1E,
            //        0x83, 0x45,
            //        0x70, 0x05,
            //        0x33, 0x00,
            //        0x12, 0x0C,
            //        0xF9, 0x0A,
            //        0x00, 0xE0,
            //        0xFF, 0xFF
            //    });A

            var file = new BinaryReader(File.Open("TETRIS", FileMode.Open));
            var prog = file.ReadBytes((int)file.BaseStream.Length);
            _core.Load(prog);

            try
            {
                int i = 3;
                while (_running)
                {
                    _core.ExecuteCycle();
                    if (i <= 0)
                    {
                        Render rend = new Render(_core._screen, _form1);
                        i = 3;
                    }
                    i--;
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch
            {
                _running = false;
            }
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
