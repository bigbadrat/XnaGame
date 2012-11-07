using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace XnaGame
{
    public interface IBehavior
    {
        bool IsComplete();
        void Update(GameTime gametime);
        void AttachTo(IGameEntity entity);
    }

    /// <summary>
    /// The BehaviorComponent is a component thought to be composed
    /// of some other smaller objects that model a specific behavior
    /// for the owner of this component. There's a category of behaviors
    /// based on movement (and such, they require a SpatialComponent), and 
    /// other's to model timers.
    /// </summary>
    public class BehaviorComponent : BaseComponent<BehaviorComponent>, IEntityComponent
    {
        List<IBehavior> _behaviors ;

        public BehaviorComponent():
            base()
        {           
            _behaviors = new List<IBehavior>();
            IntrusiveListItem<BehaviorComponent>.AddToTail(this);
        }

        public string Name { get { return "Behavior"; } }

        /// <summary>
        /// Any time based update should be placed here.
        /// </summary>
        /// <param name="gametime"></param>
        public virtual void Update(GameTime gametime)
        {
            foreach (IBehavior bh in _behaviors)
            {
                bh.Update(gametime);
            }
        }

        #region Behavior functions
        /// <summary>
        /// Behavior event management
        /// </summary>
        
        public void AddBehavior(IBehavior behav)
        {
            _behaviors.Add(behav);
            behav.AttachTo(Owner);
        }

        public virtual void RemoveAllBehaviors()
        {
            _behaviors.Clear();
        }
        #endregion
    }

    
}