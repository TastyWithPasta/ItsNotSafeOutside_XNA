using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public class EarthTile : GameObject
    {
		public const int VerticalOffset = -42; //Because it's what life's about
        public const int EarthTileWidth = 900;
        public static SpriteSheet[] GroundSectionTextures;

        public EarthTile(int index)
        {
            SpriteSheet texture = GroundSectionTextures[Globals.Random.Next(0, GroundSectionTextures.Length - 1)];
			m_sprite = new Sprite(Globals.TheGame, texture, Transform);
			m_sprite.Origin = Vector2.Zero;
           // m_groundSprite.Width = GroundSectionWidth;
           // _sectionWidth = GroundSectionWidth;

			Transform.PosX = index * EarthTileWidth;
            Transform.PosY = -42;
            //_groundSprite.SourceRectangle = new Rectangle((_sectionIndex % splits) * SectionWidth,
            //    _groundSprite.SourceRectangle.Y,
            //    SectionWidth,
            //    _groundSprite.SourceRectangle.Height);

			World.DL_EarthTiles.Add(this, 0);
        }
		public override void Update()
		{
			throw new NotImplementedException();
		}
        public override void Draw()
        {
			m_sprite.Draw();
        }
    }
}
