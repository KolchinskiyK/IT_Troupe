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
    public class MovingPlatform
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public int directionX { get; set; } = 1; // 1 for moving right and -1 for moving left
        public int directionY { get; set; } = 1; // 1 for moving down and -1 for up
        public float minX { get; set; }
        public float maxX { get; set; } // minimum and maximum horizontal moving area
        public float minY { get; set; }
        public float maxY { get; set; } // minimum and maximum vertical moving area
        public float speedX { get; set; } //horizontal platform moving speed 
        public float speedY { get; set; }  //vertical platform moving speed

        [JsonConstructor]
        public MovingPlatform(float x, float y, float width, float height, float minX, float maxX, float minY, float maxY, float speedX, float speedY)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.speedX = speedX;
            this.speedY = speedY;
        }

        public void Update()
        {
            // move the platform
            x += speedX * directionX; // horizontal movement
            y += speedY * directionY; // vertical movement

            // update horizontal platform movement
            if (x < minX && directionX < 0 || x + width > maxX && directionX > 0)
            {
                directionX *= -1; // change direction

            }
            // update vertical platform movement
            if (y < minY && directionY < 0 || y + height > maxY && directionY > 0)
            {
                directionY *= -1; // change direction

            }
        }
        public void Draw()
        {
            DrawRectangle((int)x, (int)y, (int)width, (int)height, Color.Gray);
        }

        // get platform attributes
        public float GetX() { return x; }
        public float GetY() { return y; }
        public float GetWidth() { return width; }
        public float GetHeight() { return height; }
        public float GetSpeedX() { return speedX * directionX; }
        public float GetSpeedY() { return speedY * directionY; }
        public float GetDirectionY() { return directionY; }
    }
}
