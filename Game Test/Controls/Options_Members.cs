using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Options_Members
    {
        public enum Actions
        {
            changeResolution,
        };

        public void ExcuteAction(Actions action, int index)
        {
            switch(action)
            {
                case Actions.changeResolution:
                    GameSettings.Instance.Dimensions = new Vector2(Displaymodes[index].Width, Displaymodes[index].Height);
                    GameSettings.Instance.ScreenDimChanged = true;
                    break;
            }
        }


        List<string> Resolutions = new List<string>();
        List<DisplayMode> Displaymodes = new List<DisplayMode>();

        public void Create_Lists()
        {
            #region "Resolutions"
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                string temp;
                if (mode.AspectRatio >= 1.6f && mode.AspectRatio <= 1.9f)
                {
                    temp = mode.Width.ToString() + "x" + mode.Height;

                    bool check = false;
                    foreach (string item in Resolutions)
                    {
                        if (item == temp)
                            check = true;
                    }
                    if (!check)
                    {
                        Resolutions.Add(temp);
                        Displaymodes.Add(mode);
                    }
                }
            }
            #endregion
        }

        public List<string> GetList(int ID)
        {
            switch(ID)
            {
                case 1:
                    return Resolutions;

            }
            return null;
        }

        public string GetString(int ID, int index)
        {
            switch(ID)
            {
                case 1:
                    return Resolutions[index];
            }
            return "error";
        }
    }
}
