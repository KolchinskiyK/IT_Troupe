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
using IT_Troupe;
using System.ComponentModel;


class Program
{
    // Make sure everything is public
    public static List<Player_Attack> shoots = new List<Player_Attack>();
    public static List<Player_AttackM2> missiles = new List<Player_AttackM2>();

    // Custom Textures & Fonts
    public static Texture2D hpFrame = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "HPframe.png"));   // Hp&Energy Frame
    public static Texture2D cursor = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "cursor.png"));     // Custom cursor
    public static Font font = LoadFont(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "zelda.ttf"));                // Custom Font, "geometryM" other fav

    public static Texture2D settings = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "BGsettings.png"));     // Settings
    public static Texture2D happyend = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Textures", "happyend.png"));     // Settings

    public static AllEnemies enemys = new AllEnemies();

    public static AllObstacles obsts = new AllObstacles();


    //public static List<Enemy_Procrastination> procrastinations = new List<Enemy_Procrastination>()
    //{
    //    new Enemy_Procrastination(new List<float> {850, 990, 50,60,750,1150}),
    //    new Enemy_Procrastination(new List<float> {800, 735, 50,60,660,1000}),
    //    new Enemy_Procrastination(new List<float> {1690, 1345, 50,60,1600,1800})
    //};

    // Public sound/music
    public static Sounds sounds = new Sounds();

    public static MainMenu mainMenu = new MainMenu();

    public static Colors colors = new Colors();

    public static Drawing draw = new Drawing();

    public static Player player = new Player();

    public static GameOverScreen gameOverScreen = new GameOverScreen(player.maxHP, new Vector2(11, 352));

    public static Level_Texture texture = new Level_Texture();

    public const int screenWidth = 1280;
    public const int screenHeight = 720;
    public static float scale = Math.Min((float)GetScreenWidth() / 1280, (float)GetScreenHeight() / 720);

    //public static int level = 1; // this varialbe was moved to PlayerVariables.cs

    static void Main(string[] args)
    {
        // Initialisierung von Raylib
        InitWindow(screenWidth, screenHeight, "IT Troupe");
        SetTargetFPS(60);
        //ToggleFullscreen();
        SetConfigFlags(ConfigFlags.VSyncHint);
        SetConfigFlags(ConfigFlags.HighDpiWindow);

        //float deltaTime = GetFrameTime();
        InitAudioDevice();

        bool isOnCooldown = false;
        DateTime lastAttackTime = DateTime.MinValue;
        TimeSpan attackCooldown = TimeSpan.FromSeconds(5);

        player.camera = new Camera2D
        {
            Offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2),          // Center the camera in the middle of the screen
            Target = player.playerPosition,                                             // Center the camera in the middle of the screen
            Rotation = 0.0f,
            Zoom = 1.2f * (1 + (float)Math.Log(scale, 5))
        };

        // Activation of Main-Menu Music
        Sounds.playMainMusic();

        //Vector4 Vector4One = new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        Shader shader = LoadShader(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Shaders", "shader.vs"), System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Shaders", "shader.fs"));
        Vector4 shaderColor = new Vector4(0.88f, 0.95f, 0.8f, 1.0f);

        int shaderColorLocation = GetShaderLocation(shader, "shaderColor");
        SetShaderValue(shader, shaderColorLocation, shaderColor, ShaderUniformDataType.Vec4);
        Vector4 lightColor = new Vector4(1.0f, 0.9f, 0.6f, 1.0f); // Warm Light
        int lightColorLocation = GetShaderLocation(shader, "lightColor");
        SetShaderValue(shader, lightColorLocation, lightColor, ShaderUniformDataType.Vec4);

        //float lightRadius = 200.0f; // Radius
        //int lightRadiusLocation = GetShaderLocation(shader, "lightRadius");
        //SetShaderValue(shader, lightRadiusLocation, lightRadius, ShaderUniformDataType.Float);

        int lightPosLocation = GetShaderLocation(shader, "lightPos");
        int timeLocation = GetShaderLocation(shader, "time");

        bool shaderOn = false;

        enemys.ReloadEnemies(player.level);
        obsts.ReloadObstacles(player.level);
        texture.ReloadLevel(player);

        HideCursor();       // "Hides" Windows cursor

        while (!WindowShouldClose())
        {
            if (mainMenu.IsActive)
            {
                bool startGame = mainMenu.HandleInput(player); // Benutzereingaben verarbeiten
                mainMenu.Draw(); // Hauptmenü zeichnen
                Sounds.checkMainMusic();            // Plays the right music-part
                player.settingsFromMain = player.saveLoadFromMain = true;
                //if (IsKeyPressed(KeyboardKey.Space)) { player.isSaveLoadMenu = !player.isSaveLoadMenu; }  // Acces "Load Game" on MainMenu

                if (startGame)
                {
                    continue;
                }
            }
            else
            {
                if (IsKeyPressed(KeyboardKey.I)) { player.isSaveLoadMenu = !player.isSaveLoadMenu; player.isSettings = false; }  // Press "I" to access Save and Load Menu
                if (IsKeyPressed(KeyboardKey.O)) { player.isSettings = !player.isSettings; player.isSaveLoadMenu = false; }      // Press "O" to access Settings

                if (player.isSaveLoadMenu && player.saveLoadFromMain) { ShowSaveLoadMenu(1); }
                else if (player.isSaveLoadMenu) { ShowSaveLoadMenu(2); }                                    // Open the Save and Load Menu
                else if (player.isSettings && player.settingsFromMain) { ShowSettings(1); }
                else if (player.isSettings) { ShowSettings(2); }                                    // Open the Save and Load Menu
                else if (player.gameCompleted) { ShowEnd(); }                                    // Show Endscreen
                else
                {
                    player.settingsFromMain = player.saveLoadFromMain = false;
                    // Shaders
                    float time = (float)GetTime();
                    SetShaderValue(shader, timeLocation, time, ShaderUniformDataType.Float);
                    SetShaderValue(shader, lightPosLocation, player.playerPosition, ShaderUniformDataType.Vec2);

                    if (shaderOn)
                        BeginShaderMode(shader);

                    BeginDrawing();
                    ClearBackground(Color.RayWhite);
                    // Level BG
                    DrawRectangle(-2500, -2500, 8000, 8000, colors.backColor);
                    BeginMode2D(player.camera);          // Start drawing relative to the camera

                    // Draw Level_Texture
                    texture.Draw();
                    EndShaderMode();

                    Vector2 mousePos = GetMousePosition();  //get Mouse position for later use

                    float deltaTime = GetFrameTime();

                    if (player.currentHP <= 0)
                    {
                        gameOverScreen.TriggerGameOver();
                    }

                    if (gameOverScreen.IsGameOver)
                    {
                        var (restart, newPlayerPosition, restoredHP) = gameOverScreen.HandleInput();

                        if (restart)
                        {
                            if (player.level == 1)
                            {
                                newPlayerPosition = new Vector2(55, 1340);
                            }
                            else if (player.level == 2)
                            {
                                newPlayerPosition = new Vector2(30, 352);
                            }
                            else if (player.level == 3)
                            {
                                newPlayerPosition = new Vector2(50, 3070);
                            }
                            player.playerPosition = newPlayerPosition;  // Reset player position
                            player.currentHP = restoredHP;              // Reset HP

                            enemys.ReloadEnemies(player.level);         // Reset Enemy
                            obsts.ReloadObstacles(player.level);        // Reset Obstacles
                            texture.ReloadLevel(player);                // Reset Texture
                            continue; // Skip the rest of the loop to restart
                        }

                        gameOverScreen.Draw();
                        continue; // Skip the rest of the loop while in Game Over state
                    }

                    // After updating the player's position, set the camera's target to the player's position
                    player.camera.Target = player.playerPosition;


                    // Enemy_Burnout
                    foreach (var burnout in enemys.burnoutEnemies)
                    {
                        if (burnout.currentHP > 0)
                        {
                            burnout.Draw();
                            burnout.Update(player, deltaTime);

                            if (burnout.CheckFireballCollision(new Rectangle(player.playerPosition, player.playerWidth, player.playerHeight)))
                            {
                                player.currentHP -= 10;
                            }
                        }
                    }
                    enemys.burnoutEnemies.RemoveAll(enemy => (enemy.currentHP <= 0));

                    // Enemy_Procrastination
                    foreach (var procrastination in enemys.procrastinationEnemies)
                    {
                        if (procrastination.currentHP > 0)
                        {
                            procrastination.Draw();
                            procrastination.Movement(player, deltaTime);
                        }
                    }
                    enemys.procrastinationEnemies.RemoveAll(enemy => (enemy.currentHP <= 0));

                    // Enemy_Fear
                    foreach (var enemy in enemys.testEnemies)
                    {
                        float distance = enemy.CheckDistance(player);
                        if (distance > 300 || player.playerPosition.Y <= 440)
                        {
                            //testEnemy1.Move();
                            enemy.DetectPlayer(player, obsts);
                        }

                        // Enemy attacks the player
                        if (enemy.currentHP > 0)
                        {
                            enemy.Attack(player);

                            if (!isOnCooldown)
                            {
                                bool attacked = enemy.Attack2(player);
                                if (attacked)
                                {
                                    isOnCooldown = true;
                                    lastAttackTime = DateTime.Now;
                                }
                            }
                            if (isOnCooldown && DateTime.Now - lastAttackTime >= attackCooldown)
                            {
                                isOnCooldown = false;
                            }

                        }
                    }
                    enemys.testEnemies.RemoveAll(enemy => (enemy.currentHP <= 0));

                    Drawing.Paint(player, obsts, colors, enemys.testEnemies);
                    player.Update(obsts, player.camera, gameOverScreen, colors, enemys, shoots, missiles, enemys.burnoutEnemies, enemys.procrastinationEnemies);
                    if (IsKeyPressed(KeyboardKey.P)) { Sounds.SwapLevelMusic(); }  // Press "P" DEBUG
                    if (IsKeyPressed(KeyboardKey.X)) { if (shaderOn) shaderOn = false; else shaderOn = true; }  // Press "X"

                    Sounds.UpdateLevelMusic();
                    // DEBUG
                    if (IsKeyPressed(KeyboardKey.U))    // Press "U" for Level update
                    {
                        UpdateLevel();
                    }


                    EndMode2D();   // End camera mode

                    // Draw everything that shouldn't move outside the camera
                    player.OutOfEnergy();
                    //DrawText("IT Troupe" + " " + scale, 330, 50, 30, Color.Black);      // Text on (330, 50), font size: 30

                    // Draw the HP bar
                    DrawRectangle(20, 20, (int)(200 * scale), (int)(30 * scale), Color.Gray);                                               // Background for HP bar
                    DrawRectangle(20, 20, (int)(200 * ((float)player.currentHP / player.maxHP) * scale), (int)(30 * scale), Color.Red);     // Current HP
                    //DrawText($"{player.currentHP}/{player.maxHP}", 30 + (int)(40 * scale), 20 + (int)(2 * scale), (int)(30 * scale), Color.White);   // Display current HP
                    DrawTextEx(font, $"{player.currentHP}/{player.maxHP}", new Vector2(30 + (int)(40 * scale), 20 + (int)(2 * scale)), (int)(30 * scale), 1.5f, Color.White);

                    // Draw the energy bar
                    DrawRectangle(20, 20 + (int)(30 * scale), (int)(200 * scale), (int)(30 * scale), Color.LightGray);                                  // Background for energy bar 
                    DrawRectangle(20, 20 + (int)(30 * scale), (int)(200 * (player.energy / 100) * scale), (int)(30 * scale), Color.Yellow);             // Current energy
                    //DrawText($"{player.energy:f0}/{100}", 30 + (int)(40 * scale), 20 + (int)(30 * scale), (int)(30 * scale), Color.Black);    // Display current energy in text
                    DrawTextEx(font, $"{player.energy:f0}/{100}", new Vector2(30 + (int)(40 * scale), 20 + (int)(30 * scale)), (int)(30 * scale), 1.5f, Color.Black);

                    // DrawTexture(hpFrame, 15, 15, Color.White);  // Custom Frame texture
                    DrawTexturePro(hpFrame, new Rectangle(0, 0, 225, 101), new Rectangle((15 + 225 / 2), (15 + 101 / 2), 232 * scale, 85 * scale), new Vector2(225 / 2, 101 / 2), 0, Color.White);

                    // Custom Mouse
                    //DrawTexture(cursor, (int)mousePos.X, (int)mousePos.Y, Color.White);
                    DrawTexturePro(cursor, new Rectangle(0, 0, 100, 96), new Rectangle((int)mousePos.X, (int)mousePos.Y, 30 * scale, 22 * scale), new Vector2(100 / 2, 96 / 2), 0, Color.White);
                    //Hotkey Help
                    DrawTextEx(font, $"Press 'H' to hide/show Hotkey Help", new Vector2(10 + (int)(5 * scale), 100 + (int)(40 * scale)), (int)(20 * scale), 1.5f, Color.White);
                    if (IsKeyPressed(KeyboardKey.H)) { if (player.hotkeysShown) player.hotkeysShown = false; else player.hotkeysShown = true; }  // Toggle
                    DrawHotkeys();

                    EndDrawing();
                    //EndShaderMode();
                }

            }
        }

        // Unload the texture
        UnloadShader(shader);
        texture.Unload();

        CloseWindow();
    }

    public static void ShowSaveLoadMenu(int from)  // New method to handle the save/load menu, (1 = from Main Menu, 2 = from Game)
    {
        if (IsKeyPressed(KeyboardKey.W)) { player.selectedSaveSlot = (player.selectedSaveSlot - 1 + 3) % 3; }
        if (IsKeyPressed(KeyboardKey.S)) { player.selectedSaveSlot = (player.selectedSaveSlot + 1) % 3; }

        // Save/Load Logic
        if (IsKeyPressed(KeyboardKey.K))    // Press "K" to save
        {
            string saveFile = "save_slot_" + (player.selectedSaveSlot + 1) + ".json";
            SaveLoad.SaveGame(saveFile, enemys, obsts, sounds, mainMenu, colors, player); // Save Game

            Console.WriteLine($"Game saved to {saveFile}");
        }
        if (IsKeyPressed(KeyboardKey.L))    // Press "L" to load
        {
            string saveFile = "save_slot_" + (player.selectedSaveSlot + 1) + ".json";
            SaveLoad.LoadGame(saveFile, ref enemys, ref obsts, ref sounds, ref mainMenu, ref colors, ref player); // Load game

            player.isSaveLoadMenu = false;  // Close the Save/Load Menu after loading
            mainMenu.isInMainMenu = false;  // Ensure we exit the main menu after loading
            Console.WriteLine($"Game loaded from {saveFile}");
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);
        //DrawTexture(settings, 0, 0, Color.White);
        DrawTexturePro(settings, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, 1280 * scale, 720 * scale), new Vector2(0, 0), 0, Color.White);
        /*
        DrawText("ESC Menu - Press 'K' to Save, 'L' to Load", 200, 200, 30, Color.Black);
        if(from == 1)DrawText("Press 'B' to return to Menu", 200, 250, 20, Color.Black);
        else DrawText("Press 'I' to return to game", 200, 250, 20, Color.Black);
        */
        DrawTextEx(font, "ESC Menu - Press 'K' to Save, 'L' to Load", new Vector2(200 * scale, 190 * scale), 40 * scale, 10, Color.White);
        if (from == 1) DrawTextEx(font, "Press 'B' to return to Menu", new Vector2(200 * scale, 25 + 225 * scale), 30 * scale, 10, Color.White);
        else DrawTextEx(font, "Press 'I' to return to Game", new Vector2(200 * scale, 25 + 225 * scale), 30 * scale, 10, Color.White);

        // Draw save/load options with highlighting
        string[] saveOptions = { "Save Slot 1", "Save Slot 2", "Save Slot 3" };
        for (int i = 0; i < saveOptions.Length; i++)
        {
            Color highlightColor = (i == player.selectedSaveSlot) ? Color.Orange : Color.White;
            DrawTextEx(font, saveOptions[i], new Vector2(350 * scale, (300 + i * 50) * scale), 30 * scale, 10, highlightColor);
        }

        if (IsKeyPressed(KeyboardKey.B) && from == 1) { mainMenu.isInMainMenu = true; player.isSaveLoadMenu = false; player.saveLoadFromMain = false; }
        EndDrawing();
    }

    public static void ShowSettings(int from)  // Settings (1 = from Main Menu, 2 = from Game)
    {
        // if (from == 1)
        //{ player.settingsFromMain = false; }

        // Resolutions Array
        Vector2[] resolutions = {
            new Vector2(800, 600),
            new Vector2(1280, 720),
            new Vector2(1920, 1080),
            new Vector2(2560, 1440)
        };
        // Volume Array
        float[] volumes = {
            0.2f,
            0.4f,
            0.6f,
            0.8f,
            1.0f,
            0.0f
        };
        // Zooms Array
        float[] zooms = {
            0.8f,
            1f,
            1.2f,
            1.4f,
            1.6f
        };
        string[] fullscreen = {
            "On",
            "Off"
        };


        string[] settingsOptions = {
            "Resolution: " + resolutions[player.chosenResolution],
            "Volume: " + volumes[player.chosenVolume],
            "Zoom: " + zooms[player.chosenZoom],
            "Fullscreen: " + fullscreen[player.chosenFullScreen]
        };


        if (IsKeyPressed(KeyboardKey.W)) { player.selectedSettingsSlot = (player.selectedSettingsSlot - 1 + settingsOptions.Length) % settingsOptions.Length; }
        if (IsKeyPressed(KeyboardKey.S)) { player.selectedSettingsSlot = (player.selectedSettingsSlot + 1) % settingsOptions.Length; }

        BeginDrawing();
        ClearBackground(Color.RayWhite);
        // DrawTexture(settings, 0, 0, Color.White);
        DrawTexturePro(settings, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, 1280 * scale, 720 * scale), new Vector2(0, 0), 0, Color.White);
        DrawTextEx(font, "Press 'Space' to Apply Changes ", new Vector2(200 * scale, 190 * scale), 40 * scale, 10, Color.White);
        if (from == 1) DrawTextEx(font, "Press 'B' to return to Menu", new Vector2(200 * scale, 25 + 225 * scale), 30 * scale, 10, Color.White);
        else DrawTextEx(font, "Press 'O' to return to Game", new Vector2(200 * scale, 25 + 225 * scale), 30 * scale, 10, Color.White);

        for (int i = 0; i < settingsOptions.Length; i++)
        {
            Color highlightColor = (i == player.selectedSettingsSlot) ? Color.Orange : Color.White;
            DrawTextEx(font, settingsOptions[i], new Vector2(350 * scale, (300 + i * 50) * scale), 30 * scale, 10, highlightColor);
        }
        if (IsKeyPressed(KeyboardKey.B) && from == 1) { mainMenu.isInMainMenu = true; player.isSettings = false; player.settingsFromMain = false; }

        switch (player.selectedSettingsSlot)
        {
            case 0:
                if (IsKeyPressed(KeyboardKey.A)) { player.chosenResolution = (player.chosenResolution - 1 + resolutions.Length) % resolutions.Length; }
                if (IsKeyPressed(KeyboardKey.D)) { player.chosenResolution = (player.chosenResolution + 1) % resolutions.Length; }
                if (IsKeyPressed(KeyboardKey.Space))
                {
                    SetWindowSize((int)resolutions[player.chosenResolution].X, (int)resolutions[player.chosenResolution].Y);
                    player.camera.Target = player.playerPosition;
                    player.camera.Offset = new Vector2((int)resolutions[player.chosenResolution].X / 2, (int)resolutions[player.chosenResolution].Y / 2);
                    scale = Math.Max((float)resolutions[player.chosenResolution].X / 1280, (float)resolutions[player.chosenResolution].Y / 720);
                }
                //UpdateCam();
                //player.camera.Target = player.playerPosition;
                //player.camera.Offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
                //scale = Math.Min((float)GetScreenWidth() / 1280, (float)GetScreenHeight() / 720);
                break;

            case 1:
                if (IsKeyPressed(KeyboardKey.A)) { player.chosenVolume = (player.chosenVolume - 1 + volumes.Length) % volumes.Length; }
                if (IsKeyPressed(KeyboardKey.D)) { player.chosenVolume = (player.chosenVolume + 1) % volumes.Length; }
                if (IsKeyPressed(KeyboardKey.Space)) { SetMasterVolume(volumes[player.chosenVolume]); }

                break;

            case 2:
                if (IsKeyPressed(KeyboardKey.A)) { player.chosenZoom = (player.chosenZoom - 1 + zooms.Length) % zooms.Length; }
                if (IsKeyPressed(KeyboardKey.D)) { player.chosenZoom = (player.chosenZoom + 1) % zooms.Length; }
                if (IsKeyPressed(KeyboardKey.Space)) { player.camera.Zoom = zooms[player.chosenZoom] * (1 + (float)Math.Log(scale, 5)); }

                break;

            case 3:
                //if (IsKeyPressed(KeyboardKey.A)) { player.chosenFullScreen = (player.chosenFullScreen - 1 + 2) % 2; }
                //if (IsKeyPressed(KeyboardKey.D)) { player.chosenFullScreen = (player.chosenFullScreen + 1) % 2; }

                if (IsKeyPressed(KeyboardKey.Space)) { ToggleFullscreen(); player.chosenFullScreen = (player.chosenFullScreen + 1) % 2; }

                break;

        }
        EndDrawing();
    }

    public static void UpdateCam()
    {
        //Update camera position depending on players position
        player.camera.Target = player.playerPosition;
        float lerpSpeed = 0.1f;
        Vector2 desiredPosition = player.playerPosition;
        player.camera.Target = Vector2.Lerp(player.camera.Target, desiredPosition, lerpSpeed);
    }
    public static void DrawHotkeys()
    {
        string[] hotkeys = {
            "Walk:  A - D ",
            "Jump:  'Space' ",
            "Plunge Attack:  'Space + S' ",
            "Climb:  'W - S' ",
            "Character Swap:  'F / R' ",
            "Movement Ability:  'Left Shift'",
            "Combat Ability:  'E'",
            "Light Attack:  'M1/M'",
            "Strong Attack:  'M2/N'",
            "Open Settings:  'O'",
            "Open Save/Load Menu:  'I'",
            "Toggle Shaders:  'X'",
        };
        if (player.hotkeysShown)
        {
            for (int i = 0; i < hotkeys.Length; i++)
            {
                DrawTextEx(font, hotkeys[i], new Vector2(10 + (int)(5 * scale), (int)((160 + i * 20) * scale)), (int)(20 * scale), 2f, Color.White);
            }
        }
    }
    public static void ShowEnd()
    {
        BeginDrawing();
        ClearBackground(Color.RayWhite);
        DrawTexturePro(happyend, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, 1280 * scale, 720 * scale), new Vector2(0, 0), 0, Color.White);
        DrawTextEx(font, "Well done!!! Thank You for playing", new Vector2(240 * scale, 200 * scale), 40 * scale, 8, new Color(235, 255, 200, 255));
        DrawTextEx(font, "Press 'SPACE' to exit. Till next time =)", new Vector2(240 * scale, 250 * scale), 40 * scale, 8, new Color(235, 255, 200, 255));
        if (IsKeyPressed(KeyboardKey.Space)) { mainMenu.isInMainMenu = true; player.isSaveLoadMenu = false; player.saveLoadFromMain = false; player.gameCompleted = false; CloseWindow(); }
        EndDrawing();
    }

    public static void UpdateLevel()
    {
        player.verticalVelocity = 0;
        player.level++;  // Increment the level

        // Reset level to 1 if it exceeds 3
        if (player.level > 3)
        {
            player.level = 1;
            player.gameCompleted = true;    
            ShowEnd();
        }
        else
        {
            enemys.ReloadEnemies(player.level);
            obsts.ReloadObstacles(player.level);
            colors.switchBackColor(player.level);
            Sounds.SwapLevelMusic();
            texture.ReloadLevel(player);
        }

    }
}