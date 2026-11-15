using Raylib_cs;
using System;
using System.Numerics;
using System.Text.Json.Serialization;
using static Raylib_cs.Raylib;  // This removes the need to type "Raylib."

namespace IT_Troupe
{
    public class Enemy_Burnout: Enemy_Base
    {
        [JsonConverter(typeof(RectangleConverter))]
        public static Texture2D burn = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Enemies", "burnout.png"));

        [JsonConverter(typeof(ColorConverter))]
        public Color color { get; set; } = new Color(200, 200, 255, 180);
        public float fireCooldown { get; set; } = 3f;

        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle sourceRect { get; set; } = new Rectangle(0, 0, burn.Width, burn.Height);
        public List<Fireball> fireballs = new List<Fireball>();

        //// Parameterless constructor is automatically used during deserialization
        //public Enemy_Burnout() : base(0, 0, 0, 0, 0, 0) {
        //}

        public Enemy_Burnout(float x, float y, float width, float height, float startRange, float endRange): base(x, y,width, height, startRange, endRange)
        {
            this.maxHP = 125;
            this.currentHP = 125;
        }

        public void Update(Player player,float deltaTime)
        {
            if (!CheckAttackDistance(player))
            {
                //rect.X += speed * enemyDirection * deltaTime;
                rect = new Rectangle(rect.X + speed * enemyDirection * deltaTime, rect.Y, rect.Width, rect.Height);

            }

            if (CheckDistanc(player))
            {
                //rect.X += speed * -enemyDirection * deltaTime;
                rect = new Rectangle(rect.X + speed * -enemyDirection * deltaTime, rect.Y, rect.Width, rect.Height);
            }

            // Use startRange and endRange for boundary logic
            if (rect.X <= startRange || rect.X + rect.Width >= endRange)
            {
                if (rect.X <= startRange)
                {
                    // Snap to the start range
                    rect = new Rectangle(startRange, rect.Y, rect.Width, rect.Height);
                }
                else if (rect.X + rect.Width >= endRange)
                {
                    // Snap to the end range
                    rect = new Rectangle(endRange - rect.Width, rect.Y, rect.Width, rect.Height);
                }
                enemyDirection *= -1;  // Flip enemyDirection
            }

            fireCooldown -= deltaTime;

            if (fireCooldown <= 0 && CheckAttackDistance(player))
            {
                Vector2 fireDirection = Vector2.Normalize(player.playerPosition - new Vector2(rect.X, rect.Y));
                fireballs.Add(new Fireball(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, fireDirection, 200));
                fireCooldown = 2f;

                Console.WriteLine($"Attack Distance Check: {Math.Abs(player.playerPosition.X - rect.X)}");
            }

            foreach (var fireball in fireballs)
            {
                fireball.Update(deltaTime,player);
            }

            fireballs.RemoveAll(f => !f.isActive);
        }

        public override void Draw()
        {
            if (currentHP > 0)
            {
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
                DrawTexturePro(
                    burn, // The texture
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

                foreach (var fireball in fireballs)
                {
                    fireball.Draw();
                }

                // Calculate HP bar width based on current HP
                float hpRatio = (float)currentHP / maxHP;
                int hpBarCurrentWidth = (int)(rect.Width * hpRatio);

                // Draw HP bar above the rectangle
                DrawRectangle((int)rect.X, (int)rect.Y - 13, (int)rect.Width, hpBarHeight, Color.Red);
                DrawRectangle((int)rect.X, (int)rect.Y - 13, hpBarCurrentWidth, hpBarHeight, Color.Green);
            }
        }

        public bool CheckFireballCollision(Rectangle playerRect)
        {
            foreach (var fireball in fireballs)
            {
                if (CheckCollisionRecs(fireball.rect, playerRect))
                {
                    fireball.isActive = false;
                    return true;
                }
            }
            return false;
        }

        public bool CheckAttackDistance(Player player)
        {
            return Math.Abs(player.playerPosition.X - rect.X) < 400; // true if the distance less than 300 frames
        }

        public bool CheckDistanc(Player player)
        {
            return Math.Abs(player.playerPosition.X - rect.X) < 300; // true if the distance less than 200 frames
        }
    }

}
