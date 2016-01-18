using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Game_Test
{
    public class ArduinoManager
    {
        public static List<SerialPort> Good_Ports = new List<SerialPort>();
        public static bool portFound = false;


        public ArduinoManager()
        {

        }

        public bool SearchArduinoComPort()
        {
            SerialPort Port;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    Port = new SerialPort(port, 9600);
                    if (DetectArduino(Port))
                    {
                        Good_Ports.Add(Port);
                        portFound = true;
                    }
                }

                return portFound;
            }
            catch (Exception e)
            {
                return portFound;
            }
        }

        private bool DetectArduino(SerialPort Port)
        {
            try
            {
                /*//"HELLO"-Handshake
                byte[] buffer = new byte[2];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);

                

                Port.Open();
                Port.Write(buffer, 0, 2);
                */

                int intReturnASCII = 0;

                Port.Open();
                Port.Write("#Hello Arduino%");
                Thread.Sleep(100);

                int count = Port.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = Port.ReadByte();
                    returnMessage += Convert.ToChar(intReturnASCII);
                    count--;
                }
                Port.Close();
                if (returnMessage.Contains("Hello PC"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
