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

        List<Player> players = new List<Player>();

        List<Enemy> enemies;
        List<Vector4> DeadEnemies = new List<Vector4>();

        private bool PlayerActive;

        Texture2D grid;
        ContentManager content;

        //Property's
        public Vector2 mapDimensions { get; private set; }
        public int NumberLayers { get; private set; }

        int layer_player_num = 0;

        private const double RespawnTime = 0.5;

        public Map(string mapName)
        {
            mapLoader.LoadMap(mapName);

            Layers = mapLoader.GetLayers();
            mapDimensions = mapLoader.GetMapDimensions();
            NumberLayers = mapLoader.GetNumLayers();
            spriteSheets = mapLoader.GetSpritesheetList();

            for (int i = 0; i < ScreenManager.Instance.Controllers.Count; i++)
            {
                players.Add(new Player(i));
            }
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
            foreach (Player player in players)
                player.SetLayernumber(NumberLayers - layer_player_num);
            GetLayer("Collision", temp++);
            GetLayer("Zone", temp++);
            GetLayer("Doors", temp++);

            for (int l = layer_player_num; l < Layers.Count - 3; l++)
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
            foreach (Player player in players)
            {
                Vector2 temp = SetPlayerSpawn(player);
                player.LoadContent((int)(temp.X * GameSettings.Instance.Tilescale.X), (int)(temp.Y * GameSettings.Instance.Tilescale.Y));
            }

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
            foreach (Player player in players)
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
            foreach (Player player in players)
                player.Update(gameTime);

            foreach(Vector4 temp in DeadEnemies)
            {
                if (gameTime.TotalGameTime.TotalMinutes - temp.W >= RespawnTime)
                {
                    Database database = new Database();
                    Enemy enemy = new Enemy((int)temp.X, (int)temp.Y, (int)temp.Z, Convert.ToInt32(database.ReturnEnemyHP("'Wolf'")));
                    NumberLayers = mapLoader.GetNumLayers();
                    enemy.SetLayernumber(NumberLayers - layer_player_num);
                    int temp2 = 0;
                    GetLayerEnemy("Collision", temp2++, enemy);
                    GetLayerEnemy("Zone", temp2++, enemy);
                    GetLayerEnemy("Doors", temp2++, enemy);
                    for (int l = layer_player_num; l < Layers.Count - 3; l++)
                    {
                        GetLayerEnemy(Layers[l].Layername, temp2++, enemy);
                    }
                    enemy.LoadContent();
                    enemies.Add(enemy);
                    foreach (Player player in players)
                        player.AddEnemy(enemy);
                    DeadEnemies.Remove(temp);
                    break;
                }
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemy.healthbar.rectwidth == 1 && enemy.AnimationFinished == false && enemy.Die == false)
                {
                    enemy.Die = true;
                    players[enemy.LastHitBy].AddExp();
                    enemy.Time = gameTime.TotalGameTime.TotalMinutes;
                    DeadEnemies.Add(new Vector4(enemy.TilePosition, enemy.getZoneNR(), (float)gameTime.TotalGameTime.TotalMinutes));
                }
                if (enemy.AnimationFinished)
                {
                    enemy.UnloadContent();
                    foreach (Player player in players)
                        player.RemoveEnemy(enemy);
                    enemies.Remove(enemy);
                    break;
                }
                else
                {
                    enemy.PlayerState.Clear();
                    enemy.PlayerLookDirection.Clear();
                    enemy.PlayerPosition.Clear();
                    enemy.PlayerSprSheetX.Clear();
                    enemy.PlayerZones.Clear();
                    for (int i = 0; i < players.Count; i++)
                    {
                        enemy.PlayerPosition.Insert(i, players[i].GetPosition());
                        enemy.PlayerLookDirection.Insert(i, players[i].lookDirection);
                        enemy.PlayerState.Insert(i, players[i].State);
                        enemy.PlayerSprSheetX.Insert(i, (int)players[i].sprSheetX);
                        enemy.PlayerZones.Insert(i, players[i].InZone);

                        players[i].SendPosition(enemy.GetPosition());
                        players[i].EnemyLookDirection = enemy.lookDirection;
                        players[i].EnemyState = enemy.State;
                        players[i].EnemySprSheetX = (int)enemy.sprSheetX;
                    }
                    enemy.Update(gameTime);

                    foreach (Arrow arrow in enemy.HitArrows)
                    {
                        players[arrow.PlayerID].Arrows.Remove(arrow);
                        arrow.UnloadContent();
                    }

                    List<Arrow> temp = new List<Arrow>();
                    foreach (Player player in players)
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
                        if (Layers[l].Layername != "Collision" && Layers[l].Layername != "Player" && Layers[l].Layername != "Zone" && Layers[l].Layername != "Enemy" && Layers[l].Layername != "Doors")
                            Layers[l].DrawTile(spriteBatch, x, y);
                        if (Layers[l].Layername == "Player" && PlayerActive == false)
                        {
                            foreach (Enemy enemy in enemies)
                                if (!enemy.Die || (enemy.Die && !enemy.AnimationFinished))
                                    enemy.Draw(spriteBatch);

                            foreach (Player player in players)
                                player.Draw(spriteBatch);

                            PlayerActive = true;
                        }
                    }
                }
            }
            //player.Draw(spriteBatch);
            PlayerActive = false;
            foreach (Player player in players)
                player.DrawTop(spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.DrawTop(spriteBatch);

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

            if (players[0].Debug)
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

                Layer temp = Layers.Find(x => x.Layername == "Collision");
                for (int y = 0; y < mapDimensions.Y; y++)
                    for (int x = 0; x < mapDimensions.X; x++)
                        temp.DrawTile(spriteBatch, x, y);
            }
        }

        public void GetLayer(string Name, int number)
        {
            for (int l = 0; l < Layers.Count; l++)
            {
                if (Layers[l].Layername == Name)
                {
                    foreach (Player player in players)
                        player.SendLayer(Layers[l], number);
                    foreach (Enemy enemy in enemies)
                        enemy.SendLayer(Layers[l], number);
                }
            }
        }

        private void GetLayerEnemy(string Name, int number, Enemy enemy)
        {
            for (int l = 0; l < Layers.Count; l++)
            {
                if (Layers[l].Layername == Name)
                {
                    enemy.SendLayer(Layers[l], number);
                }
            }
        }

        public void CreateEnemies()
        {
            Database database = new Database();
            Layer temp = Layers.Find(x => x.Layername == "Enemy");
            enemies = new List<Enemy>();
            Enemy enemy;

            for (int x = 0; x < GameSettings.Instance.TileMapSize.X; x++)
                for (int y = 0; y < GameSettings.Instance.TileMapSize.Y; y++)
                    if (temp.getTileID(x, y) != 0)
                    {
                        enemy = new Enemy(x, y, temp.getTileID(x, y), Convert.ToInt32(database.ReturnEnemyHP("'Wolf'")));
                        enemies.Add(enemy);
                    }

            /*enemy = new Enemy((int)(20 * GameSettings.Instance.Tilescale.X), (int)(20 * GameSettings.Instance.Tilescale.Y), 2);
            enemies.Add(enemy);

            enemy = new Enemy((int)(50 * GameSettings.Instance.Tilescale.X), (int)(20 * GameSettings.Instance.Tilescale.Y), 3);
            enemies.Add(enemy);*/

            foreach (Player player in players)
                player.SetEnemies(enemies);
        }

        public Vector2 SetPlayerSpawn(Player player)
        {
            Layer temp = Layers.Find(x => x.Layername == "Player");
            int tempid = player.PlayerID + 1;

            for (int x = 0; x < GameSettings.Instance.TileMapSize.X; x++)
                for (int y = 0; y < GameSettings.Instance.TileMapSize.Y; y++)
                    if (temp.getTileID(x, y) == tempid)
                        return new Vector2(x, y);
            //player.SetPosition((int)(x * GameSettings.Instance.Tilescale.X), (int)(y * GameSettings.Instance.Tilescale.Y));
            return new Vector2(2, 2);
        }
    }
}
