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

        public ArduinoRead(SerialPort port)
        {
            CurrentPort = port;
        }



        public string Read()
        {
            if (!CurrentPort.IsOpen)
            {
                CurrentPort.Open();
            }
           
            if (CurrentPort.BytesToRead > 0)
            {
                string message = "";

                int incomingByte = CurrentPort.ReadByte();
                char readChar = (char)incomingByte;

                if (readChar == '#')
                {
                    while (readChar != '%')
                    {
                        message += readChar;
                        incomingByte = CurrentPort.ReadByte();
                        readChar = (char)incomingByte;                        
                    }
                    message = message.Remove(0,1);
                }
                CurrentPort.DiscardInBuffer();
                CurrentPort.Close();
                return message;
                }
            //CurrentPort.Close();
            return null;
            
        }         
    }
}
