using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
//using System.Windows.Forms;


namespace Game_Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameInstance : Game
    {
        public GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        public static bool ExitGame;

        public GameInstance()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f); //Verander de laatste float naar aantal frames per seconde
            this.Graphics.PreferredBackBufferWidth = (int)GameSettings.Instance.Dimensions.X;
            this.Graphics.PreferredBackBufferHeight = (int)GameSettings.Instance.Dimensions.Y;
            //if (!Graphics.IsFullScreen)
                //Graphics.ToggleFullScreen();
            this.Graphics.ApplyChanges();


            this.Window.Position = GameSettings.Instance.Position;
            this.Window.IsBorderless = true;
            this.Window.AllowAltF4 = true;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameSettings.Instance.ScreenDimChanged)
                SceenReload();
            if (ExitGame == true)
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape) || ExitGame == true)
                Exit();

            ScreenManager.Instance.Update(gameTime);

            if (GameSettings.Instance.ScreenDimChanged)
                SceenReload();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.TransparentBlack);

            ScreenManager.Instance.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void SceenReload()
        {
            
            this.Graphics.PreferredBackBufferWidth = (int)GameSettings.Instance.Dimensions.X;
            this.Graphics.PreferredBackBufferHeight = (int)GameSettings.Instance.Dimensions.Y;
            this.Graphics.ApplyChanges();

            

            this.Window.Position = new Point(
                                                        x: (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - GameSettings.Instance.Dimensions.X) / 2,
                                                        y: (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - GameSettings.Instance.Dimensions.Y) / 2);
            GameSettings.Instance.ScreenDimChanged = false;
            ScreenManager.Instance.ChangeScreen("OptionsScreen");

        }
    }
}
