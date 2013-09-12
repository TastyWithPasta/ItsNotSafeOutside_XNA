using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestBed
{
    public class AttackManager
    {
		public const int AmountOfAttackTypes = 4;
		const int LengthTrigger = 100;

		public static Slash Slash;


		public static void Initialise(GraphicsDevice device)
		{
			Slash = new Slash(device, LengthTrigger);
		}
    }
}
