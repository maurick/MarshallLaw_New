using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Text;

namespace Game_Test
{
    public class ArduinoPortDetection
    {
        List<SerialPort> Ports;
        SerialPort currentPort;
        public bool portFound;

        public ArduinoPortDetection()
        {
            Ports = new List<SerialPort> { };
            SetComPort();
            if (Ports.Count > 0)
            {
                portFound = true;
            }
            else
            {
                portFound = false;
            }
        }

        private void SetComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        Ports.Add(currentPort);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private bool DetectArduino()
        {
            try
            {
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[2];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);

                int intReturnASCII = 0;

                currentPort.Open();
                currentPort.Write(buffer, 0, 2);
                Thread.Sleep(1000);

                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage += Convert.ToChar(intReturnASCII);
                    count--;
                }
                currentPort.Close();
                if (returnMessage.Contains("HELLO FROM ARDUINO"))
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

        public List<SerialPort> ReturnPorts()
        {
            return Ports;
        }
    }
}
