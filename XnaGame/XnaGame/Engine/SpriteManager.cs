using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    public class SpriteManager: DrawableGameComponent, ISpriteManager
    {
        SpriteBatch _spritebatch;
        int _current_id;

        public SpriteManager(Game game) :
            base(game)
        {
            game.Services.AddService(typeof(ISpriteManager), this);
            _current_id = 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            _spritebatch = new SpriteBatch(Game.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SpriteGridSheetComponent sgc = IntrusiveListItem<SpriteGridSheetComponent>.Head();
            while (sgc != null)
            {
                sgc.Update(gameTime);
                sgc = (SpriteGridSheetComponent)sgc.Next;
            }

            SpriteSheetComponent ssc = IntrusiveListItem<SpriteSheetComponent>.Head();
            while (ssc != null)
            {
                ssc.Update(gameTime);
                ssc = (SpriteSheetComponent)ssc.Next;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            IRenderer render = MyGame.GetService<IRenderer>();
            render.BeginSpriteRendering();
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            _spritebatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            SpriteComponent sc = IntrusiveListItem<SpriteComponent>.Head();
            while (sc != null)
            {
                sc.Draw(_spritebatch);
                sc = (SpriteComponent) sc.Next;
            }

            SpriteGridSheetComponent sgc = IntrusiveListItem<SpriteGridSheetComponent>.Head();
            while (sgc != null)
            {
                sgc.Draw(_spritebatch);
                sgc = (SpriteGridSheetComponent)sgc.Next;
            }

            SpriteSheetComponent ssc = IntrusiveListItem<SpriteSheetComponent>.Head();
            while (ssc != null)
            {
                ssc.Draw(_spritebatch);
                ssc = (SpriteSheetComponent)ssc.Next;
            }
            _spritebatch.End();
            render.EndSpriteRendering();
        }
  
    }
}
