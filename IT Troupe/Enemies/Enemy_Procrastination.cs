using Raylib_cs;
using System;
using System.Numerics;
using System.Text.Json.Serialization;
using static Raylib_cs.Raylib;  // This removes the need to type "Raylib."

namespace IT_Troupe
{
	public class Enemy_Procrastination : Enemy_Base
	{
        [JsonConverter(typeof(RectangleConverter))]
        public static Texture2D procrastination = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Enemies", "procrastination_v1.png"));

        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle sourceRect { get; set; } = new Rectangle(0, 0, procrastination.Width, procrastination.Height);

        public bool playerTouchedEnemy { get; set; } = false;

        public float moveFrames { get; set; } = 200;
        public bool movingForward { get; set; } = true;
        public int frameCounter { get; set; } = 0;

        public float enemyAttackCooldown { get; set; } = 5f;
        public float enemyAttackTimer { get; set; } = 0f;

        // Variables to track collision duration
        public bool isColliding { get; set; } = false;
        DateTime collisionStartTime = DateTime.MinValue;
        TimeSpan collisionDuration = TimeSpan.Zero;

        public Color EnemyAttackColor = new Color(255,255, 0, 70);
        public Color EnemyAttackColor2 = new Color(255, 150, 0, 50);
        public Color EnemyAttackColor3 = new Color(255, 0, 0, 30);

        public Enemy_Procrastination(float x, float y, float width, float height, float startRange, float endRange) : base(x, y, width, height, startRange, endRange)
        {
            // Set the values from the constructor parameters to the properties
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.maxHP = 100;
            this.currentHP = 100;

            // Initialize the Rectangle with the given position and size
            this.rect = new Rectangle(x, y, width, height);

            // Set the start and end range from the parameters
            this.startRange = startRange;
            this.endRange = endRange;
        }

        public void Movement(Player player, float deltaTime)
        {
            // Update timers
            UpdateTimers(deltaTime);

            if (IsPlayerInRange(player) && enemyAttackTimer == 0f)
            {
                if (!isColliding)
                {
                    collisionStartTime = DateTime.Now; // Start tracking time
                    isColliding = true;
                }

                DrawEnemyAttack(60, EnemyAttackColor);
                collisionDuration = DateTime.Now - collisionStartTime;
             
                if (collisionDuration > TimeSpan.FromSeconds(0.5))
                {
                    DrawEnemyAttack(100, EnemyAttackColor2);
                    if (collisionDuration > TimeSpan.FromSeconds(1))
                    {
                        DrawEnemyAttack(140, EnemyAttackColor3);
                        
                        if (collisionDuration > TimeSpan.FromSeconds(1.5))
                        {
                            Explosion(player);
                        }
                    }
                }
            }
            else
            {
                playerTouchedEnemy = false;
                MoveEnemy(deltaTime);
                collisionDuration = TimeSpan.Zero;
                isColliding = false;
            }
            rect = new Rectangle(x, y, Width, Height);
        }

        public void UpdateTimers(float deltaTime)
        {
            enemyAttackTimer = Math.Max(0, enemyAttackTimer - deltaTime);
        }

        public bool IsPlayerInRange(Player player)
        {
            return CheckDistanceToPlayer(player) < 140f && Math.Abs(player.playerPosition.Y - y) < 20f;

        }

        public void MoveEnemy(float deltaTime)
        {
            x += speed * enemyDirection * deltaTime;

            if(x <= startRange || x+Width >= endRange)
            {
                enemyDirection *= -1; // Change direction when reaching the boundary
            }
        }

        public float CheckDistanceToPlayer(Player player)
        {
            return Math.Abs(player.playerPosition.X - x);
        }

        public void Explosion(Player player)
        {
            if (currentHP <= 0)
            {
                return;
            }

            if (playerTouchedEnemy == false)
            {
                currentHP *= 0.5;
                playerTouchedEnemy = true;
                player.currentHP = (int)(player.currentHP * 0.8);
                enemyAttackTimer = enemyAttackCooldown;

            }
        }

        public override void Draw()
        {
            //DrawTexture(procrastination, (int)x, (int)y, Color.White);
            // Flip the texture only when necessary
            if (enemyDirection == -1)
            {
                if (sourceRect.Width > 0)
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height); // Flip texture for left movement
                                                                                                                  //sourceRect.Width = -sourceRect.Width; // Flip texture for left movement
            }
            else if (enemyDirection == 1)
            {
                if (sourceRect.Width < 0)
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height); // Restore texture for right movement
                                                                                                                  //sourceRect.Width = -sourceRect.Width; // Restore texture for right movement
            }

            if (currentHP > 0)
            {
                // Use DrawTexturePro to draw the texture with flipping applied
                DrawTexturePro(
                    procrastination, // The texture
                    sourceRect, // The source rectangle (flipped when moving backward)
                    new Rectangle(
                        (int)x + rect.Width / 2, // Center X position
                        (int)y + rect.Height / 2, // Center Y position
                        rect.Width, // Target width
                        rect.Height // Target height
                    ),
                    new Vector2(rect.Width / 2, rect.Height / 2), // Origin point for rotation
                    0f, // Rotation angle (no rotation in this case)
                    Color.White // Texture tint
                );

                // Calculate HP bar width based on current HP
                float hpRatio = (float)currentHP / maxHP;
                int hpBarCurrentWidth = (int)(rect.Width * hpRatio);

                // Draw HP bar above the rectangle
                DrawRectangle((int)x, (int)y - 13, (int)rect.Width, hpBarHeight, Color.Red);
                DrawRectangle((int)x, (int)y - 13, hpBarCurrentWidth, hpBarHeight, Color.Green);
            }
        }

        public void DrawEnemyAttack(int radius, Color color)
        {
            DrawCircle((int)(x + (Width / 2)), (int)(y + (Height / 2)), radius, color);
        }
    }
}

