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
    public class LinearOscillateComponent : BaseComponent<LinearOscillateComponent>, IEntityComponent
    {
        SpatialComponent _spatial;
        public float Speed;
        public Vector3 A, B;
        protected float Alfa;
        protected bool LeftToRight;

        public LinearOscillateComponent(float s, Vector3 PointA, Vector3 PointB)
            : base()
        {           
            Speed = s;
            A = PointA;
            B = PointB;
            Alfa = 0;
            LeftToRight = true;
        }

        public string Name { get { return "Oscillate"; } }

        public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
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

        override public void OnOwnerChanged()
        {
            _spatial = (SpatialComponent)Owner.GetComponent("Spatial");
        }  
    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Catmull-Rom interpolation. Never completes.
    /// </summary>
    public class CatmullOscillateComponent : BaseComponent<CatmullOscillateComponent>, IEntityComponent
    {
        SpatialComponent _spatial;
        public float Speed;
        public Vector3 A, B;
        public Vector3 C, D;
        protected float Alfa;
        protected bool LeftToRight;

        public CatmullOscillateComponent(float s, Vector3 PointA, Vector3 PointB, Vector3 PointC, Vector3 PointD)
            : base()
        {           
            Speed = s;
            A = PointA;
            B = PointB;
            C = PointC;
            D = PointD;
            Alfa = 0;
            LeftToRight = true;
        }

        public string Name { get { return "Oscillate"; } }

        public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
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

        protected Vector3 GetInterpolatedValue()
        {
            return Vector3.CatmullRom(A, B, C, D, Alfa);
        }

        override public void OnOwnerChanged()
        {
            _spatial = (SpatialComponent)Owner.GetComponent("Spatial");
        }  
    }

    /// <summary>
    /// Behavior to move an entity from point A to B and go back using a 
    /// Smoothie interpolation. Never completes.
    /// </summary>
    public class SmoothOscillateComponent : BaseComponent<SmoothOscillateComponent>, IEntityComponent
    {
        SpatialComponent _spatial;
        public float Speed;
        public Vector3 A, B;
        protected float Alfa;
        protected bool LeftToRight;

        public SmoothOscillateComponent(float s, Vector3 PointA, Vector3 PointB)
            : base()
        {
            Speed = s;
            A = PointA;
            B = PointB;
            Alfa = 0;
            LeftToRight = true;
        }

        public string Name { get { return "Oscillate"; } }

        public void Update(GameTime gametime)
        {
            if (Speed == 0)
                return;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            AddWeight(elapsed);

            _spatial.Position = GetInterpolatedValue();
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

        protected Vector3 GetInterpolatedValue()
        {
            return Vector3.SmoothStep(A, B, Alfa);
        }

        override public void OnOwnerChanged()
        {
            _spatial = (SpatialComponent)Owner.GetComponent("Spatial");
        }
    }
}
