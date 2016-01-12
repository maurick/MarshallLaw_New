using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    class ArduinoControllerPlayer
    {
        ArduinoPortDetection ports;
        ArduinoRead reader;
        ArduinoCmds cmnds;
        public string allbuttons;
        public string previousbuttons;

        public ArduinoControllerPlayer(int Controller)
        {
            ports = new ArduinoPortDetection();
            reader = new ArduinoRead(ports.ReturnPorts()[Controller-1]);
            cmnds = new ArduinoCmds(ports.ReturnPorts()[Controller-1]);
            allbuttons = "000000000";
            previousbuttons = "000000000";
        }

        public void Read()
        {
            string x = reader.Read();
            if(x != null && x != "")
            {
                previousbuttons = allbuttons;
                allbuttons = x;
            }

        }
        public bool fbutt(int x)
        {
            if (allbuttons[x] == '1')
            {
                return true;
            }else
            {
                return false;
            }
                
        }

        public bool Up()
        {
            return fbutt(2);
        }
        public bool Down()
        {
            return fbutt(3);
        }
        public bool Left()
        {
            return fbutt(1);
        }
        public bool Right()
        {
            return fbutt(0);
        }
        public bool lbutt()
        {
            return fbutt(4);
        }
    }
}
