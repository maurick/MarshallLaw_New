using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Game_Test
{
    public class Arduino
    {
        static int NextAvailableID = 0;
        public int ControllerID;

        private SerialPort port;

        public string allbuttons;
        public string prevbuttons;

        CharacterInfo characterInfo;

        public Arduino(SerialPort port)
        {

            ControllerID = NextAvailableID++;
            this.port = port;

            characterInfo = new CharacterInfo(ControllerID);

            allbuttons = "000000000";
            prevbuttons = "000000000";

            port.Open();
            port.Write("#GetInfo%");
            Thread.Sleep(1000);

            int count = port.BytesToRead;
            string returnMessage = "";
            int intReturnASCII = 0;

            while (count > 0)
            {
                intReturnASCII = port.ReadByte();
                returnMessage += Convert.ToChar(intReturnASCII);
                count--;
            }

            port.Close();

            bool CharachterInfo_Collected = false;


            string message = returnMessage.Substring(returnMessage.IndexOf("#Info:"), 21);

            if(message.Substring(6, 2) == "55")
            {
                CharachterInfo_Collected = true;
                characterInfo.NotFound = true;

                string name = message.Substring(8, 10);
                int gender = Convert.ToInt32(message.Substring(18, 1));
                int skincolor = Convert.ToInt32(message.Substring(19, 1));

                characterInfo.SetCharacterInfo(name, gender, skincolor);
            }
            else
            {
                CharachterInfo_Collected = true;
                characterInfo.NotFound = false;
            }

            AllowControls();
        }

        //Port Detection
        /*private void SetComPort()
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
                portFound = false;
            }
        }*/

        /*private bool DetectArduino()
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
        }*/

        //Read
        public string Read()
        {
            if (!port.IsOpen)
            {
                port.Open();
            }
            string message = "";
            int incomingByte = port.ReadByte();
            char readChar = (char)incomingByte;

            while (port.BytesToRead > 0)
            {
                incomingByte = port.ReadByte();
                readChar = (char)incomingByte;

                if (readChar == '#')
                {
                    message = "";
                }

                if (readChar == '%')
                {
                    port.Close();
                    return message;
                }

                message += readChar;
            }
            port.Close();
            return message;
        }


        public bool MessageAvailable()
        {
            if (!port.IsOpen)
            {
                port.Open();
            }

            if (port.BytesToRead > 0)
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

            if (!port.IsOpen)
            {
                port.Open();
            }
            port.Write(buffer, 0, 2);
            port.Close();
        }

        private void Write(string Text)
        {
            if (!port.IsOpen)
            {
                port.Open();
            }
            port.Write("#"+Text+"%");
            port.Close();
        }

        private void Write(int[] Array)
        {
            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(8);

            if (!port.IsOpen)
            {
                port.Open();
            }
            port.Write(buffer, 0, 2);
            foreach (var nr in Array)
            {
                port.Write(nr.ToString());
            }
            port.Close();
        }

        //Controller
        public void Update()
        {
            prevbuttons = allbuttons;
            if(MessageAvailable())
            {
                allbuttons = this.Read();
                if(allbuttons.Length < 9)
                {
                    allbuttons = "000000000";
                }
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

        private bool Debounce(int Button)
        {
            if (allbuttons[Button] > prevbuttons[Button])
            {
                return true;
            }
            return false;
        }

        //Methods
        public bool Up(bool debounce)
        {
            if (debounce)
            {
                return Debounce(3);
            }
            return Button(3);
        }
        public bool Down(bool debounce)
        {
            if (debounce)
            {
                return Debounce(4);
            }
            return Button(4);
        }
        public bool Left(bool debounce)
        {
            if (debounce)
            {
                return Debounce(2);
            }
            return Button(2); 
        }
        public bool Right(bool debounce)
        {
            if (debounce)
            {
                return Debounce(1);
            }
            return Button(1);
        }
        public bool Joystick_Button(bool debounce)
        {
            if (debounce)
            {
                return Debounce(0);
            }
            return Button(0);
        }
        public bool X_Button(bool debounce)
        {
            if (debounce)
            {
                return Debounce(8);
            }
            return Button(8);
        }
        public bool A_Button(bool debounce)
        {
            if (debounce)
            {
                return Debounce(7);
            }
            return Button(7);
        }
        public bool B_Button(bool debounce)
        {
            if (debounce)
            {
                return Debounce(5);
            }
            return Button(5);
        }
        public bool Y_Button(bool debounce)
        {
            if (debounce)
            {
                return Debounce(6);
            }
            return Button(6);
        }

        public void AllowControls()
        {
            port.Open();
            port.Write("#AllowControls%");
            port.Close();
        }

        public void SendStats(int[] Stats)
        {
            Write(Stats);
        }

        public void Save()
        {
            Write(64);
        }

        public void Exit()
        {
            //SendStats();
            Save();
            Write("ControllerOff");
        }
    }
}
