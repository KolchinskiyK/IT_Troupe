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
    public class Floor
    {
        public float x { get; set; }
        public float y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [JsonConstructor]
        public Floor(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
        }
        public void Draw()
        {
            // Draw the platform with full transparency (invisible)
            DrawRectangle((int)x, (int)y, (int)Width, (int)Height, new Color(255, 255, 255, 0)); // Transparent platform
            // DrawRectangle((int)x, (int)y, (int)Width, (int)Height, new Color(255, 255, 255, 255)); // White platform

        }

    }
}
