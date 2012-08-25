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
        List<SceneEntity> _entity_list = new List<SceneEntity>();
        Dictionary<int, SceneEntity> _entity_id_index = new Dictionary<int, SceneEntity>();
        Dictionary<string, SceneEntity> _entity_name_index = new Dictionary<string, SceneEntity>();

        List<IUpdatableEntity> _updatable_list = new List<IUpdatableEntity>();
        List<IDrawableEntity> _drawable_list = new List<IDrawableEntity>();

        List<SceneEntity> EntityToBeRemovedList = new List<SceneEntity>();
        List<SceneEntity> NewlyCreatedEntityList = new List<SceneEntity>();

        #endregion


        #region Constructors
        public ObjectManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IObjectManager), this);
        }
        #endregion

        #region IEntityManager Interface methods

        public void Add(SceneEntity ent) { NewlyCreatedEntityList.Add(ent); }
        public void Remove(SceneEntity ent) { EntityToBeRemovedList.Add(ent); }
        public void RemoveAll() { _entity_list.Clear(); }
        
        public SceneEntity GetEntity(int i)
        {
            SceneEntity val = null;
            _entity_id_index.TryGetValue(i, out val);
            return val;
        }

        public SceneEntity GetEntity(string name)
        {
            SceneEntity val = null;
            _entity_name_index.TryGetValue(name, out val);
            return val;
        }
        
        #endregion


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Remove any entity scheduled to be removed
            RemoveCorpses();

            //Add any entity to lists
            AddNewborns();

            foreach (SceneEntity e in _entity_list)
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

            GraphicsDevice.Clear(Color.Black);

            //Start drawing
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (IDrawableEntity e in _drawable_list)
            {
                e.DrawEntity(view, proj, "whatever", Game.GraphicsDevice);
            }

            base.Draw(gameTime);

            //Signal end
            renderer.EndSceneRendering();
        }

        private void RemoveCorpses()
        {
            foreach (SceneEntity e in EntityToBeRemovedList)
            {
                _entity_list.Remove(e);
                if (e is IDrawableEntity)
                    _drawable_list.Remove((IDrawableEntity)e);
                if (e is IUpdatableEntity)
                    _updatable_list.Remove((IUpdatableEntity)e);
                _entity_id_index.Remove(e.Id);
                _entity_name_index.Remove(e.Name);
            }
        }
        
        private void AddNewborns()
        {
            foreach (SceneEntity e in NewlyCreatedEntityList)
            {
                e.Init();
                _entity_list.Add(e);
                if (e is IDrawableEntity)
                    _drawable_list.Add((IDrawableEntity)e);
                if (e is IUpdatableEntity)
                    _updatable_list.Add((IUpdatableEntity)e);

                _entity_id_index.Add(e.Id, e);
                _entity_name_index.Add(e.Name, e);
            }
            NewlyCreatedEntityList.Clear();
        }
    }

}
