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
        string allbuttons;
        string lastconfig;

        public ArduinoControllerPlayer(int Controller)
        {
            ports = new ArduinoPortDetection();
            if (ports.portFound)
            {
                reader = new ArduinoRead(ports.ReturnPorts()[Controller - 1]);
                cmnds = new ArduinoCmds(ports.ReturnPorts()[Controller - 1]);
                
            }
            allbuttons = "000000000";
            lastconfig = "000000000";
        }

        public void Update()
        {
            return;
            lastconfig = allbuttons;
            string x = reader.Read();
            if(x != null && x != "")
            {
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

        public bool MenuUp()
        {
            if(allbuttons[2] > lastconfig[2])
            {
                return true;
            }
            return false;        
        }
        public bool MenuDown()
        {
            if (allbuttons[3] > lastconfig[3])
            {
                return true;
            }
            return false;
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
        public bool rbutt()
        {
            return fbutt(4);
        }
        public bool ubutt()
        {
            return fbutt(5);
        }
        public bool dbutt()
        {
            return fbutt(6);
        }
        public bool lbutt()
        {
            return fbutt(7);
        }
        public bool jbutt()
        {
            return fbutt(8);
        }
    }
}
