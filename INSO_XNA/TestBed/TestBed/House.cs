using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{   
    public class House : GameObject
    {
        public static SpriteSheet GroundTexture;

        protected Vector2 _ninjaHotSpot;
        Sprite m_entranceMask;

		public House(SpriteSheet houseTexture, SpriteSheet maskTexture)
            :base()
        {
            Transform transform;

			m_transform = new Transform();

			transform = new Transform();
			transform.ParentTransform = m_transform;
			m_sprite = new Sprite(Globals.TheGame, houseTexture, m_transform);
            m_sprite.Origin = new Vector2(0, 1);

			transform = new Transform();
			transform.ParentTransform = m_transform;
			m_entranceMask = new Sprite(Globals.TheGame, maskTexture, m_transform);
            m_entranceMask.Origin = new Vector2(0, 1);

			m_drawingList = World.DL_House;
			m_drawingList.Add(this, 0);
        }


        public Sprite EntranceMask
        {
            get { return m_entranceMask; }
        }
        public Vector2 NinjaHotspot
        {
            get { return _ninjaHotSpot; }
        }
        public float HouseExitPoint
        {
			get { return m_sprite.Transform.PositionGlobal.X + 200; }
        }

		public override void Update()
		{
			throw new NotImplementedException();
		}

        public override void Draw()
        {
			m_sprite.Draw();
            m_entranceMask.Draw();
        } 
    }

    public class Hs_Building : House
    {
        public Hs_Building()
			: base(TextureLibrary.GetSpriteSheet("hs_building2", 1, 4), TextureLibrary.GetSpriteSheet("hs_building2m"))
        {
            _ninjaHotSpot = new Vector2(m_sprite.Transform.PosX + 50, -330);
        }
    }
}
