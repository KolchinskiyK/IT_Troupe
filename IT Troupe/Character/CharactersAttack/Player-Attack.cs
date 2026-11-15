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
	public class Player_Attack
	{
        [JsonConverter(typeof(RectangleConverter))] // Apply the custom converter for Rectangle
        public Rectangle rect { get; set; }

        [JsonConverter(typeof(Vector2Converter))] // Apply the custom converter for Vector2
        public Vector2  direction { get; set; }

        public float speed { get; set; } = 550f;
		public bool isActive { get; set; } = true;

		public float maxRangeM1 { get; set; } = 280f;
        public float distanceTraveled { get; set; } = 0f;

        public static float shootDelay { get; set; } = 0.20f; // 4 sec.

        public static bool isReady { get; set; } = true;
        public static float timeSinceLastShot { get; set; } = shootDelay;

        Texture2D[] hackerM1 = new Texture2D[]
        {
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "M1_1.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "M1_2.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "M1_3.png")),
            LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "AttackAnimation", "M1_4.png")),
         };
        public int currentFrame = 0; // current animation frame
        public float frameTimer = 0f; // time for frame change
        public const float frameDuration = 1f / 6f; // 6 fps


        [JsonConstructor]
        public Player_Attack(float x, float y, Vector2 direction)
		{
            rect = new Rectangle(x, y, 10, 10);
            //Console.WriteLine(x);
            //Console.WriteLine(y);
           
            this.direction = direction;
            isActive = true;
            distanceTraveled = 0f;
        }
        public static bool CanShoot(float deltaTime)
        {
            timeSinceLastShot += deltaTime;

            if (timeSinceLastShot >= shootDelay)
            {
                isReady = true;
                return true;
            }
            if (timeSinceLastShot < shootDelay)
            {
                isReady = false;
            }
            return false;
        }

        public void ShootUpdate(float deltaTime)
		{
            if (isActive)
            {
                float distanceThisFrame = speed * deltaTime;

                // Modify the Rect directly using its getter and setter.
                rect = new Rectangle(rect.X + direction.X * distanceThisFrame,
                                     rect.Y + direction.Y * distanceThisFrame,
                                     rect.Width, rect.Height);

                /*rect.X += direction.X * distanceThisFrame;
                rect.Y += direction.Y * distanceThisFrame;*/

                distanceTraveled += distanceThisFrame;
                if (distanceTraveled >= maxRangeM1)
                {
                    isActive = false;
                }

                // Update the animation frame
                frameTimer += deltaTime;
                if (frameTimer >= frameDuration)
                {
                    frameTimer -= frameDuration;
                    currentFrame = (currentFrame + 1) % hackerM1.Length;
                }

                //Console.WriteLine("Bullet position X"+ rect.X);
                //Console.WriteLine("Bullet position Y" + rect.Y);
                //Raylib.DrawRectangle((int)rect.X, (int)rect.Y, 30, 30, Color.Red);
            }
        }


		public void ShootDraw(Colors colors)
		{
			if (isActive)
            { 
                // Draw the current frame of the animation
                if (hackerM1 != null && hackerM1.Length > 0)
                {
                    DrawTexturePro(hackerM1[currentFrame],
                         new Rectangle(0, 0, hackerM1[currentFrame].Width, hackerM1[currentFrame].Height),
                         new Rectangle((int)rect.X, (int)rect.Y, hackerM1[currentFrame].Width * 2, hackerM1[currentFrame].Height * 2),
                         new Vector2(hackerM1[currentFrame].Width / 2, hackerM1[currentFrame].Height / 2), 0, Color.White);

                    //DrawTexture(hackerM1[currentFrame], (int)rect.X, (int)rect.Y, Color.White);
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

