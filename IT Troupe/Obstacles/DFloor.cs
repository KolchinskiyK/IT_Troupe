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
    public class DFloor
    {
        public static Texture2D dfloor = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "Dfloor_64x16.png"));
        public float x { get; set; }
        public float y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [JsonConstructor]
        public DFloor(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
        }

        //checks for collision with player
        public bool IsCollidingWithPlayer(Rectangle player)
        {
            return CheckCollisionRecs(new Rectangle(x, y, Width, Height), player);
        }
        //removes the destroyed floor
        public void DestroyOnCollision(bool isPlunging, Rectangle player, List<DFloor> dfloorsToRemove)
        {
            if (isPlunging && IsCollidingWithPlayer(player))
            {
                dfloorsToRemove.Add(this);
            }
        }

        //draws the dfloor
        public void Draw()
        {
            //DrawTexture(dfloor, (int)x, (int)y, Color.White);
            //DrawRectangle((int)x, (int)y, (int)Width, (int)Height, Color.Gray);
            DrawTexturePro(
            dfloor,
            new Rectangle(0, 0, dfloor.Width, dfloor.Height),
            new Rectangle((int)x + dfloor.Width / 2, (int)y + dfloor.Height / 2, Width, Height), // Zielrechteck
            new Vector2(dfloor.Width / 2, dfloor.Height / 2),
            0,
            Color.White
            );
        }

    }
}
