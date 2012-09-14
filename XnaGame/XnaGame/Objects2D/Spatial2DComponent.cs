using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class Spatial2DComponent : BaseComponent<Spatial2DComponent>, IEntityComponent
    {
        Vector2 _pos;
        float _z;

        public Spatial2DComponent()
            : base()
        {
            IntrusiveListItem<Spatial2DComponent>.AddToTail(this);
        }

        public string Name { get { return "Spatial2D"; } }

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public void Move(float x, float y)
        {
            _pos.X += x;
            _pos.Y += y;
        }

        /// <summary>
        /// Layer to draw the sprite. Using a 0-255 scale. The higher the layer
        /// the later this sprite will be drawn, resulting in the sprite drawn 
        /// on TOP of lower level layers
        /// </summary>
        public float Layer
        {
            get { return _z; }
            set { _z = (255 - value) * 1.0f / 255; }
        }
    }
}
