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
        public List<IUpdatableEntity> UpdatableList = new List<IUpdatableEntity>();
        public List<IDrawableEntity> DrawableList = new List<IDrawableEntity>();
        
        
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
            base.Update(gameTime);
            //Remove any entity scheduled to be removed
            RemoveCorpses();

            //Add any entity to lists
            AddNewborns();

            foreach (SceneEntity e in EntityList)
            {
                if (e is IUpdatableEntity)
                    ((IUpdatableEntity)e).Update(gameTime);
            }   
        }

        public override void Draw(GameTime gameTime)
        {            
            //Get camera information
            ICamera camera = MyGame.GetService<ICamera>();
            Matrix view = camera.ViewMatrix;
            Matrix proj = camera.ProjectionMatrix;
            //Signal the renderer
            IRenderer renderer = MyGame.GetService<IRenderer>();
            renderer.BeginSceneRendering();

            Game.GraphicsDevice.Clear(Color.Black);

            //Start drawing
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (IDrawableEntity e in DrawableList)
            {
                e.DrawEntity(view, proj, "whatever", Game.GraphicsDevice);
            }

            base.Draw(gameTime);

            //Signal end
            renderer.EndSceneRendering();
        }

        public void Init()
        {
            //Check for leftovers 
            RemoveCorpses();

            //Check if theres something else waiting to be initialized
            AddNewborns();

            //Check what i need to init.
            foreach (SceneEntity b in EntityList)
                b.Init();
        }

        private void RemoveCorpses()
        {
            foreach (SceneEntity e in EntityToBeRemovedList)
            {
                EntityList.Remove(e);
                if (e is IDrawableEntity)
                    DrawableList.Remove((IDrawableEntity)e);
                if (e is IUpdatableEntity)
                    UpdatableList.Remove((IUpdatableEntity)e);
            }
        }
        
        private void AddNewborns()
        {
            foreach (SceneEntity e in NewlyCreatedEntityList)
            {
                e.Init();
                EntityList.Add(e);
                if (e is IDrawableEntity)
                    DrawableList.Add((IDrawableEntity)e);
                if (e is IUpdatableEntity)
                    UpdatableList.Add((IUpdatableEntity)e);
            }
            NewlyCreatedEntityList.Clear();
        }
    }

}
