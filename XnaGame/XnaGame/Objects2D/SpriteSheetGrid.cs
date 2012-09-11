using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    public class SpriteGridSheet :SpriteBasic
    {
        Vector2 _sheet_size;
        Vector2 _frame_size;
        float _framerate;
        int _last_update;
        int _frame;
        

        public SpriteGridSheet(string spriteName, string assetName, Vector2 sheetSize) :
            base(spriteName,assetName)
        {
            _sheet_size = sheetSize;
            _frame_size.X = _tex.Width / sheetSize.X;
            _frame_size.Y = _tex.Height / sheetSize.Y;
            _framerate = 15;
            _last_update = 0;
            _frame = 0;
        }

        public void GetFrame(int i, out Rectangle frame_coord)
        {            
            
            int start_y = ( i / (int)_sheet_size.X )* (int)_frame_size.Y;
            int start_x = (i % (int)_sheet_size.X ) * (int)_frame_size.X;
            frame_coord = new Rectangle(start_x, start_y,
                                (int)_frame_size.X, (int)_frame_size.Y);
            
        }

        public void Update(GameTime gameTime)
        {
            
            _last_update = gameTime.TotalGameTime.Milliseconds;

            ++ _frame;
            if (_frame == _sheet_size.X * _sheet_size.Y)
            {
                _frame = 0;
                return;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle source;
            GetFrame(_frame, out source);
            System.Console.WriteLine("Frame:" + _frame +" Source " + source );
            sb.Draw(_tex, _pos, source, _color, 0, Vector2.Zero, 1, SpriteEffects.None, _z);
        }

        Dictionary<string, Rectangle> spriteSourceRectangles = new Dictionary<string, Rectangle>();

        void ReadSheet()
        {
            // open a StreamReader to read the index
            string path =  "Content\\Sprites\\Sprites.txt";
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
