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
    public class GameOverScreen
    {
        public int selectedOption { get; set; }
        public bool isGameOver { get; set; }
        public int maxHP { get; set; }

        [JsonConverter(typeof(Vector2Converter))]       // Apply the custom converter for Vector2
        public Vector2 resetPlayerPosition { get; set; }

        [JsonConstructor]
        public GameOverScreen(int maxHP, Vector2 resetPosition)
        {
            this.maxHP = maxHP;
            resetPlayerPosition = resetPosition;
            selectedOption = 0; // Default: Restart option
            isGameOver = false;
        }

        public bool IsGameOver => isGameOver; // Exposes the state of the game

        public void TriggerGameOver()
        {
            isGameOver = true;
        }

        public (bool Restart, Vector2 NewPlayerPosition, int HP) HandleInput()
        {
            if (!isGameOver) return (false, Vector2.Zero, 0);

            // Handle option navigation
            if (IsKeyPressed(KeyboardKey.W))
            {
                selectedOption = 0; // Select Restart
            }
            if (IsKeyPressed(KeyboardKey.S))
            {
                selectedOption = 1; // Select Quit
            }

            // Handle option selection
            if (IsKeyPressed(KeyboardKey.Space))
            {
                if (selectedOption == 0) // Restart
                {
                    isGameOver = false; // Exit game over state
                    return (true, resetPlayerPosition, maxHP); // Restart game with full HP and reset position
                }
                else if (selectedOption == 1) // Quit
                {
                    CloseWindow(); // Close the game window
                }
            }

            return (false, Vector2.Zero, 0); // No changes
        }

        public void Draw()
        {
            if (!isGameOver) return;

            // Display the "Game Over" screen
            BeginDrawing();
            ClearBackground(Color.RayWhite);
            DrawText("Game Over", 400, 300, 50, Color.Red);

            // Draw menu options
            DrawText("Restart", 420, 400, 30, selectedOption == 0 ? Color.Orange : Color.Black);
            DrawText("Quit the Game", 400, 450, 30, selectedOption == 1 ? Color.Orange : Color.Black);
            EndDrawing();
        }
    }
}
