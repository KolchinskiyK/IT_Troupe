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
    public partial class Player
    {
        // List of created by ability I of character 3 areas
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Rectangle> paintings { get; set; } = new List<Rectangle>();
        // List of Plunging trace
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Rectangle> plungingPillar { get; set; } = new List<Rectangle>();
        // List of Fire traces
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Rectangle> fireTrace { get; set; } = new List<Rectangle>();

        public void EnergyRegen()
        {
            // Energy generation
            if (energy <= 100)
                energy += GetFrameTime() * 5;       // Regeneration speed could be changed
        }
        // "Out of Energy" text
        public void OutOfEnergy()
        {
            if (outOfEnergy > 0)
            {
                DrawText("Out of energy!!", 20, 100, 25, Color.Black);
                outOfEnergy--;
            }
        }

        // Ability I, Character 3: Painting
        public void PaintingAbility()
        {
            if (IsKeyPressed(KeyboardKey.E) && character == 3)
            {
                if (energy >= 50)                   // Checks, if enough energy
                {
                    energy -= 50;                   // Consuming energy

                    if (paintingtimer <= 400)       // If there is already painting exist, delete it
                    {
                        paintings.RemoveAt(0);
                    }
                    paintings.Add(new Rectangle(playerPosition.X - 185, playerPosition.Y - 100, 400, 200));
                    paintingtimer = 0;              // Sets a timer to 0, after it Rectangle-Zone disappeares
                }
                else
                {
                    outOfEnergy = 20;   // Sets the duration of "Out of Energy" text
                }
            }
        }
        public void PaintingLogic(Colors colors)
        {
            // Painting logic
            if (paintingtimer <= 400)               // Counts timer
            {
                paintingtimer += 1;
            }
            if (paintingtimer == 400)               // Timer ends, removes Rectangle
            {
                paintings.RemoveAt(0);
            }
            foreach (Rectangle painting in paintings)
            {
                DrawRectangleRec(painting, colors.paintingcolor);
                if (CheckCollisionRecs(new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight), painting))
                {
                    speedbonus = 1.5;               // Giving speedbonus
                    countspeedframes = 4;           // For 1 second (5-4)
                }
            }
        }
        // Ability I, Character 1: Advanced Plunge
        public void AdvancedPlunge(AllEnemies enemies, Colors colors)
        {
            if (IsKeyPressed(KeyboardKey.E) && character == 1)        // Binding should be changed 
            {
                if (energy >= 50)                   // Checks, if enough energy
                {
                    energy -= 50;                   // Consuming energy
                    isSearching = true;             // Starts searching for enemies
                }
                else
                {
                    outOfEnergy = 20;   // Sets the duration of "Out of Energy" text
                }
            }
            // When in searching state, check collision with all enemiies' hitboxes
            if (isSearching)
            {
                foreach (var enemy in enemies.testEnemies)
                {
                    if (CheckCollisionRecs(new Rectangle(playerPosition.X - 175, playerPosition.Y - 150, 350 + playerWidth, 225 + playerHeight), new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)) && !isVictim)
                    {
                        isVictim = true;                        // Target found
                        //character = 2;
                        UpdateCharacterProperties(colors);
                        playerPosition.Y = enemy.rect.Y - 125;       // Teleport above enemy
                        nonGravityTime = 10;                         // Gives 10 frames without gravity
                        plungeDuration = 50;                         // Gives Oportunity to deal dmg with plunge attack
                        playerPosition.X = enemy.rect.X;
                        plungingPillar.Add(new Rectangle(playerPosition.X - 5, playerPosition.Y - 10, 40, 250));    // Plunging Trace
                        plungingtimer = 0;                      // Duration of Plunging Trace
                        isSearching = false;                    // Exiting searching-state
                        break;
                    }
                }
                foreach (var enemy in enemies.burnoutEnemies)
                {
                    if (CheckCollisionRecs(new Rectangle(playerPosition.X - 175, playerPosition.Y - 150, 350 + playerWidth, 225 + playerHeight), new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)) && !isVictim)
                    {
                        isVictim = true;                        // Target found
                        //character = 2;
                        UpdateCharacterProperties(colors);
                        playerPosition.Y = enemy.rect.Y - 125;       // Teleport above enemy
                        nonGravityTime = 10;                         // Gives 10 frames without gravity
                        plungeDuration = 50;                         // Gives Oportunity to deal dmg with plunge attack
                        playerPosition.X = enemy.rect.X;
                        plungingPillar.Add(new Rectangle(playerPosition.X - 5, playerPosition.Y - 10, 40, 250));    // Plunging Trace
                        plungingtimer = 0;                      // Duration of Plunging Trace
                        isSearching = false;                    // Exiting searching-state
                        break;
                    }
                }
                foreach (var enemy in enemies.procrastinationEnemies)
                {
                    if (CheckCollisionRecs(new Rectangle(playerPosition.X - 175, playerPosition.Y - 150, 350 + playerWidth, 225 + playerHeight), new Rectangle(enemy.x, enemy.y, enemy.rect.Width, enemy.rect.Height)) && !isVictim)
                    {
                        isVictim = true;                        // Target found
                        //character = 2;
                        UpdateCharacterProperties(colors);
                        playerPosition.Y = enemy.y - 125;       // Teleport above enemy
                        nonGravityTime = 10;                         // Gives 10 frames without gravity
                        plungeDuration = 50;                         // Gives Oportunity to deal dmg with plunge attack
                        playerPosition.X = enemy.x;
                        plungingPillar.Add(new Rectangle(playerPosition.X - 5, playerPosition.Y - 10, 40, 250));    // Plunging Trace
                        plungingtimer = 0;                      // Duration of Plunging Trace
                        isSearching = false;                    // Exiting searching-state
                        break;
                    }
                }

                if (!isVictim)               // If nothing found after foreach-iteration, regain energy and exit state
                {
                    energy += 45;
                    isSearching = false;
                }
            }
            if (plungingtimer < 50)
            {
                plungingtimer += 1;              // Counts timer
            }
            foreach (Rectangle plunge in plungingPillar)
            {
                DrawRectangleRec(plunge, colors.plungingcolor);
            }
            // Timer ends
            if (plungingtimer == 50)
            {
                plungingPillar.RemoveAt(0);     // Removes Rectangle
                plungingtimer += 1;
                isVictim = false;               // Resetting variables
            }
            // Check if multiple rectangles, delete all
            if (plungingPillar.Count > 0 && plungingtimer==51)
            {
                plungingPillar.RemoveAt(0);     // Removes Rectangle
            }
        }
        // Ability I, Character 2: Rock&Rage
        public void RockRage(AllEnemies enemies, Colors colors)
        {
            // Activate the RockRage ability
            if (IsKeyPressed(KeyboardKey.E) && character == 2)
            {
                if (energy >= 50) // Ensure enough energy
                {
                    energy -= 50; // Consume energy
                    firing = 250; // Set the duration of the firing state
                }
                else
                {
                    outOfEnergy = 20; // Display "Out of Energy" for a duration
                }
            }

            // Execute firing state
            if (firing > 0)
            {
                firing--; // Decrease firing timer

                // Check for collisions between the player and enemies
                foreach (var enemy in enemies.burnoutEnemies)
                {
                    if (CheckCollisionCircleRec(playerPosition, 100, new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.25; // Decrease enemy health
                    }
                }
                foreach (var enemy in enemies.testEnemies)
                {
                    if (CheckCollisionCircleRec(playerPosition, 100, new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.25; // Decrease enemy health
                    }
                }
                foreach (var enemy in enemies.procrastinationEnemies)
                {
                    if (CheckCollisionCircleRec(playerPosition, 100, new Rectangle(enemy.x, enemy.y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.25; // Decrease enemy health
                    }
                }

                // Spawn fire traces periodically
                if (firing % 10 == 0)
                {
                    fireTrace.Add(new Rectangle(playerPosition.X - 10, playerPosition.Y + playerHeight - 20, 35, 20));
                }
            }

            // Remove fire traces after the firing state ends
            if (firing == 0 && fireTrace.Count > 0)
            {
                fireTrace.Clear(); // Clear all fire traces
            }
            // Check collisions between fire traces and enemies
            foreach (var fire in fireTrace)
            {
                foreach (var enemy in enemies.testEnemies)
                {
                    DrawRectangleRec(fire, colors.firecolor);
                    if (CheckCollisionRecs(fire, new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.05; // Decrease enemy health
                    }
                }
                foreach (var enemy in enemies.burnoutEnemies)
                {
                    DrawRectangleRec(fire, colors.firecolor);
                    if (CheckCollisionRecs(fire, new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.05; // Decrease enemy health
                    }
                }
                foreach (var enemy in enemies.procrastinationEnemies)
                {
                    DrawRectangleRec(fire, colors.firecolor);
                    if (CheckCollisionRecs(fire, new Rectangle(enemy.rect.X, enemy.rect.Y, enemy.rect.Width, enemy.rect.Height)))
                    {
                        enemy.currentHP -= 0.05; // Decrease enemy health
                    }
                }
            }
        }
    }
}
