using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    /// <summary>
    /// Base class for behaviors that are dependant of the owner
    /// having a spatial component (that is, they have a location 
    /// in space)
    /// </summary>
    public class SpatialBehavior : IBehavior
    {
        protected SpatialComponent _spatial;
        protected bool _completed;

        public SpatialBehavior()
        {
            _completed = false;
        }

        public void AttachTo(IGameEntity entity)
        {
            _spatial = (SpatialComponent)entity.GetComponent("Spatial");
            OnAttach();
        }

        public bool IsComplete() { return _completed; }

        virtual public void Update(GameTime gameTime) { }

        virtual public void OnAttach()  {  }
    }

    /// <summary>
    /// Behavior to move an entity through space with a constant velocity.
    /// Never completes.
    /// </summary>
    public class ConstantLinearMoveBehavior: SpatialBehavior
    {        
        public float Speed;
        public Vector3 Direction;

        public ConstantLinearMoveBehavior(Vector3 v)
            : base()
        {            
            Speed = v.Length();
            v.Normalize();
            Direction = v;
        }

        public override void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            Vector3 newpos = (float)gametime.ElapsedGameTime.TotalSeconds * Speed * Direction + _spatial.Position;
            _spatial.Position = newpos;
        }
    }

    /// <summary>
    /// Behavior to move an entity through space using kind of inertia.
    /// All time there's some negative accel trying to move you back and make 
    /// you still .
    /// </summary>
    public class InertialMoveBehavior : SpatialBehavior
    {
        public float Speed;
        public float MaxSpeed;
        public float Accel;
        public Vector3 Direction;

        public InertialMoveBehavior(Vector3 v, float a, float mv)
            : base()
        {
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
                Vector3 newpos = delta * Speed * Direction + _spatial.Position;
                _spatial.Position = newpos;
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
    public class LinearMoveToBehavior : SpatialBehavior
    {
        float elapsed;
        float timeTarget;
        Vector3 origPosition;
        Vector3 targetPosition;

        public LinearMoveToBehavior(Vector3 t, float time)
            : base()
        {
            elapsed = 0;
            targetPosition = t;
            timeTarget = time;
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
            _spatial.Position = newpos;
            if (newpos == targetPosition)
            {
                //pOwner.RaiseLocalEvent(AbyssEventType.EVT_GoalReached, new EventArgs());
                _completed = true;
            }
        }

        public override void OnAttach()
        {
            origPosition = _spatial.Position;
        }

    }

    
    /// <summary>
    /// Very simple behavior to make something rotate on its own Z axis
    /// at a given angular speed.
    /// </summary>
    public class RotateBehavior : SpatialBehavior
    {
        public float RotSpeed;

        public RotateBehavior(float rotSpeed)
            : base()
        {
            RotSpeed = MathHelper.ToRadians(rotSpeed);
        }

        public override void Update(GameTime gametime)
        {
            Vector3 newrot = _spatial.Rotation + new Vector3(RotSpeed * (float)gametime.ElapsedGameTime.TotalSeconds, 0, 0);
            if (newrot.X > 360)
                newrot.X -= 360;
            else if (newrot.X < -360)
                newrot.X += 360;
            _spatial.Rotation = newrot;
        }

    }

    /// <summary>
    /// Behavior to make something rotate around a point
    /// at a given angular speed using a fixed radius.
    /// TODO: FIX THIS!!! THIS IS BROKEN!
    /// </summary>
    public class RotateAroundBehavior : SpatialBehavior
    {
        public float RotSpeed;
        public float RotRadius;

        public RotateAroundBehavior(float rotSpeed, float rotRadius)
            : base()
        {
            RotSpeed = MathHelper.ToRadians(rotSpeed);
            RotRadius = rotRadius;
        }

        public override void Update(GameTime gametime)
        {
            Vector3 newrot = _spatial.Rotation + new Vector3(RotSpeed * (float)gametime.ElapsedGameTime.TotalSeconds, 0, 0);
            if (newrot.X > 360)
                newrot.X -= 360;
            else if (newrot.X < -360)
                newrot.X += 360;
            //pOwner.Rotation = newrot;
        }

    }

    public class TimedGrowComponent : SpatialBehavior
    {
        float m_fTargetBigScale;
        float m_fTargetSmallScale;
        float m_fCurrentScale;
        float m_fTargetFreq;
        float m_fCurrentTime;
        bool m_bFlag;

        public TimedGrowComponent(float targetSmallScale, float targetBigScale, float targetFreq)
            : base()
        {
            m_fCurrentTime = 0.0f;
            m_fTargetFreq = targetFreq;
            m_fCurrentScale = 1.0f;
            m_fTargetSmallScale = targetSmallScale;
            m_fTargetBigScale = targetBigScale;
            m_bFlag = true;
        }

        public override void Update(GameTime gametime)
        {
            m_fCurrentTime += (float)gametime.ElapsedGameTime.TotalSeconds;
            float delta = m_fCurrentTime / m_fTargetFreq;

            if (m_bFlag)
                m_fCurrentScale = MathHelper.Lerp(m_fTargetSmallScale, m_fTargetBigScale, delta);
            else
                m_fCurrentScale = MathHelper.Lerp(m_fTargetBigScale, m_fTargetSmallScale, delta);

            _spatial.Scale = new Vector3(m_fCurrentScale);

            if (m_fCurrentTime >= m_fTargetFreq)
            {
                //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Signal, new EventArgs());
                m_fCurrentTime -= m_fTargetFreq;
                m_bFlag = !m_bFlag;
            }
        }

    }
}
