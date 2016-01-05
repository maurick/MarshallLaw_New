using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Text;

namespace Game_Test
{
    class ArduinoRead
    {
        private SerialPort CurrentPort;
        private int x;
        private int y;

        public ArduinoRead(SerialPort port)
        {
            CurrentPort = port;
            x = 0;
            y = 0;
        }

        public int X()
        {
            if(Read() == "#X%")
            {
                x = Convert.ToInt32(Read());
            }
            return x;
        }

        private string Read()
        {
            int intReturnASCII = 0;
            CurrentPort.Open();
            Thread.Sleep(1000);
            int count = CurrentPort.BytesToRead;
            string returnMessage = "";
            while (count > 0)
            {
                intReturnASCII = CurrentPort.ReadByte();
                returnMessage += Convert.ToChar(intReturnASCII);
                count--;
            }
            CurrentPort.Close();
            return returnMessage;
        }
    }
}
