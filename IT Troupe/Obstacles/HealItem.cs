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
    public class HealItem
    {
        public Texture2D apple = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "apple.png"));
        public float x { get; set; }
        public float y { get; set; }
        public float radius { get; set; } = 10f;
        public bool collected { get; set; } = false;

        [JsonConstructor]
        public HealItem(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // heals player by 20% of their HP
        public void Collect(Player player)
        {
            if (!collected)
            {
                float healAmount = player.maxHP * 0.2f;
                player.AppleHeal(healAmount);
                collected = true;
            }
        }

        // draw the HealItem as long as it wasn't collected
        public void Draw()
        {
            if (!collected)
            {
                DrawTexture(apple, (int)x, (int)y, Color.White);
            }
        }
    }

}
