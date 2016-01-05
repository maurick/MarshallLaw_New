using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Map
    {
        //Fields
        //Vector2 tileDimensions;
        //string orientation;
        MapLoader mapLoader = new MapLoader();
        public List<Layer> Layers = new List<Layer>();
        List<string> spriteSheets = new List<string>();

        Player player;

        List<Enemy> enemies;

        private bool PlayerActive;

        Texture2D grid;
        ContentManager content;

        //Property's
        public Vector2 mapDimensions { get; private set; }
        public int NumberLayers { get; private set; }

        int layer_player_num = 0;

        public Map(string mapName)
        {
            mapLoader.LoadMap(mapName);

            Layers = mapLoader.GetLayers();
            mapDimensions = mapLoader.GetMapDimensions();
            NumberLayers = mapLoader.GetNumLayers();
            spriteSheets = mapLoader.GetSpritesheetList();
            
            player = new Player();
            CreateEnemies();

            for (int l = 0; l < Layers.Count; l++)
            {
                if (Layers[l].Layername == "Player")
                {
                    layer_player_num = l;
                }
            }

            int temp = 0;

            foreach (Enemy enemy in enemies)
                enemy.SetLayernumber(NumberLayers - layer_player_num);
            player.SetLayernumber(NumberLayers - layer_player_num);
            GetLayer("Collision", temp++);


            for (int l = layer_player_num; l < Layers.Count - 1; l++)
            {
                GetLayer(Layers[l].Layername, temp++);
            }

            foreach (var layer in Layers)
            {
                layer.GiveSpriteSheetList(spriteSheets);
            }

        }

        public virtual void LoadContent()
        {
            player.LoadContent(32, 32);

            foreach (Enemy enemy in enemies)
                enemy.LoadContent();

            foreach (var layer in Layers)
            {
                layer.LoadContent();
            }

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            grid = content.Load<Texture2D>("Spritesheets/grid");
            //grid.LoadContent(0, 0, false, new Vector2(GameSettings.Instance.Dimensions.X / 1920, GameSettings.Instance.Dimensions.Y / 1080));
            //grid.SetScale(new Vector2(GameSettings.Instance.Dimensions.X / 1920, GameSettings.Instance.Dimensions.Y / 1080));
        }

        public virtual void UnloadContent()
        {
            player.UnloadContent();

            foreach (Enemy enemy in enemies)
                enemy.UnloadContent();

            foreach (var layer in Layers)
            {
                layer.UnloadContent();
            }

            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            foreach (Enemy enemy in enemies)
            {
                enemy.SendPosition(player.GetPosition());
                enemy.PlayerLookDirection = player.lookDirection;
                enemy.PlayerState = player.State;
                enemy.Update(gameTime);

                player.SendPosition(enemy.GetPosition());
                player.EnemyLookDirection = enemy.lookDirection;
                player.EnemyState = enemy.State;
            }

            //foreach (var layer in Layers)
            //{
            //layer.Update(gameTime);
            //}
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < mapDimensions.Y; y++)
            {
                for (int x = 0; x < mapDimensions.X; x++)
                {
                    for (int l = layer_player_num; l < Layers.Count; l++)
                    {
                        if (Layers[l].Layername != "Collision" && Layers[l].Layername != "Player")
                            Layers[l].DrawTile(spriteBatch, x, y);
                        if (Layers[l].Layername == "Player" && PlayerActive == false)
                        {
                            foreach (Enemy enemy in enemies)
                                enemy.Draw(spriteBatch);

                            player.Draw(spriteBatch);

                            PlayerActive = true;
                        }
                    }
                }
            }
            //player.Draw(spriteBatch);
            PlayerActive = false;

        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < mapDimensions.Y; y++)
            {
                for (int x = 0; x < mapDimensions.X; x++)
                {
                    for (int l = 0; l < layer_player_num; l++)
                    {
                            Layers[l].DrawTile(spriteBatch, x, y);
                    }

                    Layers[11].DrawTile(spriteBatch, x, y);
                }
            }

            spriteBatch.Draw(
                texture: grid, 
                position: new Vector2(0, 0), 
                sourceRectangle: new Rectangle(0, 0, 1920, 1088), 
                color: Color.White, 
                rotation: 0f, 
                origin: Vector2.Zero, 
                scale: new Vector2(24f / 32f, 24f / 32f), 
                effects: SpriteEffects.None, 
                layerDepth: 0.0f);
        }

        public void GetLayer(string Name, int number)
        {
            for (int l = 0; l < Layers.Count; l++)
            {
                if (Layers[l].Layername == Name)
                { 
                    player.SendLayer(Layers[l], number);
                    foreach (Enemy enemy in enemies)
                        enemy.SendLayer(Layers[l], number);
                }
            }
        }

        public void CreateEnemies()
        {
            enemies = new List<Enemy>();

            Enemy enemy = new Enemy(500, 500);
            
            enemies.Add(enemy);

            player.SetEnemies(enemies);
        }
    }
}
