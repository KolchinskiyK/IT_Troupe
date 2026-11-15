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
    public class AllObstacles
    {
        public AllObstacles()
        {

        }

        //[JsonConverter(typeof(RectangleListConverter))]
        //public int levelcounter = 1;

        // Player attack shooting(M1)
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Player_Attack> shoots { get; set; } = new List<Player_Attack>();

        // Player attack missile(M2)
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Player_AttackM2> missiles { get; set; } = new List<Player_AttackM2>();


        // List of destroyable_floor obstacles
        [JsonConverter(typeof(RectangleListConverter))]
        public List<DFloor> dfloors { get; set; } = new List<DFloor>();

        // List of floors
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Floor> floors { get; set; } = new List<Floor>();

        // List of grass obstacles/ water
        [JsonConverter(typeof(RectangleListConverter))]
        public List<GrassObstacle> grassObstacles { get; set; } = new List<GrassObstacle>();

        // List of JumpPads obstacles
        [JsonConverter(typeof(RectangleListConverter))]
        public List<JumpPad> jumppads { get; set; } = new List<JumpPad>();

        // List of ladders (as rectangles)
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Ladder> ladders { get; set; } = new List<Ladder>();

        [JsonConverter(typeof(RectangleListConverter))]
        public List<MovingPlatform> movingPlatforms { get; set; } = new List<MovingPlatform>();

        // List of platforms (as rectangles)
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Platforms> platforms { get; set; } = new List<Platforms>();

        // List of destroyable_wall obstacles
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Wall> walls { get; set; } = new List<Wall>();

        [JsonConverter(typeof(RectangleListConverter))]
        public List<HealItem> healitems { get; set; } = new List<HealItem>();

        [JsonConverter(typeof(RectangleListConverter))]
        public List<Portal> portals { get; set; } = new List<Portal>();


        public void ReloadObstacles(int levelcounter)
        {
            if (levelcounter == 1)        // Level 1
            {
                ClearObstacles();

                dfloors.AddRange(new List<DFloor>
                {
                    new DFloor(3615,644, 195, 28),
                });

                floors.AddRange(new List<Floor>
                {
                    new Floor(33, 1440, 95, 100),
                    new Floor(129, 1470, 96, 100),
                    new Floor(225, 1504, 95, 100),
                    new Floor(322, 1535, 95, 100),
                    new Floor(20, 1313, 15, 127),
                    new Floor(30, 1216, 930, 96),
                    new Floor(607, 1313, 34, 159),
                    new Floor(417, 1567, 574, 100),
                    new Floor(992, 1601, 257, 100),
                    new Floor(1251, 1567, 254, 100),
                    new Floor(1505, 1535, 96, 100),
                    new Floor(1602, 1505, 95, 100),
                    new Floor(1699, 1473, 103, 100),
                    new Floor(1794, 1441, 100, 100),
                    new Floor(1890, 1278, 20, 165),
                    new Floor(1697, 1344, 95, 28),
                    new Floor(1601, 1315, 31, 24),
                    new Floor(1409, 1280, 123, 29),
                    new Floor(1280, 1280, 30, 190),
                    new Floor(1731, 1248, 150, 28),
                    new Floor(1762, 1215, 63, 28),
                    new Floor(1828, 1183, 60, 28),
                    new Floor(1889, 703, 32, 483),
                    new Floor(1058, 1409, 61, 28),
                    new Floor(992, 1343, 30, 64),
                    new Floor(963, 1312, 30, 128),
                    new Floor(928, 640, 30, 830),
                    new Floor(1090, 1152, 60, 28),
                    new Floor(1121, 1121, 60, 28),
                    new Floor(1153, 1090, 60, 28),
                    new Floor(1218, 1027, 28, 28),
                    new Floor(1280, 991, 417, 28),
                    new Floor(1280, 991, 28, 193),
                    new Floor(1796, 991, 91, 28),
                    new Floor(1602, 928, 28, 28),
                    new Floor(1280, 832, 28, 60),
                    new Floor(1314, 865, 28, 28),
                    new Floor(1411, 899, 60, 28),
                    new Floor(1120, 801, 96, 28),
                    new Floor(962, 737, 28, 96),
                    new Floor(993, 770, 28, 60),
                    new Floor(1026, 800, 28, 28),
                    new Floor(1060, 671, 91, 28),
                    new Floor(1220, 641, 60, 28),
                    new Floor(1279, 480, 28, 254),
                    new Floor(928, 353, 28, 191),
                    new Floor(30, 640, 930, 100),
                    new Floor(609, 353, 28, 191),
                    new Floor(194, 576, 62, 28),
                    new Floor(30, 545, 98, 28),
                    new Floor(30, 482, 28, 28),
                    new Floor(98, 421, 94, 28),
                    new Floor(0, 30, 28, 610),
                    new Floor(0, 0, 1920, 28),
                    new Floor(332, 321, 734, 28),
                    new Floor(30, 321, 98, 28),
                    new Floor(1185, 321, 704, 28),
                    new Floor(1891, 31, 28, 98),
                    new Floor(1891, 261, 28, 170),
                    new Floor(610, 32, 28, 192),
                    new Floor(930, 32, 28, 192),
                    new Floor(1280, 32, 28, 192),
                    new Floor(3200, 225, 28, 1406),
                    new Floor(3200, 32, 28, 63),
                    new Floor(3200, 0, 1249, 28),
                    new Floor(3521, 32, 28, 192),
                    new Floor(3841, 32, 28, 192),
                    new Floor(3232, 321, 839, 28),
                    new Floor(4067, 289, 94, 28),
                    new Floor(4164, 258, 28, 28),
                    new Floor(4257, 289, 28, 28),
                    new Floor(4290, 226, 65, 125),
                    new Floor(4354, 193, 94, 28),
                    new Floor(4449, 32, 28, 481),
                    new Floor(4354, 513, 94, 28),
                    new Floor(4258, 545, 95, 28),
                    new Floor(4163, 579, 120, 28),
                    new Floor(4065, 610, 99, 28),
                    new Floor(3810, 642, 256, 28),
                    new Floor(3520, 352, 28, 191),
                    new Floor(3843, 353, 28, 191),
                    new Floor(3235, 640, 381, 28),
                    new Floor(3329, 515, 28, 28),
                    new Floor(3363, 483, 28, 90),
                    new Floor(3395, 449, 28, 94),
                    new Floor(3426, 419, 28, 128),
                    new Floor(3460, 386, 28, 124),
                    new Floor(1251, 671, 637, 28),
                    new Floor(3490, 342, 28, 170),
                    new Floor(3841, 675, 28, 795),
                    new Floor(3489, 961, 352, 28),
                    new Floor(3233, 961, 62, 28),
                    new Floor(3522, 672, 28, 191),
                    new Floor(3428, 801, 93, 28),
                    new Floor(3297, 769, 28, 95),
                    new Floor(3265, 705, 28, 124),
                    new Floor(3235, 1087, 157, 28),
                    new Floor(3459, 1124, 63, 28),
                    new Floor(3235, 1119, 124, 28),
                    new Floor(3235, 1150, 100, 28),
                    new Floor(3235, 1184, 59, 28),
                    new Floor(3521, 994, 28, 189),
                    new Floor(3330, 1280, 510, 28),
                    new Floor(3520, 1312, 28, 159),
                    new Floor(3843, 1312, 28, 159),
                    new Floor(3233, 1600, 160, 28),
                    new Floor(3393, 1568, 223, 28),
                    new Floor(3715, 1568, 220, 28),
                    new Floor(4163, 1568, 314, 28),
                    new Floor(3872, 1280, 577, 28),
                    new Floor(4449, 1312, 28, 256),
                    new Floor(3616, 1600, 98, 28),
                    new Floor(3935, 1600, 225, 28),
                    new Floor(1186, 326, 100, 84)
                });


                grassObstacles.AddRange(new List<GrassObstacle>
                {
                    new GrassObstacle(992, 1570, 257, 31),
                    new GrassObstacle(3233, 1568, 257 ,32),
                    new GrassObstacle(3616, 1568, 98, 32),
                    new GrassObstacle(3935, 1568, 225, 32),
                });

                jumppads.AddRange(new List<JumpPad>
                {
                    new JumpPad(962, 1305),
                    new JumpPad(1252, 1560),
                    new JumpPad(310, 630),
                    new JumpPad(3689, 958),
                });

                ladders.AddRange(new List<Ladder>
                {

                });

                movingPlatforms.AddRange(new List<MovingPlatform>
                {
                    new MovingPlatform(2077, 355, 100, 20, 0, 0, 100, 600, 0, 2),    // Vertical Platform
                    new MovingPlatform(2300, 355, 100, 20, 2300, 2550, 0, 0, 2, 0),  // Horizontal Platform
                    new MovingPlatform(2700, 355, 100, 20, 0, 0, 50, 800, 0, 3),     // Vertical Platform
                    new MovingPlatform(2850, 330, 100, 20, 2850, 3200, 0, 0, 2, 0),  // Horizontal Platform 

                });

                platforms.AddRange(new List<Platforms>
                {

                });

                walls.AddRange(new List<Wall>
                {
                    new Wall(87, 35, 28, 283),
                });

                healitems.AddRange(new List<HealItem>
                {
                    new HealItem(1180, 1050),
                    new HealItem(1240, 605),
                    new HealItem(50, 280),
                    new HealItem(4300, 500),
                    new HealItem(3300, 1050),
                    new HealItem(4175, 1520),
                    new HealItem(3262, 608),
                });

                portals.AddRange(new List<Portal>
                {
                    new Portal(4200, 1475),
                });
            }
            else if (levelcounter == 2)   // Level 2
            {
                ClearObstacles();

                dfloors.AddRange(new List<DFloor>
                {
                    new DFloor(300, 800, 195, 25)
                });

                floors.AddRange(new List<Floor>
                {
                    new Floor(0, 582, 305, 250),
                    new Floor(0, 0, 640, 207),
                    new Floor(-30, 0, 50, 700), // that wall on spawn
                    new Floor(630, 0, 250, 550),
                    new Floor(630, 540, 45, 350),
                    new Floor(500, 810, 220, 260),
                    new Floor(0, 780, 108, 290),
                    new Floor(0, 1070, 365, 335),
                    new Floor(515, 1430, 265, 100),
                    new Floor(230, 1470, 365, 100),
                    new Floor(0, 1400, 265, 100),
                    new Floor(775, 1370, 365, 200),
                    new Floor(1035, 1050, 425, 450),
                    new Floor(630, 1065, 150, 100),
                    new Floor(715, 1050, 250, 70),
                    new Floor(785, 790, 400, 50),
                    new Floor(1010, 650, 150, 150),
                    new Floor(1110, 550, 630, 250),
                    new Floor(1920, 550, 480, 350),
                    new Floor(675, 790, 45, 350),
                    new Floor(1160, 785, 300, 300),
                    new Floor(1690, 150, 55, 220),
                    new Floor(860, 0, 1400, 140),
                    new Floor(2250, 140, 150, 700),
                    new Floor(2400, 430, 600, 40),
                    new Floor(2960, 475, 50, 300),
                    new Floor(1575, 1400, 260, 100),
                    new Floor(1830, 1335, 65, 120),
                    new Floor(1905, 1450, 165, 100),
                    new Floor(2050, 1335, 320, 120),
                    new Floor(2340, 1275, 320, 120),
                    new Floor(1460, 1445, 120, 120),
                    new Floor(1606, 1110, 600, 95),
                    new Floor(1605, 980, 140, 130),
                    new Floor(1860, 920, 80, 60),
                    new Floor(1607, 1200, 130, 45),
                    new Floor(1930, 885, 270, 240),
                    new Floor(2665, 1080, 50, 200),
                    new Floor(2505, 750, 500, 330),
                    new Floor(2200, 750, 250, 330)
                });


                grassObstacles.AddRange(new List<GrassObstacle>
                {
                    new GrassObstacle(241,1435,360,60),
                    new GrassObstacle(1750,990,180,130),
                    new GrassObstacle(1455,1401,135,50),
                    new GrassObstacle(1905,1342,165,125)

                });

                jumppads.AddRange(new List<JumpPad>
                {
                    new JumpPad(930, 780)
                });

                ladders.AddRange(new List<Ladder>
                {
                    new Ladder(1010,1053,30,225),
                    new Ladder(720,800,30,255),
                    new Ladder(2460,740,30,500)
                });

                movingPlatforms.AddRange(new List<MovingPlatform>
                {
                    //new MovingPlatform(100, 300, 100, 20, 50, 400, 300, 300, 2, 0), // Horizontal Platform
                    //new MovingPlatform(1400, 300, 120, 20, 1400, 1400, 300, 550, 0, 1) // Vertical Platform
                });
                
                platforms.AddRange(new List<Platforms>
                {
                    new Platforms(515, 470, 120, 15),
                    new Platforms(300, 362, 120, 15),
                    new Platforms(100, 930, 120, 15),
                    new Platforms(100, 930, 120, 15),
                    new Platforms(545, 1300, 110, 15),
                    new Platforms(865, 405, 110, 15),
                    new Platforms(970, 240, 80, 15),
                    new Platforms(1120, 435, 80, 15),
                    new Platforms(1515, 370, 80, 15),
                    new Platforms(1280, 275, 140, 15),
                    new Platforms(2025, 410, 140, 15),
                    new Platforms(2680, 640, 80, 15),
                });

                walls.AddRange(new List<Wall>
                {
                    new Wall(780, 1150, 20, 220),  // Destroyable_Wall1
                    new Wall(2344, 1080, 30, 196)
                });

                healitems.AddRange(new List<HealItem>
                {
                    new HealItem(1700, 1360),
                    new HealItem(610, 1390),
                    new HealItem(1995, 520),
                    new HealItem(2425, 713),
                });

                portals.AddRange(new List<Portal>
                {
                    new Portal(2815, 650),
                });
            }
            else // Level 3
            {
                ClearObstacles();

                dfloors.AddRange(new List<DFloor>
                {
                    //new DFloor(555, 545, 300, 30),
                    //new DFloor(555, 545, 300, 30),
                    //new DFloor(555, 545, 300, 30),
                });

                floors.AddRange(new List<Floor>
                {
                        new Floor(10, 3170, 1100, 150),  // Floor 0
    //new Floor(175, 3120, 100, 15),
    new Floor(0, 1585, 10, 1615),
    new Floor(10, 3170, 1100, 30),
    new Floor(300, 2865, 20, 170),  // Vert
    new Floor(625, 2865, 20, 170),
    new Floor(1100, 2865, 340, 350),  // Wall F0
    new Floor(945, 3010, 65, 15),   // Stair 1-1
    new Floor(975, 2980, 65, 15),   // Stair 1-3
    new Floor(1010, 2945, 65, 15),  // Stair 1-5
    new Floor(1040, 2915, 65, 15),  // Stair 1-7
    new Floor(1070, 2875, 65, 15),  // Stair 1-9
    new Floor(10, 2865, 425, 30),   // Floor 1
    new Floor(560, 2865, 350, 30),  // Floor 1
    new Floor(625, 2550, 20, 170),
    new Floor(1100, 2550, 20, 170),
    new Floor(1425, 1970, 25, 1000),  // WALL Right
    new Floor(465, 2655, 60, 15),   // Stair 2-1
    new Floor(495, 2625, 40, 15),  // Stair 2-3
    new Floor(545, 2575, 70, 60),  // Stair 2-6
    new Floor(575, 2545, 900, 30),  // F3
    new Floor(10, 2545, 435, 30),
    new Floor(300, 2225, 20, 170),  // Vert
    new Floor(625, 2225, 20, 170),  // Vert
    new Floor(1105, 2225, 20, 95),  // Vert
    //new Floor(1090, 2500, 40, 15  ),  // F3
    //new Floor(1120, 2435, 60, 15  ),  // F3
    new Floor(1185, 2370, 90, 60  ),  // Stairs 3-3
    new Floor(1300, 2255, 150, 60  ),  // Stairs 3-4
    new Floor(1315, 2225, 150, 60  ),  // F4
    new Floor(1235, 2225, 30, 15  ),
    new Floor(1090, 2225, 60, 15  ),
    new Floor(945, 2225, 30, 15  ),
    new Floor(780, 2225, 90, 15  ),
    new Floor(620, 2225, 60, 15  ),
    new Floor(415, 2225, 75, 15  ),
    new Floor(10, 2225, 320, 15  ),
    new Floor(100, 2080, 60, 15  ),
    new Floor(75, 2050, 45, 15  ),
    new Floor(30, 2000, 45, 15  ),
    new Floor(10, 1970, 35, 15  ),
    new Floor(975, 1900, 210, 15  ),  // F5
    new Floor(115, 1900, 60, 15  ),
    new Floor(285, 1900, 120, 15  ),
    new Floor(540, 1900, 260, 15  ),
    new Floor(300, 1900, 20, 170  ),
    new Floor(625, 1900, 20, 150  ),
    new Floor(1100, 1900, 20, 170  ),
    new Floor(865, 1855, 105, 15  ),
    new Floor(1170, 1840, 75, 15  ),
    new Floor(1300, 1810, 75, 15  ),
    new Floor(1490, 1775, 310, 30  ),
    new Floor(1775, 1665, 45, 120  ),
    new Floor(10, 1585, 370, 15  ),  // F6
    new Floor(575, 1585, 100, 15  ),
    new Floor(865, 1585, 130, 15  ),
    new Floor(1085, 1585, 60, 15  ),
    new Floor(300, 1585, 15, 75  ),
    new Floor(625, 1585, 15, 75  ),
    new Floor(1100, 1585, 15, 115  ),
    new Floor(800, 1390, 60, 15  ),
    new Floor(845, 1135, 30, 15  ),
    new Floor(930, 780, 220, 30  ),
    new Floor(915, 821, 45, 225  ),
    new Floor(1000, 1000, 350, 355  ),
                });


                grassObstacles.AddRange(new List<GrassObstacle>
                {

                });

                jumppads.AddRange(new List<JumpPad>
                {
                    new JumpPad(910, 3165),
                    //new JumpPad(380, 2855),
                    new JumpPad(1370, 2215),
                    new JumpPad(1260, 1990),
                    new JumpPad(1040, 1890),
                    new JumpPad(1040, 1700),

            });

                ladders.AddRange(new List<Ladder>
                {

                });

                movingPlatforms.AddRange(new List<MovingPlatform>
                {
                 new MovingPlatform(900, 1340, 80, 20, 0, 0, 1100, 1350, 0, 2),  // Horizontal Platform 
                 new MovingPlatform(660, 1080, 100, 20, 0, 0, 750, 1120, 0, 2),  // Horizontal Platform 
                });

                platforms.AddRange(new List<Platforms>
                {
                 new Platforms(175, 3120, 100, 15),
    new Platforms(1090, 2500, 40, 15  ),  // F3
    new Platforms(1130, 2435, 60, 15  ),  // F3
    new Platforms(1245, 2820, 30, 15  ),
    new Platforms(1310, 2785, 80, 15  ),
    new Platforms(1170, 2725, 90, 15  ),
    new Platforms(1295, 2640, 105, 15 ),
    new Platforms(50, 2800, 65, 15 ),
    new Platforms(125, 2835, 50, 15 ),
    new Platforms(195, 2750, 85, 15 ),
    new Platforms(50, 2700, 70, 15 ),
    new Platforms(180, 2655, 45, 15 ),
    new Platforms(255, 2640, 60, 15 ),
    new Platforms(305, 2640, 15, 60 ),
    new Platforms(365, 2450, 45, 15 ),
    new Platforms(445, 2500, 45, 15 ),
    new Platforms(190, 2415, 45, 15 ),
    new Platforms(510, 2390, 75, 15 ),
    new Platforms(670, 2370, 45, 15 ),
    new Platforms(765, 2425, 75, 15 ),
    new Platforms(880, 2354, 75, 15 ),
    new Platforms(960, 2465, 75, 15 ),
    new Platforms(655, 1795, 60, 25 ),
    new Platforms(695, 1760, 60, 25 ),
    new Platforms(785, 1665, 60, 25 ),
    new Platforms(815, 1635, 60, 25 ),
    new Platforms(770, 1550, 30, 25 ),
    new Platforms(640, 1505, 75, 15 ),
    new Platforms(560, 1440, 60, 15 ),
    new Platforms(705, 1425, 45, 15 ),
                });

                walls.AddRange(new List<Wall>
                {
                    new Wall(625, 3040, 15, 130 ),
                    new Wall(305, 2400, 15, 150 ),
                });

                healitems.AddRange(new List<HealItem>
                {
                    new HealItem(1335, 2615),
                    new HealItem(215, 2390),
                    new HealItem(1675, 1750),
                });

                portals.AddRange(new List<Portal>
                {
                   new Portal(1000, 685),
                });
            }
        }

        public void ClearObstacles()
        {
            dfloors.Clear();
            floors.Clear();
            grassObstacles.Clear();
            jumppads.Clear();
            ladders.Clear();
            movingPlatforms.Clear();
            platforms.Clear();
            walls.Clear();
            healitems.Clear();
            portals.Clear();
        }

        /*
        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.U))
            {
                ReloadObstacles();
            }
        }
        */
        
    }

}
