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
    public class Fireball
    {
        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle rect { get; set; }

        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 direction { get; set; }

        //public Enemy_Burnout burnout = new Enemy_Burnout();
        public bool isActive { get; set; } = true;
        public float speed { get; set; }

        Texture2D[] fireball = new Texture2D[]
        {
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "Fireball1.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "Fireball2.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "Fireball3.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "Fireball4.png")),
        };
        Texture2D[] rfireball = new Texture2D[]
        {
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "rFireball1.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "rFireball2.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "rFireball3.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "rFireball4.png")),
        };

        public int currentFrame = 0; // current animation frame
        public float frameTimer = 0f; // time for frame change
        public const float frameDuration = 1f / 6f; // 6 fps

        [JsonConstructor]
        public Fireball(float x, float y, Vector2 direction, float speed)
        {
            this.rect = new Rectangle(x, y, 15, 15);
            this.direction = direction;
            this.speed = speed;
        }

        public void Update(float deltaTime, Player player)
        {
            rect = new Rectangle(rect.X + direction.X * speed * deltaTime, rect.Y + direction.Y * speed * deltaTime, rect.Width, rect.Height);

            if (!IsPlayerInRange(player))
            {
                isActive = false;
            }

            // Update the animation frame
            frameTimer += deltaTime;
            if (frameTimer >= frameDuration)
            {
                frameTimer -= frameDuration;
                currentFrame = (currentFrame + 1) % fireball.Length;
            }
        }

        public bool IsPlayerInRange(Player player)
        {
            return CheckDistanceToPlayer(player) < 400f && Math.Abs(player.playerPosition.Y - rect.Y) < 100f;

        }

        public float CheckDistanceToPlayer(Player player)
        {
            return Math.Abs(player.playerPosition.X - rect.X);
        }

        public void Draw()
        {
            if (isActive)
            {
                // Draw the current frame of the animation
                if (fireball != null && fireball.Length > 0)
                {
                    if (direction.X > 0)
                        DrawTexture(rfireball[currentFrame], (int)rect.X, (int)rect.Y, Color.White);
                    else
                        DrawTexture(fireball[currentFrame], (int)rect.X, (int)rect.Y, Color.White);

                }
                else
                {
                    // Fallback: Draw a circle if textures are not set
                    Raylib.DrawCircle((int)rect.X, (int)rect.Y, rect.Width / 2, Color.Orange);
                    Raylib.DrawCircle((int)rect.X, (int)rect.Y, rect.Width / 3, Color.Red);
                }

            }
        }
    }

}

