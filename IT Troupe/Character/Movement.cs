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
        public void Move()
        {
            if (IsKeyDown(KeyboardKey.A))
            {
                playerPosition.X -= playerSpeed * (float)speedbonus * GetFrameTime();               // Move left based on speed
                playerDirection = -1;
            }

            if (IsKeyDown(KeyboardKey.D))
            {
                playerPosition.X += playerSpeed * (float)speedbonus * GetFrameTime();               // Move right based on speed
                playerDirection = 1;
            }
        }
        public void Jump()
        {
            // Jump Handling (double jump logic)
            if (IsKeyPressed(KeyboardKey.Space) && jumpCount < maxJumps)        // Allow jump if jump count is less than maxJumps
            {
                verticalVelocity = jumpStrength * (float)(1 + Math.Log(speedbonus, 11)) * (float)jumpBoost;     // Set vertical velocity to jump strength, depends on speedbonus logarithmically and jumpbonus
                jumpCount++;                                                                                    // Increment jump count for each jump
                jumpBoost = 1;                                                                                  // Reset Boost
                jumpBoostDuration = 0;                                                                          // Reset Boost Duration
            }
        }
        public void PlungeAttack()
        {
            // Check for plunge (S + Space key pressed together)
            // Make sure this works regardless of the current jump state
            if (IsKeyDown(KeyboardKey.S) && IsKeyPressed(KeyboardKey.Space))    // Plunge down if both keys are pressed
            {
                verticalVelocity = gravity * 1.5f;  // Apply a strong downward force for plunging
                isPlunging = true;
            }
            if(isPlunging)
            {
                plungeDuration += 1;
            }
            else
            {
                plungeDuration = 0;
            }
        }

        /*Dash*/
        public void Dash()
        {
            // Left-Dash logic
            if (IsKeyDown(KeyboardKey.A) && IsKeyPressed(KeyboardKey.LeftShift) && dashcooldown <= 0 && character == 2)    //Checking, if player was moving left
            {
                dashtimer = 6;                  // Set a Dash-Duration
                isDashingLeft = true;
                dashcooldown = 1f;              // Set a Cooldown
                isDashing = true;
            }
            // Left-Dash action
            if (dashtimer >= 1 && isDashingLeft)    // Makes a smooth left-motion
            {
                playerPosition.X -= 15;
                dashtimer--;                        // Reduces Dash-time left
            }
            // Right-Dash logic
            if (IsKeyDown(KeyboardKey.D) && IsKeyPressed(KeyboardKey.LeftShift) && dashcooldown <= 0 && character == 2)    //Checking, if player was moving right
            {
                dashtimer = 6;                  // Set a Dash-Duration
                isDashingRight = true;
                dashcooldown = 1f;              // Set a Cooldown              
                isDashing = true;               // Making possible to break walls
            }
            // Right-Dash action
            if (dashtimer >= 1 && isDashingRight)   // Makes a smooth right-motion
            {
                playerPosition.X += 15;
                dashtimer--;                        // Reduces Dash-time left
            }
            // Resetting Dash-Variables
            if (dashtimer == 0)
            {
                isDashingLeft = isDashingRight = false;
                isDashing = false;
            }
            dashcooldown -= GetFrameTime();         // Reduces Dash-Cooldown
        }

        /*Backflip*/
        // Looking right, flip left
        public void Backflip()
        {
            if (IsKeyPressed(KeyboardKey.LeftShift) && playerDirection == 1 && flipcooldown <= 0 && character == 3)
            {
                fliptimer = 11;                 // Set a Flip-Duration
                flipcooldown = 1f;              // Set a Cooldown 
                isFlippingRight = true;         // Starts Flip-Action
            }
            if (fliptimer >= 1 && isFlippingRight)   // Makes a smooth right-up-motion with rotation
            {
                playerPosition.X -= 13;                                         // Left Movement
                playerPosition.Y -= (float)1.6 * (7 + fliptimer - 10) - 2;      // Some Formula for smooth flip
                fliptimer--;                                                    // Reduces Flipping-time
                playerRotationAngle -= 20;                                      // Rotation anti-clock-wise
            }
            // Looking left, flip right
            if (IsKeyPressed(KeyboardKey.LeftShift) && playerDirection == -1 && flipcooldown <= 0 && character == 3)
            {
                fliptimer = 10;                 // Set a Flip-Duration
                flipcooldown = 1f;              // Set a Cooldown 
                isFlippingLeft = true;          // Starts Flip-Action
            }
            if (fliptimer >= 1 && isFlippingLeft)   // Makes a smooth right-up-motion with rotation
            {
                playerPosition.X += 13;                                         // Right Movement
                playerPosition.Y -= (float)1.6 * (7 + fliptimer - 10) - 2;      // Some Formula for smooth flip
                fliptimer--;                                                    // Reduces Flipping-time    
                playerRotationAngle += 20;                                      // Rotation clock-wise
            }
            // Resetting Flip-Variables
            if (fliptimer == 0)                     // Resetting Flip variables
            {
                isFlippingLeft = isFlippingRight = false;
                playerRotationAngle = 0;
            }
            flipcooldown -= GetFrameTime();         // Reduces Flip-Cooldown
        }

        /*Short_Teleport*/
        public void Teleport(List<Platforms> platforms, List<Floor> floors, Camera2D camera)
        {
            // Resetting variables
            pXl = pXr = 0;
            fXl = fXr = 0;
            isCollidingWithPlatformleft = isCollidingWithPlatformright = false;
            isCollidingWithFloorleft = isCollidingWithFloorright = false;
            // Check potential collision for all platforms
            foreach (Platforms platform in platforms)
            {
                if (CheckCollisionRecs(new Rectangle(playerPosition.X - teleportrange, playerPosition.Y, playerWidth, playerHeight), new Rectangle(platform.x, platform.y, platform.Width, platform.Height)))
                {
                    isCollidingWithPlatformleft = true;
                    pXl = platform.x;                               // Saves X location of platforms left side
                    break; // Stop checking another platforms
                }
                if (CheckCollisionRecs(new Rectangle(playerPosition.X + teleportrange, playerPosition.Y, playerWidth, playerHeight), new Rectangle(platform.x, platform.y, platform.Width, platform.Height)))
                {
                    isCollidingWithPlatformright = true;
                    pXr = platform.x + platform.Width;              // Saves X location of platform right side
                    break; // Stop checking another platforms
                }
            }
            foreach (Floor floor in floors)
            {
                if (CheckCollisionRecs(new Rectangle(playerPosition.X - teleportrange, playerPosition.Y, playerWidth, playerHeight - 10), new Rectangle(floor.x, floor.y + 10, floor.Width, floor.Height - 20)))
                {
                    isCollidingWithFloorleft = true;
                    fXl = floor.x + floor.Width + 1;
                    break; // Stop checking another Floor
                }
                if (CheckCollisionRecs(new Rectangle(playerPosition.X + teleportrange, playerPosition.Y, playerWidth, playerHeight - 10), new Rectangle(floor.x, floor.y + 10, floor.Width, floor.Height - 20)))
                {
                    isCollidingWithFloorright = true;
                    fXr = floor.x - playerWidth - 1;
                    break; // Stop checking another Floor
                }
            }

            // Teleport in the pressed direction, 3 ways of action: 
            if (IsKeyPressed(KeyboardKey.LeftShift) && IsKeyDown(KeyboardKey.A) && character == 1 && tpcooldown <= 0)
            {
                if (!isCollidingWithFloorleft)
                {
                    if (!isCollidingWithPlatformleft)
                        playerPosition.X -= teleportrange;           // Short teleport
                    else
                        playerPosition.X = pXl;                      // Teleport behind the object, that stands on the way (left)

                    Program.UpdateCam();
                    tpcooldown = 1f;                                 // Making Cooldown
                }
                else
                {
                    playerPosition.X = fXl;                          // Teleports before object, if its floor
                    Program.UpdateCam();
                    tpcooldown = 1f;                                 // Making Cooldown
                }

            }
            if (IsKeyPressed(KeyboardKey.LeftShift) && IsKeyDown(KeyboardKey.D) && character == 1 && tpcooldown <= 0)
            {
                if (!isCollidingWithFloorright)
                {
                    if (!isCollidingWithPlatformright)
                        playerPosition.X += teleportrange;           // Short teleport
                    else
                        playerPosition.X = pXr;                      // Teleport behind the object, that stands on the way (right)

                    Program.UpdateCam();
                    tpcooldown = 1f;                                 // Making Cooldown
                }
                else
                {
                    playerPosition.X = fXr;                          // Teleports before object, if its floor
                    Program.UpdateCam();
                    tpcooldown = 1f;                                 // Making Cooldown
                }
            }
            tpcooldown -= 0.01f;    // Debug
        }
        /*Climbing*/
        public void Climbing(Ladder ladder)
        {
            // Check if the player is climbing any ladder
            isClimbing = false;
            if (CheckCollisionRecs(new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight), new Rectangle(ladder.x, ladder.y, ladder.Width, ladder.Height)))
            {
                isClimbing = true;                                      // Start climbing if colliding with any ladder
                jumpCount = 1;
                verticalVelocity = 0;                                   // Reset vertical velocity while climbing
                isPlunging = false;                                     // Resetting Plunging-state

                // Climbing logic for current ladder
                if (IsKeyDown(KeyboardKey.W))                           // Move up while climbing
                {
                    playerPosition.Y -= playerSpeed * GetFrameTime();
                }
                if (IsKeyDown(KeyboardKey.S))                           // Move down while climbing
                {
                    playerPosition.Y += playerSpeed * GetFrameTime();
                }
                lastSafePosition = playerPosition;                      // Update the last position before jumping

            }// Reset climbing state
        }

        /*Gravity*/
        public void ApplyGravity()
        {
            // Reduces nonGravityTime each frame
            if (nonGravityTime > 0)
                nonGravityTime -= 1;

            isOnPlatform = false;                                           // Reset the platform state at the start of each frame
                                                                            // If the player is not standing on any platform or climbing and is not jumping, they fall
            if (!isOnPlatform && !isClimbing && !(nonGravityTime > 0))
            {
                // The player is mid-air, so apply gravity
                verticalVelocity += gravity * GetFrameTime();               // Apply gravity every frame
                playerPosition.Y += verticalVelocity * GetFrameTime();      // Update vertical position
            }
        }

    }
}
