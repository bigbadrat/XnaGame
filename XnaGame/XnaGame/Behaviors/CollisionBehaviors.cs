using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class BehaviorTouchable : BehaviorBase
    {
        public SpatialEntity pOwner;
        public BoundingSphere Volume;
        public int TouchMask; //What type of things this collision should check
        public int TouchType; //What type this is
        public bool bActive;

        static List<BehaviorTouchable> TouchableItems = new List<BehaviorTouchable>();

        public BehaviorTouchable(SpatialEntity o, float radius)
            : base(o)
        {
            pOwner = o;
            Volume = new BoundingSphere(o.Position, radius);
            TouchableItems.Add(this);
            bActive = true;
        }

        protected override void RegisterBehaviorEvents(GameEntity b)
        {
            //b.RegisterEvent(AbyssEventType.EVT_Touch);
        }

        public override void Update(GameTime gametime)
        {
            Volume.Center = pOwner.Position;
            if (!bActive || TouchMask == 0) //if i shouldn't check collisions
                return;
            foreach (BehaviorTouchable toucher in TouchableItems)
            {
                if (toucher == this || !toucher.bActive)
                    continue;
                if ((TouchMask & toucher.TouchType) == 0) //if the other is of the type i care about
                    continue;
                if (Volume.Intersects(toucher.Volume))
                {
                    //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Touch, new InteractionEventArgs(toucher.pOwner));
                    //AbyssEventDispatcher.Get().RaiseGlobalEvent(AbyssEventType.EVT_Touch, pOwner);
                }
            }
        }

        public override void FinishBehavior()
        {
            TouchableItems.Remove(this);
        }
    }
}
