using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Text;

namespace Game_Test
{
    class ArduinoCmds
    {
        private SerialPort CurrentPort;

        public ArduinoCmds(SerialPort port)
        {
            CurrentPort = port;
        }

        public void AllowControls()
        {
            Write(32);
        }

        public void Save()
        {
            Write(64);
        }

        private void Write(int CmdByte)
        {
            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(CmdByte);

            CurrentPort.Open();
            CurrentPort.Write(buffer, 0, 2);
            Thread.Sleep(1000);
            CurrentPort.Close();
        }
    }
}
