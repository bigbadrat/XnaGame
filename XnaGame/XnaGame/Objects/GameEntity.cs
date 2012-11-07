
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
    public class GameEntity : IGameEntity
    {
        
        string _name;
        public static int IdCounter;
        public int Id;

        public GameEntity() 
        { 
            RegisterWithManager(); 
            _name = "entity_"+Id; 
        }

        public GameEntity(string entname) 
        {
            RegisterWithManager(); 
            _name = entname;             
        }

        public string Name
        {
            get { return _name; }
        }

        protected List<IEntityComponent> _components = new List<IEntityComponent>();

        public IEntityComponent GetComponent(string name)
        {
            foreach( IEntityComponent comp in _components)
            {
                if (comp.Name == name)
                    return comp;
            }
            return null;
        }

        public void AddComponent(IEntityComponent comp)
        {
            comp.Owner = this;
            _components.Add(comp);
        }

        private void RegisterWithManager()
        {
            IObjectManager man = MyGame.GetService<IObjectManager>();
            man.Add(this);
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




        public void ReceiveMessage(Message msg)
        {
            foreach( IEntityComponent ec in _components)
            {
                ec.Process(msg);
            }
        }
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


    }

}
 