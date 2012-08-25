using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace XnaGame
{
    //public delegate void AbyssEventHandler(Object o, EventArgs args);

    public interface IBehavior
    {
        bool IsComplete();
        void Update(GameTime gametime);
        void FinishBehavior();
    }

    public class BehaviorBase : IBehavior
    {

        public BehaviorBase(SceneEntity b)
        {           
            b.AddBehavior(this);
            RegisterBehaviorEvents(b);
        }

        /// <summary>
        /// Here we should register all events produced by this event
        /// </summary>
        protected virtual void RegisterBehaviorEvents(SceneEntity b) { }

        /// <summary>
        /// Any time based update should be placed here.
        /// </summary>
        /// <param name="gametime"></param>
        public virtual void Update(GameTime gametime) { }

        /// <summary>
        /// The behavior reached its objective.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsComplete() { return false; }

        public virtual void FinishBehavior() { }

    }

    
}