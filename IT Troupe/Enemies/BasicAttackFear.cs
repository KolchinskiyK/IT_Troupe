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
	public class BasicAttackFear
	{
		public float attackInterval { get; set; }
		public double lastAttackTime { get; set; } = GetTime() * 1000;

        [JsonConstructor]
        public BasicAttackFear(float attackInterval)
		{
			this.attackInterval = attackInterval;
			//this.lastAttackTime = GetTime() * 1000; 
		}

		public void CanAttackBasic(Player player)
		{
			double currentTime = GetTime() * 1000;

			if (currentTime - lastAttackTime >= attackInterval)
			{
				player.TakeDamage(5);
				lastAttackTime = currentTime;
			}

		}

        public void CanAttack(Player player)
        {
            double currentTime = GetTime() * 1000;

            if (currentTime - lastAttackTime >= 3000)
            {
                player.TakeDamage(10);
                lastAttackTime = currentTime;
                // Console.WriteLine("lastAttackTime: " + lastAttackTime);
            }

        }

    }

}

