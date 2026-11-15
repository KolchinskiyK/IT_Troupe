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
    public class Portal
    {
        public Texture2D portal = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "portal.png"));
        public float x { get; set; }
        public float y { get; set; }
        public bool entered { get; set; } = false;

        [JsonConstructor]
        public Portal(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // Draw the Portal
        public void Draw()
        {
            DrawTexture(portal, (int)x, (int)y, Color.White);
        }
    }

}