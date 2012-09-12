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
        float _timeperframe;
        double _last_update;
        int _frame;
        

        public SpriteGridSheet(string spriteName, string assetName, Vector2 sheetSize) :
            base(spriteName,assetName)
        {
            _sheet_size = sheetSize;
            _frame_size.X = _tex.Width / sheetSize.X;
            _frame_size.Y = _tex.Height / sheetSize.Y;
            _framerate = 30;
            _last_update = 0;
            _frame = 0;
            _timeperframe = 1.0f / _framerate;            
        }

        /// <summary>
        /// Here we calculate the source rectangle for frame i
        /// assuming the sprite sheet contains a regular grid
        /// </summary>
        /// <param name="i"> The frame to calculate</param>
        /// <returns>The rectangle to use as source for frame i</returns>
        public Rectangle GetFrame(int i )
        {   
            int start_y = ( i / (int)_sheet_size.X )* (int)_frame_size.Y;
            int start_x = (i % (int)_sheet_size.X ) * (int)_frame_size.X;
            return new Rectangle(start_x, start_y,
                                (int)_frame_size.X, (int)_frame_size.Y);
            
        }

        /// <summary>
        /// Checks how much time has passed since last update and update a frame
        /// if needed. 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _last_update += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeperframe > _last_update )
                return;
            
            _last_update -= _timeperframe;

            ++ _frame;
            if (_frame >= _sheet_size.X * _sheet_size.Y)
            {
                _frame = 0;
                return;
            }

        }

        /// <summary>
        /// Simply query for the the current source rectangle and draw it
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            Rectangle source = GetFrame(_frame);            
            sb.Draw(_tex, _pos, source, _color, 0, Vector2.Zero, 1, SpriteEffects.None, _z);
        }

        

    }
}
