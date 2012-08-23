using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XnaGame
{

  
    public class ObjectManager: DrawableGameComponent, IObjectManager
    {
        #region Fields
        public List<SceneEntity> EntityList = new List<SceneEntity>();
        
        
        #endregion

        List<SceneEntity> EntityToBeRemovedList = new List<SceneEntity>();
        List<SceneEntity> NewlyCreatedEntityList = new List<SceneEntity>();

        #region Constructors
        public ObjectManager(Game game)
            : base(game)
        {
            EntityList.Clear();
            game.Services.AddService(typeof(IObjectManager), this);
        }
        #endregion

        #region IEntityManager Interface methods

        public void Add(SceneEntity ent) { NewlyCreatedEntityList.Add(ent); }
        public void Remove(SceneEntity ent) { EntityToBeRemovedList.Add(ent); }
        public void RemoveAll() { EntityList.Clear(); }
        public SceneEntity GetEntity(int i) { return EntityList[i]; }
        public SceneEntity GetEntity(string name)
        {
            foreach (SceneEntity e in EntityList)
            {
                if (e.Name == name)
                    return e;
            }
            return null;
        }
        
        #endregion


        public override void Update(GameTime gameTime)
        {
            foreach (SceneEntity e in EntityList)
            {
                if (e is IUpdatableEntity)
                    ((IUpdatableEntity)e).Update(gameTime);
            }
            foreach (SceneEntity e in EntityToBeRemovedList)
            {
                EntityList.Remove(e);
            }
            foreach (SceneEntity e in NewlyCreatedEntityList)
            {
                EntityList.Add(e);
            }
            NewlyCreatedEntityList.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {            
            //Get camera information
            ICamera camera = Game1.GetService<ICamera>();
            Matrix view = camera.ViewMatrix;
            Matrix proj = camera.ProjectionMatrix;
            //Signal the renderer
            IRenderer renderer = Game1.GetService<IRenderer>();
            renderer.BeginSceneRendering();

            //Start drawing
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (SceneEntity e in EntityList)
            {
                if (e is IDrawableEntity )
                {
                    IDrawableEntity m = (IDrawableEntity)e;
                    m.DrawEntity(view, proj, "whatever", Game.GraphicsDevice);
                }
            }

            base.Draw(gameTime);

            //Signal end
            renderer.EndSceneRendering();
        }

        public void Init()
        {
            foreach (SceneEntity b in EntityList)
                b.Init();
        }
    }

}
