using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestBed
{
	public static class Globals
	{
		public static Random Random = new Random();
		public static InsoGame theGame;
		//public static Score GameScore;
		//public static CoinBar GameCoinBar;
		//public static Chest GameChest;
		//public static Moon GameMoon;
		//public static PointIndicators GamePointIndicators;
		//public static LevelGenerator LevelGenerator;
		//public static UpgradeManager UpgradeManager;
		//public static AttackManager AttackManager;
		public static BackgroundObjectLibrary Backgrounds;
		public static GameScene GameScene;
		//public static Ninja Ninja;


		public static SpriteFont debugFont;
		public static KeyboardState kbs;


		public static InsoGame TheGame
		{
			get { return theGame; }
		}


		public static void Initialise(InsoGame insoGame)
		{
			theGame = insoGame;

			

			Backgrounds = new BackgroundObjectLibrary();
			TextureLibrary.Initialise(theGame.GraphicsDevice);
			debugFont = insoGame.Content.Load<SpriteFont>("Spritefonts/debug");

			EarthTile.GroundSectionTextures = new SpriteSheet[] 
			{
			    TextureLibrary.GetSpriteSheet("section"),
			};

			ShurikenReceiver.ImpactTexture = TextureLibrary.GetSpriteSheet("shuriken_impacteffect", 1, 4);

			//Destructible.MiniFireTexture = TextureLibrary.GetSpriteSheet("minifire", 1, 4);

			//FireworkSpark.SparkTexture = InsoGame.Pixel;
			//FireworkBlast.ParticleTextures = new SpriteSheet[] { 
			//    TextureLibrary.GetSpriteSheet("firework_blastp"),
			//    TextureLibrary.GetSpriteSheet("firework_blastp_2"),
			//    TextureLibrary.GetSpriteSheet("firework_blastp_3"),
			//    TextureLibrary.GetSpriteSheet("firework_blastp_4"),
			//    TextureLibrary.GetSpriteSheet("firework_blastp_5"),
			//};
			//FireworkBlastParticle.BackgroundTexture = TextureLibrary.GetSpriteSheet("firework_blastp_b");
			//FireworkBlast.HaloTexture = TextureLibrary.GetSpriteSheet("firework_halo");
			//PointDigit.IndicatorTexture = TextureLibrary.GetSpriteSheet("counter_vertical");
			//House.GroundTexture = TextureLibrary.GetSpriteSheet("section_1");
			//Tpl_Enemy.DefaultThumbnail = TextureLibrary.GetSpriteSheet("thb_default");
			//BurnSmoke.SmokeTexture = TextureLibrary.GetSpriteSheet("smoke1");
			//Confetti.ConfettiTexture = TextureLibrary.GetSpriteSheet("confetti", 1, 4);
			Coin.CoinTextures = new SpriteSheet[] 
            {
                TextureLibrary.GetSpriteSheet("coin_copper", 1, 6),
                TextureLibrary.GetSpriteSheet("coin_silver", 1, 6),
                TextureLibrary.GetSpriteSheet("coin_gold", 1, 6),
            };
			//StackCoin.StackCoinTextures = new Texture2D[]
			//{
			//    TextureLibrary.GetSpriteSheet("coin_copper_pile"),
			//    TextureLibrary.GetSpriteSheet("coin_silver_pile"),
			//    TextureLibrary.GetSpriteSheet("coin_gold_pile"),
			//};
			//Ts_BaseZombie.ZombieTombStoneTextures = new Texture2D[]
			//{
			//    TextureLibrary.GetSpriteSheet("ts_bz_1"),
			//    TextureLibrary.GetSpriteSheet("ts_bz_2"),
			//};
		}
	}
}
