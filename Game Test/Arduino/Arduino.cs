using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Game_Test
{
    class Arduino
    {
        private SerialPort Port;
        List<SerialPort> Ports;
        public bool portFound;
        public bool ControllerConnected;

        public string allbuttons;
        public string prevbuttons;

        public Arduino(int Controller)
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
            if(portFound)
            {
                Port = Ports[Controller - 1];
                ControllerConnected = true;
            }
            if (ControllerConnected)
            {
                AllowControls();
            }
            allbuttons = "000000000";
            prevbuttons = "000000000";
        }

        //Port Detection
        private void SetComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    Port = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        Ports.Add(Port);
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
                //"HELLO"-Handshake
                byte[] buffer = new byte[2];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);

                int intReturnASCII = 0;

                Port.Open();
                Port.Write(buffer, 0, 2);
                Thread.Sleep(1000);

                int count = Port.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = Port.ReadByte();
                    returnMessage += Convert.ToChar(intReturnASCII);
                    count--;
                }
                Port.Close();
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

        //Read
        public string Read()
        {
            if (!Port.IsOpen)
            {
                Port.Open();
            }
            string message = "";
            int incomingByte = Port.ReadByte();
            char readChar = (char)incomingByte;

            while (Port.BytesToRead > 0)
            {
                incomingByte = Port.ReadByte();
                readChar = (char)incomingByte;

                if (readChar == '#')
                {
                    message = "";
                }

                if (readChar == '%')
                {
                    Port.Close();
                    return message;
                }

                message += readChar;
            }
            Port.Close();
            return message;
        }

        public bool MessageAvailable()
        {
            if (!Port.IsOpen)
            {
                Port.Open();
            }

            if (Port.BytesToRead > 0)
            {
                return true;
            }
            return false;
        }

        //Write
        private void Write(int CmdByte)
        {
            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(CmdByte);

            Port.Open();
            Port.Write(buffer, 0, 2);
            Port.Close();
        }

        private void Write(int[] Array)
        {
            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(8);

            Port.Open();
            Port.Write(buffer, 0, 2);
            foreach (var nr in Array)
            {
                Port.Write(nr.ToString());
            }
            Port.Close();
        }

        //Controller
        public void Update()
        {
            prevbuttons = allbuttons;
            if(MessageAvailable())
            {
                allbuttons = this.Read();
            }
        }

        public bool Button(int x)
        {
            if (allbuttons[x] == '1')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Methods
        public bool MenuUp()
        {
            if(allbuttons[2] > prevbuttons[2])
            {
                return true;
            }
            return false;        
        }
        public bool MenuDown()
        {
            if (allbuttons[3] > prevbuttons[3])
            {
                return true;
            }
            return false;
        }

        public bool Up()
        {
            return Button(2);
        }
        public bool Down()
        {
            return Button(3);
        }
        public bool Left()
        {
            return Button(1);
        }
        public bool Right()
        {
            return Button(0);
        }
        public bool rbutt()
        {
            return Button(4);
        }
        public bool ubutt()
        {
            return Button(5);
        }
        public bool dbutt()
        {
            return Button(6);
        }
        public bool lbutt()
        {
            return Button(7);
        }
        public bool jbutt()
        {
            return Button(8);
        }

        public void AllowControls()
        {
            Write(32);
        }

        public void SendStats(int[] Stats)
        {
            Write(Stats);
        }

        public void Save()
        {
            Write(64);
        }
    }
}
