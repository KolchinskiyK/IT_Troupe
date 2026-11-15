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
using System.ComponentModel;

namespace IT_Troupe
{
    public class MainMenu
    {
        public int selectedOption { get; set; }// chosen option atm
        public bool isInMainMenu { get; set; }

        [JsonConstructor]
        public MainMenu()
        {
            selectedOption = 0; // standart: "New Game" is chosen
            isInMainMenu = true;
        }

        // Load all textures
        Texture2D BG = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "MenuTry2.png"));

        Texture2D newGame1 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "New game.png"));
        Texture2D newGame2 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "New game Dark.png"));
        Texture2D loadGame1 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "Load game.png"));
        Texture2D loadGame2 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "Load game Dark.png"));
        Texture2D settings1 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "settings.png"));
        Texture2D settings2 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "settings Dark.png"));
        Texture2D quit1 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "quit_game.png"));
        Texture2D quit2 = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "quit game Dark.png"));


        public bool IsActive => isInMainMenu; // This will return 'true' if isInMainMenu is true, otherwise false
        public bool HandleInput(Player player)
        {
            if (!isInMainMenu) return false;

            // navigate through the menu
            if (IsKeyPressed(KeyboardKey.W))
            {
                selectedOption = (selectedOption - 1 + 4) % 4;
            }
            if (IsKeyPressed(KeyboardKey.S))
            {
                selectedOption = (selectedOption + 1) % 4;
            }

            // Bestätigung der Auswahl
            if (IsKeyPressed(KeyboardKey.Space))
            {
                switch (selectedOption)
                {
                    case 0: // New Game
                        // Unload textures to free memory
                        UnloadTexture(BG);
                        UnloadTexture(newGame1);
                        UnloadTexture(newGame2);
                        UnloadTexture(loadGame1);
                        UnloadTexture(loadGame2);
                        UnloadTexture(settings1);
                        UnloadTexture(settings2);
                        UnloadTexture(quit1);
                        UnloadTexture(quit2);
                        isInMainMenu = false; // leave menu

                        // Musik related Stuff
                        //Sounds.partI = true;

                        Sounds.StopMusic();
 
                        return true;         // start the game

                    case 1: // Load Game
                        isInMainMenu = false;
                        player.isSaveLoadMenu = true;
                        Program.ShowSaveLoadMenu(1); // Open the Save/Load menu
                        break;

                    case 2: // Settings
                        isInMainMenu = false;
                        player.isSettings = true;
                        Program.ShowSaveLoadMenu(1); // Open the Save/Load menu
                        break;

                    case 3: // Quit the Game
                        CloseWindow();
                        break;
                }
            }

            return false; // no action at the start of the game
        }

        // Draws the menu
        public void Draw()
        {
            if (!isInMainMenu) return;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // All different Scales, used different if were created originally for different resolutions
            float scale = Math.Min((float)GetScreenWidth() / 1920, (float)GetScreenHeight() / 1080);        // Scale for textures, that were originally for 1920x1080
            float scale2 = Math.Min((float)GetScreenWidth() / 1280, (float)GetScreenHeight() / 720);        // Scale for textures, that were originally for 1280x720

            // Background calculations
            Rectangle sourceBG = new Rectangle(0, 0, 1280, 720);
            Rectangle destBG = new Rectangle(0, 0, 1280 * scale2, 720 * scale2);
            DrawTexturePro(BG, sourceBG, destBG, Vector2.Zero, 0, Color.White);

            // Button Sizes
            int buttonWidth = 400;
            int buttonHeight = 70;

            // Space between Buttons
            int buttonSpacing = 20;

            // Place for upper Button
            int upperY = GetScreenHeight() / 2 + (int)(buttonHeight * 0.5f);

            // Coordinates for Buttons (sourche copies the texture from chosen area)(dest makes "new" rectangle), all automatisated for different Resolutions
            Rectangle sourceN = new Rectangle(0, 0, buttonWidth, buttonHeight);
            Rectangle destN = new Rectangle(GetScreenWidth() / 2 - (buttonWidth * scale - 30) / 2, upperY, buttonWidth * scale, buttonHeight * scale);

            Rectangle sourceL = new Rectangle(0, 0, buttonWidth, buttonHeight);
            Rectangle destL = new Rectangle(GetScreenWidth() / 2 - (buttonWidth * scale - 30) / 2, upperY + buttonHeight * scale + buttonSpacing, buttonWidth * scale, buttonHeight * scale);

            Rectangle sourceS = new Rectangle(0, 0, buttonWidth, buttonHeight);
            Rectangle destS = new Rectangle(GetScreenWidth() / 2 - (buttonWidth * scale - 30) / 2, upperY + 2 * (buttonHeight * scale + buttonSpacing), buttonWidth * scale, buttonHeight * scale);

            Rectangle sourceQ = new Rectangle(0, 0, buttonWidth, buttonHeight);
            Rectangle destQ = new Rectangle(GetScreenWidth() / 2 - (buttonWidth * scale - 30) / 2, upperY + 3 * (buttonHeight * scale + buttonSpacing), buttonWidth * scale, buttonHeight * scale);

            // Draw different buttons, if different states chosen
            switch (selectedOption)
            {
                case 0:     // New Game
                    DrawTexturePro(newGame1, sourceN, destN, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(loadGame2, sourceL, destL, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(settings2, sourceS, destS, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(quit2, sourceQ, destQ, Vector2.Zero, 0, Color.White);
                    break;

                case 1:     // Load Game
                    DrawTexturePro(newGame2, sourceN, destN, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(loadGame1, sourceL, destL, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(settings2, sourceS, destS, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(quit2, sourceQ, destQ, Vector2.Zero, 0, Color.White);
                    break;

                case 2:     // Settings
                    DrawTexturePro(newGame2, sourceN, destN, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(loadGame2, sourceL, destL, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(settings1, sourceS, destS, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(quit2, sourceQ, destQ, Vector2.Zero, 0, Color.White);
                    break;

                case 3:     // Quit Game
                    DrawTexturePro(newGame2, sourceN, destN, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(loadGame2, sourceL, destL, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(settings2, sourceS, destS, Vector2.Zero, 0, Color.White);
                    DrawTexturePro(quit1, sourceQ, destQ, Vector2.Zero, 0, Color.White);
                    break;
            }

            EndDrawing();
        }


    }
}