using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed {

    public enum COIN_TYPE
    {
        COPPER = 0,
        SILVER = 1,
        GOLD = 2,
    }

	class Coin : GameObject 
    {
		public const int CopperWorth = 1;
		public const int SilverWorth = 10;
		public const int GoldWorth = 100;

        const float AlphaDecrement = 0.1f;
        const float PickUpVelocity = -10;
 
        public static SpriteSheet[] CoinTextures;
        private COIN_TYPE _type;
        private bool _isPickedUp;

		CircleCollider m_circleCollider;
		SpriteSheetAnimation m_animation;

		public static void SpawnCoins(Loot loot, Vector2 position)
		{
			for (int i = 0; i < loot.copper_amount; i++)
				SpawnCoin(COIN_TYPE.COPPER, position);
			for (int i = 0; i < loot.silver_amount; i++)
				SpawnCoin(COIN_TYPE.SILVER, position);
			for (int i = 0; i < loot.gold_amount; i++)
				SpawnCoin(COIN_TYPE.GOLD, position);
		}
		public static void SpawnCoin(COIN_TYPE type, Vector2 position)
		{
			Coin c = new Coin(type);
			position.Y -= 20;
			c.Transform.Position = position;
		}

		public Coin(COIN_TYPE type) : base()
        {
			_type = type;
			m_sprite = new Sprite(Globals.TheGame, CoinTextures[(int)type], Transform);
			m_sprite.Origin = new Vector2(0.5f, 1.0f);
			m_physics = new PhysicsComponent(Globals.TheGame, Transform);
			m_animation = new SpriteSheetAnimation(m_sprite, 0, 5, 0.1f, -1);
			m_animation.Start();
			m_physics.Throw((float)Globals.Random.NextDouble() * 25, Globals.Random.Next(-30, -10), 0);
            m_physics.Mass = 1.3f;
            m_physics.Restitution = 0.75f;
			//m_physics.GroundLevel = Globals.Random.Next(-8, 3);
			m_physics.GroundLevel = 0;
			m_circleCollider = new CircleCollider(this);

			World.UL_Global.Add(this, 0);
			World.DL_ItemDrops.Add(this, 1);
			
			//ADDTHISLATER
			//Backpacker.HitPlayerColliders.Add(m_circleCollider, PickUp);
		}

        private void PickUp(Collider player)
        {
            _isPickedUp = true;
            int value = 0;

            if(_type == COIN_TYPE.COPPER)
                value = CopperWorth;
            else if (_type == COIN_TYPE.SILVER)
                value = SilverWorth;
            else if(_type == COIN_TYPE.GOLD)
                value = GoldWorth;

			//ADDTHISLATER
            //Globals.GameScore.AddToScore(value);
			//Globals.GameCoinBar.AddCoin(_type);
        }
		public override void Update() 
        {
			bool test;
			if (Globals.kbs.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C) && m_transform.PosY < -5)
				test = true;

			m_circleCollider.Update();
			m_physics.Update();
			m_animation.Update();

            if (_isPickedUp)
            {
                Transform.PosY += PickUpVelocity;
                Transform.SclX = Transform.SclY = Transform.SclX * 1.075f;

				if (m_sprite.Alpha - AlphaDecrement > 0)
					m_sprite.Alpha -= AlphaDecrement;
                else
                {
                    //CoinPiles.AddCoin(_type);
					m_sprite.Alpha = 0;
                }
            }


			bool removeMe = m_sprite.Alpha == 0;

			if (removeMe)
				ClearLists();
		}
		public override void Draw()
		{
			m_sprite.Draw();
		}

		//private void DoPartyLeaderCollision()
		//{
		//    Rectangle rect = BackpackerGroup.PartyLeader.BoundingRectangle;
		//    float x = ScreenX;
		//    float y = ScreenY;
		//    if (x > rect.X && x < rect.X + rect.Width
		//        && y > rect.Y)
		//        PickUp();
		//}
	}
}
