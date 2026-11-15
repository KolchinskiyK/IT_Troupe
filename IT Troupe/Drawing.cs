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
    public class Drawing
    {
        public Drawing()
        {
            
        }

        public static void Paint(Player player, AllObstacles obsts, Colors colors, List<Test_Enemy> testEnemies)
        {
            foreach (Platforms platform in obsts.platforms)
            {
                platform.Draw();
            }
            foreach (Ladder ladder in obsts.ladders)
            {
                //ladder.Draw();
            }
            foreach (GrassObstacle grassObstacle in obsts.grassObstacles)
            {
                //grassObstacle.Draw();
            }
            foreach (Floor floor in obsts.floors)
            {
                floor.Draw();
            }
            foreach (MovingPlatform movingPlatform in obsts.movingPlatforms)
            {
                movingPlatform.Draw();
            }
            foreach (var dfloor in obsts.dfloors.ToList()) // copy of the list of destroyable floors to enable changes during the iteration
            {
                player.DFloorCollision(dfloor, obsts.dfloors);
            }
            foreach (DFloor dfloor in obsts.dfloors)
            {
                dfloor.Draw();
            }
            foreach (var wall in obsts.walls.ToList())     // copy of the list of destroyable walls to enable changes during the iteration
            {
                wall.Draw();
            }
            foreach (var pad in obsts.jumppads)    
            {
               pad.Draw();
            }
            foreach (var hitem in obsts.healitems)
            {
               hitem.Draw();
            }
            foreach (var portal in obsts.portals)
            {
               portal.Draw();
            }


            //player.Teleport(platforms, camera);
            player.Draw();
            player.DrawFire(colors);

            //Draw Enemies
            foreach (Test_Enemy tenemy in testEnemies)
            {
                tenemy.Draw();
            }
            //testEnemy1.DrawEnemy();                 
        }
    }
}
