using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNAWP_TextureLibrary
{
    public class SpritefontManager
    {
        static Dictionary<string, SpriteFont> _spritefontDictionary = new Dictionary<string,SpriteFont>();
        static string _basePath = "Spritefonts";

        public static string BasePath
        {
            get { return _basePath; }
            set { _basePath = value; }
        }

        public static void LoadSpritefonts(ContentManager Content, string[] spritefontNames)
        {
            for (int i = 0; i < spritefontNames.Length; ++i)
                LoadSpritefont(Content, spritefontNames[i]);
        }
        public static void LoadSpritefont(ContentManager Content, string spritefontName)
        {
            string fullPath = _basePath + "/" + spritefontName;
            _spritefontDictionary.Add(spritefontName, Content.Load<SpriteFont>(fullPath));
        }
        public static SpriteFont Get(string name)
        {
            return _spritefontDictionary[name];
        }
    }
}
