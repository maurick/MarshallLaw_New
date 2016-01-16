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

        public int PlayerID { get; private set; }

        private Vector2 EnemyPosition;

        private float SpeedScale; //Scales up the movementspeed
        
        private Vector2 direction;

        //Slow down animation speed
        private const float BaseInterval = 0.15f;
        private float Interval = 0.15f;

        //Collisionlayer and Tree layer(s)
        Layer[] layer;

        Image boundingBox;

        public float sprSheetX { get; private set; }
        private PlayerEnums.Action sprSheetY { get; set; }
        public PlayerEnums.ActionState State { get; private set; }
        public PlayerEnums.ActionState EnemyState { get; set; }
        public PlayerEnums.LookDirection lookDirection { get; private set; }
        public PlayerEnums.LookDirection EnemyLookDirection { get; set; }
        public int EnemySprSheetX { get; set; }

        private List<Weapon> inventory = new List<Weapon>();
        private Weapon CurrentWeapon;

        private List<Enemy> enemies = new List<Enemy>();
        
        private SprSheetImage sprite;

        private bool knockback;
        private double knockbacktimer;
        private Vector2 knockbackdirection;

        public List<Arrow> Arrows = new List<Arrow>();
        private string ArrowPath = "Weapons/Arrows/arrow_flying";
        private bool RecentlyShot = false;

        public bool Debug { get; private set; }

        private Healthbar Healthbar;

        private Arduino Controller;
        public bool NoConnect;

        private bool ZoneCheck = false;
        private Vector2 ZoneCheckCoor;
        private bool ZoneEnter = false;
        public int InZone { get; private set; }

        public Player(int controller)
        {
            PlayerID = controller;
            NoConnect = false;
            if (ScreenManager.Instance.Controllers[controller] != null)
            {
                //TODO add playerstats
                //this.player = player;
                State = PlayerEnums.ActionState.None;
                lookDirection = PlayerEnums.LookDirection.Down;
                sprSheetY = PlayerEnums.Action.None;
                sprSheetX = 0;


                Controller = ScreenManager.Instance.Controllers[controller];

                direction = new Vector2(0, 1);

                sprite = new SprSheetImage("CharacterSprites/Male/SkinColor/Light");

                SpeedScale = 1.5f;

                Healthbar = new Healthbar();
            }else
            {
                NoConnect = true;
            }
        }

        public void LoadContent(int X, int Y)
        {
            sprite.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));

            boundingBox = new Image("Images/green");
            boundingBox.LoadContent( X + (0.5f * GameSettings.Instance.Tilescale.X), Y + GameSettings.Instance.Tilescale.Y, false, new Vector2(GameSettings.Instance.Tilescale.X, GameSettings.Instance.Tilescale.Y));

            CurrentWeapon = new Weapon("Weapons/Bow/bow", "Weapons/Bow/quiver", "Weapons/Bow/arrow", sprite.Position,1, this);
            inventory.Add(CurrentWeapon);
            Weapon tempweapon = new Weapon("Weapons/Spear/Male/spear_male", PlayerEnums.Weapontype.Spear, sprite.Position,2, this);
            inventory.Add(tempweapon);
        }

        public void UnloadContent()
        {
            sprite.UnloadContent();
            foreach (Weapon weapon in inventory)
                weapon.UnloadContent();
            boundingBox.UnloadContent();
            foreach (Arrow arrow in Arrows)
                arrow.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            #region Knockback
            Vector2 temp;
            foreach (Enemy enemy in enemies)
            {
                temp = CheckHit();
                if (temp.X == 1 && knockback == false)
                {
                    Healthbar.LoseHealth(2 * GameSettings.Instance.Tilescale.X * 0.1f);
                    knockback = true;
                    knockbacktimer = 0.2f;
                    knockbackdirection.X = 0;
                    sprSheetY = PlayerEnums.Action.Hit;
                    sprSheetX = 4;
                    knockbackdirection = temp;

                    return;
                }
            }
            
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
                    CurrentWeapon.Update(gameTime);
                    return;
                }
            }
            #endregion
            //controller.Update();
            //Check if keys are pressed
            #region Attack
            if (InputManager.Instance.KeyDown(Keys.Space) || Controller.dbutt(false))
            {
                switch (CurrentWeapon.weapontype)
                {
                    case PlayerEnums.Weapontype.Spear:
                        if (!(State == PlayerEnums.ActionState.Thrust))
                            State = PlayerEnums.ActionState.Thrust;
                        break;
                    case PlayerEnums.Weapontype.Sword:
                        if (!(State == PlayerEnums.ActionState.Slash))
                            State = PlayerEnums.ActionState.Slash;
                        break;
                    case PlayerEnums.Weapontype.Bow:
                        if (!(State == PlayerEnums.ActionState.Shoot))
                            State = PlayerEnums.ActionState.Shoot;
                        break;
                }
            }
            #endregion
            #region Movement
            else
            {
                if (InputManager.Instance.KeyDown(Keys.W) || Controller.Up(false))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Up;
                    direction.Y = -1;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                }
                if (InputManager.Instance.KeyDown(Keys.S) || Controller.Down(false))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Down;
                    direction.Y = 1;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                }
                if (InputManager.Instance.KeyDown(Keys.A) || Controller.Left(false))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.left;
                    direction.X = -1;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                }
                if (InputManager.Instance.KeyDown(Keys.D) || Controller.Right(false))
                {
                    State = PlayerEnums.ActionState.Walk;
                    lookDirection = PlayerEnums.LookDirection.Right;
                    direction.X = 1;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                }
            }
            #endregion

            #region Keyreleased
            if (InputManager.Instance.KeyReleased(Keys.Space))
            {
                RecentlyShot = false;
                State = PlayerEnums.ActionState.None;
                sprSheetY = PlayerEnums.Action.None;
                sprSheetX = 0;
                sprite.SprSheetX = 0;
                direction = new Vector2(0, 0);
            }

            bool ControlsActive = false;
            for (int i = 0; i < 8; i++)
            {
                    if (Controller.prevbuttons[i] == '1' && !Controller.Button(i))
                    {
                    ControlsActive = true;
                    }
            
            }


            if (ControlsActive || (InputManager.Instance.KeyReleased(Keys.W) || InputManager.Instance.KeyReleased(Keys.A) || InputManager.Instance.KeyReleased(Keys.S) || InputManager.Instance.KeyReleased(Keys.D)) && InputManager.Instance.KeyDown(Keys.Space) == false)
            {
                State = PlayerEnums.ActionState.None;
                sprSheetY = PlayerEnums.Action.None;
                sprSheetX = 0;
                sprite.SprSheetX = 0;
                direction = new Vector2(0, 0);
            }
            #endregion

            #region SwitchWeapon
            if (InputManager.Instance.KeyPressed(Keys.LeftShift) || Controller.lbutt(true))
            {
                int tempID = 1;
                if (CurrentWeapon.WeaponID != CurrentWeapon.GetMaxID)
                    tempID = CurrentWeapon.WeaponID + 1;
                CurrentWeapon = this.inventory.Find(x => x.WeaponID == tempID);
                CurrentWeapon.setPosition(sprite.Position);
            }
            #endregion
            #region DebugMode
            if (InputManager.Instance.KeyPressed(Keys.Tab))
            {
                Debug = !Debug;
            }
            #endregion
            else if (State == PlayerEnums.ActionState.Walk)
            {
                Move(direction, gameTime);
                CheckZone(sprite.Position);
            }
            else if (State == PlayerEnums.ActionState.Thrust || State == PlayerEnums.ActionState.Slash || State == PlayerEnums.ActionState.Shoot)
                Attack(gameTime);
            
            SetAnimationFrame();
            if (Debug)
                boundingBox.Update(gameTime);
            sprite.Update(gameTime);
            CurrentWeapon.Update(gameTime);

            foreach (Arrow arrow in Arrows)
            {
                arrow.Update(gameTime);
                if (arrow.CheckCollision(layer[0]))
                    if (Arrows.Remove(arrow))
                        break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Debug)
                boundingBox.Draw(spriteBatch);
            if (CurrentWeapon.Quiver)
                CurrentWeapon.DrawQuiver(spriteBatch);
            sprite.Draw(spriteBatch);
            CurrentWeapon.Draw(spriteBatch);
            foreach (Arrow arrow in Arrows)
                arrow.Draw(spriteBatch);
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            Healthbar.Draw(spriteBatch, sprite.Position);
        }

        private void Attack(GameTime gameTime)
        {
            Arrow arrow;
            PlayerEnums.Action up = PlayerEnums.Action.None, left = PlayerEnums.Action.None, down = PlayerEnums.Action.None, right = PlayerEnums.Action.None;
            switch (CurrentWeapon.weapontype)
            {
                case PlayerEnums.Weapontype.Spear:
                    up = PlayerEnums.Action.SpearUp;
                    left = PlayerEnums.Action.SpearLeft;
                    down = PlayerEnums.Action.SpearDown;
                    right = PlayerEnums.Action.SpearRight;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                    break;
                case PlayerEnums.Weapontype.Sword:
                    up = PlayerEnums.Action.SlashUp;
                    left = PlayerEnums.Action.SlashLeft;
                    down = PlayerEnums.Action.SlashDown;
                    right = PlayerEnums.Action.SlashRight;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                    break;
                case PlayerEnums.Weapontype.Bow:
                    up = PlayerEnums.Action.ShootUp;
                    left = PlayerEnums.Action.ShootLeft;
                    down = PlayerEnums.Action.ShootDown;
                    right = PlayerEnums.Action.ShootRight;
                    Interval = BaseInterval * (int)State / (int)PlayerEnums.ActionState.Walk;
                    break;
            }
            switch (lookDirection)
            {
                case PlayerEnums.LookDirection.Up:
                    if (sprSheetY == up)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = up;
                    }
                    if (CurrentWeapon.weapontype == PlayerEnums.Weapontype.Bow && sprSheetX >= (int)PlayerEnums.ActionState.Shoot - 3 && RecentlyShot == false)
                    {
                        arrow = new Arrow(ArrowPath, new Vector2(sprite.Position.X, sprite.Position.Y), 2, 1, PlayerID);
                        Arrows.Add(arrow);
                        RecentlyShot = true;
                    }
                    break;
                case PlayerEnums.LookDirection.left:
                    if (sprSheetY == left)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = left;
                    }
                    if (CurrentWeapon.weapontype == PlayerEnums.Weapontype.Bow && sprSheetX >= (int)PlayerEnums.ActionState.Shoot - 3 && RecentlyShot == false)
                    {
                        arrow = new Arrow(ArrowPath, new Vector2(sprite.Position.X, sprite.Position.Y + GameSettings.Instance.Tilescale.Y / 4), 1, 2, PlayerID);
                        Arrows.Add(arrow);
                        RecentlyShot = true;
                    }
                    break;
                case PlayerEnums.LookDirection.Down:
                    if (sprSheetY == down)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = down;
                    }
                    if (CurrentWeapon.weapontype == PlayerEnums.Weapontype.Bow && sprSheetX >= (int)PlayerEnums.ActionState.Shoot - 3 && RecentlyShot == false)
                    {
                        arrow = new Arrow(ArrowPath, new Vector2(sprite.Position.X, sprite.Position.Y), 2, 2, PlayerID);
                        Arrows.Add(arrow);
                        RecentlyShot = true;
                    }
                    break;
                case PlayerEnums.LookDirection.Right:
                    if (sprSheetY == right)
                        UpdateAnimationFrame(gameTime);
                    else
                    {
                        sprSheetY = right;
                    }
                    if (sprSheetX == 12)
                    {
                        break;
                    }
                    if (CurrentWeapon.weapontype == PlayerEnums.Weapontype.Bow && sprSheetX >= (int)PlayerEnums.ActionState.Shoot - 3 && RecentlyShot == false)
                    {
                        arrow = new Arrow(ArrowPath, new Vector2(sprite.Position.X, sprite.Position.Y + GameSettings.Instance.Tilescale.Y / 4), 1, 1, PlayerID);
                        Arrows.Add(arrow);
                        RecentlyShot = true;
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
                else if (direction.X == 0 && knockback == false)
                {
                    sprSheetY = PlayerEnums.Action.WalkUp;
                }
            }
            if (direction.Y == 1)//down
            {
                if (sprSheetY == PlayerEnums.Action.WalkDown)
                    UpdateAnimationFrame(gameTime);
                else if(direction.X == 0 && knockback == false)
                {
                    sprSheetY = PlayerEnums.Action.WalkDown;
                }
            }
            if (direction.X == -1)//left
            {
                if (sprSheetY == PlayerEnums.Action.WalkLeft)
                    UpdateAnimationFrame(gameTime);
                else if (!(knockback))
                {
                    sprSheetY = PlayerEnums.Action.WalkLeft;
                }
            }
            if (direction.X == 1)//right
            {
                if (sprSheetY == PlayerEnums.Action.WalkRight)
                    UpdateAnimationFrame(gameTime);
                else if (!(knockback))
                {
                    sprSheetY = PlayerEnums.Action.WalkRight;
                }
            }
            
            for (int l = 2; l < layer.Length; l++)
                ChangeAlpha(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), l);
            sprite.Position = new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY); //Set new position
            boundingBox.Position = new Vector2(boundingBox.Position.X + dirX, boundingBox.Position.Y + dirY);
            CurrentWeapon.setPosition(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY)); //Move weapon with you
        }

        #region CheckCollision
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
            int temp1 = 0, temp2 = 0;
            
            temp1 = CheckCollision3(x[0], y[0], playerRect);
            temp2 = CheckCollision3(x[1], y[1], playerRect);

            return new Vector2(temp1, temp2);
        }

        private int CheckCollision3(int x, int y, Rectangle playerRect)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            Rectangle rect;
            int temp = 0;
            int TileID = layer[0].getTileID(x, y);

            if (TileID != 0)
            {
                switch (TileID)
                {
                    case 1677: //Full
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1678: //Tophalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1679: //Bottomhalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1680: //Diagonallefttoright
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1681: //Diagonalrighttoleft
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1683: //Righthalf
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1684: //Lefttopcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1685: //Righttopcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1686: //Lefttop
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1687: //Righttop
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1689: //Lefthalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1690: //Leftbottomcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1691: //Rightbottomcorner
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1692: //Leftbottom
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1693: //Rightbottom
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1694: //Bridgeright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1688: //Bridgeleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1695: //Bridgebottomright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y + (int)(tilescale_y * 0.5), (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1696: //Bridgebottomleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_y * 0.5), (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1697: //Bridgetopright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1698: //Bridgetopleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;

                }
            }
            return temp;
        }

        private int CheckCollision4(Rectangle rect, Rectangle playerRect)
        {
            if (rect.Intersects(playerRect))
            {
                return 1;
            }
            else return 0;
        }
