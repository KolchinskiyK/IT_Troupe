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

        [JsonConstructor]
        public Player()
        {          
            playerTexture = hacker; // Load the texture
            playerPosition = new Vector2(30, 352);
            playerWidth = 30;
            playerHeight = 60;
            sourceRect = new Rectangle(0, 0, playerTexture.Width, playerTexture.Height);
            player = new Rectangle(playerPosition, player.Width, player.Height);
            // Initialisiere die Standardwerte für andere Attribute
            character = 1;
            lastSafePosition = playerPosition;
            currentHP = maxHP;        // Spieler beginnt mit voller HP
        }

        /*Character Swap*/
        private void SwapCharacter(Colors colors)
        {
            if (character == 1)
            {
                character++;                    // Swap to the next
                playerPosition.Y += 10;         // Moves it 10f under to avoid Height-Difference  
            }
            else if (character == 2)
            {
                character++;                    // Swap to the next
            }
            else
            {
                character = 1;                  // Swap to the first
            }

            UpdateCharacterProperties(colors);
        }
        public void SwapCharacterR(Colors colors)
        {
            if (character == 1)
            {
                character = 3;                    // Swap to the previous
            }
            else if (character == 3)
            {
                character = 2;                    // Swap to the previous
                playerPosition.Y += 5;            // Moves it 5f under to avoid Height-Difference 
            }
            else
            {
                character = 1;                    // Swap to the first
            }
            UpdateCharacterProperties(colors);
        }
        // Character swap in 2 ways
        public void CheckforSwap(Colors colors)
        {
            if (IsKeyPressed(KeyboardKey.F))
            {
                SwapCharacter(colors);      // Next
            }
            if (IsKeyPressed(KeyboardKey.R))
            {
                SwapCharacterR(colors);     // Previous
            }
        }
        public void UpdateCharacterProperties(Colors colors)
        {
            // change attributes based on character
            switch (character)
            {
                case 1: // Hacker
                    playerTexture = hacker;
                    playerWidth = 30;
                    playerHeight = 60;
                    break;

                case 2: // Rocker
                    playerTexture = rocker;
                    playerWidth = 45;
                    playerHeight = 55;
                    break;

                case 3: // Artist
                    playerTexture = artist;
                    playerWidth = 35;
                    playerHeight = 55;
                    break;
            }
            sourceRect = new Rectangle(0, 0, playerTexture.Width, playerTexture.Height);
        }
        public void Shoot(List<Player_Attack> shoots, List<Test_Enemy> testEnemies, Colors colors, List<Enemy_Burnout> burnoutEnemies, List<Enemy_Procrastination> procrastinations)
        {
            // Player Attack Shooting
            Player_Attack.CanShoot(deltaTime);
            if ((IsKeyPressed(KeyboardKey.M) || IsMouseButtonDown(MouseButton.Left)) && Player_Attack.isReady)
            {
                //Console.WriteLine("PlayerPositon X " + playerPosition.X);
                //Console.WriteLine("PlayerPositon Y " + playerPosition.Y);

                Player_Attack.timeSinceLastShot = 0f;
                Vector2 shootDirection = new Vector2(playerDirection, 0);

                shoots.Add(new Player_Attack(playerPosition.X + player.Width / 2, playerPosition.Y + player.Height / 2, shootDirection));

            }
            // Update shooting
            foreach (var shoot in shoots)
            {
                shoot.ShootUpdate(deltaTime);
                shoot.ShootDraw(colors);
            }
            shoots.RemoveAll(p => !p.isActive);
            foreach (var shoot in shoots)
            {
                foreach (var enemy in testEnemies)
                {
                    //Console.WriteLine(shoot.isActive);
                    //Console.WriteLine(testAnemy1.CheckCollision(shoot.rect));
                    if (shoot.isActive && enemy.CheckCollision(shoot.rect))
                    {
                        shoot.isActive = false;
                        enemy.DealDamageToEnemy(8);
                    }
                }

                foreach (var burnout in burnoutEnemies)
                {
                    //Console.WriteLine(shoot.isActive);
                    //Console.WriteLine(testAnemy1.CheckCollision(shoot.rect));
                    if (shoot.isActive && burnout.CheckCollision(shoot.rect))
                    {
                        shoot.isActive = false;
                        burnout.DealDamageToEnemy(8);
                    }
                }

                foreach (var procrastination in procrastinations)
                {
                    //Console.WriteLine(shoot.isActive);
                    //Console.WriteLine(testAnemy1.CheckCollision(shoot.rect));
                    if (shoot.isActive && procrastination.CheckCollision(shoot.rect))
                    {
                        shoot.isActive = false;
                        procrastination.DealDamageToEnemy(8);
                    }
                }

            }

        }
        public void Missile(List<Player_AttackM2> missiles, List<Test_Enemy> testEnemies, List<Wall> walls, Colors colors, List<Enemy_Burnout> burnoutEnemies, List<Enemy_Procrastination> procrastinations)
        {
            Player_AttackM2.CanShootMissile(deltaTime);
            // Player Attack Missile
            if ((IsKeyPressed(KeyboardKey.N) || IsMouseButtonPressed(MouseButton.Right)) && Player_AttackM2.isReady)
            {
                Player_AttackM2.timeSinceLastShot = 0f;
                Vector2 missileDirection = new Vector2(playerDirection, 0);

                missiles.Add(new Player_AttackM2(playerPosition.X + player.Width / 2, playerPosition.Y + player.Height / 2, missileDirection));

            }

            // Update missile
            foreach (var missile in missiles)
            {
                missile.MissileUpdate(deltaTime);
                missile.MissileDraw(colors);
            }

            missiles.RemoveAll(p => !p.isActive);

            foreach (var missile in missiles)
            {
                foreach (var wall in walls.ToList())        // Making ToList() to iterate the copy of walls
                {
                    if (missile.isActive && CheckCollisionRecs(new Rectangle(wall.x, wall.y, wall.Width, wall.Height), missile.rect))
                    {
                        missile.isActive = false;           // Disables Missile
                        walls.Remove(wall);                 // Deletes wall
                    }
                }
                foreach (var enemy in testEnemies)
                {
                    if (missile.isActive && enemy.CheckCollision(missile.rect))
                    {
                        missile.isActive = false;
                        enemy.DealDamageToEnemy(20);
                    }
                }

                foreach (var burnout in burnoutEnemies)
                {
                    if (missile.isActive && burnout.CheckCollision(missile.rect))
                    {
                        missile.isActive = false;
                        burnout.DealDamageToEnemy(20);
                    }
                }

                foreach (var procrastination in procrastinations)
                {
                    if (missile.isActive && procrastination.CheckCollision(missile.rect))
                    {
                        missile.isActive = false;
                        procrastination.DealDamageToEnemy(20);
                    }
                }

            }
        }



        // Überladene Methode für eine Liste von Plattformen
        public void Update(AllObstacles obsta, Camera2D camera, GameOverScreen gameover, Colors colors, AllEnemies enemies, List<Player_Attack> shoots, List<Player_AttackM2> missiles, List<Enemy_Burnout> burnoutEnemies, List<Enemy_Procrastination> procrastinations)
        {
            deltaTime = GetFrameTime(); // update deltaTime
            Move();
            CheckforSwap(colors);
            Jump();
            ApplyGravity();
            PlungeAttack();
            AdvancedPlunge(enemies, colors);
            PaintingAbility();
            PaintingLogic(colors);
            EnergyRegen();
            RockRage(enemies, colors);
            Dash();
            Backflip();
            Teleport(obsta.platforms, obsta.floors, camera);

            Shoot(obsta.shoots, enemies.testEnemies, colors, burnoutEnemies, procrastinations);
            Missile(obsta.missiles, enemies.testEnemies, obsta.walls, colors, burnoutEnemies, procrastinations);

            // update platform collision
            CheckAllPlatCollisions(obsta, gameover, enemies, colors);

            Speedbuff();
            Jumpbuff();
        }

        //heal player for 20% (apple)
        public void AppleHeal(float amount)
        {
            currentHP += (int)amount;
            if (currentHP >= maxHP)
                currentHP = maxHP;  //makes sure player HP won't be bigger than the maximum HP
        }
        // Draw the player as a rectangle
        public void Draw()
        {
            // flip texture when you move left/right    
            if (playerDirection == -1)
            {
                if (sourceRect.Width > 0)
                {
                    //sourceRect.Width = -sourceRect.Width;
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height);
                }

            }
            else if (playerDirection == 1)
            {
                if (sourceRect.Width <= 0)
                {
                    //sourceRect.Width = -sourceRect.Width;
                    sourceRect = new Rectangle(sourceRect.X, sourceRect.Y, -sourceRect.Width, sourceRect.Height);
                }

            }
            // Method "...Pro" allowes to rotate rectangle around the vector
            DrawTexturePro(
            playerTexture,     
            sourceRect,
            new Rectangle((int)playerPosition.X + playerWidth / 2, (int)playerPosition.Y + playerHeight / 2, playerWidth, playerHeight), // Zielrechteck
            new Vector2(playerWidth / 2, playerHeight / 2), 
            playerRotationAngle,                           
            Color.White                                    
            );
            
        }
        public void DrawFire(Colors colors)
        {
            // Draw Fire aroung character if in firing state
            if (firing > 0)
            {
                DrawCircle((int)playerPosition.X + playerWidth / 2, (int)playerPosition.Y + playerHeight / 2, 100, colors.burningcolor);
            }
        }
        public void TakeDamage(int damage)
        {
            currentHP -= damage;

            if (currentHP < 0) currentHP = 0;
        }

        /*Speedbonus*/
        // Counts 5 seconds, sets default speed after it expires
        public void Speedbuff()
        {
            countspeedframes += GetFrameTime();
            if (countspeedframes >= 5) // test for speedbonus for 5 seconds
            {
                speedbonus = 1;
            }
        }

        /*Jumpbonus*/
        // Counts down, till inactive
        public void Jumpbuff()
        {
            jumpBoostDuration -= 1;
            if (jumpBoostDuration <= 0) // test for speedbonus for 5 seconds
            {
                jumpBoost = 1;
            }
        }

    }
}
