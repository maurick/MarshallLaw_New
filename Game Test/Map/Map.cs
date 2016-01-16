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

        List<Player> player = new List<Player>();

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
            
            CreateEnemies();
            CreatePlayers();

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
            player[0].SetLayernumber(NumberLayers - layer_player_num);
            if (player.Count > 1)
                player[1].SetLayernumber(NumberLayers - layer_player_num);
            GetLayer("Collision", temp++);
            GetLayer("Zone", temp++);

            for (int l = layer_player_num; l < Layers.Count - 2; l++)
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
            player[0].LoadContent(32, 32);

            foreach (Enemy enemy in enemies)
                enemy.LoadContent();

            foreach (var layer in Layers)
            {
                layer.LoadContent();
            }

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            grid = content.Load<Texture2D>("Spritesheets/grid");
        }

        public virtual void UnloadContent()
        {
            player[0].UnloadContent();
            if (player.Count > 1)
                player[1].UnloadContent();

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
            player[0].Update(gameTime);
            if (player.Count > 1)
                player[1].Update(gameTime);
            foreach (Enemy enemy in enemies)
            {
                if (enemy.healthbar.rectwidth == 1 && enemy.AnimationFinished == false)
                {
                    enemy.Die = true;
                    enemy.DieAnimation(gameTime);
                }
                if (enemy.AnimationFinished)
                {
                    enemy.UnloadContent();
                    enemies.Remove(enemy);
                    break;
                }
                else
                {
                    enemy.PlayerState.Clear();
                    enemy.PlayerLookDirection.Clear();
                    enemy.PlayerPosition.Clear();
                    enemy.PlayerSprSheetX.Clear();
                    for (int i = 0; i < player.Count; i++)
                    {
                        enemy.PlayerPosition.Insert(i, player[i].GetPosition());
                        enemy.PlayerLookDirection.Insert(i, player[i].lookDirection);
                        enemy.PlayerState.Insert(i, player[i].State);
                        enemy.PlayerSprSheetX.Insert(i, (int)player[i].sprSheetX);

                        player[i].SendPosition(enemy.GetPosition());
                        player[i].EnemyLookDirection = enemy.lookDirection;
                        player[i].EnemyState = enemy.State;
                        player[i].EnemySprSheetX = (int)enemy.sprSheetX;
                    }
                    enemy.Update(gameTime);

                    List<Arrow> temp = new List<Arrow>();
                    foreach (Player player in player)
                        temp.AddRange(player.Arrows);
                    enemy.arrows = temp;
                }
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
                        if (Layers[l].Layername != "Collision" && Layers[l].Layername != "Player" && Layers[l].Layername != "Zone")
                            Layers[l].DrawTile(spriteBatch, x, y);
                        if (Layers[l].Layername == "Player" && PlayerActive == false)
                        {
                            foreach (Enemy enemy in enemies)
                                enemy.Draw(spriteBatch);

                            foreach(Player player in player)
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
                }
            }

            if (player[0].Debug)
            {
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

                for (int y = 0; y < mapDimensions.Y; y++)
                    for (int x = 0; x < mapDimensions.X; x++)
                        Layers[12].DrawTile(spriteBatch, x, y);
            }
        }

        public void GetLayer(string Name, int number)
        {
            for (int l = 0; l < Layers.Count; l++)
            {
                if (Layers[l].Layername == Name)
                { 
                    foreach (Player player in player)
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

            foreach(Player player in player)
                player.SetEnemies(enemies);
        }

        public void CreatePlayers()
        {
            Player player = new Player();
            this.player.Add(player);
        }
    }
}
