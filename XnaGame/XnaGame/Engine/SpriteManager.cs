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
        List<SpriteBasic> _sprites;
        Dictionary<string, SpriteBasic> _sprites_name_index;
        Dictionary<int, SpriteBasic> _sprites_id_index;
        int _current_id;

        public SpriteManager(Game game) :
            base(game)
        {
            _sprites = new List<SpriteBasic>();
            _sprites_name_index = new Dictionary<string, SpriteBasic>();
            _sprites_id_index = new Dictionary<int, SpriteBasic>();
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
            foreach (SpriteBasic s in _sprites)
            {
                if (s is SpriteGridSheet)
                    ((SpriteGridSheet)s).Update(gameTime);
                else if (s is SpriteSheet)
                    ((SpriteSheet)s).Update(gameTime);
            }

            SpriteGridSheetComponent sgc = IntrusiveListItem<SpriteGridSheetComponent>.Head();
            while (sgc != null)
            {
                System.Console.WriteLine("drawing a sheet sprite");
                sgc.Update(gameTime);
                sgc = (SpriteGridSheetComponent)sgc.Next;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            IRenderer render = MyGame.GetService<IRenderer>();
            render.BeginSpriteRendering();
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            _spritebatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (SpriteBasic s in _sprites)
            {
                s.Draw(_spritebatch);
            }

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
            _spritebatch.End();
            render.EndSpriteRendering();
        }

        public void AddSprite(SpriteBasic sp)
        {
            _sprites.Add(sp);
            sp.Id = _current_id;
            ++_current_id;

            _sprites_id_index.Add(sp.Id, sp);
            _sprites_name_index.Add(sp.Name, sp);

        }

        public SpriteBasic GetSprite(int i)
        {
            SpriteBasic val = null;
            _sprites_id_index.TryGetValue(i, out val);;
            return val;
        }

        public SpriteBasic GetSprite(string n)
        {
            SpriteBasic val = null;
            _sprites_name_index.TryGetValue(n, out val); ;
            return val;
        }        
    }
}
