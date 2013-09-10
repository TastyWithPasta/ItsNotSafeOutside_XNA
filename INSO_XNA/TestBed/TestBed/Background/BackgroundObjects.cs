using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public class BackgroundObjectLibrary
    {
        //Style > Depth > BackgroundObjects[]
        Dictionary<StyleManager.LevelStyle, BackgroundObject[][]> _backgroundObjects = 
            new Dictionary<StyleManager.LevelStyle, BackgroundObject[][]>();

        public BackgroundObjectLibrary()
        {
            int amountOfStyles = StyleManager.AmountOfStyles;
            for(int i = 0; i < amountOfStyles; ++i)
				_backgroundObjects.Add((StyleManager.LevelStyle)i, new BackgroundObject[BackgroundObject.AmountOfBackgroundLayers][]);

            InitialiseCountryStyle();
			InitialiseCloudStyle();
        }
        private void InitialiseCountryStyle()
        {
            BackgroundObject[] layer0Objects = new BackgroundObject[]{
                new Bg_Hill_Naked(0),
                new Bg_Hill_Trees(0),
                
                new Bg_Cliff_Small(0),
            };
            BackgroundObject[] layer1Objects = new BackgroundObject[]{
                new Bg_Hill_Trees(1),
                new Bg_Hill_Naked(1),
                new Bg_Cliff_Small(1),
				new Bg_Hill_Tall(0),
            };
            BackgroundObject[] layer2Objects = new BackgroundObject[]{
                new Bg_Hill_Trees(2),
                new Bg_Hill_Naked(2),
                new Bg_Cliff_Small(2),
            };
            _backgroundObjects[StyleManager.LevelStyle.Country][0] = layer0Objects;
            _backgroundObjects[StyleManager.LevelStyle.Country][1] = layer1Objects;
            _backgroundObjects[StyleManager.LevelStyle.Country][2] = layer2Objects;
        }
		private void InitialiseCloudStyle()
		{
			BackgroundObject[] layer0Objects = new BackgroundObject[]{
                new Bg_Cloud_Long(0),
            };
			BackgroundObject[] layer1Objects = new BackgroundObject[]{
                new Bg_Cloud_Long(1),
            };
			BackgroundObject[] layer2Objects = new BackgroundObject[]{
                new Bg_Cloud_Long(2),
            };
			_backgroundObjects[StyleManager.LevelStyle.Clouds][0] = layer0Objects;
			_backgroundObjects[StyleManager.LevelStyle.Clouds][1] = layer1Objects;
			_backgroundObjects[StyleManager.LevelStyle.Clouds][2] = layer2Objects;
		}

        public BackgroundObject GetObject(StyleManager.LevelStyle style, int depth)
        { 
            return _backgroundObjects[style][depth][Globals.Random.Next(0, _backgroundObjects[style][depth].Length)];
        } 
    }
    public class BackgroundObject : GameObject
    {
		public const int AmountOfBackgroundLayers = 3;
        public const int ObjectWidth = 300;
        protected Vector2 m_placementJitter = Vector2.Zero;
		private int m_depth;

		public BackgroundObject(BackgroundObject objectToGenerateFrom) : base(objectToGenerateFrom)
		{
			Transform.ScaleUniform = objectToGenerateFrom.Transform.SclX + 6;

			m_placementJitter = objectToGenerateFrom.m_placementJitter;
			m_depth = objectToGenerateFrom.m_depth;

			m_drawingList = World.DL_BgLayers[m_depth];
			m_drawingList.Add(this, 0);
		}


		public static List<BackgroundObject> CreateBackgroundObjects(int amountToCreate, Transform transform, Vector2 generationBounds, Vector2 spacing, StyleManager.LevelStyle style, int depth)
		{
			List<BackgroundObject> result = new List<BackgroundObject>();
			int maxAmountOfObjectsHorizontal = (int)(generationBounds.X / spacing.X);
			if (spacing.X == 0) maxAmountOfObjectsHorizontal = 1;
			int maxAmountOfObjectsVertical = (int)(generationBounds.Y / spacing.Y);
			if (spacing.Y == 0) maxAmountOfObjectsVertical = 1;
			int maxAmountOfObjects = maxAmountOfObjectsHorizontal * maxAmountOfObjectsVertical;
			
			List<int> indexes = new List<int>();
			for (int i = 0; i < maxAmountOfObjects; ++i)
				indexes.Add(i);

			for (int i = 0; i < amountToCreate; ++i)
			{
				int selectionIndex = Globals.Random.Next(0, indexes.Count);
				int placementX = indexes[selectionIndex] % maxAmountOfObjectsHorizontal;
				int placementY = indexes[selectionIndex] / maxAmountOfObjectsVertical;
				indexes.RemoveAt(selectionIndex);

				BackgroundObject objectToGenerateFrom = Globals.Backgrounds.GetObject(style, depth);
				BackgroundObject newObject = new BackgroundObject(objectToGenerateFrom);
				if(transform != null)
					newObject.Transform.ParentTransform = transform;
				newObject.Transform.PosX = spacing.X * placementX;
				newObject.Transform.PosY = spacing.Y * placementY;
				newObject.ApplyJitter();
				result.Add(newObject);
			}
			return result;
		}

		public BackgroundObject(SpriteSheet texture, int depth)
			: base()
        {
			m_sprite = new Sprite(Globals.TheGame, texture, Transform);
			m_depth = depth;
		}

		public void ApplyJitter()
		{
			Transform.PosX += -m_placementJitter.X * 0.5f + Globals.Random.Next(0, (int)m_placementJitter.X + 1);
			Transform.PosY += -m_placementJitter.Y * 0.5f + Globals.Random.Next(0, (int)m_placementJitter.Y + 1);
		}

		public override void Update()
		{ }
		public override void Draw()
		{
			m_sprite.Draw();
		}
    }


	public class Bg_Cloud_Long : BackgroundObject
	{
		public Bg_Cloud_Long(int depth)
            : base(TextureLibrary.GetSpriteSheet("bg_cloudlong"), depth)
        {
            //_placementOffset = new Vector2(100, 24);
			m_sprite.Alpha = 0.2f;
            m_placementJitter = new Vector2(600, 100);
			Transform.Scale = new Vector2(2.0f, 2.0f);
        }
	}
    public class Bg_Cliff_Small : BackgroundObject
    {
		public Bg_Cliff_Small(int depth)
			: base(TextureLibrary.GetSpriteSheet("bg_cliff_small"), depth)
        {
            //_placementOffset = new Vector2(50, 24);
            m_placementJitter = new Vector2(50, 0);
        }
    }
    public class Bg_Hill_Tall : BackgroundObject
    {
		public Bg_Hill_Tall(int depth)
			: base(TextureLibrary.GetSpriteSheet("bg_hill_tall"), depth)
        {
            //_placementOffset = new Vector2(100, 24);
            m_placementJitter = new Vector2(100, 0);
        }
    }
    public class Bg_Hill_Naked : BackgroundObject
    {
		public Bg_Hill_Naked(int depth)
			: base(TextureLibrary.GetSpriteSheet("bg_hill"), depth)
        {
            //_placementOffset = new Vector2(100, 50);
            m_placementJitter = new Vector2(50, 0);
        }
    }
    public class Bg_Hill_Trees : BackgroundObject
    {
		public Bg_Hill_Trees(int depth)
			: base(TextureLibrary.GetSpriteSheet("bg_hill_trees"), depth)
        {
            //_placementOffset = new Vector2(100, 50);
            m_placementJitter = new Vector2(50, 0);
        }
    }
    //public class Bg_Hill_Tall : BackgroundObject
    //{
    //    public Bg_Hill_Tall()
    //        : base(TextureLibrary.GetSpriteSheet("bg_hill_tall"))
    //    {
    //        _placementOffset = new Vector2(200, 25);
    //    }
    //}
}
