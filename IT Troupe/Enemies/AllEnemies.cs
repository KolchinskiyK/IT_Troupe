
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
    public class AllEnemies
    {
        public AllEnemies()
        {
        }

        //Variables
        [JsonConverter(typeof(RectangleListConverter))]
        public List<Enemy_Burnout> burnoutEnemies { get; set; } = new List<Enemy_Burnout>();

        [JsonConverter(typeof(RectangleListConverter))]
        public List<Test_Enemy> testEnemies { get; set; } = new List<Test_Enemy>();

        [JsonConverter(typeof(RectangleListConverter))]
        public List<Enemy_Procrastination> procrastinationEnemies { get; set; } = new List<Enemy_Procrastination>();

        public void ReloadEnemies(int levelcounter)
        {
           if(levelcounter == 1)        // Level 1
           {
                ClearEnemies();
                burnoutEnemies.AddRange(new List<Enemy_Burnout>
                {
                    new Enemy_Burnout(510, 575, 50, 60, 510, 900),
                    new Enemy_Burnout(640, 265, 50, 60, 640, 930),
                    new Enemy_Burnout(3670, 265, 50, 60, 3360, 3900),

                });
                testEnemies.AddRange(new List<Test_Enemy>
                {
                    new Test_Enemy(700,1515, 45, 70, 5000),
                    new Test_Enemy(1270, 1515, 45, 70, 5000),
                    new Test_Enemy(500, 270, 45, 70, 5000),
                    new Test_Enemy(3570, 280, 45, 70, 5000),
                    new Test_Enemy(3615, 1230, 45, 70, 5000),
                });

                procrastinationEnemies.AddRange(new List<Enemy_Procrastination>
                {
                    new Enemy_Procrastination(700, 1515, 50, 60, 700, 1000),
                    new Enemy_Procrastination(1330, 940, 50, 60, 1330, 1570),
                    new Enemy_Procrastination(1500, 265, 50, 60, 1500, 1800),
                    new Enemy_Procrastination(3840, 590, 50, 60, 3840, 4020),
                    new Enemy_Procrastination(3415, 1520, 50, 60, 3415, 3590),
                    new Enemy_Procrastination(3730, 1520, 50, 60, 3730, 3900),
                    
                });

           }
           else if(levelcounter == 2)   // Level 2
           {
                ClearEnemies();
                burnoutEnemies.AddRange(new List<Enemy_Burnout>
                {
                    new Enemy_Burnout(1400, 495, 50, 60, 1400, 1700),
                    new Enemy_Burnout(2558, 690, 50, 60, 2490, 2680),
                });

                testEnemies.AddRange(new List<Test_Enemy>
                {
                    new Test_Enemy(835, 1345, 45, 70, 5000),
                    new Test_Enemy(1525, 550, 45, 70, 5000),
                    new Test_Enemy(2180, 1290, 45, 70, 5000),
                });

                procrastinationEnemies.AddRange(new List<Enemy_Procrastination>
                {
                    new Enemy_Procrastination(850, 990, 50,60,750,1150),
                    new Enemy_Procrastination(800, 735, 50,60,660,1000),
                    new Enemy_Procrastination(1690, 1345, 50,60,1600,1800)
                });

           }
           else                         // Level 3
           {
                ClearEnemies();
                burnoutEnemies.AddRange(new List<Enemy_Burnout>
                {
                     new Enemy_Burnout(690, 2335, 50, 60, 660, 750),
                     new Enemy_Burnout(950, 2325, 50, 60, 900, 1000),
                     new Enemy_Burnout(500, 1250, 50, 60, 450, 900),
                     new Enemy_Burnout(600, 1000, 50, 60, 550, 950),
                     new Enemy_Burnout(700, 750, 50, 60, 650, 1000)
                });
                testEnemies.AddRange(new List<Test_Enemy>
                {
                    new Test_Enemy(650, 2830, 45, 70, 5000),
                    new Test_Enemy(650, 2500, 45, 70, 5000),
                    new Test_Enemy(1170, 2500, 45, 70, 5000),
                });

                procrastinationEnemies.AddRange(new List<Enemy_Procrastination>
                {
                    new Enemy_Procrastination(700, 3125, 50,60,680,1025),
                    new Enemy_Procrastination(235, 2200, 50,60,35,320),
                    new Enemy_Procrastination(370, 2815, 50,60,260,440),
                });

            }
        }

        public void ClearEnemies()
        {
            burnoutEnemies.Clear();
            testEnemies.Clear();
            procrastinationEnemies.Clear();
        }

        /*
        public void Update()
        {
            if(IsKeyPressed(KeyboardKey.U))
            {
                ReloadEnemies();
            }
        }
        */
    }
}

