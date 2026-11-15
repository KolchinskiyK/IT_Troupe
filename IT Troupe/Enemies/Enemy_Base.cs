using System;
using Raylib_cs;
using System.Numerics;
using System.Text.Json.Serialization;
using static Raylib_cs.Raylib;  // This removes the need to type "Raylib."

namespace IT_Troupe
{
	abstract public class Enemy_Base
	{
        public float x { get; set; }
        public float y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float startRange { get; set; }
        public float endRange { get; set; }
        public double currentHP { get; set; }
        public int maxHP { get; set; }
        public int hpBarHeight { get; set; } = 10;
        public int enemyDirection = 1;
        public float speed { get; set; } = 50f;

        [JsonConverter(typeof(RectangleConverter))]
        public Rectangle rect { get; set; }

        public Enemy_Base(float x, float y, float width, float height, float startRange, float endRange)
        {
            this.rect = new Rectangle(x, y, width, height);
            this.startRange = startRange;
            this.endRange = endRange;
        }

        public bool CheckCollision(Rectangle playerAttack)
        {
            return CheckCollisionRecs(rect, playerAttack);
        }

        public void DealDamageToEnemy(int i)
        {
            currentHP -= i;
        }

        public abstract void Draw();
    }
}

