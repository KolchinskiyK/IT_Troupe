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
    // Delete this class

    /*public class HUD
    {
        private Player player; // reference for the player

        [JsonConstructor]
        public HUD(Player player)
        {
            this.player = player;
        }
        //draws the HUD
        public void Draw()
        {
            // Draw the HP bar
            DrawRectangle(20, 20, 200, 30, Color.Gray);                                                   // Background for HP bar
            DrawRectangle(20, 20, (int)(200 * ((float)player.currentHP / player.maxHP)), 30, Color.Red);  // Current HP
            DrawText($"{player.currentHP}/{player.maxHP}", 70, 20, 30, Color.White);                      // Display current HP

            // Draw the energy bar
            DrawRectangle(20, 50, 200, 30, Color.LightGray);                                // Background for energy bar 
            DrawRectangle(20, 50, (int)(200 * (player.energy / 100)), 30, Color.Yellow);    // Current energy
            DrawText($"{player.energy:f0}/{100}", 70, 50, 30, Color.Black);                 // Display current energy in text
        }
    }*/
}
