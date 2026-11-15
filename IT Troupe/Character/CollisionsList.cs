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
        /*Platform Collisions*/
        public void CheckAllPlatCollisions(AllObstacles obsta, GameOverScreen gameover, AllEnemies enemies, Colors colors)
        {
            foreach (var platform in obsta.platforms)
            {
                PlatformCollision(platform, enemies, colors, gameover);
            }

            // update ladder collision
            foreach (var ladder in obsta.ladders)
            {
                Climbing(ladder);
            }
            //Update floor collision
            foreach (var floor in obsta.floors)
            {
                FloorCollision(floor, gameover, enemies);
            }

            // update destroyable floors
            foreach (var dfloor in obsta.dfloors.ToList())
            {
                DFloorCollision(dfloor, obsta.dfloors);
            }

            // update walls
            foreach (var wall in obsta.walls.ToList())
            {
                WallCollision(wall, obsta.walls);
            }

            // update grass obstacles
            foreach (var grassObstacle in obsta.grassObstacles)
            {
                GrasObstacleCollision(grassObstacle, gameover);
            }

            // update grass obstacles
            foreach (var pad in obsta.jumppads)
            {
                JumpPadCollision(pad);
            }
            foreach (MovingPlatform movingPlatform in obsta.movingPlatforms)
            {
                movingPlatform.Update();
                MovingPlatformCollision(movingPlatform, enemies);
            }

            foreach (HealItem hitem in obsta.healitems)
            {
                CheckCollision(hitem);
            }
            /*foreach (var portal in obsta.portals)
            {
                PortalCollision(portal);
            }*/
            PortalCollision(obsta.portals[0]);
        }


        /* Collision detection for platform*/
        // Check for landing on platforms or the ground
        public void PlatformCollision(Platforms platform, AllEnemies enemies, Colors colors, GameOverScreen gameover)
        {
            // Check if overlapping with the platform horizontally
            // Check if the player is positioned horizontally over the platform
            bool isOverlappingHorizontally = playerPosition.X + playerWidth > platform.x && playerPosition.X < platform.x + platform.Width;
          
            // Check if the player is landing on top of the platform while falling down
            if (isOverlappingHorizontally && playerPosition.Y + playerHeight >= platform.y && !(playerPosition.Y + playerHeight >= platform.y + 18) &&
                playerPosition.Y + playerHeight <= platform.y + platform.Height && verticalVelocity > 0)
            {
                playerPosition.Y = platform.y - playerHeight;           // Land on top of the platform
                verticalVelocity = 0f;                                  // Reset vertical velocity
                jumpCount = 0;                                          // Reset jump count when landing
                isOnPlatform = true;                                    // Mark as on platform
                lastSafePosition = playerPosition;                      // Update the last position before jumping

                PlungeDamageLogic(enemies);

                isPlunging = false;                                     // Resetting Plunging-state
            }

            //checks if player tries to jump through the platform from below
            if (playerPosition.X + playerWidth + 10 > platform.x && playerPosition.X < platform.x + platform.Width - 10 && 
                playerPosition.Y <= platform.y + platform.Height &&
                playerPosition.Y >= platform.y && verticalVelocity < 0)
            {
                playerPosition.Y = platform.y + platform.Height;        // Stop player at the bottom of the platform
                verticalVelocity = 0f;                                  // Stop upward movement
            }
            // Side collisions (prevent moving through sides)
            if (isOverlappingHorizontally && playerPosition.Y + playerHeight > platform.y && playerPosition.Y < platform.y + platform.Height)
            {
                // Left side collision
                if (playerPosition.X + playerWidth >= platform.x && playerPosition.X < platform.x)
                {
                    playerPosition.X = platform.x - playerWidth;        // Prevent moving right through platform
                }
                // Right side collision
                else if (playerPosition.X <= platform.x + platform.Width && playerPosition.X + playerWidth > platform.x + platform.Width)
                {
                    playerPosition.X = platform.x + platform.Width;     // Prevent moving left through platform
                }
            }
            // Handle floor-specific logic (now integrated in platform logic)
            // Check if player has fallen past the platform's position (floor logic)
            if (playerPosition.Y > fallLimit)
            {
                playerPosition = lastSafePosition;  // Respawn at the last safe position
                verticalVelocity = 0f;              // Reset vertical velocity
                jumpCount = 0;                      // Reset jump count
                currentHP -= 10;                    // Reduce HP as penalty for falling
                if (currentHP <= 0)
                {
                    //gameover.TriggerGameOver();     // Trigger game over if HP reaches 0
                }
            }
        }

        /*Collision detection for floor*/
        public void FloorCollision(Floor floor, GameOverScreen gameover, AllEnemies enemies)
        {
            // Check if the player is positioned horizontally over the floor
            bool isOverlappingHorizontally = playerPosition.X + playerWidth > floor.x && playerPosition.X < floor.x + floor.Width;
            bool isOverlappingVertically = playerPosition.Y + playerHeight < floor.y + floor.Height && playerPosition.Y < floor.y - 35;

            // Check if the player has landed on the ground
            if (playerPosition.Y >= floor.y - playerHeight && playerPosition.X + playerWidth > floor.x &&
                playerPosition.X < floor.x + floor.Width && !(playerPosition.Y >= floor.y - playerHeight + 20))               // Ground level is List<Rectangle> floors
            {
                playerPosition.Y = floor.y - playerHeight;              // Set position to the ground
                verticalVelocity = 0f;                                  // Reset vertical velocity
                jumpCount = 0;                                          // Reset the jump count when landing
                isOnPlatform = true;                                    // Mark as on ground
                lastSafePosition = playerPosition;                      // Update the last position before jumping

                PlungeDamageLogic(enemies);
                isPlunging = false;                                     // Resetting Plunge-state
            }
            /*Falls, if not on the floor (new code after implementing the camera)*/
            if (playerPosition.Y > fallLimit)
            {
                playerPosition = lastSafePosition;                          // Respawn at the last position before jumping
                verticalVelocity = 0f;                                      // Reset vertical velocity
                jumpCount = 0;
                currentHP -= 10;                                            // Reduce HP by 10 as a penalty for falling
                if (currentHP <= 0)
                { gameover.TriggerGameOver(); }                                      // If HP reaches 0, trigger game over 
            }
            // Side collisions (prevent moving through sides)
            if (isOverlappingHorizontally && playerPosition.Y + playerHeight > floor.y && playerPosition.Y < floor.y + floor.Height)
            {
                // Left side collision
                if (playerPosition.X + playerWidth >= floor.x && playerPosition.X < floor.x)
                {
                    playerPosition.X = floor.x - playerWidth;        // Prevent moving right through platform
                }
                // Right side collision
                else if (playerPosition.X <= floor.x + floor.Width && playerPosition.X + playerWidth > floor.x + floor.Width)
                {
                    playerPosition.X = floor.x + floor.Width;     // Prevent moving left through platform
                }
            }
            //checks if player tries to jump through the floor from below
            if (playerPosition.X + playerWidth > floor.x + 10 && playerPosition.X < floor.x + floor.Width - 10
               && playerPosition.Y <= floor.y + floor.Height
               && playerPosition.Y >= floor.y && verticalVelocity < 0)
            {
                playerPosition.Y = floor.y + floor.Height;        // Stop player at the bottom of the floor
                verticalVelocity = 0f;                                  // Stop upward movement
            }

        }

        /* Collision detection for destroyable_Floors*/
        // Check for landing on floor
        public void DFloorCollision(DFloor floor, List<DFloor> dfloors)
        {
            // Check if the player is positioned horizontally over the destroyable_Floor (he is between its left and right border)
            bool isOver = playerPosition.X + playerWidth > floor.x && playerPosition.X < floor.x + floor.Width;

            // Check if the player is landing on top of the destroyable_Floor while falling down
            if (isOver && playerPosition.Y + playerHeight >= floor.y &&
                playerPosition.Y + playerHeight <= floor.y + floor.Height && verticalVelocity > 0)
            {
                if (!isPlunging)
                {
                    playerPosition.Y = floor.y - playerHeight;                  // Land on top of the floor
                    verticalVelocity = 0f;                                      // Reset vertical velocity
                    jumpCount = 0;                                              // Reset jump count when landing
                    isOnPlatform = true;                                        // Mark as on platform
                }
                else
                {
                    dfloors.Remove(floor);                                      // Remove floor on Plunging
                    isPlunging = false;                                         // Resetting plunging-state
                    verticalVelocity = 0;                                       // Resetting Falling Speed
                }
            }
            // Prevent player from passing through the floor from below
            if (isOver && playerPosition.Y <= floor.y + floor.Height &&
                playerPosition.Y >= floor.y && verticalVelocity < 0)
            {
                playerPosition.Y = floor.y + floor.Height;                  // Stop player at the bottom of the floor
                verticalVelocity = 0f;                                      // Stop upward movement
            }

            // Side collisions (prevent moving through sides)
            if (isOver && playerPosition.Y + playerHeight > floor.y && playerPosition.Y < floor.y + floor.Height)
            {
                // Left side collision
                if (playerPosition.X + playerWidth >= floor.x && playerPosition.X < floor.x)
                {
                    playerPosition.X = floor.x - playerWidth;         // Prevent moving right through floor
                }
                // Right side collision
                else if (playerPosition.X <= floor.x + floor.Width && playerPosition.X + playerWidth > floor.x + floor.Width)
                {
                    playerPosition.X = floor.x + floor.Width;         // Prevent moving left through floor
                }
            }
        }
        /* Collision detection for destroyable_Walls*/
        // Check for touching wall
        public void WallCollision(Wall wall, List<Wall> walls)
        {
            // Check if the player is positioned horizontally over the destroyable_Wall(he is between its left and right border)
            bool isOver = playerPosition.X + playerWidth > wall.x && playerPosition.X < wall.x + wall.Width;

            // Side collisions (prevent moving through sides)
            if (isOver && playerPosition.Y + playerHeight > wall.y && playerPosition.Y < wall.y + wall.Height)
            {
                if (isDashing && CheckCollisionRecs(new Rectangle(playerPosition.X - 5, playerPosition.Y - 10, playerWidth + 10, playerHeight + 20), new Rectangle(wall.x, wall.y, wall.Width, wall.Height)))
                {
                    walls.Remove(wall);                                        //remove wall on dashing
                    isDashing = false;                                         // Resetting dashing-state
                    dashtimer = 0;
                }
                // Left side collision
                if (playerPosition.X + playerWidth >= wall.x && playerPosition.X < wall.x)
                {
                    playerPosition.X = wall.x - playerWidth;        // Prevent moving right through wall
                }
                // Right side collision
                else if (playerPosition.X <= wall.x + wall.Width && playerPosition.X + playerWidth > wall.x + wall.Width)
                {
                    playerPosition.X = wall.x + wall.Width;         // Prevent moving left through wall
                }
            }
        }
        /* Obstacles */
        // Check for collision with grass obstacles
        public void GrasObstacleCollision(GrassObstacle grassObstacle, GameOverScreen gameover)
        {
            // Check if the player is within the X and Y range of the obstacle
            bool isCollidingHorizontally = playerPosition.X + playerWidth > grassObstacle.x && playerPosition.X < grassObstacle.x + grassObstacle.Width;
            bool isCollidingVertically = playerPosition.Y + playerHeight >= grassObstacle.y && playerPosition.Y + playerHeight <= grassObstacle.y + grassObstacle.Height;

            // Only check for damage if the player is in the correct vertical range (stepping on it)
            if (isCollidingHorizontally && isCollidingVertically)
            {
                // Ensure damage happens only once until player leaves the obstacle
                if (grassTimer <= 0)
                {
                    currentHP -= 10;    // Reduce HP by 10 if the player touches the grass obstacle
                    if (currentHP <= 0)
                    {
                        currentHP = 0;
                        gameover.TriggerGameOver();     // Set the game to "Game Over" when HP reaches 0
                    }
                    grassTimer = 100;
                }
                else
                {
                    speedbonus = 1.5; // Giving speedbonus
                    countspeedframes = 0; // For 5 seconds (5-0)
                }
            }

            grassTimer -= 1;

        }



        /*public void GrasObstacleCollision(GrassObstacle grassObstacle, GameOverScreen gameover)
          {
              bool isCollidingWithGrass = false;                              // Check if the player is colliding with any grass obstacle
              if (CheckCollisionRecs(new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight), new Rectangle(grassObstacle.x, grassObstacle.y, grassObstacle.Width, grassObstacle.Height)))
              {
                  isCollidingWithGrass = true;                            // Check if the player hasn't taken damage yet
                  if (grassCount == 0)
                  {
                      currentHP -= 10;                                    // Reduce HP by 10 if the player touches the grass obstacle
                      if (currentHP <= 0)
                      {
                          currentHP = 0;
                          gameover.TriggerGameOver();                              // Set the game to "Game Over" when HP reaches 0
                      }
                      grassCount++;                                       // Increment count to indicate damage has been taken
                  }
                  else
                  {
                      speedbonus = 1.5;                   // Giving speedbonus
                      countspeedframes = 0;               // For 5 seconds (5-0)
                  }
                  //break;                                                  // Exit the loop to prevent further checks once a collision is detected
              }
              // Reset count if the player is not colliding with any grass obstacle
              if (!isCollidingWithGrass)
              { grassCount = 0; }

          } */



        /*Moving Platform Collision Check*/
        public void MovingPlatformCollision(MovingPlatform platform, AllEnemies enemies)
        {
            // Check if overlapping with the platform horizontally
            // Check if the player is positioned horizontally over the platform
            bool isOverlappingHorizontally = playerPosition.X + playerWidth > platform.GetX() && playerPosition.X < platform.GetX() + platform.GetWidth();

            // Check if the player is landing on top of the platform while falling down
            if (isOverlappingHorizontally && playerPosition.Y + playerHeight >= platform.GetY() &&
                playerPosition.Y + playerHeight <= platform.GetY() + platform.GetHeight() && verticalVelocity > 0)
            {
                playerPosition.Y = platform.GetY() - playerHeight;
                playerPosition.X += platform.GetSpeedX(); // player moves horizontally with the platform
                //makes sure the players moves correctly up and down with the platform
                if (platform.GetDirectionY() >= 0)
                {
                    playerPosition.Y += platform.GetSpeedY();
                    //prevents a bug: player won't get thrown of the platform if player is at the edge (jumping on the platform while it goes down won't be smooth anymore)
                    /*if (playerPosition.X < platform.GetX())
                    {
                        playerPosition.X = platform.GetX(); // player stays at the left edge
                    }
                    if (playerPosition.X + playerWidth > platform.GetX() + platform.GetWidth())
                    {
                        playerPosition.X = platform.GetX() + platform.GetWidth() - playerWidth; // player stays at the right edge
                    }*/
                }

                verticalVelocity = 0f;                                  // Reset vertical velocity
                jumpCount = 0;                                          // Reset jump count when landing
                isOnPlatform = true;                                    // Mark as on platform
                lastSafePosition = playerPosition;                      // Update the last position before jumping

                PlungeDamageLogic(enemies);
                isPlunging = false;                                     // Resetting Plunging-state
            }
            else
            {
                isOnPlatform = false; // Spieler ist nicht auf der Plattform
            }
            //checks if player tries to jump through the platform from below
            if (isOverlappingHorizontally && playerPosition.Y <= platform.GetY() + platform.GetHeight() &&
                        playerPosition.Y >= platform.GetY() && verticalVelocity < 0)
            {
                playerPosition.Y = platform.GetY() + platform.GetHeight();        // Stop player at the bottom of the platform
                verticalVelocity = 0f;                                  // Stop upward movement
            }
            // Side collisions (prevent moving through sides)
            if (isOverlappingHorizontally && playerPosition.Y + playerHeight > platform.GetY() && playerPosition.Y < platform.GetY() + platform.GetHeight() && !isOnPlatform)
            {
                // Left side collision
                if (playerPosition.X + playerWidth >= platform.GetX() && playerPosition.X < platform.GetX())
                {
                    playerPosition.X = platform.GetX() - playerWidth;        // Prevent moving right through platform
                }
                // Right side collision
                else if (playerPosition.X <= platform.GetX() + platform.GetWidth() && playerPosition.X + playerWidth > platform.GetX() + platform.GetWidth() && !isOnPlatform)
                {
                    playerPosition.X = platform.GetX() + platform.GetWidth();     // Prevent moving left through platform
                }
            }
        }
        public void JumpPadCollision(JumpPad pad)
        {
            if (!pad.used)
            {
                // Check if player touches the JumpPad, if so, gives a Jumpboost for a ~0,5sec and disables the Platform for ~3sec
                if (CheckCollisionRecs(new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight), new Rectangle(pad.x, pad.y, pad.width, pad.height)))
                {
                    pad.used = true;
                    jumpBoost = 1.8;
                    pad.usedTimer = 180;
                    jumpBoostDuration = 30;
                }
            }
            else
            {
                pad.usedTimer -= 1;         // Each frame reduces the timer
                if(pad.usedTimer <= 0)      // If timer is down, resets the JumpPad
                {
                    pad.used = false;
                    pad.usedTimer = 0;
                }
            }
        }
        public void PlungeDamageLogic(AllEnemies enemies)
        {
            // Upon landing, if it plunging state over 7/fps seconds, deal damage to all enemies in range
            if (isPlunging == true && plungeDuration > 7)
            {
                foreach (var enemy in enemies.testEnemies)
                {
                    if (enemy.CheckCollision(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight + 60)))
                    {
                        enemy.DealDamageToEnemy(25);
                    }
                }
                foreach (var enemy in enemies.burnoutEnemies)
                {
                    if (enemy.CheckCollision(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight + 60)))
                    {
                        enemy.DealDamageToEnemy(25);
                    }
                }
                foreach (var enemy in enemies.procrastinationEnemies)
                {
                    if (enemy.CheckCollision(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight + 60)))
                    {
                        enemy.DealDamageToEnemy(25);
                    }
                }
                plungingPillar.Add(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight));    // Plunging Trace
                plungingtimer = 0;                      // Duration of Plunging Trace
            }
        }

        public void PlungeDamageLogic1(List<Enemy_Burnout> burnouts)
        {
            // Upon landing, if it plunging state over 7/fps seconds, deal damage to all enemies in range
            if (isPlunging == true && plungeDuration > 7)
            {
                foreach (var burnout in burnouts)
                {
                    if (burnout.CheckCollision(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight + 60)))
                    {
                        burnout.DealDamageToEnemy(20);
                    }
                }
                plungingPillar.Add(new Rectangle(playerPosition.X - 75, playerPosition.Y, playerWidth + 160, playerHeight));    // Plunging Trace
                plungingtimer = 0;                      // Duration of Plunging Trace
            }
        }

        //collects the HealItem once player comes in contact (it dissapears)
        public void CheckCollision(HealItem hitem)
        {
            if (!hitem.collected)
            {
                // check if player makes contact with the HealItem
                if (CheckCollisionCircleRec(new Vector2(hitem.x, hitem.y), hitem.radius, new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight)))
                {
                    float healAmount = maxHP * 0.2f;
                    AppleHeal(healAmount);
                    hitem.collected = true;
                }
            }
        }
        public void PortalCollision(Portal portal)
        {
            //if (!portal.entered)
            {
                // check if player makes contact with the HealItem
                if (CheckCollisionRecs(new Rectangle(portal.x, portal.y, 96, 96), new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight)))
                {
                    Program.UpdateLevel();
                }
            }
        }
    }
}