#endregion

        private void ChangeAlpha(Vector2 Position, int number)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;

            int x1 = (int)(Position.X / tilescale_x),
            y1 = (int)(Position.Y / tilescale_y),
            x2 = (int)((Position.X + 2 * tilescale_x) / tilescale_x),
            y2 = (int)((Position.Y + 2 * tilescale_y) / tilescale_y);

            Rectangle PlayerRect = new Rectangle(new Point((int)(Position.X + 0.5 * tilescale_x), (int)(Position.Y + 0.5 * tilescale_y)), new Point((int)tilescale_x, (int)(tilescale_y * 2 - 0.5 * tilescale_y)));

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
                        if (rect.Intersects(PlayerRect))
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
            {
                sprSheetX = 0;
                if (RecentlyShot == true)
                    RecentlyShot = false;
            }
            
        }
        
        private void SetAnimationFrame()
        {
            sprite.SprSheetX = (int)sprSheetX;
            CurrentWeapon.SprSheetX = (int)sprSheetX;

            if (sprSheetY != PlayerEnums.Action.None)
            {
                sprite.SprSheetY = (int)sprSheetY;
                CurrentWeapon.SprSheetY = (int)sprSheetY;
            }
        }

        public Vector2 GetPosition()
        {
            return sprite.Position;
        }

        private Vector2 CheckHit()
        {
            Vector2 returnvalue = new Vector2(0, 0);
            Rectangle Playerrect = new Rectangle(new Point((int)(sprite.Position.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)sprite.Position.Y), new Point((int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y * 2))),
                Enemyrect = new Rectangle((int)(EnemyPosition.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)(EnemyPosition.Y + GameSettings.Instance.Tilescale.Y), (int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y));
            if (EnemyState == PlayerEnums.ActionState.Thrust && EnemySprSheetX == 5)
            {
                switch (EnemyLookDirection)
                {
                    case PlayerEnums.LookDirection.Up:
                        Enemyrect.Y -= (int)GameSettings.Instance.Tilescale.Y / 4;
                        Enemyrect.Height = (int)GameSettings.Instance.Tilescale.Y;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 1);
                        break;
                    case PlayerEnums.LookDirection.left:
                        Enemyrect.X -= (int)GameSettings.Instance.Tilescale.X / 4;
                        Enemyrect.Width = (int)GameSettings.Instance.Tilescale.X;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 2);
                        break;
                    case PlayerEnums.LookDirection.Down:
                        Enemyrect.Height = (int)GameSettings.Instance.Tilescale.Y;
                        Enemyrect.Y = (int)(EnemyPosition.Y + (2 * GameSettings.Instance.Tilescale.Y - 0.5 * Playerrect.Height));
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 3);
                        break;
                    case PlayerEnums.LookDirection.Right:
                        Enemyrect.Width = (int)GameSettings.Instance.Tilescale.X;
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

        private void CheckZone(Vector2 Position)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;

            int x1 = (int)((Position.X + 0.5 * tilescale_x) / tilescale_x),
            y1 = (int)((Position.Y + tilescale_y) / tilescale_y),
            x2 = (int)((Position.X + 1.5 * tilescale_x) / tilescale_x),
            y2 = (int)((Position.Y + 2 * tilescale_y) / tilescale_y);

            Rectangle PlayerRect = new Rectangle(new Point((int)(Position.X + 0.5 * tilescale_x), (int)(Position.Y + tilescale_y)), new Point((int)tilescale_x, (int)(tilescale_y)));

            int TileID;

            Rectangle temp;
            if (ZoneCheckCoor != Vector2.Zero)
            {
                temp = new Rectangle((int)ZoneCheckCoor.X, (int)ZoneCheckCoor.Y, (int)tilescale_x, (int)tilescale_y);
                if (!(temp.Intersects(PlayerRect)))
                {
                    ZoneCheck = false;
                    ZoneCheckCoor = Vector2.Zero;
                }
            }

            for (int i = y1; i <= y2; i++)
            {
                for (int j = x1; j <= x2; j++)
                {
                    TileID = layer[1].getTileID(j, i);
                    Rectangle rect;
                    if (TileID != 0)
                    {
                        rect = new Rectangle(j * (int)tilescale_x, i * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                        if (TileID == 1)
                        {
                            if (rect.Intersects(PlayerRect) && ZoneCheck == false)
                            {
                                ZoneCheck = true;
                                ZoneCheckCoor = new Vector2(j, i);
                                if (InZone == 0)
                                    ZoneEnter = true;
                                if (InZone != 0)
                                {
                                    InZone = 0;
                                }
                            }
                        }
                        else
                        {
                            if (ZoneEnter && rect.Intersects(PlayerRect))
                                InZone = TileID;
                        }
                    }
                }
            }
        }

        public void AddExp()
        {
            /*
            string query;
            int temp = 0, temp2 = 0;
            //temp = get exp
            if (temp > 100)
            {
                temp -= 100;
                //temp2 = get level
                query = "update playerstats set level" + temp2 + "where playerid = " + PlayerID + ";";
            }
            query = "update playerstats set exp" + temp + "where playerid = " + PlayerID + ";";
            Database.Instance.ExecuteQuery(query);
            */
        }
    }
}
