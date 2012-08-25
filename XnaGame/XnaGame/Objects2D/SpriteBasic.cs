using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class SpriteBasic
    {
        string _name;
        Texture2D _tex;
        Vector2 _pos;
        Color _color;
        int _id;
        float _z;

        public SpriteBasic(string spriteName, string assetName)
        {
            _pos = new Vector2(0, 0);
            _color = Color.White;
            _name = spriteName;

            Asset asset =  MyGame.GetService<IAssetManager>().GetAsset(assetName);
            _tex = asset.GetAssetAs<Texture2D>();

            ISpriteManager man = MyGame.GetService<ISpriteManager>();
            man.AddSprite(this);
        }

        public void Draw(SpriteBatch sb)
        {            
            sb.Draw(_tex, _pos, null,_color, 0, Vector2.Zero, 1, SpriteEffects.None,_z);
        }

        public void Move(float x, float y)
        {
            _pos.X += x;
            _pos.Y += y;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public float Layer
        {
            get { return _z; }
            set { _z = value*1.0f/255; }
        }

                  
    }
}
