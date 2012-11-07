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
    public class SpriteGridSheetComponent : Component<SpriteGridSheetComponent>, IEntityComponent
    {
        Texture2D _tex;
        Color _color;
        Spatial2DComponent _spatial2D;
        Vector2 _sheet_size;
        Vector2 _frame_size;
        float _framerate;
        float _timeperframe;
        double _last_update;
        int _frame;


        public SpriteGridSheetComponent(string assetName, Vector2 sheetSize) :
            base()
        {
            _color = Color.White;
            Asset asset = MyGame.GetService<IAssetManager>().GetAsset(assetName);
            _tex = asset.GetAssetAs<Texture2D>();

            _sheet_size = sheetSize;
            _frame_size.X = _tex.Width / sheetSize.X;
            _frame_size.Y = _tex.Height / sheetSize.Y;
            _framerate = 30;
            _last_update = 0;
            _frame = 0;
            _timeperframe = 1.0f / _framerate;

            IntrusiveListItem<SpriteGridSheetComponent>.AddToTail(this);
        }

        public string Name { get { return "SpriteGridSheet"; } }

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
        public void Draw(SpriteBatch sb)
        {
            Rectangle source = GetFrame(_frame);

            sb.Draw(_tex,                   //Texture to draw
                _spatial2D.Position,        //position
                source,                     //Source (no source means the whole rectangle)
                _color,                     //Color
                0,                          //Rotation
                Vector2.Zero,               //Origin of the sprite
                1,                          //Scale
                SpriteEffects.None,         //Effectes
                _spatial2D.Layer);          //Depth
        }

        public override void OnOwnerChanged()
        {
            _spatial2D = (Spatial2DComponent)Owner.GetComponent("Spatial2D");
        }

    }
}
