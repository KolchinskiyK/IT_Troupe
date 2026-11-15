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
    public class Wall
    {
        public static Texture2D dwall = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "DWall_64x16.png"));
        public float x { get; set; }
        public float y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [JsonConstructor]
        public Wall(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
        }
        //check for collision with player
        public bool IsCollidingWithPlayer(Rectangle player)
        {
            return CheckCollisionRecs(new Rectangle(x, y, Width, Height), player);
        }
        //removes destroyed wall
        public void DestroyOnCollision(bool isDashing, Rectangle player, List<Wall> wallsToRemove)
        {
            if (isDashing && IsCollidingWithPlayer(player))
            {
                wallsToRemove.Add(this);
            }
        }
        //draws floor
        public void Draw()
        {
            //DrawTexture(dwall, (int)x, (int)y, Color.White);
            //DrawRectangle((int)x, (int)y, (int)Width, (int)Height, Color.Gray);
            DrawTexturePro(
            dwall,
            new Rectangle(0, 0, dwall.Width, dwall.Height),
            new Rectangle((int)x + dwall.Width / 2, (int)y + dwall.Height / 2, Width, Height), // Zielrechteck
            new Vector2(dwall.Width / 2, dwall.Height / 2),
            0,
            Color.White
            );
        }
    }
}
