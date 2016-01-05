using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game_Test
{
    public class Player
    {
        //Playerstats player;

        private Vector2 EnemyPosition;

        private float SpeedScale; //Scales up the movementspeed

        private float sprSheetX;
        
        private Vector2 direction;

        //Slow down animation speed
        private float Interval = 0.125f;

        //Collisionlayer and Tree layer(s)
        Layer[] layer;

        Image boundingBox;
        
        private PlayerEnums.Action sprSheetY { get; set; }
        public PlayerEnums.ActionState State { get; set; }
        public PlayerEnums.ActionState EnemyState { get; set; }
        public PlayerEnums.LookDirection lookDirection { get; set; }
        public PlayerEnums.LookDirection EnemyLookDirection { get; set; }

        private List<Enemy> enemies = new List<Enemy>();
        
        private SprSheetImage sprite;

        private Weapon weapon;

        private bool knockback;
        private double knockbacktimer;
        private Vector2 knockbackdirection;

        public Player()
        {
            //TODO add playerstats
            //this.player = player;
            State = PlayerEnums.ActionState.None;
            lookDirection = PlayerEnums.LookDirection.Down;
            sprSheetY = PlayerEnums.Action.None;
            sprSheetX = 0;
            
            direction = new Vector2(0, 1);
            
            sprite = new SprSheetImage("Character/light");
            
            SpeedScale = 1.5f;

            weapon = new Weapon();
        }

        public void LoadContent(int X, int Y)
        {
            sprite.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
            weapon.LoadContent(X, Y);

            boundingBox = new Image("Images/green");
            boundingBox.LoadContent(X + (0.5f * GameSettings.Instance.Tilescale.X), Y + GameSettings.Instance.Tilescale.Y, false, new Vector2(GameSettings.Instance.Tilescale.X, GameSettings.Instance.Tilescale.Y));
        }

        public void UnloadContent()
        {
            sprite.UnloadContent();
            weapon.UnloadContent();
            boundingBox.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Vector2 temp;
            foreach (Enemy enemy in enemies)
            {
                temp = CheckHit();
                if (temp.X == 1)
                {
                    //TODO
                    //Lose health
                    knockback = true;
                    knockbacktimer = 0.2f;
                    knockbackdirection.X = 0;
                    sprSheetY = PlayerEnums.Action.Hit;
                    sprSheetX = 4;
                    knockbackdirection = temp;

                    return;
                }
            }

            #region Knockback
            if (knockback)
            {
                SpeedScale = 2.0f;
                switch ((int)knockbackdirection.Y)
                {
                    case 1:
                        Move(direction = new Vector2(0, -1), gameTime);
                        break;
                    case 2:
                        Move(direction = new Vector2(-1, 0), gameTime);
                        break;
                    case 3:
                        Move(direction = new Vector2(0, 1), gameTime);
                        break;
                    case 4:
                        Move(direction = new Vector2(1, 0), gameTime);
                        break;
                }

                knockbacktimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (knockbacktimer <= 0)
                {
                    knockback = false;
                    knockbackdirection.Y = 0;
                }
                else
                {
                    SetAnimationFrame();
                    sprite.Update(gameTime);
                    weapon.Update(gameTime);
                    return;
                }
            }
            #endregion
            //controller.Update();
            //Check if keys are pressed
            if (InputManager.Instance.KeyDown(Keys.Space))
            {
                if (State != PlayerEnums.ActionState.Thrust)
                    State = PlayerEnums.ActionState.Thrust;
            }
            #region Movement
            else
            {
                
                if (InputManager.Instance.KeyDown(Keys.W))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Up;
                    direction.Y = -1;
                }
                if (InputManager.Instance.KeyDown(Keys.S))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Down;
                    direction.Y = 1;
                }
                if (InputManager.Instance.KeyDown(Keys.A))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.left;
                    direction.X = -1;
                }
                if (InputManager.Instance.KeyDown(Keys.D))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Right;
                    direction.X = 1;
                }
            }
            #endregion

            #region Keyreleased
            if (/*!ControlsActive ||*/ (InputManager.Instance.KeyReleased(Keys.W) || InputManager.Instance.KeyReleased(Keys.A) || InputManager.Instance.KeyReleased(Keys.S) || InputManager.Instance.KeyReleased(Keys.D)) && InputManager.Instance.KeyDown(Keys.Space) == false || InputManager.Instance.KeyReleased(Keys.Space))
            {
                State = PlayerEnums.ActionState.None;
                sprSheetY = PlayerEnums.Action.None;
                sprSheetX = 0;
                sprite.SprSheetX = 0;
                direction = new Vector2(0, 0);
            }
            #endregion
            else if (State == PlayerEnums.ActionState.Walk)
                Move(direction, gameTime);
            else if (State == PlayerEnums.ActionState.Thrust)
                Attack(gameTime);

            SetAnimationFrame();
            sprite.Update(gameTime);
            weapon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            boundingBox.Draw(spriteBatch);
            sprite.Draw(spriteBatch);
            weapon.Draw(spriteBatch);
            
        }

        private void Attack(GameTime gameTime)
        {
            switch (lookDirection)
            {
                case PlayerEnums.LookDirection.Up:
                    if (sprSheetY == PlayerEnums.Action.SpearUp)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = PlayerEnums.Action.SpearUp;
                    }
                    break;
                case PlayerEnums.LookDirection.left:
                    if (sprSheetY == PlayerEnums.Action.SpearLeft)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = PlayerEnums.Action.SpearLeft;
                    }
                    break;
                case PlayerEnums.LookDirection.Down:
                    if (sprSheetY == PlayerEnums.Action.SpearDown)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = PlayerEnums.Action.SpearDown;
                    }
                    break;
                case PlayerEnums.LookDirection.Right:
                    if (sprSheetY == PlayerEnums.Action.SpearRight)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = PlayerEnums.Action.SpearRight;
                    }
                    break;
            }
        }

        private void Move(Vector2 direction, GameTime gameTime)
        {
            float dirX = direction.X,
            dirY = direction.Y;

            //Scale the movement
            dirX *= SpeedScale * (32 / GameSettings.Instance.Tilescale.X);
            dirY *= SpeedScale * (32 / GameSettings.Instance.Tilescale.X);

            /*bool CollisionY = CheckCollision(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), (int)direction.Y);
            bool CollisionX = CheckCollision(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), (int)direction.X + 1);*/
            Vector2 Collision = CheckCollision(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), sprite.Position, direction);
            if (Collision.X == 0)
                dirX = 0;
            if (Collision.Y == 0)
                dirY = 0;
            
            //change sprSheetX and sprSheetY based on previous movement direction
            if (direction.Y == -1)//up
            {
                if (sprSheetY == PlayerEnums.Action.WalkUp)
                    UpdateAnimationFrame(gameTime);
                else if (direction.X == 0)
                {
                    sprSheetY = PlayerEnums.Action.WalkUp;
                }
            }
            if (direction.Y == 1)//down
            {
                if (sprSheetY == PlayerEnums.Action.WalkDown)
                    UpdateAnimationFrame(gameTime);
                else if(direction.X == 0)
                {
                    sprSheetY = PlayerEnums.Action.WalkDown;
                }
            }
            if (direction.X == -1)//left
            {
                if (sprSheetY == PlayerEnums.Action.WalkLeft)
                    UpdateAnimationFrame(gameTime);
                else
                {
                    sprSheetY = PlayerEnums.Action.WalkLeft;
                }
            }
            if (direction.X == 1)//right
            {
                if (sprSheetY == PlayerEnums.Action.WalkRight)
                    UpdateAnimationFrame(gameTime);
                else
                {
                    sprSheetY = PlayerEnums.Action.WalkRight;
                }
            }
            
            for (int l = 2; l < layer.Length; l++)
                ChangeAlpha(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), l);
            sprite.Position = new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY); //Set new position
            boundingBox.Position = new Vector2(boundingBox.Position.X + dirX, boundingBox.Position.Y + dirY);
            weapon.setPosition(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY)); //Move weapon with you
        }

        private Vector2 CheckCollision(Vector2 PositionNew, Vector2 PositionOld, Vector2 Direction)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;

            int xnew = (int)((PositionNew.X + 0.5 * tilescale_x) / tilescale_x),
            xold = (int)((PositionOld.X + 0.5 * tilescale_x) / tilescale_x),
            ynew = (int)((PositionNew.Y + tilescale_y) / tilescale_y),
            yold = (int)((PositionOld.Y + tilescale_y) / tilescale_y);

            int[] x1 = new int[2], x2 = new int[2], y1 = new int[2], y2 = new int[2];
            x1[0] = xnew;
            x1[1] = xnew;
            x2[0] = xold;
            x2[1] = xold;

            y1[0] = ynew;
            y1[1] = ynew;
            y2[0] = yold;
            y2[1] = yold;

            Vector2 temp = new Vector2(1, 1);
            Vector2 returnvalue = new Vector2(1, 1);

            Rectangle playerRectHor = new Rectangle(new Point((int)(PositionNew.X + 0.5 * tilescale_x), (int)(PositionOld.Y + tilescale_y)), new Point((int)tilescale_x, (int)(tilescale_y))),
            playerRectVer = new Rectangle(new Point((int)(PositionOld.X + 0.5 * tilescale_x), (int)(PositionNew.Y + tilescale_y)), new Point((int)tilescale_x, (int)(tilescale_y)));

            if (Direction.X == -1)//links
            {
                returnvalue.X = -1;
                if (playerRectHor.Y % tilescale_y != 0)
                    y2[1]++;
                temp = CheckCollision2(playerRectHor, x1, y2);
                if (temp.X == 1 || temp.Y == 1)
                    returnvalue.X = 0;
            }
            if (Direction.X == 1)//rechts
            {
                x1[0]++;
                x1[1]++;
                if (playerRectHor.Y % tilescale_y != 0)
                    y2[1]++;
                temp = CheckCollision2(playerRectHor, x1, y2);
                if (temp.X == 1 || temp.Y == 1)
                    returnvalue.X = 0;
            }
            if (Direction.Y == -1)//omhoog
            {
                returnvalue.Y = -1;
                if (playerRectVer.X % tilescale_x != 0)
                    x2[1]++;
                temp = CheckCollision2(playerRectVer, x2, y1);
                if (temp.X == 1 || temp.Y == 1)
                    returnvalue.Y = 0;
            }
            if (Direction.Y == 1)//omlaag
            {
                if (playerRectVer.X % tilescale_x != 0)
                    x2[1]++;
                y1[0]++;
                y1[1]++;
                temp = CheckCollision2(playerRectVer, x2, y1);
                if (temp.X == 1 || temp.Y == 1)
                    returnvalue.Y = 0;
            }

            return returnvalue;
        }
        
        private Vector2 CheckCollision2(Rectangle playerRect, int[] x, int[] y)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            int TileID;
            Rectangle rect;
            int temp1 = 0, temp2 = 0;

            TileID = layer[0].getTileID(x[0], y[0]);
            if (TileID != 0)
            {
                rect = new Rectangle((x[0]) * (int)tilescale_x, (y[0]) * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                if (rect.Intersects(playerRect))
                {
                    temp1 = 1;
                }
            }

            TileID = layer[0].getTileID(x[1], y[1]);
            if (TileID != 0)
            {
                rect = new Rectangle((x[1]) * (int)tilescale_x, (y[1]) * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                if (rect.Intersects(playerRect))
                {
                    temp2 = 1;
                }
            }

            return new Vector2(temp1, temp2);
        }

        private void ChangeAlpha(Vector2 position, int number)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;

            int x1 = (int)(position.X / tilescale_x),
            y1 = (int)(position.Y / tilescale_y),
            x2 = (int)((position.X + 2 * tilescale_x) / tilescale_x),
            y2 = (int)((position.Y + 2 * tilescale_y) / tilescale_y);

            Rectangle playerRect = new Rectangle(new Point((int)(position.X) + 12, (int)(position.Y + 10)), new Point((int)tilescale_x, (int)(tilescale_y + 12)));

            int TileID;

            for (int i = y1; i < y2 + 1; i++)
            {
                for (int j = x1; j < x2 + 1; j++)
                {
                    TileID = layer[number].getTileID(j, i);
                    Rectangle rect;
                    if (TileID != 0)
                    {
                        rect = new Rectangle(j * (int)tilescale_x, i * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                        if (rect.Intersects(playerRect))
                            layer[number].ChangeTileAlpha(j, i, 0.5f);
                        else if (layer[number].GetTileAlpha(j, i) != 1.0f)
                            layer[number].ChangeTileAlpha(j, i, 1.0f);
                    }
                }
            }
        }

        public void SendLayer(Layer layer, int number)
        {
            this.layer[number] = layer;
        }

        public void SetLayernumber(int number)
        {
            layer = new Layer[number];
        }

        private void UpdateAnimationFrame(GameTime gameTime)
        {
            sprSheetX += (float)gameTime.ElapsedGameTime.TotalMilliseconds / gameTime.ElapsedGameTime.Milliseconds * Interval;
            
            //Reset X at the final animation frame
            if ((int)sprSheetX >= (int)State)
                sprSheetX = 0;
        }
        
        private void SetAnimationFrame()
        {
            sprite.SprSheetX = (int)sprSheetX;
            weapon.SprSheetX = (int)sprSheetX;

            if (sprSheetY != PlayerEnums.Action.None)
            {
                sprite.SprSheetY = (int)sprSheetY;
                weapon.SprSheetY = (int)sprSheetY;
            }
        }

        public Vector2 GetPosition()
        {
            return sprite.Position;
        }

        private Vector2 CheckHit()
        {
            Vector2 returnvalue = new Vector2(0, 0);
            Rectangle Playerrect = new Rectangle(new Point((int)(sprite.Position.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)(sprite.Position.Y + GameSettings.Instance.Tilescale.Y)), new Point((int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y))),
                Enemyrect = new Rectangle((int)(EnemyPosition.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)(EnemyPosition.Y + GameSettings.Instance.Tilescale.Y), (int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y));
            if (EnemyState == PlayerEnums.ActionState.Thrust)
            {
                switch (EnemyLookDirection)
                {
                    case PlayerEnums.LookDirection.Up:
                        Enemyrect.X = (int)GameSettings.Instance.Tilescale.X / 4;
                        Enemyrect.Height = 2 * (int)GameSettings.Instance.Tilescale.Y / 4;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 1);
                        break;
                    case PlayerEnums.LookDirection.left:
                        Enemyrect.Y = (int)GameSettings.Instance.Tilescale.Y / 4;
                        Enemyrect.Width = 2 * (int)GameSettings.Instance.Tilescale.X / 4;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 2);
                        break;
                    case PlayerEnums.LookDirection.Down:
                        Enemyrect.Height = 2 * (int)GameSettings.Instance.Tilescale.Y / 4;
                        Enemyrect.Y = (int)(EnemyPosition.Y + (2 * GameSettings.Instance.Tilescale.Y - 0.5 * Playerrect.Height));
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 3);
                        break;
                    case PlayerEnums.LookDirection.Right:
                        Enemyrect.Width = 2 * (int)GameSettings.Instance.Tilescale.X / 4;
                        Enemyrect.X = (int)(EnemyPosition.X + (1.5 * GameSettings.Instance.Tilescale.X - 0.5 * Playerrect.Width));
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 4);
                        break;
                }
            }
            return returnvalue;
        }

        public void SendPosition(Vector2 position)
        {
            EnemyPosition = position;
        }

        public void SetEnemies(List<Enemy> enemies)
        {
            this.enemies.Clear();
            this.enemies = enemies;            
        }
    }
}
