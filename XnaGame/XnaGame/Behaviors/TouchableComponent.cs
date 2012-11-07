using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class TouchableComponent : Component<TouchableComponent>, IEntityComponent
    {
        SpatialComponent _spatial;
        BoundingSphere _volume;

        public int TouchMask; //What type of things this collision should check
        public int TouchType; //What type this is
        public bool _active;

        public TouchableComponent(float radius)
            : base()
        {            
            _volume = new BoundingSphere(Vector3.Zero, radius);           
            _active = true;
        }

        public string Name { get { return "Touchable"; } }

        //public override void Update(GameTime gametime)
        //{
        //    _volume.Center = _spatial.Position;
        //    if (!_active || TouchMask == 0) //if i shouldn't check collisions
        //        return;
        //    foreach (BehaviorTouchable toucher in TouchableItems)
        //    {
        //        if (toucher == this || !toucher.bActive)
        //            continue;
        //        if ((TouchMask & toucher.TouchType) == 0) //if the other is of the type i care about
        //            continue;
        //        if (Volume.Intersects(toucher.Volume))
        //        {
        //            //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Touch, new InteractionEventArgs(toucher.pOwner));
        //            //AbyssEventDispatcher.Get().RaiseGlobalEvent(AbyssEventType.EVT_Touch, pOwner);
        //        }
        //    }
        //}

        override public void OnOwnerChanged()
        {
            _spatial = (SpatialComponent)Owner.GetComponent("Spatial");
        }  

    }
}
