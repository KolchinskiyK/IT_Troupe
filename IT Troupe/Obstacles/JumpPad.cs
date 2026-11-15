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
    public class JumpPad
    {
        public Texture2D grass = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "grassPad.png"));
        public float x { get; set; }
        public float y { get; set; }
        public float width{ get; set; } = 40;
        public float height { get; set; } = 10;
        public bool used { get; set; } = false;         // Shows if in active or inactive state
        public float usedTimer { get; set; } = 0;       // Shows, how long it stays in inactive state

        [JsonConstructor]
        public JumpPad(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public void Draw()
        {
            if (used)       // Draw the inactive state, smaller
            {
                //DrawRectangle((int)x, (int)y, (int)width, (int)height, Color.Magenta);
                DrawTexturePro(
                grass,
                new Rectangle(0, 0, grass.Width, grass.Height),
                new Rectangle((int)x + grass.Width / 2, (int)y + grass.Height/4, width, height), // Zielrechteck
                new Vector2(grass.Width / 2, grass.Height / 2),
                0,
                Color.White
                );
            }
            else            // Draw the active state, higher
            {
                //DrawRectangle((int)x, (int)y - (int)height, (int)width, (int)height * 2, Color.Magenta);
                DrawTexturePro(
                grass,
                new Rectangle(0, 0, grass.Width, grass.Height),
                new Rectangle((int)x + grass.Width / 2, (int)y, width, height*2), // Zielrechteck
                new Vector2(grass.Width / 2, grass.Height / 2),
                0,
                Color.White
                );
            }
        }
    }
}
