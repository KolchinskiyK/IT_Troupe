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
    public class GrassObstacle
    {
        public Texture2D grass = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "grass.png"));
        public float x { get; set; }
        public float y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [JsonConstructor]
        public GrassObstacle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            //this.Width = grass.Width;
            //this.Height = grass.Height;
            this.Width = width;
            this.Height = height;

        }
        public void Draw()
        {
            DrawTexture(grass, (int)x, (int)y, Color.White);
        }
    }
}
