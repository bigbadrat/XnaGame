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
        List<GameEntity> _entity_list = new List<GameEntity>();
        Dictionary<int, GameEntity> _entity_id_index = new Dictionary<int, GameEntity>();
        Dictionary<string, GameEntity> _entity_name_index = new Dictionary<string, GameEntity>();

        List<IUpdatableEntity> _updatable_list = new List<IUpdatableEntity>();
        List<IDrawableEntity> _drawable_list = new List<IDrawableEntity>();

        List<GameEntity> EntityToBeRemovedList = new List<GameEntity>();
        List<GameEntity> NewlyCreatedEntityList = new List<GameEntity>();

        #endregion


        #region Constructors
        public ObjectManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IObjectManager), this);
        }
        #endregion

        #region IEntityManager Interface methods

        public void Add(GameEntity ent)
        {
            NewlyCreatedEntityList.Add(ent);
        }

        public void Remove(GameEntity ent)
        {
            EntityToBeRemovedList.Add(ent);
        }

        public void RemoveAll()
        {
            _entity_list.Clear();
        }
        
        public GameEntity GetEntity(int i)
        {
            GameEntity val = null;
            if ( _entity_id_index.TryGetValue(i, out val) )
                return val;
            return null;
        }

        public GameEntity GetEntity(string name)
        {
            GameEntity val = null;
            if ( _entity_name_index.TryGetValue(name, out val) )
                return val;
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

            foreach (IUpdatableEntity e in _updatable_list)
            {
                e.Update(gameTime);
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
            foreach (GameEntity e in EntityToBeRemovedList)
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
            foreach (GameEntity e in NewlyCreatedEntityList)
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
