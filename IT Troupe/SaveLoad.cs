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
    public class SaveLoad
    {
        public class GameStateSnapshot
        {
            public string PlayerPosition { get; set; }

            public string CameraOffset { get; set; }
            public string CameraTarget { get; set; }
            public float CameraRotation { get; set; }
            public float CameraZoom { get; set; }

            //public List<Player_Attack> PlayerAttacks { get; set; } = new List<Player_Attack>();
            //public List<Player_AttackM2> PlayerAttacksM2 { get; set; } = new List<Player_AttackM2>();

            //public List<BasicAttackFear> BasicAttackFears { get; set; } = new List<BasicAttackFear>();
            public List<Enemy_Burnout> Enemy_Burnout { get; set; } = new List<Enemy_Burnout>();
            public List<Test_Enemy> Test_Enemy { get; set; } = new List<Test_Enemy>();
            public List<Enemy_Procrastination> Enemy_Procrastination { get; set; } = new List<Enemy_Procrastination>();


            public List<DFloor> DFloors { get; set; } = new List<DFloor>();
            public List<Floor> Floors { get; set; } = new List<Floor>();
            public List<GrassObstacle> GrassObstacles { get; set; } = new List<GrassObstacle>();
            public List<HealItem> HealItems { get; set; } = new List<HealItem>();
            public List<JumpPad> JumpPads { get; set; } = new List<JumpPad>();
            public List<Ladder> Ladders { get; set; } = new List<Ladder>();
            public List<MovingPlatform> MovingPlatforms { get; set; } = new List<MovingPlatform>();
            public List<Platforms> Platforms { get; set; } = new List<Platforms>();
            public List<Wall> Walls { get; set; } = new List<Wall>();

            public Sounds Sounds { get; set; } = new Sounds();

            public MainMenu MainMenu { get; set; } = new MainMenu();

            public Colors Colors { get; set; } = new Colors();

            public Player Player { get; set; } = new Player(); 
        }


        public static void SaveGame(
                                    string fileName,

                                    //List<Player_Attack> shoots,
                                    //List<Player_AttackM2> missiles,

                                    AllEnemies enemys,

                                    AllObstacles obsts,

                                    Sounds sounds,

                                    MainMenu mainMenu,

                                    Colors colors,

                                    Player player
        )
        {
            try
            {
                // Set culture to ensure consistent formatting
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                // Serialize the entire GameStateSnapshot object to JSON
                var jsonString = JsonSerializer.Serialize(new
                {
                    PlayerPosition = $"{player.playerPosition.X},{player.playerPosition.Y}",

                    CameraOffset = player.camera.Offset,
                    CameraTarget = player.camera.Target,
                    CameraRotation = player.camera.Rotation,
                    CameraZoom = player.camera.Zoom,

                    //PlayerAttacks = shoots,
                    //PlayerAttacksM2 = missiles,

                    Enemy_Burnout = enemys.burnoutEnemies,
                    Test_Enemy = enemys.testEnemies,
                    Enemy_Procrastination = enemys.procrastinationEnemies,

                    DFloors = obsts.dfloors,
                    Floors = obsts.floors,
                    GrassObstacles = obsts.grassObstacles,
                    HealItems = obsts.healitems,
                    JumpPads = obsts.jumppads,
                    Ladders = obsts.ladders,
                    MovingPlatforms = obsts.movingPlatforms,
                    Platforms = obsts.platforms,
                    Walls = obsts.walls,

                    Sounds = sounds,

                    MainMenu = mainMenu,

                    Colors = colors,

                    Player = player

                }, new JsonSerializerOptions
                {
                    WriteIndented = true, // Enable indentation and line breaks
                    Converters = { new Vector2Converter(), new RectangleConverter(), new ColorConverter(), new RectangleListConverter() }, // Add necessary converters for other types
                    PropertyNameCaseInsensitive = true // Ensure case-insensitive matching
                });;

                // Optionally print out the serialized JSON for debugging
                Console.WriteLine($"GameStateSnapshot before saving: {jsonString}");

                // Define the file path and save the JSON string to the file
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                Console.WriteLine($"Saving to {filePath}");
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }





        public static void LoadGame(
                                   string fileName,

                                    //ref List<Player_Attack> shoots,
                                    //ref List<Player_AttackM2> missiles,

                                    ref AllEnemies enemys,

                                    ref AllObstacles obsts,

                                    ref Sounds sounds,

                                    ref MainMenu mainMenu,

                                    ref Colors colors,

                                    ref Player player
        )
        {
            try
            {
                // Define the file path and read the JSON string from the file
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                string jsonString = File.ReadAllText(filePath);


                // Print the entire JSON content to the terminal
                Console.WriteLine("Loaded JSON content:");
                Console.WriteLine(jsonString);  // Print the JSON string to the console


                // Deserialize the JSON string into an anonymous object with converters for Vector2 and other custom types
                var gameState = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString, new JsonSerializerOptions
                {
                    WriteIndented = true, // Enable indentation
                    Converters = { new Vector2Converter(), new RectangleConverter(), new ColorConverter(), new RectangleListConverter() }, // Add custom converters
                    PropertyNameCaseInsensitive = true // Ensure case-insensitive matching
                });


                // Deserialize other properties
                //shoots = JsonSerializer.Deserialize<List<Player_Attack>>(gameState["PlayerAttacks"]);
                //missiles = JsonSerializer.Deserialize<List<Player_AttackM2>>(gameState["PlayerAttacksM2"]);

                enemys.burnoutEnemies = JsonSerializer.Deserialize<List<Enemy_Burnout>>(gameState["Enemy_Burnout"]);
                enemys.testEnemies = JsonSerializer.Deserialize<List<Test_Enemy>>(gameState["Test_Enemy"]);
                enemys.procrastinationEnemies = JsonSerializer.Deserialize<List<Enemy_Procrastination>>(gameState["Enemy_Procrastination"]);

                obsts.dfloors = JsonSerializer.Deserialize<List<DFloor>>(gameState["DFloors"]);
                obsts.floors = JsonSerializer.Deserialize<List<Floor>>(gameState["Floors"]);
                obsts.grassObstacles = JsonSerializer.Deserialize<List<GrassObstacle>>(gameState["GrassObstacles"]);
                obsts.healitems = JsonSerializer.Deserialize < List<HealItem>>(gameState["HealItems"]);
                obsts.ladders = JsonSerializer.Deserialize<List<Ladder>>(gameState["Ladders"]);
                obsts.movingPlatforms = JsonSerializer.Deserialize<List<MovingPlatform>>(gameState["MovingPlatforms"]);
                obsts.platforms = JsonSerializer.Deserialize<List<Platforms>>(gameState["Platforms"]);
                obsts.walls = JsonSerializer.Deserialize<List<Wall>>(gameState["Walls"]);

                sounds = JsonSerializer.Deserialize<Sounds>(gameState["Sounds"]);

                mainMenu = JsonSerializer.Deserialize<MainMenu>(gameState["MainMenu"].ToString());

                colors = JsonSerializer.Deserialize<Colors>(gameState["Colors"]);

                player = JsonSerializer.Deserialize<Player>(gameState["Player"]);


                // Restore the player position
                var playerPosition = gameState["PlayerPosition"].GetString().Split(',');
                player.playerPosition.X = float.Parse(playerPosition[0]);
                player.playerPosition.Y = float.Parse(playerPosition[1]);

                // Parse and restore the camera values using ParseVector2
                player.camera.Offset = ParseVector2(gameState["CameraOffset"].GetString());
                player.camera.Target = ParseVector2(gameState["CameraTarget"].GetString());
                player.camera.Rotation = gameState["CameraRotation"].GetSingle();
                player.camera.Zoom = gameState["CameraZoom"].GetSingle();

                Console.WriteLine($"Game loaded from {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }

        }

        // Helper function to parse Vector2 from a string (e.g., "500,400")
        private static Vector2 ParseVector2(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Input string is null or empty.", nameof(str));
            }

            var parts = str.Split(',');

            if (parts.Length != 2 ||
                !float.TryParse(parts[0], out float x) ||
                !float.TryParse(parts[1], out float y))
            {
                throw new FormatException($"Invalid Vector2 string format: {str}");
            }
            return new Vector2(x, y);
        }

        // Helper method to parse the PlayerColor string and return a Color object
        public static Color ParseColor(string colorString)
        {
            // Trim curly braces and split by commas
            var colorParts = colorString.Trim(new char[] { '{', '}' }).Split(',');

            // Parse each color component (R, G, B, A)
            byte r = byte.Parse(colorParts[0].Split(':')[1].Trim());
            byte g = byte.Parse(colorParts[1].Split(':')[1].Trim());
            byte b = byte.Parse(colorParts[2].Split(':')[1].Trim());
            byte a = byte.Parse(colorParts[3].Split(':')[1].Trim());

            // Return the Color object
            return new Color(r, g, b, a);
        }

    }


     
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Get the string value, e.g. "503.87695,540"
            string position = reader.GetString();

            // Split the string by comma
            var parts = position.Split(',');

            // Parse the X and Y components
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);

            return new Vector2(x, y);
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            // Write the Vector2 as a string, e.g. "503.87695,540"
            string position = $"{value.X},{value.Y}";
            writer.WriteStringValue(position);
        }
    }


    public class RectangleConverter : JsonConverter<Rectangle>
    {
        public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the string value, e.g. "0,0,100,200"
            string rectString = reader.GetString();

            // Split the string by commas
            var parts = rectString.Split(',');

            // Parse X, Y, Width, and Height
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);
            float width = float.Parse(parts[2]);
            float height = float.Parse(parts[3]);

            return new Rectangle(x, y, width, height);
        }

        public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
        {
            // Convert the Rectangle to a string, e.g. "0,0,100,200"
            string rectString = $"{value.X},{value.Y},{value.Width},{value.Height}";
            writer.WriteStringValue(rectString);
        }
    }


    public class ColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the string, e.g. "0,105,75,255"
            string colorString = reader.GetString();

            // Split the string by commas
            var parts = colorString.Split(',');

            if (parts.Length == 4)
            {
                // Parse the RGBA components
                int r = int.Parse(parts[0]);
                int g = int.Parse(parts[1]);
                int b = int.Parse(parts[2]);
                int a = int.Parse(parts[3]);

                return new Color(r, g, b, a);
            }

            throw new JsonException("Invalid color format. Expected 'R,G,B,A'.");
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            // Convert the Color to a string in "R,G,B,A" format
            string colorString = $"{value.R},{value.G},{value.B},{value.A}";
            writer.WriteStringValue(colorString);
        }
    }


    public class RectangleListConverter : JsonConverter<List<Rectangle>>
    {
        public override List<Rectangle> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize the list of rectangles (as strings)
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var rectangles = new List<Rectangle>();

                foreach (var item in root.EnumerateArray())
                {
                    string rectString = item.GetString();
                    var parts = rectString.Split(',');

                    // Parse X, Y, Width, and Height
                    float x = float.Parse(parts[0]);
                    float y = float.Parse(parts[1]);
                    float width = float.Parse(parts[2]);
                    float height = float.Parse(parts[3]);

                    rectangles.Add(new Rectangle(x, y, width, height));
                }

                return rectangles;
            }
        }

        public override void Write(Utf8JsonWriter writer, List<Rectangle> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var rectangle in value)
            {
                // Write the Rectangle as a string
                string rectString = $"{rectangle.X},{rectangle.Y},{rectangle.Width},{rectangle.Height}";
                writer.WriteStringValue(rectString);
            }
            writer.WriteEndArray();
        }
    }
}

