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
    public class Player_AttackM2
    {
        [JsonConverter(typeof(RectangleConverter))] // Apply the custom converter for Rectangle
        public Rectangle rect { get; set; }

        [JsonConverter(typeof(Vector2Converter))] // Apply the custom converter for Vector2
        public Vector2 direction { get; set; }

        public float speed { get; set; } = 450f;
        public bool isActive { get; set; } = true;

        public static float missileDelay { get; set; } = 1f; // 8 sec.
        public static float timeSinceLastShot { get; set; } = missileDelay;
        public static bool isReady { get; set; } = true;

        public float gravity { get; set; } = 500f;
        public float verticalVelocity { get; set; } = -300f;

        Texture2D[] hackerM2 = new Texture2D[]
        {
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerBullet1.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerBullet2.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerBullet3.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerBullet4.png")),
        };
        Texture2D[] hackerMissile = new Texture2D[]
        {
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerMissile1.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerMissile2.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "HackerMissile3.png")),
        };

        public int currentFrame = 0; // current animation frame
        public float frameTimer = 0f; // time for frame change
        public const float frameDuration = 1f / 6f; // 6 fps

        [JsonConstructor]
        public Player_AttackM2(float x, float y, Vector2 direction)
        {
            rect = new Rectangle(x, y, 20, 20);
            this.direction = direction;
            isActive = true;

            verticalVelocity = -300f;
        }

        public static bool CanShootMissile(float deltaTime)
        {
            timeSinceLastShot += deltaTime;
            //DrawText($"{ timeSinceLastShot}", 500, 1500, 40, Color.White);
            if (timeSinceLastShot >= missileDelay)
            {
                isReady = true;
                return true;
            }
            if (timeSinceLastShot < missileDelay)
            {
                isReady = false;
            }
            return false;
        }

        public void MissileUpdate(float deltaTime)
        {
            if (isActive)
            {
                float distanceThisFrameX = direction.X * speed * deltaTime;
                rect = new Rectangle(rect.X + distanceThisFrameX, rect.Y, rect.Width, rect.Height);
                /*rect.X += distanceThisFrameX;*/

                verticalVelocity += gravity * deltaTime;
                float distanceThisFrameY = verticalVelocity * deltaTime;
                rect = new Rectangle(rect.X, rect.Y + distanceThisFrameY, rect.Width, rect.Height);
                /*rect.Y += distanceThisFrameY;*/

                if (rect.Y >= 6000)
                {
                    isActive = false;
                    timeSinceLastShot = 0f;
                }

                // Update the animation frame
                frameTimer += deltaTime;
                if (frameTimer >= frameDuration)
                {
                    frameTimer -= frameDuration;
                    currentFrame = (currentFrame + 1) % hackerMissile.Length;
                }
            }
        }

        public void MissileDraw(Colors colors)
        {
            if (isActive)
            {
                // Draw the current frame of the animation
                if (hackerM2 != null && hackerM2.Length > 0)
                {
                    DrawTexture(hackerMissile[currentFrame], (int)rect.X, (int)rect.Y, Color.White);
                }
                else
                {
                    // Fallback: Draw a rectangle if textures are not set
                    DrawRectangleRec(rect, colors.characterIcolor);
                }
            }
        }
    }
}
