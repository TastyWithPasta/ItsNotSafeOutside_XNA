using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
	//public class BackgroundObjectManager
	//{
	//    public const int DefaultSpacingX = 300;
	//    public const int DefaultSpacingY = 300;

	//    Camera2D _backgroundCamera;
	//    BackgroundObject[] m_backgroundObjects;
	//    public int m_backgroundDepth;
	//    Vector2 m_maxAmountOfObjects = new Vector2(1, 1);
	//    Vector2 m_objectSpacing = new Vector2(DefaultSpacingX, DefaultSpacingY);
	//    Transform m_transform;

	//    public Vector2 ObjectSpacing
	//    {
	//        get { return m_objectSpacing; }
	//        set { m_objectSpacing = value; }
	//    }

	//    public Vector2 MaxBackgroundObjects
	//    {
	//        get { return m_maxAmountOfObjects; }
	//    }

	//    public BackgroundObjectManager(Vector2 maxAmountOfObjects, int depth, Camera2D backgroundCamera)
	//        : base()
	//    {
	//        m_transform = new Transform();
	//        _backgroundCamera = backgroundCamera;
	//        m_backgroundDepth = depth;
	//        m_maxAmountOfObjects = maxAmountOfObjects;
	//        m_backgroundObjects = new BackgroundObject[(int)(m_maxAmountOfObjects.X * m_maxAmountOfObjects.Y)];
	//    }

	//    public void CopyObjectsRow(BackgroundObjectManager copyFrom, int indexCopyFrom, int indexCopyTo, int size)
	//    {
	//        for (int i = 0; i < size; ++i)
	//        {
	//            BackgroundObject objectToCopy = copyFrom.m_backgroundObjects[indexCopyFrom];
	//            if (objectToCopy != null)
	//            {
	//                Vector2 jitter = new Vector2(objectToCopy.X - copyFrom.m_objectSpacing.X * indexCopyFrom, objectToCopy.Y);
	//                objectToCopy.Transform.ParentTransform = m_transform;
	//                objectToCopy.Transform.PosX = indexCopyTo * m_objectSpacing.X + jitter.X;
	//                objectToCopy.Transform.PosY = jitter.Y;
	//            }

	//            m_backgroundObjects[indexCopyTo] = objectToCopy;

	//            indexCopyFrom++;
	//            indexCopyTo++;
	//        }
	//    }


	//    public void PlaceObjects(float objectRatio, StyleManager.LevelStyle style)
	//    {
	//        PlaceObjects(objectRatio, style, 0, m_backgroundObjects.Length);
	//    }
	//    public void PlaceObjects(float objectRatio, StyleManager.LevelStyle style, int startIndex, int amount)
	//    {
	//        int maxAmountOfObjects = (int)(m_maxAmountOfObjects.X * m_maxAmountOfObjects.Y);
	//        int amountOfObjects = (int)(amount * objectRatio);
			
	//        List<int> indexes = new List<int>();
	//        for (int i = startIndex; i < startIndex + amount; ++i)
	//            indexes.Add(i);

	//        for (int i = 0; i < amountOfObjects; ++i)
	//        {
	//            int selectionIndex = Globals.Random.Next(0, indexes.Count);
	//            int placementX = indexes[selectionIndex] % (int)m_maxAmountOfObjects.X;
	//            int placementY = indexes[selectionIndex] / (int)m_maxAmountOfObjects.X;
	//            indexes.RemoveAt(selectionIndex);

	//            BackgroundObject objectToGenerateFrom = Globals.BackgroundObjectLibrary.GetObject(style, m_backgroundDepth);
	//            m_backgroundObjects[selectionIndex] = new BackgroundObject(objectToGenerateFrom);
	//            m_backgroundObjects[selectionIndex].Transform.ParentTransform = m_transform;
	//            m_backgroundObjects[selectionIndex].Transform.PosX = m_objectSpacing.X * placementX;
	//            m_backgroundObjects[selectionIndex].Transform.PosY = m_objectSpacing.Y * placementY;
	//            m_backgroundObjects[selectionIndex].ApplyJitter();
	//        }
	//        for (int i = 0; i < indexes.Count; ++i)
	//        {
	//            m_backgroundObjects[indexes[i]] = null;
	//        }
	//    }

	//    public void Update(GameTime gameTime)
	//    {
	//        for (int i = 0; i < m_backgroundObjects.Length; ++i)
	//            if(m_backgroundObjects[i] != null)
	//                m_backgroundObjects[i].Update();
	//    }
	//    public void Draw(SpriteBatch spriteBatch)
	//    {
	//        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, _backgroundCamera.CameraMatrix);
	//        for (int i = 0; i < m_backgroundObjects.Length; ++i)
	//            if (m_backgroundObjects[i] != null)
	//                m_backgroundObjects[i].Draw();
	//        spriteBatch.End();
	//    }
	//}
}
