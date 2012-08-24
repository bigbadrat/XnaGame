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
    public class BehaviorLinearOscillate : BehaviorBase
    {
        public SpatialEntity pOwner;
        public float Speed;
        public Vector3 A, B;
        protected float Alfa;
        protected bool LeftToRight;

        public BehaviorLinearOscillate(SpatialEntity o, float s, Vector3 PointA, Vector3 PointB)
            : base(o)
        {
            pOwner = o;
            Speed = s;
            A = PointA;
            B = PointB;
            pOwner.Position = A;
            Alfa = 0;
            LeftToRight = true;
        }

        public override void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            pOwner.Position = GetInterpolatedValue();
        }

        protected void AddWeight(float elapsedTime)
        {
            if (LeftToRight)
                Alfa += elapsedTime * Speed;
            else
                Alfa -= elapsedTime * Speed;

            if (LeftToRight && Alfa >= 1)
            {
                LeftToRight = false;
                Alfa = 1;
            }
            else if (!LeftToRight && Alfa <= 0)
            {
                LeftToRight = true;
                Alfa = 0;
            }
        }

        protected virtual Vector3 GetInterpolatedValue()
        {
            return Vector3.Lerp(A, B, Alfa);
        }

    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Catmull-Rom interpolation. Never completes.
    /// </summary>
    public class BehaviorCatmullOscillate : BehaviorLinearOscillate
    {
        public Vector3 C, D;

        public BehaviorCatmullOscillate(SpatialEntity o, float s, Vector3 PointA, Vector3 PointB, Vector3 PointC, Vector3 PointD)
            : base(o, s, PointA, PointB)
        {
            C = PointC;
            D = PointD;
        }

        protected override Vector3 GetInterpolatedValue()
        {
            return Vector3.CatmullRom(A, B, C, D, Alfa);
        }

    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Smoothie interpolation. Never completes.
    /// </summary>
    public class BehaviorSmoothOscillate : BehaviorLinearOscillate
    {
        public BehaviorSmoothOscillate(SpatialEntity o, float s, Vector3 PointA, Vector3 PointB)
            : base(o, s, PointA, PointB)
        {
        }

        protected override Vector3 GetInterpolatedValue()
        {
            return Vector3.SmoothStep(A, B, Alfa);
        }

    }
}
