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

        public CharacterInfo characterInfo;

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


            string message = returnMessage.Substring(returnMessage.IndexOf("#Info:"), 28);

            if(message.Substring(6, 2) == "55")
            {
                CharachterInfo_Collected = true;
                characterInfo.NotFound = false;

                string name = message.Substring(8, 10);
                int gender = Convert.ToInt32(message.Substring(18, 1));
                int skincolor = Convert.ToInt32(message.Substring(19, 1));
                int head = Convert.ToInt32(message.Substring(20, 1));
                int shirt = Convert.ToInt32(message.Substring(21, 1));
                int belt = Convert.ToInt32(message.Substring(22, 1));
                int pants = Convert.ToInt32(message.Substring(23, 1));
                int xp = Convert.ToInt32(message.Substring(24, 3));

                characterInfo.SetCharacterInfo(name, gender, skincolor, head, shirt, belt, pants, xp);

            }
            else
            {
                CharachterInfo_Collected = true;
                characterInfo.NotFound = true;
                //characterInfo.SetCharacterInfo("nonamefond", 1, 2,);
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
            port.Write("#AllowControls%");
            port.Write("#AllowControls%");
            port.Write("#AllowControls%");
            port.Write("#AllowControls%");
            port.Write("#AllowControls%");

            port.Close();
        }

        public void SendStats(int[] Stats)
        {
            Write(Stats);
        }

        public void SaveSettings()
        {
            port.Open();
            port.Write("#ControlsOff");
            port.Close();

            Thread.Sleep(1000);



            string message = "#Save:55";



            for (int i = 0; i < characterInfo.Name.Length; i++)
            {
                message += ConvertToInt(characterInfo.Name[i]);
            }

            message += characterInfo.Gender.ToString();
            message += characterInfo.Skincolor.ToString();
            message += characterInfo.Head.ToString();
            message += characterInfo.Shirt.ToString();
            message += characterInfo.Belt.ToString();
            message += characterInfo.Pants.ToString();
            message += characterInfo.XP.ToString();

            message += "%";

            port.Open();

            bool Saved = false;

            while (Saved == false)
            {
                port.Write(message);

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
                
                if(returnMessage.Contains("Saved"))
                {
                    Saved = true;
                }
            }
            port.Close();

            Thread.Sleep(1000);
        }

        public void Exit()
        {
            if(port.IsOpen != true)
                port.Open();
            port.Write("#Exit%");
            port.Close();
            
            Thread.Sleep(1000);
            GameInstance.ExitGame = true;

        }

        public string ConvertToInt(char text)
        {
            text = Char.ToUpper(text);
            switch(text)
            {
                case ' ':
                    return "10";
                case 'A':
                    return "11";
                case 'B':
                    return "12";
                case 'C':
                    return "13";
                case 'D':
                    return "14";
                case 'E':
                    return "15";
                case 'F':
                    return "16";
                case 'G':
                    return "17";
                case 'H':
                    return "18";
                case 'I':
                    return "19";
                case 'J':
                    return "20";
                case 'K':
                    return "21";
                case 'L':
                    return "22";
                case 'M':
                    return "23";
                case 'N':
                    return "24";
                case 'O':
                    return "25";
                case 'P':
                    return "26";
                case 'Q':
                    return "27";
                case 'R':
                    return "28";
                case 'S':
                    return "29";
                case 'T':
                    return "30";
                case 'U':
                    return "31";
                case 'V':
                    return "32";
                case 'W':
                    return "33";
                case 'X':
                    return "34";
                case 'Y':
                    return "35";
                case 'Z':
                    return "36";
                default:
                    return " ";
            }
        }
    }
}
