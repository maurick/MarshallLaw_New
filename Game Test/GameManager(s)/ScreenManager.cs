using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game_Test
{
    public class ScreenManager
    {
        //Fields
        private static ScreenManager instance;

        /// <summary>
        /// Maken van een Instance, Deze Class is een Singleton class
        /// </summary>
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

        //Contentmanager regelt het laden en verwijdered van content, zoals sprites en text
        public ContentManager Content { private set; get; }


        public bool IsTransitioning;
        public bool HasChangedScreen;

        public GraphicsDevice GraphicsDevice;   //Maak een Graphics Devic
        public SpriteBatch SpriteBatch;         //Maak een Sprite

        Screen currentscreen;
        string newscreen;

        Image fade;
        FadeEffect fadeEffect;
        public List<Arduino> Controllers;

        //Contructor
        private ScreenManager()
        {
            //CurrentScreen begint met het SplashScreen
            currentscreen = new MenuScreen();
            IsTransitioning = false;

            Controllers = new List<Arduino> { new Arduino(1) };
            //for(int i=2;Controllers.Count !=  Controllers[0].Ports.Count;i++)
            //{
            //    Controllers.Add(new Arduino(i));
            //}
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentscreen.LoadContent();

            if (fade == null)
                fade = new Image("Images/black");
            if (fadeEffect == null)
                fadeEffect = new FadeEffect(3.0f, 0.0f);
            fade.SourceRect = new Rectangle(0, 0, (int)GameSettings.Instance.Dimensions.X, (int)GameSettings.Instance.Dimensions.Y);
            fade.LoadContent( 0, 0, true, new Vector2(GameSettings.Instance.Dimensions.X, GameSettings.Instance.Dimensions.Y));
        }

        public void UnloadContent()
        {
            currentscreen.UnloadContent();
            fade.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            TransitionScreen(gameTime);
            currentscreen.Update(gameTime);
            foreach (Arduino Controller in Controllers)
            {
                if (Controller.ControllerConnected)
                {
                    Controller.Update();
                }
            }      
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            currentscreen.Draw(spriteBatch);

            if (IsTransitioning)
                fade.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Start()
        {
            currentscreen.UnloadContent();
            currentscreen = new MenuScreen();
            currentscreen.LoadContent();
            fade.SetScale(new Vector2(GameSettings.Instance.Dimensions.X, GameSettings.Instance.Dimensions.Y));
        }

        public void ChangeScreen(string screenName)
        {
            newscreen = screenName;
            IsTransitioning = true;
            fade.Alpha = 0.0f;
        }
        
        public void TransitionScreen(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                fade.Alpha = fadeEffect.Update(gameTime);
                if (fade.Alpha >= 1.0f)
                {
                    currentscreen.UnloadContent();
                    currentscreen = (Screen)Activator.CreateInstance(Type.GetType("Game_Test." + newscreen));
                    currentscreen.LoadContent();
                    fade.SetScale(new Vector2(GameSettings.Instance.Dimensions.X, GameSettings.Instance.Dimensions.Y));
                    HasChangedScreen = true;
                }

                if (HasChangedScreen && fade.Alpha <= 0.0f)
                    IsTransitioning = false;

                //fade.Update(gameTime);
            }
        }
    }
}
