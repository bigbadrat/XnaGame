
#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion 


namespace XnaGame
{
    /// <summary>
    /// Stub of the very basic entity type. Something that actually doesn't hold anything
    /// visible and only has a name. Note that here we don't really care for a world
    /// location since we could be talking of just logical elements.
    /// Also BaseEntity provides the support for the event structure.
    /// </summary>
    public class SceneEntity
    {
        public string Name { get; set; }

        public static int IdCounter;
        public int Id;
        protected IObjectManager EntityManager;

        public SceneEntity() 
        { 
            RegisterWithManager(); 
            Name = "unnamed"+Id; 
        }

        public SceneEntity(string entname) 
        { 
            Name = entname; 
            RegisterWithManager(); 
        }

        private void RegisterWithManager()
        {
            IObjectManager man = Game1.GetService<IObjectManager>();
            man.Add(this);
            EntityManager = man;
            Id = IdCounter++; //get a unique id for this object
            //RegisterEvent(AbyssEventType.EVT_Signal);
        }

        /// <summary>
        /// Called from the manager when all entities have been created, so
        /// we can acquire references to other entities safely. Particularly useful
        /// to acquire references to assets. You have to call it by hand if 
        /// the entity is created after level creation.
        /// </summary>
        virtual public void Init() { }

        /// <summary>
        /// Called from the manager when the entity is about to be removed
        /// here we should remove all objects created by the entity.
        /// </summary>
        virtual public void Destroy() { }

        #region Behavior functions
        /// <summary>
        /// Behavior event management
        /// </summary>
        //List<IBehavior> behaviors = new List<IBehavior>();

        //public void AddBehavior(IBehavior behav)
        //{
        //    behaviors.Add(behav);
        //}
        
        //public virtual void RemoveAllBehaviors()
        //{
        //    behaviors.Clear();
        //}
        #endregion

        //#region EventHandling functions

        //List<AbyssEventDescription> EventList = new List<AbyssEventDescription>();

        //AbyssEventDescription FindEventDescription(AbyssEventType type)
        //{
        //    return EventList.Find(
        //        delegate(AbyssEventDescription evtDesc)
        //        {
        //            return (evtDesc.Type == type);
        //        }

        //    );
        //}

        //public bool HasEvent(AbyssEventType type)
        //{
        //    return FindEventDescription(type) != null;
        //}

        //public bool HasEvent(string name)
        //{
        //    AbyssEventType evt = AbyssEventDispatcher.Get().GetEventTypeByName(name);
        //    return HasEvent(evt);
        //}

        //public void RegisterEvent(AbyssEventType type)
        //{
        //    if (HasEvent(type))
        //        return;
        //    AbyssEventDescription descrip = new AbyssEventDescription(type);
        //    EventList.Add(descrip);
        //}

        //public void RegisterEvent(string type)
        //{
        //    AbyssEventType evt = AbyssEventDispatcher.Get().GetEventTypeByName(type);
        //    RegisterEvent(evt);
        //}
        
        //public void RaiseLocalEvent(AbyssEventType type, EventArgs args)
        //{
        //    AbyssEventDescription desc = FindEventDescription(type);
        //    if (desc == null)
        //        return;
        //    desc.RaiseEvent(this, args);            
        //}

        //public void RaiseLocalEvent(string type, EventArgs args)
        //{
        //    AbyssEventType evt = AbyssEventDispatcher.Get().GetEventTypeByName(type);
        //    RaiseLocalEvent(evt,args);
        //}

        //public void SuscribeToEvent(AbyssEventType type, AbyssEventHandler handler)
        //{
        //    AbyssEventDescription desc = FindEventDescription(type);
        //    if (desc == null)
        //        return;
        //    desc.eventHandler += handler;
        //}

        //public void UnsuscribeToEvent(AbyssEventType type, AbyssEventHandler handler)
        //{
        //    AbyssEventDescription desc = FindEventDescription(type);
        //    if (desc == null)
        //        return;
        //    desc.eventHandler -= handler;
        //}

        //#endregion

        //#region Utility functions
        //public void SetDelayedCall(CBehaviorDelayedCall.DelayedFunction f, float t, bool loop)
        //{
        //    new CBehaviorDelayedCall(this, t, loop, f);
        //}

        //#endregion
        /// <summary>
        /// Basic Update for all the entities. Basically handle behaviors
        /// </summary>
        /// <param name="gameTime"></param>
        //public virtual void Update(GameTime gameTime)
        //{
            //Update each behavior
            //foreach (IBehavior beh in behaviors)
            //{
            //    beh.Update(gameTime);
            //}

            ////Remove completed behavior
            //for (int i = 0; i < behaviors.Count(); )
            //{
            //    if (behaviors[i].IsComplete())
            //    {
            //        behaviors.RemoveAt(i);                
            //    }
            //    else
            //    {
            //        ++i;
            //    }
            //}

        //}

    }

    public class UpdatableEntity : SceneEntity
    {
        public virtual void Update(GameTime gameTime)
        {
        }
    }
    /// <summary>
    /// Basic features for an entity to have a position in space.
    /// </summary>    
    public class SpatialEntity : SceneEntity, ISpatialEntity
    {
        /// <summary>
        /// Position of the entity. Note that when setting the position, the 
        /// world matrix will be auto updated when requested.
        /// </summary>
        public Vector3 Position
        {
            get { return pos; }
            set
            {
                if (pos == value) return; //Only move if really moved
                pos = value;
                _matrixIsDirty = true;                 
            }
        }
        Vector3 pos = new Vector3(0, 0, 0);

        /// <summary>
        /// Rotation of the entity in Euler angles. Note that when setting the
        /// rotation, the world matrix will be auto updated when requested.
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; _matrixIsDirty = true; }
        }
        Vector3 rotation = new Vector3(0, 0, 0);

        public Vector3 Scale
        { 
            get { return scale;}
            set { scale = value; _matrixIsDirty = true;}
        }
        Vector3 scale = Vector3.One;

        /// <summary>
        /// World matrix used to correctly position the entity in the world. Note
        /// that if the matrix is outdated, it will be recalculated before being
        /// returned
        /// </summary>
        public Matrix WorldMatrix
        {
            get { if (_matrixIsDirty) UpdateWorldMatrix(); return worldMatrix; }
        }
        Matrix worldMatrix;

        //flag to mark the need to update the matrix. Initialized 
        private bool _matrixIsDirty = true;

        public Vector3 Direction{ get; set; }
        
        /// <summary>
        /// Method to update the world matrix. This should be auto-called when needed.
        /// </summary>
        private void UpdateWorldMatrix()
        {
            worldMatrix = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.X,Rotation.Y,Rotation.Z) * Matrix.CreateTranslation(Position); 
            _matrixIsDirty = false; 
        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="name"> Name of the entity</param>
        /// <param name="pos"> Position of the entity</param>
        public SpatialEntity(string name, Vector3 pos)
            : base(name)
        {
            Position = pos;            
        }

        public SpatialEntity(): base() { }

    }


}
 