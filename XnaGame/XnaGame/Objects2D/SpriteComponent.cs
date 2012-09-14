using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class SpriteComponent : BaseComponent<SpriteComponent>, IEntityComponent
    {
        Texture2D _tex;
        Color _color;
        
        Spatial2DComponent _spatial2D;

        public SpriteComponent(string assetName) :base()
        {
            _color = Color.White;

            Asset asset =  MyGame.GetService<IAssetManager>().GetAsset(assetName);
            _tex = asset.GetAssetAs<Texture2D>();

            IntrusiveListItem<SpriteComponent>.AddToTail(this);
        }

        public string Name { get { return "Sprite"; } }

        public virtual void Draw(SpriteBatch sb)
        {            
            sb.Draw(_tex,                   //Texture to draw
                _spatial2D.Position,        //position
                null,                       //Source (no source means the whole rectangle)
                _color,                     //Color
                0,                          //Rotation
                Vector2.Zero,               //Origin of the sprite
                1,                          //Scale
                SpriteEffects.None,         //Effectes
                _spatial2D.Layer);          //Depth
        } 

        public override void OnOwnerChanged()
        {
            _spatial2D = (Spatial2DComponent) Owner.GetComponent("Spatial2D");
        }
                  
    }
}
