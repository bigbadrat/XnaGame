using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    public class SpriteSheetComponent: BaseComponent<SpriteSheetComponent>, IEntityComponent
    {
        Texture2D _tex;
        Color _color;
        Spatial2DComponent _spatial2D;
        Dictionary<string, Rectangle> _frames;
        double _last_update;
        double _timeperframe;
        string _frame;

        public SpriteSheetComponent(string assetName) :
            base()
        {
            _color = Color.White;
            Asset asset = MyGame.GetService<IAssetManager>().GetAsset(assetName);
            _tex = asset.GetAssetAs<Texture2D>();

            _frames = new Dictionary<string, Rectangle>();
            _frame = "amg1_fr1";
            _last_update = 0;
            _timeperframe = 0.3;
            ReadSheet();
            IntrusiveListItem<SpriteSheetComponent>.AddToTail(this);
        }

        public string Name { get { return "SpriteSheet"; } }

        public void LinkPrev(IEntityComponent comp)
        {
            _link.Prev = (SpriteSheetComponent)comp;
        }

        public void LinkNext(IEntityComponent comp)
        {
            _link.Next = (SpriteSheetComponent)comp;
        }

        Rectangle GetFrame(string frame_name)
        {
            return _frames[frame_name];
        }

        public void Update(GameTime gameTime)
        {
            _last_update += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeperframe > _last_update)
                return;

            _last_update -= _timeperframe;

            if (_frame == "amg1_fr1")
                _frame = "amg1_fr2";
            else
                _frame = "amg1_fr1";
            
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle source = GetFrame(_frame);
            sb.Draw(_tex, _spatial2D.Position , source, _color, 0, Vector2.Zero, 1, SpriteEffects.None, _spatial2D.Layer);
        }

        void ReadSheet()
        {
            // open a StreamReader to read the index
            string path = "Content\\Sprites\\Sheet3.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                // while we're not done reading...
                while (!reader.EndOfStream)
                {
                    // get a line
                    string line = reader.ReadLine();

                    // split at the equals sign
                    string[] sides = line.Split('=');

                    // trim the right side and split based on spaces
                    string[] rectParts = sides[1].Trim().Split(' ');

                    // create a rectangle from those parts
                    Rectangle r = new Rectangle(
                       int.Parse(rectParts[0]),
                       int.Parse(rectParts[1]),
                       int.Parse(rectParts[2]),
                       int.Parse(rectParts[3]));

                    // add the name and rectangle to the dictionary
                    _frames.Add(sides[0].Trim(), r);
                }

            }
        }

        public override void OnOwnerChanged()
        {
            _spatial2D = (Spatial2DComponent)Owner.GetComponent("Spatial2D");
        }
    }
}
