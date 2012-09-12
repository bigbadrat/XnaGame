using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    public class SpriteSheet: SpriteBasic
    {

        double _last_update;
        double _timeperframe;
        string _frame;

        public SpriteSheet(string spriteName, string assetName ) :
            base(spriteName,assetName)
        {
            _frame = "amg1_fr1";
            _last_update = 0;
            _timeperframe = 0.3;
            ReadSheet();
        }

        Dictionary<string, Rectangle> spriteSourceRectangles = new Dictionary<string, Rectangle>();

        Rectangle GetFrame(string frame_name)
        {
            return spriteSourceRectangles[frame_name];
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

        public override void Draw(SpriteBatch sb)
        {
            Rectangle source = GetFrame(_frame);
            sb.Draw(_tex, _pos, source, _color, 0, Vector2.Zero, 1, SpriteEffects.None, _z);
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
                    spriteSourceRectangles.Add(sides[0].Trim(), r);
                }

            }
        }
    }
}
