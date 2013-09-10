using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PastaGameLibrary
{
	public class SpriteDrawingList : ActiveList
	{
		public SpriteDrawingList(MyGame theGame, float updateTickInSeconds) : base(theGame, updateTickInSeconds)
		{
		}

		public void Add(Sprite sprite)
		{
			throw new NotImplementedException();
		}

		public void Remove(Sprite sprite)
		{
			throw new NotImplementedException();
		} 

		protected override void OnUpdate()
		{
			TheGame.SpriteBatch.Begin();
			base.OnUpdate();
			TheGame.SpriteBatch.End();
		}
	}
}
