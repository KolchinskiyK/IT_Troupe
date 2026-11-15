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
	public class Level_Texture
    { 
        public int screenWidth = 1280;
        public int screenHeight = 720;

        public Texture2D levelT;

        public Level_Texture()
		{
            levelT = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "World_Level_0", "_composite.png"));
        }
        // public void ReloadLevel(ref int levelcounter, Player player)
        public void ReloadLevel(Player player)
        {
            if (player.level == 1)        // Level 1
            {
                Unload();
                player.playerPosition.X = 55;
                player.playerPosition.Y = 1340;

                levelT = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "World_Level_0", "hq.png"));
                //levelcounter++;
            }
            else if (player.level == 2)   // Level 2
            {
                Unload();
                player.playerPosition.X = 30;
                player.playerPosition.Y = 352;

                levelT = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "World_Level_0", "_composite.png"));
                // levelcounter++;
            }
            else if (player.level == 3)                       // Level 3
            {
                Unload();
                player.playerPosition.X = 50;
                player.playerPosition.Y = 3050;

                levelT = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "World_Level_0", "lib.png"));
                //levelcounter = 1;
            }
        }
        public void Unload()
        {
            UnloadTexture(levelT);
        }

        public void Draw()
        {
            // Manually set the X positions for each texture
            int[] xOffsets = { 0, 512, 1024, 1280 };

            // Drawing each texture at specific X positions
            DrawTexturePro(
                levelT,
                new Rectangle(0, 0, levelT.Width, levelT.Height),   // Source: Full texture
                new Rectangle(xOffsets[0], 0, levelT.Width, levelT.Height), // Destination
                new Vector2(0, 0),                                           // No rotation
                0,                                                           // No rotation
                Color.White                                                   // No tint
            );


         /* DrawTexturePro(
                composite1,
                new Rectangle(0, 0, composite1.Width, composite1.Height),   // Source: Full texture
                new Rectangle(xOffsets[1], 0, composite1.Width, composite1.Height), // Destination
                new Vector2(0, 0),                                           // No rotation
                0,                                                           // No rotation
                Color.White                                                   // No tint
            );

            DrawTexturePro(
                composite2,
                new Rectangle(0, 0, composite2.Width, composite2.Height),   // Source: Full texture
                new Rectangle(xOffsets[2], 0, composite2.Width, composite2.Height), // Destination
                new Vector2(0, 0),                                           // No rotation
                0,                                                           // No rotation
                Color.White                                                   // No tint
            );

            DrawTexturePro(
                composite3,
                new Rectangle(0, 0, composite3.Width, composite3.Height),   // Source: Full texture
                new Rectangle(xOffsets[3], 0, composite3.Width, composite3.Height), // Destination
                new Vector2(0, 0),                                           // No rotation
                0,                                                           // No rotation
                Color.White                                                   // No tint
            ); */
        }
    }
}


// Load the composite texture (Windows-style relative path)
// Texture2D composite0 = LoadTexture("Assets\\World_Level_0\\_composite.png");
