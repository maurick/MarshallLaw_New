using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game_Test
{
    public class Enemy
    {
        //Playerstats player;

        private Vector2 Position;
        public Vector2 TilePosition { get; set; }

        private float SpeedScale; //Scales up the movementspeed
        
        private Vector2 direction;

        //Slow down animation speed
        private float Interval = 0.125f;

        //Collisionlayer and Tree layer(s)
        Layer[] layer;

        public List<Vector2> PlayerPosition { get; set; }
        public float sprSheetX { get; private set; }
        private PlayerEnums.Action sprSheetY { get; set; }
        public PlayerEnums.ActionState State { get; set; }
        public List<PlayerEnums.ActionState> PlayerState { get; set; }
        public PlayerEnums.LookDirection lookDirection { get; set; }
        public List<PlayerEnums.LookDirection> PlayerLookDirection { get; set; }
        public List<int> PlayerSprSheetX { get; set; }
        public List<int> PlayerZones { get; set; }

        private SprSheetImage sprite;

        private Weapon weapon;

        private bool knockback;
        private double knockbacktimer;
        private Vector2 knockbackdirection;

        private int dir = 0;
        private double duration = 0;
        public int minDuration = 1, maxDuration = 4;
        Random Random = new Random();
        private bool MoveBreak = false;

        private const int AggroDistance = 250;
        private bool Aggro = false;

        public List<Arrow> arrows = new List<Arrow>();
        public List<Arrow> HitArrows = new List<Arrow>();

        public Healthbar healthbar;
        public LevelIndicator levelindicator;
        public bool Die { get; set; }
        public bool AnimationFinished { get; private set; }

        private int ZoneNumber;

        public double Time { get; set; }

        public int LastHitBy { get; private set; }

        public Enemy(int X, int Y, int ZoneNR)
        {
            //TODO add playerstats
            //this.player = player

            TilePosition = new Vector2(X, Y);
            Position = new Vector2(X * GameSettings.Instance.Tilescale.X, Y * GameSettings.Instance.Tilescale.Y);

            State = PlayerEnums.ActionState.None;
            if (PlayerState != null)
                for (int i = 0; i < PlayerState.Count; i++)
                    PlayerState[i] = PlayerEnums.ActionState.None;
            lookDirection = PlayerEnums.LookDirection.Down;
            sprSheetY = PlayerEnums.Action.WalkDown;
            sprSheetX = 0;

            direction = new Vector2(0, 1);

            int rnd = Random.Next(9);
            switch (rnd)
            {
                case 0:
                    sprite = new SprSheetImage("EnemySprites/Female/darkelf");
                    break;
                case 1:
                    sprite = new SprSheetImage("EnemySprites/Female/darkelf2");
                    break;
                case 2:
                    sprite = new SprSheetImage("EnemySprites/Female/orc");
                    break;
                case 3:
                    sprite = new SprSheetImage("EnemySprites/Female/red_orc");
                    break;
                case 4:
                    sprite = new SprSheetImage("EnemySprites/Male/darkelf");
                    break;
                case 5:
                    sprite = new SprSheetImage("EnemySprites/Male/darkelf2");
                    break;
                case 6:
                    sprite = new SprSheetImage("EnemySprites/Male/orc");
                    break;
                case 7:
                    sprite = new SprSheetImage("EnemySprites/Male/red_orc");
                    break;
                case 8:
                    sprite = new SprSheetImage("EnemySprites/Male/skeleton");
                    break;
            }

            SpeedScale = 0.5f;

            weapon = new Weapon("Weapons/Spear/Male/spear_male", PlayerEnums.Weapontype.Spear, sprite.Position, 1, this);

            healthbar = new Healthbar();
            levelindicator = new LevelIndicator();
            PlayerLookDirection = new List<PlayerEnums.LookDirection>();
            PlayerState = new List<PlayerEnums.ActionState>();
            PlayerSprSheetX = new List<int>();
            PlayerPosition = new List<Vector2>();
            PlayerZones = new List<int>();

            ZoneNumber = ZoneNR;
        }

        public void LoadContent()
        {
            sprite.LoadContent(Position.X, Position.Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
            weapon.LoadContent((int)Position.X, (int)Position.Y);
        }

        public void UnloadContent()
        {
            sprite.UnloadContent();
            weapon.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Die)
            {
                DieAnimation(gameTime);
                return;
            }
            #region Knockback
            Vector2 temp = Vector2.Zero;
            for (int i = 0; i < PlayerState.Count; i++)
            {
                temp = CheckHit(i);
                if (temp.X == 1)
                {
                    LastHitBy = i;
                    break;
                }
            }
            if (temp.X == 1 && knockback == false)
            {
                healthbar.LoseHealth(2 * GameSettings.Instance.Tilescale.X * 0.25f);
                knockback = true;
                knockbacktimer = 0.2f;
                duration = 0;
                knockbackdirection.X = 0;
                sprSheetY = PlayerEnums.Action.Hit;
                sprSheetX = 4;
                knockbackdirection = temp;
            }
            foreach (Arrow Arrow in arrows)
            {
                Vector2 temp2 = CheckArrowHit(Arrow);
                if (temp2.X == 1 && knockback == false)
                {
                    LastHitBy = Arrow.PlayerID;
                    healthbar.LoseHealth(2 * GameSettings.Instance.Tilescale.X * 1f);
                    knockback = true;
                    knockbacktimer = 0.2f;
                    duration = 0;
                    knockbackdirection.X = 0;
                    sprSheetY = PlayerEnums.Action.Hit;
                    sprSheetX = 4;
                    knockbackdirection = temp2;
                    HitArrows.Add(Arrow);
                    arrows.Remove(Arrow);
                    
                    break;
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
                    weapon.Update(gameTime);
                    return;
                }
            }
            #endregion

            #region Movement
            if (State != PlayerEnums.ActionState.Thrust)
            {
                for (int i = 0; i < PlayerPosition.Count; i++)
                {
                    if ((((sprite.Position.X - PlayerPosition[i].X < AggroDistance && sprite.Position.X - PlayerPosition[i].X > 0) ||
                        (PlayerPosition[i].X - sprite.Position.X < AggroDistance && PlayerPosition[i].X - sprite.Position.X > 0)) &&
                        ((sprite.Position.Y - PlayerPosition[i].Y < AggroDistance && sprite.Position.Y - PlayerPosition[i].Y > 0) ||
                        (PlayerPosition[i].Y - sprite.Position.Y < AggroDistance && PlayerPosition[i].Y - sprite.Position.Y > 0))) &&
                        PlayerinZone(i))
                    {
                        Aggro = true;
                        SpeedScale = 0.15f;
                        if (sprite.Position.Y - GameSettings.Instance.Tilescale.Y - PlayerPosition[i].Y > 0)
                        {
                            lookDirection = PlayerEnums.LookDirection.Up;
                            State = PlayerEnums.ActionState.Walk;
                            direction.Y = -1;
                        }
                        else if (sprite.Position.Y + GameSettings.Instance.Tilescale.Y - PlayerPosition[i].Y < 0)
                        {
                            lookDirection = PlayerEnums.LookDirection.Down;
                            State = PlayerEnums.ActionState.Walk;
                            direction.Y = 1;
                        }
                        else
                            direction.Y = 0;
                        if (sprite.Position.X - GameSettings.Instance.Tilescale.X - PlayerPosition[i].X > 0)
                        {
                            lookDirection = PlayerEnums.LookDirection.left;
                            State = PlayerEnums.ActionState.Walk;
                            direction.X = -1;
                        }
                        else if (sprite.Position.X + GameSettings.Instance.Tilescale.X - PlayerPosition[i].X < 0)
                        {
                            lookDirection = PlayerEnums.LookDirection.Right;
                            State = PlayerEnums.ActionState.Walk;
                            direction.X = 1;
                        }
                        else
                            direction.X = 0;

                        if (direction.Y == 0 && direction.X == 0)
                        {
                            State = PlayerEnums.ActionState.Thrust;
                            Attack(gameTime);
                        }
                        break;
                    }
                    else
                        Aggro = false;
                }
                if (Aggro == false)
                {
                    SpeedScale = 0.5f;
                    if (duration <= 0 && !MoveBreak)
                    {
                        minDuration = 1;
                        maxDuration = 4;
                        dir = Random.Next(8);
                        duration = Random.Next(minDuration, maxDuration);
                        MoveBreak = true;
                    }
                    else if (duration <= 0 && MoveBreak)
                    {
                        minDuration = 0;
                        maxDuration = 2;
                        duration = Random.Next(minDuration, maxDuration);
                        MoveBreak = false;
                        State = PlayerEnums.ActionState.None;
                        direction = Vector2.Zero;
                        sprSheetX = 0;
                        dir = 8;
                    }
                    else
                        duration -= gameTime.ElapsedGameTime.TotalSeconds;

                    switch (dir)
                    {
                        case 0://up
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.Up;
                            direction = new Vector2(0, -1);
                            break;
                        case 1://left
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.left;
                            direction = new Vector2(-1, 0);
                            break;
                        case 2://down
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.Down;
                            direction = new Vector2(0, 1);
                            break;
                        case 3://right
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.Right;
                            direction = new Vector2(1, 0);
                            break;
                        case 4://left up
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.left;
                            direction = new Vector2(-1, -1);
                            break;
                        case 5://left down
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.left;
                            direction = new Vector2(-1, 1);
                            break;
                        case 6://right up
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.Right;
                            direction = new Vector2(1, -1);
                            break;
                        case 7://right down
                            State = PlayerEnums.ActionState.Walk;
                            lookDirection = PlayerEnums.LookDirection.Right;
                            direction = new Vector2(1, 1);
                            break;
                    }
                    /*}
                    else//no movement
                    {
                        State = PlayerEnums.ActionState.None;
                        sprSheetY = PlayerEnums.Action.None;
                        sprSheetX = 0;
                        sprite.SprSheetX = 0;
                        direction = new Vector2(0, 0);
                    }*/
                }
            }
            #endregion

            if (State == PlayerEnums.ActionState.Thrust)
            {
                Attack(gameTime);
                direction = new Vector2(0, 0);
            }
            else if (State != PlayerEnums.ActionState.None)
                Move(direction, gameTime);

            SetAnimationFrame();
            sprite.Update(gameTime);
            weapon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            weapon.Draw(spriteBatch);
        }
        
        public void DrawTop(SpriteBatch spriteBatch)
        {
            healthbar.Draw(spriteBatch, sprite.Position);
            levelindicator.Draw(spriteBatch, sprite.Position);
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

            Vector2 Collision = CheckCollision(new Vector2(sprite.Position.X + dirX, sprite.Position.Y + dirY), sprite.Position, direction);
            if (Collision.X == 0)
                dirX = 0;
            if (Collision.Y == 0)
                dirY = 0;

            if (dirX == 0 && dirY == 0)
                duration = 0;

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
                else if (direction.X == 0)
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
            int TileID2 = layer[1].getTileID(x, y);

            if (TileID2 != 0)
            {
                rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                temp = CheckCollision4(rect, playerRect);
            }

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

        private void ChangeAlpha(Vector2 position, int number)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;

            int x1 = (int)(position.X / tilescale_x),
            y1 = (int)(position.Y / tilescale_y),
            x2 = (int)((position.X + 2 * tilescale_x) / tilescale_x),
            y2 = (int)((position.Y + 2 * tilescale_y) / tilescale_y);

            Rectangle Enemyrect = new Rectangle(new Point((int)(position.X + 0.5 * tilescale_x), (int)(position.Y + 0.5 * tilescale_y)), new Point((int)tilescale_x, (int)(tilescale_y * 2 - 0.5 * tilescale_y)));

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
                        if (rect.Intersects(Enemyrect))
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
            if (!(knockback))
                sprSheetX += (float)gameTime.ElapsedGameTime.TotalMilliseconds / gameTime.ElapsedGameTime.Milliseconds * Interval;

            //Reset X at the final animation frame
            if ((int)sprSheetX >= (int)State)
            {
                if (State == PlayerEnums.ActionState.Thrust)
                    State = PlayerEnums.ActionState.None;
                sprSheetX = 0;
            }
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

        private Vector2 CheckHit(int i)
        {
            Vector2 returnvalue = new Vector2(0, 0);
            Rectangle Enemyrect = new Rectangle(new Point((int)(sprite.Position.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)sprite.Position.Y), new Point((int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y * 2))),
                Playerrect = new Rectangle((int)(PlayerPosition[i].X + 0.5 * GameSettings.Instance.Tilescale.X), (int)(PlayerPosition[i].Y + GameSettings.Instance.Tilescale.Y), (int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y));
            if (PlayerState[i] == PlayerEnums.ActionState.Thrust && PlayerSprSheetX[i] == 5)
            {
                switch (PlayerLookDirection[i])
                {
                    case PlayerEnums.LookDirection.Up:
                        Playerrect.Y -= (int)GameSettings.Instance.Tilescale.Y / 4;
                        Playerrect.Height = (int)GameSettings.Instance.Tilescale.Y;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 1);
                        break;
                    case PlayerEnums.LookDirection.left:
                        Playerrect.X -= (int)GameSettings.Instance.Tilescale.X / 4;
                        Playerrect.Width = (int)GameSettings.Instance.Tilescale.X;
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 2);
                        break;
                    case PlayerEnums.LookDirection.Down:
                        Playerrect.Height = (int)GameSettings.Instance.Tilescale.Y;
                        Playerrect.Y = (int)(PlayerPosition[i].Y + (2 * GameSettings.Instance.Tilescale.Y -  0.5 * Playerrect.Height));
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 3);
                        break;
                    case PlayerEnums.LookDirection.Right:
                        Playerrect.Width = (int)GameSettings.Instance.Tilescale.X;
                        Playerrect.X = (int)(PlayerPosition[i].X + (1.5 * GameSettings.Instance.Tilescale.X -  0.5 * Playerrect.Width));
                        if (Enemyrect.Intersects(Playerrect))
                            returnvalue = new Vector2(1, 4);
                        break;
                }
            }
            return returnvalue;
        }

        private Vector2 CheckArrowHit(Arrow Arrow)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            Vector2 returnvalue = new Vector2(0, 0);
            Rectangle Enemyrect = new Rectangle(new Point((int)(sprite.Position.X + 0.5 * GameSettings.Instance.Tilescale.X), (int)(sprite.Position.Y + GameSettings.Instance.Tilescale.Y)), new Point((int)GameSettings.Instance.Tilescale.X, (int)(GameSettings.Instance.Tilescale.Y)));
            
            if (Enemyrect.Intersects(Arrow.ArrowRect))
            {
                returnvalue.X = 1;
                if (Arrow.sprSheet == new Vector2(2, 1))
                    returnvalue.Y = 1;
                if (Arrow.sprSheet == new Vector2(1, 2))
                    returnvalue.Y = 2;
                if (Arrow.sprSheet == new Vector2(2, 2))
                    returnvalue.Y = 3;
                if (Arrow.sprSheet == new Vector2(1, 1))
                    returnvalue.Y = 4;
            }

            return returnvalue;
        }

        private bool PlayerinZone(int index)
        {
            if (PlayerZones[index] == ZoneNumber)
                return true;
            return false;
        }

        private void DieAnimation(GameTime gameTime)
        {
            if (sprSheetX >= (int)PlayerEnums.ActionState.Slash)
            {
                AnimationFinished = true;
                return;
            }
            if (sprSheetY != PlayerEnums.Action.Hit)
            {
                Interval = 0.2f;
                sprSheetY = PlayerEnums.Action.Hit;
                sprite.SprSheetY = (int)sprSheetY;
                sprite.SprSheetX = 0;
                sprSheetX = 0;
                sprite.Update(gameTime);
                weapon.SprSheetX = 1;
                weapon.SprSheetY = (int)PlayerEnums.Action.WalkLeft;
                weapon.Update(gameTime);
            }
            sprSheetX += (float)gameTime.ElapsedGameTime.TotalMilliseconds / gameTime.ElapsedGameTime.Milliseconds * Interval;
            sprite.SprSheetX = (int)sprSheetX;
            sprite.Update(gameTime);
        }

        public int getZoneNR()
        {
            return ZoneNumber;
        }
    }
}
