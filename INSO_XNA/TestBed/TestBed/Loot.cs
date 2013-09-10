using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PastaGameLibrary;


namespace TestBed
{
    //Used to manage worth of coins, and how loot is generated.
    //Will later manage item drop
    public class Loot
    {
        public Loot(int copper, int silver, int gold)
        {
            copper_amount = copper;
            silver_amount = silver;
            gold_amount = gold;
        }
        public int TotalWorth
        {
			get
			{
				return copper_amount * Coin.CopperWorth
				+ silver_amount * Coin.SilverWorth 
                + gold_amount * Coin.GoldWorth; 
            }
        }
        public int AmountOfCoins
        {
            get { return copper_amount + silver_amount + gold_amount; }
        }

        public int copper_amount, silver_amount, gold_amount;

		public static Loot GenerateLoot(float worth)
		{
			int remainder = (int)(worth);
			int goldAmount = remainder / Coin.GoldWorth;
			remainder %= Coin.GoldWorth;
			int silverAmount = remainder / Coin.SilverWorth;
			remainder %= Coin.SilverWorth;
			int copperAmount = remainder / Coin.CopperWorth;
			return new Loot(copperAmount, silverAmount, goldAmount);
		}
    }


}
