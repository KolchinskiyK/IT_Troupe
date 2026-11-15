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
    public class Ladder
    {
        public Texture2D ladder = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "ladder.png"));
        public float y { get; set; }
        public float x { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [JsonConstructor]
        public Ladder(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            //this.Width = ladder.Width;
            //this.Height = ladder.Height;
            this.Width = width;
            this.Height = height;

        }
        public void Draw()
        {
            //DrawTexture(ladder, (int)x, (int)y, Color.White);
            DrawTexturePro(
            ladder,
            new Rectangle(0, 0, ladder.Width, ladder.Height),
            new Rectangle((int)x + ladder.Width / 2, (int)y + ladder.Height / 2, Width, Height), // Zielrechteck
            new Vector2(ladder.Width / 2, ladder.Height / 2),
            0,
            Color.White
            );
        }
    }
}
