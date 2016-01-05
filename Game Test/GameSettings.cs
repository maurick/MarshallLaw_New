using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class GameSettings
    {
        //Fields
        private static GameSettings instance;

        /// <summary>
        /// Maken van een Instance, Deze Class is een Singleton class
        /// </summary>
        public static GameSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameSettings();
                }
                return instance;
            }
        }

        public bool ScreenDimChanged;

        //Video Fields
        //Waardes voor het scherm zodat hiermee gewerkt kan worden in andere classes
        public Vector2 Dimensions;
        public Vector2 Screensize;
        private Vector2 tilescale;
        public Point Position;
        public DisplayMode DisplayMode;
        public DisplayMode FullScreen;

        public Vector2 Tilescale { get { return tilescale; } set { tilescale = value; } }

        private GameSettings()
        {
            DisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Screensize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            //Fullscreen
            //Dimensions = Screensize;
            Dimensions = new Vector2(1280, 720);

            tilescale = new Vector2();

            //Zorgt ervoor dat de window in het midden van je beeldscherm begint
            Position = new Point((int)(Screensize.X - Dimensions.X) / 2, (int)(Screensize.Y - Dimensions.Y) / 2);
        }
    }
}
