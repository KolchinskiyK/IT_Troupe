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
    public class Colors
    {
        [JsonConverter(typeof(ColorConverter))]
        public Color paintingcolor { get; set; } = new Color(170, 0, 130, 100);
        [JsonConverter(typeof(ColorConverter))]
        public Color plungingcolor { get; set; } = new Color(65, 240, 170, 100);
        [JsonConverter(typeof(ColorConverter))]
        public Color burningcolor { get; set; } = new Color(250, 75, 25, 100);
        [JsonConverter(typeof(ColorConverter))]
        public Color firecolor { get; set; } = new Color(250, 60, 20, 120);

        // Character-specific colors
        [JsonConverter(typeof(ColorConverter))]
        public Color characterIcolor { get; set; } = new Color(0, 180, 130, 255);

        // Level Back-BackGround
        [JsonConverter(typeof(ColorConverter))]
        public Color backColor { get; set; } = new Color(22, 40, 36, 255);


        public Colors()
        {

        }

        public void switchBackColor(int level)
        {
            if (level == 1)
            {
                backColor = new Color(22, 40, 36, 255);
            }
            else if (level == 2)
            {
                backColor = new Color(23, 13, 3, 255);
            }
            else
            {
                backColor = new Color(43, 20, 20, 255);
            }
        }
    }
}
