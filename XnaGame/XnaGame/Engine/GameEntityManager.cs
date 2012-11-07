using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XnaGame
{

  
    public class GameEntityManager: DrawableGameComponent, IObjectManager
    {
        #region Fields
        /// <summary>
        /// Lists to contain the entities and to quickly access them. They can be quick-searched
        /// by id or by name using the specific dictionaries.
        /// Also, separated lists are used to keep the entities that will enter the game loop
        /// or will exit. That way is safer to iterate the events and safely "remove" any 
        /// entity without breaking the iterators.
        /// </summary>
        List<GameEntity> _entity_list = new List<GameEntity>();
        Dictionary<int, GameEntity> _entity_id_index = new Dictionary<int, GameEntity>();
        Dictionary<string, GameEntity> _entity_name_index = new Dictionary<string, GameEntity>();

        List<GameEntity> EntityToBeRemovedList = new List<GameEntity>();
        List<GameEntity> NewlyCreatedEntityList = new List<GameEntity>();

        #endregion


        #region Constructors
        public GameEntityManager(Game game)
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

            BehaviorComponent bc;
            bc = IntrusiveListItem<BehaviorComponent>.Head();
            for (; bc != null; bc = (BehaviorComponent)bc.Next)
            {
                bc.Update(gameTime);
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

            StaticModelComponent x;
            x = IntrusiveListItem<StaticModelComponent>.Head();
            while (x != null)
            {
                x.Draw(view, proj, "whatever", Game.GraphicsDevice);
                x = (StaticModelComponent) x.Next;                
            }

            base.Draw(gameTime);

            //Signal end
            renderer.EndSceneRendering();
        }

        private void RemoveCorpses()
        {
            foreach (GameEntity e in EntityToBeRemovedList)
            {
                _entity_id_index.Remove(e.Id);
                _entity_name_index.Remove(e.Name);
                _entity_list.Remove(e);
            }
        }
        
        private void AddNewborns()
        {
            foreach (GameEntity e in NewlyCreatedEntityList)
            {
                e.Init();
                _entity_list.Add(e);
                _entity_id_index.Add(e.Id, e);
                _entity_name_index.Add(e.Name, e);
            }
            NewlyCreatedEntityList.Clear();
        }

        public void BroadcastMessage(Message msg)
        {
            foreach (GameEntity e in NewlyCreatedEntityList)
            {
                e.ReceiveMessage(msg);
            }
        }
    }

}
