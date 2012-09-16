using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace XnaGame
{
    /// <summary>
    /// Behavior to move an entity from point A to B and go back with a constant velocity.
    /// Never completes.
    /// </summary>
    public class LinearOscillateBehavior : SpatialBehavior
    {
        public float Speed;
        public Vector3 A, B;
        protected float _alfa;
        protected bool _leftToRight;

        public LinearOscillateBehavior(float s, Vector3 PointA, Vector3 PointB)
            : base()
        {           
            Speed = s;
            A = PointA;
            B = PointB;
            _alfa = 0;
            _leftToRight = true;
        }

        override public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
        }

        protected void AddWeight(float elapsedTime)
        {
            if (_leftToRight)
                _alfa += elapsedTime * Speed;
            else
                _alfa -= elapsedTime * Speed;

            if (_leftToRight && _alfa >= 1)
            {
                _leftToRight = false;
                _alfa = 1;
            }
            else if (!_leftToRight && _alfa <= 0)
            {
                _leftToRight = true;
                _alfa = 0;
            }
        }

        virtual protected Vector3 GetInterpolatedValue()
        {
            return Vector3.Lerp(A, B, _alfa);
        }

    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Catmull-Rom interpolation. Never completes.
    /// </summary>
    public class CatmullOscillateBehavior: LinearOscillateBehavior
    {
        public Vector3 C, D;

        public CatmullOscillateBehavior(float s, Vector3 PointA, Vector3 PointB, Vector3 PointC, Vector3 PointD)
            : base(s, PointA, PointB)
        {           
            C = PointC;
            D = PointD;
        }

        override public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
        }

        override protected Vector3 GetInterpolatedValue()
        {
            return Vector3.CatmullRom(A, B, C, D, _alfa);
        }

    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Smoothie interpolation. Never completes.
    /// </summary>
    public class SmoothOscillateBehavior : LinearOscillateBehavior
    {

        public SmoothOscillateBehavior(float s, Vector3 PointA, Vector3 PointB)
            : base(s, PointA, PointB)
        {
        }

        override public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
        }
        
        override protected Vector3 GetInterpolatedValue()
        {
            return Vector3.SmoothStep(A, B, _alfa);
        }

    }
}
