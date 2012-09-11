using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    /// <summary>
    /// Behavior to move an entity through space with a constant velocity.
    /// Never completes.
    /// </summary>
    public class BehaviorConstantLinearMove : BehaviorBase
    {
        public SpatialEntity pOwner;
        public float Speed;
        public Vector3 Direction;

        public BehaviorConstantLinearMove(SpatialEntity o, Vector3 v)
            : base(o)
        {
            pOwner = o;
            Speed = v.Length();
            v.Normalize();
            Direction = v;
        }

        public override void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            Vector3 newpos = (float)gametime.ElapsedGameTime.TotalSeconds * Speed * Direction + pOwner.Position;
            pOwner.Position = newpos;
        }

    }

    /// <summary>
    /// Behavior to move an entity through space using kind of inertia.
    /// All time there's some negative accel trying to move you back and make 
    /// you still .
    /// </summary>
    public class BehaviorInertialMove : BehaviorBase
    {
        public SpatialEntity pOwner;
        public float Speed;
        public float MaxSpeed;
        public float Accel;
        public Vector3 Direction;

        public BehaviorInertialMove(SpatialEntity o, Vector3 v, float a, float mv)
            : base(o)
        {
            pOwner = o;
            MaxSpeed = mv;
            Accel = a;

            if (v.LengthSquared() == 0)
            {
                Speed = 0;
                Direction = new Vector3();
            }
            else
            {
                Speed = v.Length();
                v.Normalize();
                Direction = v;
            }
        }

        public override void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float delta = (float)gametime.ElapsedGameTime.TotalSeconds;
            Speed -= Accel * delta;
            if (Speed <= 0)
            {
                Speed = 0;
                Direction = new Vector3();
            }
            else
            {
                Speed = MathHelper.Clamp(Speed, 0, MaxSpeed);
                Vector3 newpos = delta * Speed * Direction + pOwner.Position;
                pOwner.Position = newpos;
            }
        }

        public void AddVelocity(Vector3 vel)
        {
            //Calculate the new vel if we add the desired vel change.
            Vector3 newvel = Speed * Direction + vel;
            //Get new speed and clamp if needed
            Speed = MathHelper.Clamp(newvel.Length(), 0, MaxSpeed);
            //Normalize to get the direction.
            newvel.Normalize();
            Direction = newvel;
        }

    }
    /// <summary>
    /// Behavior that moves an entity at a linear speed until it reaches an
    /// objective. When the objective is reached, an event is raised.
    /// </summary>
    public class BehaviorLinearMoveTo : BehaviorBase
    {
        public SpatialEntity pOwner;

        float elapsed;
        float timeTarget;
        Vector3 origPosition;
        Vector3 targetPosition;
        bool arrived;

        public BehaviorLinearMoveTo(SpatialEntity o, Vector3 t, float time)
            : base(o)
        {
            pOwner = o;
            elapsed = 0;
            origPosition = pOwner.Position;
            targetPosition = t;
            timeTarget = time;
            arrived = false;
        }

        protected override void RegisterBehaviorEvents(GameEntity b)
        {
            //b.RegisterEvent(AbyssEventType.EVT_GoalReached);
        }

        public override void Update(GameTime gametime)
        {
            if (IsComplete())
                return;
            Vector3 newpos = Vector3.Zero;
            elapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (elapsed >= timeTarget)
                elapsed = timeTarget;
            float delta = elapsed / timeTarget;
            newpos.X = MathHelper.Lerp(origPosition.X, targetPosition.X, delta);
            newpos.Y = MathHelper.Lerp(origPosition.Y, targetPosition.Y, delta);
            newpos.Z = MathHelper.Lerp(origPosition.Z, targetPosition.Z, delta);
            pOwner.Position = newpos;
            if (newpos == targetPosition)
            {
                //pOwner.RaiseLocalEvent(AbyssEventType.EVT_GoalReached, new EventArgs());
                arrived = true;
            }
        }

        public override bool IsComplete() { return arrived; }
    }

    
    /// <summary>
    /// Very simple behavior to make something rotate on its own Z axis
    /// at a given angular speed.
    /// </summary>
    public class BehaviorRotate : BehaviorBase
    {
        public SpatialEntity pOwner;
        public float RotSpeed;

        public BehaviorRotate(SpatialEntity o, float rotSpeed)
            : base(o)
        {
            pOwner = o;
            RotSpeed = MathHelper.ToRadians(rotSpeed);
        }

        public override void Update(GameTime gametime)
        {
            Vector3 newrot = pOwner.Rotation + new Vector3(RotSpeed * (float)gametime.ElapsedGameTime.TotalSeconds, 0, 0);
            if (newrot.X > 360)
                newrot.X -= 360;
            else if (newrot.X < -360)
                newrot.X += 360;
            pOwner.Rotation = newrot;
        }

    }

    /// <summary>
    /// Behavior to make something rotate around a point
    /// at a given angular speed using a fixed radius.
    /// </summary>
    public class BehaviorRotateAround : BehaviorBase
    {
        public SpatialEntity pOwner;
        public float RotSpeed;
        public float RotRadius;

        public BehaviorRotateAround(SpatialEntity o, float rotSpeed, float rotRadius)
            : base(o)
        {
            pOwner = o;
            RotSpeed = MathHelper.ToRadians(rotSpeed);
            RotRadius = rotRadius;
        }

        public override void Update(GameTime gametime)
        {
            Vector3 newrot = pOwner.Rotation + new Vector3(RotSpeed * (float)gametime.ElapsedGameTime.TotalSeconds, 0, 0);
            if (newrot.X > 360)
                newrot.X -= 360;
            else if (newrot.X < -360)
                newrot.X += 360;
            //pOwner.Rotation = newrot;
        }

    }

    public class CBehaviorTimedGrow : BehaviorBase
    {
        public SpatialEntity pOwner;

        float m_fTargetBigScale;
        float m_fTargetSmallScale;
        float m_fCurrentScale;
        float m_fTargetFreq;
        float m_fCurrentTime;
        bool m_bFlag;

        public CBehaviorTimedGrow(SpatialEntity o, float targetSmallScale, float targetBigScale, float targetFreq)
            : base(o)
        {
            m_fCurrentTime = 0.0f;
            m_fTargetFreq = targetFreq;
            m_fCurrentScale = 1.0f;
            m_fTargetSmallScale = targetSmallScale;
            m_fTargetBigScale = targetBigScale;
            pOwner = o;
            m_bFlag = true;
        }

        protected override void RegisterBehaviorEvents(GameEntity b)
        {
            //b.RegisterEvent(AbyssEventType.EVT_Signal);
        }

        public override void Update(GameTime gametime)
        {
            m_fCurrentTime += (float)gametime.ElapsedGameTime.TotalSeconds;
            float delta = m_fCurrentTime / m_fTargetFreq;

            if (m_bFlag)
                m_fCurrentScale = MathHelper.Lerp(m_fTargetSmallScale, m_fTargetBigScale, delta);
            else
                m_fCurrentScale = MathHelper.Lerp(m_fTargetBigScale, m_fTargetSmallScale, delta);

            pOwner.Scale = new Vector3(m_fCurrentScale);

            if (m_fCurrentTime >= m_fTargetFreq)
            {
                //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Signal, new EventArgs());
                m_fCurrentTime -= m_fTargetFreq;
                m_bFlag = !m_bFlag;
            }
        }

    }
}
