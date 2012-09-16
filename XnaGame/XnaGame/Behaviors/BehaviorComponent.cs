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