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
        // Player variables
        public uint character { get; set; } = 1;                                                     // Variable representing character number(order)
        //public Vector2 playerPosition = new Vector2(250, 550);                                     // Starting Point, float
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 playerPosition;

        // Add the artist texture property
        public Texture2D playerTexture; //for switching character textures
        public Texture2D artist = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "artist.png"));
        public Texture2D rocker = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "rocker.png"));
        public Texture2D hacker = LoadTexture(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Character", "CharacterSprites", "hacker.png"));

        [JsonConverter(typeof(RectangleConverter))]
        Rectangle sourceRect { get; set; }

        // Player-Parameters, set to default values (equivanlent to character 1)
        [JsonConverter(typeof(ColorConverter))]
        public Color playerColor { get; set; } = new Color(0, 105, 75, 255);
        public int playerWidth { get; set; } = 30;
        public int playerHeight { get; set; } = 60;
        public float playerRotationAngle { get; set; } = 0;
   
        //Rectangle player => new Rectangle(playerPosition.X, playerPosition.Y, playerWidth, playerHeight);
        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle player { get; set; }
        public int playerDirection { get; set; } = 1;

        // Movement related variables
        public double speedbonus { get; set; } = 1;                                                  // Variable to scale player´s speed
        public float countspeedframes { get; set; } = 100;                                           // Variable to count the duration of speedbonus
        public float playerSpeed { get; set; } = 200f;                                               // Variable controls how fast the player moves horizontally, higher == faster
        public float gravity { get; set; } = 700f;                                                   // Variable simulates the downward force on the player
        public float verticalVelocity { get; set; } = 0f;                                            // Variable represents player's current vertical speed
        public int nonGravityTime { get; set; } = 0;                                                 // Variable represents time without gravity
        public float jumpStrength { get; set; } = -300f;                                             // Variable represents how strong player's jump is
        public int jumpCount { get; set; } = 0;                                                      // Variable to track jumps for double jump
        public int maxJumps { get; set; } = 2;                                                       // Maximum jumps allowed (double jump)
        public double jumpBoost { get; set; } = 1;                                                   // Variable for Jumpboost from JumpPad
        public float jumpBoostDuration { get; set; } = 0;                                            // Variable for Jumpboost from JumpPad                                              

        public bool isOnPlatform { get; set; } = false;                                              // Variable to track whether the player is standing on a platform
        public bool isClimbing { get; set; } = false;                                                // Variable to track whether the player is climbing the ladder
        public bool isPlunging { get; set; } = false;                                                // Variable to check, if the player is plunging
        public float plungeDuration { get; set; } = 0;                                               // Variable shows the duration of plunge action till now

        /*Movement Skills variables*/
        // Dash
        public int dashtimer { get; set; } = 0;                                                      // Variable sets the duration of Dash-action
        public bool isDashingLeft { get; set; } = false;                                             // Variable shows, if dashing left
        public bool isDashingRight { get; set; } = false;                                            // Variable shows, if dashing right
        public float dashcooldown { get; set; } = 0;                                                 // Variable needed for cooldown-logic
        public bool isDashing { get; set; } = false;                                                 // Variable to check, if in dashing state
        // BackFlip
        public bool isLookingRight { get; set; } = true;                                             // Variable shows looking direction: true = right, false = left
        public int fliptimer { get; set; } = 0;                                                      // Variable sets the duration of Backflip-action
        public bool isFlippingLeft { get; set; } = false;                                            // Variable shows, if flipping left
        public bool isFlippingRight { get; set; } = false;                                           // Variable shows, if flipping right
        public float flipcooldown { get; set; } = 0;                                                 // Variable needed for cooldown-logic
        // Short_Teleport related variables
        public bool isCollidingWithPlatformleft { get; set; } = false;                               // Shows, if there is a collision with platform at the place of teleportation left
        public float pXl { get; set; } = 0;                                                          // If there is a collision, saves the location of the platform, it colides with
        public bool isCollidingWithPlatformright { get; set; } = false;                              // Shows, if there is a collision with platform at the place of teleportation right
        public float pXr { get; set; } = 0;                                                          // If there is a collision, saves the location of the platform, it colides with
        public float teleportrange { get; set; } = 50f;                                              // Variale to define the distance of short_teleport
        public float tpcooldown { get; set; } = 0;                                                   // Variable needed for cooldown-logic
        public bool isCollidingWithFloorleft { get; set; } = false;                                  // Shows, if there is a collision with floor at the place of teleportation left
        public float fXl { get; set; } = 0;                                                          // If there is a collision, saves the location of the floor, it colides with
        public bool isCollidingWithFloorright { get; set; } = false;                                 // Shows, if there is a collision with floor at the place of teleportation right
        public float fXr { get; set; } = 0;                                                          // If there is a collision, saves the location of the floor, it colides with



        // Ability related variables
        public float energy { get; set; } = 50;                                                      // Variable for energy, default value dont matterin
        public int outOfEnergy { get; set; } = 0;                                                    // Variable shows the duration of "Out of Energy" text
        public double paintingtimer { get; set; } = 1000;                                            // Variable for counting the duration of ability
        public bool isSearching { get; set; } = false;                                               // Variable for searching a teleportable enemy
        public bool isVictim { get; set; } = false;                                                  // Variable true if an enemy to teleport is found
        public double plungingtimer { get; set; } = 1000;                                            // Variable for counting the duration of plunge path
        public double firing { get; set; } = 0;                                                      // Variable shows, character is burning

        // HP variables
        public int maxHP { get; set; } = 100;
        public int currentHP { get; set; }
        public int grassCount { get; set; } = 0;                                                     // Variable to ensure that the player only loses HP once each time they touch the grass obstacle
        public int grassTimer { get; set; } = 0;                                                     // Variable to undestand the time after taking dmg

        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 lastSafePosition { get; set; }                                                // Last location before jumping
        public float fallLimit { get; set; } = 4000f;                                                // Check if the player falls below Y = 1000, reset or trigger game over
        public float deltaTime { get; set; } = GetFrameTime();

        // Add Camera2D setup
        public Camera2D camera = new Camera2D();

        // Save and Load variables
        public bool isSaveLoadMenu { get; set; } = false;                                         // Variable to track if the Escape menu is open
        public bool isSettings { get; set; } = false;                                             // Variable to track settings state
        public int selectedSaveSlot { get; set; } = 0;                                            // To keep track of selected save slot
        public int selectedSettingsSlot { get; set; } = 0;                                        // To keep track of selected save slot
        public bool settingsFromMain { get; set; } = true;                                        // To check where from user uses Settings-Menu
        public bool saveLoadFromMain { get; set; } = true;                                        // To check where from user uses SaveLoad-Menu
        public int chosenResolution { get; set; } = 1;                                            // User defined resolution option
        public int chosenVolume { get; set; } = 4;                                                // Volume control option 
        public int chosenZoom { get; set; } = 2;                                                  // Zoom option option
        public int chosenFullScreen { get; set; } = 1;                                            // FullScreen activation
        public bool hotkeysShown { get; set; } = true;                                            // Hotkeys shown   
        public bool gameCompleted { get; set; } = false;                                          // Shows, if game is done 



        // Player is Protected
        public bool IsProtected { get; set; }

        public int level { get; set; } = 1;
    }
}
