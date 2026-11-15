using Raylib_cs;
using System;
using System.Numerics;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;  // This removes the need to type "Raylib."
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace IT_Troupe
{
    public class Test_Enemy : Enemy_Base
    {
        //[JsonConverter(typeof(RectangleConverter))]
        //public Rectangle rect { get; set; }

        //Load texture 
        public static Texture2D fear = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Enemies", "fear.png"));

        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle sourceRect { get; set; } = new Rectangle(0, 0, fear.Width, fear.Height);

        [JsonConverter(typeof(ColorConverter))]
        public Color attackAreaColor { get; set; } = new Color(0, 0, 255, 20);

        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 direction { get; set; } = new Vector2(1, 0);

        public float moveFrames { get; set; } = 200;
        public bool movingForward { get; set; } = true;
        public int frameCounter { get; set; } = 0;
        public float enemyVertVel { get; set; }     // Natural gravitation

        public BasicAttackFear attackSkill { get; set; }

        // Variables to track collision duration
        public bool isColliding { get; set; } = false;
        DateTime collisionStartTime = DateTime.MinValue;
        TimeSpan collisionDuration = TimeSpan.Zero;

        //// Parameterless constructor is automatically used during deserialization
        //public Test_Enemy() : base(0, 0, 0, 0, 0, 0) { }

        public Test_Enemy(float x, float y, float width, float height, float attackSkillValue) : base(x, y, width, height, 0, 0)
        {
            this.rect = new Rectangle(x, y, width, height);
            this.attackSkill = new BasicAttackFear(attackSkillValue);
            this.maxHP = 150;
            this.currentHP = 150;
            this.speed = 1f;

        }

        public void MoveIdle(AllObstacles obsts, bool playerFound, ref bool isOnEdgeL, ref bool isOnEdgeR)    // Natural moving on the floor as long as enemy doesnt collide with anything or standing on edge
        {
            if (!playerFound)       // Passive Movement without Playerdetection
            {
                if (movingForward) // Passive movement forward (right)
                {
                    rect = new Rectangle(rect.X + direction.X * speed, rect.Y + direction.Y * speed, rect.Width, rect.Height);
                    enemyDirection = 1; // Set direction to right
                }
                else // Passive movement back (left)
                {
                    rect = new Rectangle(rect.X - direction.X * speed, rect.Y - direction.Y * speed, rect.Width, rect.Height);
                    enemyDirection = -1; // Set direction to left
                }
            }
            enemyVertVel += 3f; // Gravitation for enemy
            // Applying zero gravitation if standing on the floor
            foreach (Floor floor in obsts.floors)
            {

                // Check if the enemy has landed on the ground
                if (rect.Y >= floor.y - rect.Height - 2 && rect.X + rect.Width > floor.x &&
                    rect.X < floor.x + floor.Width && !(rect.Y >= floor.y +5) && rect.Y <= floor.y + 100 )               // Ground level is List<Rectangle> floors
                {
                    rect = new Rectangle(rect.X, floor.y - rect.Height, rect.Width, rect.Height);
                    enemyVertVel = 0f;                                  // Reset vertical velocity
                                                                        // Changing moving direction if reaching the edge 
                    if (rect.X > floor.x + floor.Width - rect.Width - 1)
                    {
                        movingForward = !movingForward;
                        isOnEdgeR = true;
                    }
                    if (rect.X < floor.x + 1)
                    {
                        movingForward = !movingForward; // Change direction at the left edge
                        isOnEdgeL = true; // Enemy reached the left edge
                    }
                }
                //*/

                bool isOverlappingHorizontally = rect.X + rect.Width > floor.x && rect.X < floor.x + floor.Width;

                if (isOverlappingHorizontally && rect.Y + rect.Height > floor.y && rect.Y < floor.y + floor.Height)
                {
                    // Left side collision
                    if (rect.X + rect.Width >= floor.x && rect.X < floor.x)
                    {
                        rect = new Rectangle(floor.x - rect.Width, rect.Y, rect.Width, rect.Height);
                        movingForward = !movingForward;
                        isOnEdgeR = true; // Enemy hit the right edge
                    }
                    // Right side collision
                    else if (rect.X <= floor.x + floor.Width && rect.X + rect.Width > floor.x + floor.Width)
                    {
                        rect = new Rectangle(floor.x + floor.Width, rect.Y, rect.Width, rect.Height);
                        movingForward = !movingForward;
                        isOnEdgeL = true; // Enemy hit the right edge
                    }
                }
            }


            // Collision with platforms on the sides
            foreach (Platforms platform in obsts.platforms)
            {
                bool isOverlappingHorizontally = rect.X + rect.Width > platform.x && rect.X < platform.x + platform.Width;

                if (isOverlappingHorizontally && rect.Y + rect.Height > platform.y && rect.Y < platform.y + platform.Height)
                {
                    // Left side collision
                    if (rect.X + rect.Width >= platform.x && rect.X < platform.x)
                    {
                        rect = new Rectangle(platform.x - rect.Width, rect.Y, rect.Width, rect.Height);
                        movingForward = !movingForward;
                        isOnEdgeR = true; // Enemy hit the right edge
                    }
                    // Right side collision
                    else if (rect.X <= platform.x + platform.Width && rect.X + rect.Width > platform.x + platform.Width)
                    {
                        rect = new Rectangle(platform.x + platform.Width, rect.Y, rect.Width, rect.Height);
                        movingForward = !movingForward;
                        isOnEdgeL = true; // Enemy hit the right edge
                    }
                }
                // Check if the player is landing on top of the platform while falling down
                if (isOverlappingHorizontally && rect.Y + rect.Height >= platform.y &&
                   rect.Y + rect.Height <= platform.y + platform.Height && enemyVertVel > 0)
                {
                    rect = new Rectangle(rect.X, platform.y - rect.Height, rect.Width, rect.Height);
                    enemyVertVel = 0f;                                  // Reset vertical velocity

                }
            }
            if (enemyVertVel != 0) // Applying gavity
            {
                rect = new Rectangle(rect.X, rect.Y + enemyVertVel, rect.Width, rect.Height);
            }
            // DEBUG
            //DrawText($"Direction: {enemyDirection}, Moving Forward: {movingForward}, L {isOnEdgeL}, R {isOnEdgeR} ", (int)rect.X, (int)rect.Y - 35, 30, Color.Black);
        }

        // Forces the Enemy to move as idle or as Following player
        public void DetectPlayer(Player player, AllObstacles obsts)
        {
            bool isOnEdgeL = false;
            bool isOnEdgeR = false;
            float distance = CheckDistance(player);
            if (distance <= 600 && Math.Abs(rect.Y - player.playerPosition.Y) < 100)    // Check Distance and Height-Difference
            {
                MoveIdle(obsts, true, ref isOnEdgeL, ref isOnEdgeR);    // Use that Method for physic check and ref returning isOnEdge state

                if (player.playerPosition.X - rect.X >= 0)   // Player right
                {
                    if (!isOnEdgeR)     // If not on right edge
                    {                        
                        rect = new Rectangle(rect.X + direction.X * speed, rect.Y + direction.Y * speed, rect.Width, rect.Height);
                        enemyDirection = 1; // Set direction to right
                    }
                }
                else                                         // Player left
                {
                    if (!isOnEdgeL)     // If not on left edge
                    {
                        rect = new Rectangle(rect.X - direction.X * speed, rect.Y + direction.Y * speed, rect.Width, rect.Height);
                        enemyDirection = -1;    //set direction to left
                    }     
                }
            }
            else
            {
                MoveIdle(obsts, false, ref isOnEdgeL, ref isOnEdgeR);       // Use it for idle movement and physic check
            }
        }


        public void Attack(Player player)
        {
            float rectangleWidth = this.rect.Width;

            Rectangle playerCurrentPosition = new Rectangle(player.playerPosition, player.playerWidth, player.playerHeight);
            Vector2 enemyCenterPosition = new Vector2(rect.X + rectangleWidth / 2, rect.Y - rectangleWidth / 2);

            if (CheckCollisionCircleRec(enemyCenterPosition, 85, playerCurrentPosition) && player.playerPosition.Y > 440)
            {
                attackSkill.CanAttackBasic(player);
            }
        }

        public bool Attack2(Player player)
        {
            if (Math.Abs(player.playerPosition.X - rect.X) < 300 && Math.Abs(player.playerPosition.Y - rect.Y) < 75 )
            {
                DrawAttackArea(player);

                if (!isColliding)
                {
                    collisionStartTime = DateTime.Now; // Start tracking time
                    isColliding = true;
                }
                //Update coollision duration
                collisionDuration = DateTime.Now - collisionStartTime;

                //Console.WriteLine(collisionDuration);
                if(collisionDuration > TimeSpan.FromSeconds(2)) // Player has 2 Sec. to run away befor attack start.
                {
                    attackSkill.CanAttack(player);
                    return true;
                }
            }
            else
            {
                if(isColliding) // Collision just ended
                {
                    collisionDuration = TimeSpan.Zero;
                }
                isColliding = false;
            }

            return false;
        }

        public void DrawAttackArea(Player player)
        {
            if (player.playerPosition.X < rect.X)
            {
                //DrawRectangle((int)rectX, (int)rectY, (int)rectWidth, (int)rectHeight, new Color(0, 255, 0, 30));
                DrawRectangle((int)rect.X - 300, (int)(rect.Y-25), 300, 150, new Color(0, 255, 0, 30));
            }
            else
            {
                //DrawRectangle((int)rect.X + (int)rectangleWidth, (int)rectY, (int)rectWidth, (int)rectHeight, new Color(0, 255, 0, 30));
                DrawRectangle((int)rect.X, (int)(rect.Y - 25), 300, 120, new Color(0, 255, 0, 30));
            }
        }

        public override void Draw()
        {
            // Flip the texture only when necessary
            if (enemyDirection == -1)
            {
                if(sourceRect.Width > 0)
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height); // Flip texture for left movement
                    //sourceRect.Width = -sourceRect.Width; // Flip texture for left movement
            }
            else if (enemyDirection == 1)
            {
                if (sourceRect.Width < 0)
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height); // Restore texture for right movement
                    //sourceRect.Width = -sourceRect.Width; // Restore texture for right movement
            }

            float rectangleWidth = this.rect.Width;
            float rectangleHeight = this.rect.Height;

            if (currentHP > 0)
            {
                // Use DrawTexturePro to draw the texture with flipping applied
                DrawTexturePro(
                    fear, // The texture
                    sourceRect, // The source rectangle (flipped when moving backward)
                    new Rectangle(
                        (int)rect.X + rect.Width / 2, // Center X position
                        (int)rect.Y + rect.Height / 2, // Center Y position
                        rect.Width, // Target width
                        rect.Height // Target height
                    ),
                    new Vector2(rect.Width / 2, rect.Height / 2), // Origin point for rotation
                    0f, // Rotation angle (no rotation in this case)
                    Color.White // Texture tint
                );

                // Draw Basic Attack Area
                Raylib.DrawCircle((int)rect.X + (int)rectangleWidth / 2, (int)rect.Y + (int)rectangleHeight / 2, 85, attackAreaColor);

                // Calculate HP bar width based on current HP
                float hpRatio = (float)currentHP / maxHP;
                int hpBarCurrentWidth = (int)(rectangleWidth * hpRatio);

                // Draw HP bar above the rectangle
                Raylib.DrawRectangle((int)rect.X, (int)rect.Y - 13, (int)rectangleWidth, hpBarHeight, Color.Red);
                Raylib.DrawRectangle((int)rect.X, (int)rect.Y - 13, hpBarCurrentWidth, hpBarHeight, Color.Green);
            }
        }

        public float CheckDistance(Player player)
        {
            float rectangleWidth = this.rect.Width;
            float rectangleHeight = this.rect.Height;

            Vector2 centerCurrentPositionPlayer = new Vector2(player.playerPosition.X + player.playerWidth / 2, player.playerPosition.Y + player.playerHeight / 2);
            Vector2 centerCurrentPositionEnemy = new Vector2(rect.X + rectangleWidth / 2, rect.Y + rectangleHeight / 2);

            return Vector2.Distance(centerCurrentPositionPlayer, centerCurrentPositionEnemy);
        }

    }
}