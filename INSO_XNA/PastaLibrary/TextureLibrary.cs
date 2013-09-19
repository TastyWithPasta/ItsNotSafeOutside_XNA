using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public interface ContentLoader<T>
	{
		void LoadContent(ContentManager contentManager, string path);
		void UnloadContent();
		T Get(string name);
	}

    public static class TextureLibrary
    {
		private static Dictionary<string, Texture2D> m_textureLibrary = new Dictionary<string, Texture2D>();
		private static Dictionary<string, SpriteSheet> m_spriteSheets = new Dictionary<string, SpriteSheet>();
		//private string m_basePath = "Content/";

		public static Texture2D PixelTexture;
		public static SpriteSheet PixelSpriteSheet;

		public static void Initialise(GraphicsDevice device)
		{
			PixelTexture = new Texture2D(device, 1, 1);
			Color[] data = new Color[] { new Color(1.0f, 1.0f, 1.0f, 1.0f) };
			PixelTexture.SetData<Color>(data);
			PixelSpriteSheet = new SpriteSheet(PixelTexture);
		}

		public static void LoadContent(ContentManager Content, string path)
		{
			string fullpath;
			if (path == "")
			{
				fullpath = Content.RootDirectory;
			}
			else
			{
				fullpath = Content.RootDirectory + "/" + path;
				path += "/";
			}

			DirectoryInfo di = new DirectoryInfo(fullpath);
			FileInfo[] files = di.GetFiles();
			int length = files.Length;

			for (int i = 0; i < length; ++i)
			{
				string name = files[i].Name.Substring(0, files[i].Name.Length - 4);
				m_textureLibrary.Add(name, Content.Load<Texture2D>(path + name));
			}
		}

		public static void UnloadContent()
		{
			m_textureLibrary.Clear();
		}

		public static Texture2D Get(string name)
		{
			return m_textureLibrary[name];
		}

		public static SpriteSheet GetSpriteSheet(string name)
		{
			if (m_spriteSheets.ContainsKey(name))
				return m_spriteSheets[name];
			SpriteSheet newSS = new SpriteSheet(m_textureLibrary[name], 1, 1);
			m_spriteSheets.Add(name, newSS);
			return newSS;
		}
		public static SpriteSheet GetSpriteSheet(string name, int rows, int columns)
		{
			if (m_spriteSheets.ContainsKey(name))
				return m_spriteSheets[name];
			SpriteSheet newSS = new SpriteSheet(m_textureLibrary[name], rows, columns);
			m_spriteSheets.Add(name, newSS);
			return newSS;
		}
    }
}
