#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion


namespace XNATactics
{
    public class COrbitCamera : GameComponent,ICamera
    {

        float speed = 1.0f;
        float rotationSide = 0.0f;
        float rotationUp = 0.0f;

        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        private Vector3 origPosition;
        public bool CanMove { get; set; }

        public COrbitCamera(XNATacticsGame game, Vector3 pos, Vector3 look)
            : base(game)
        {
            Position = pos;
            origPosition = pos;
            LookAt = look;            
            game.Services.AddService(typeof(ICamera),this);
        }

        public override void Initialize()
        {
            // Calculate the camera matrices.           
            ViewMatrix = Matrix.CreateLookAt(Position,
                                              LookAt,
                                              Vector3.Up);

            // we can't get the Viewport until after the graphics device has 
            // been initialized, so don't move this to constructor
            float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50),
                                                        aspectRatio,
                                                        0.5f, 250.0f);

            base.Initialize();
        }

        void UpdateView()
        {
            ViewMatrix = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        }


                
        public override void Update(GameTime gameTime)
        {
            if (!CanMove)
                return;
            // This input reading should probably be done in somewhere else.
            KeyboardState keys = Keyboard.GetState(0);
            bool changed = false;
            if (keys.IsKeyDown(Keys.Right))
            {                
                OrbitSideways(speed);
                changed = true;
            }
            if (keys.IsKeyDown(Keys.Left))
            {                
                OrbitSideways(-speed);
                changed = true;
            }
            if (keys.IsKeyDown(Keys.Up))
            {                
                OrbitUpways(-speed);
                changed = true;
            }
            if (keys.IsKeyDown(Keys.Down))
            {             
                OrbitUpways(speed);
                changed = true;
            }
            if (changed)
            {
               UpdateView();
               // Trace.WriteLine("in the update, New pos " + Position);
            }
        }

        public void OrbitSideways(float angle)
        {
            rotationSide += angle;
            if (rotationSide > 360.0f || rotationSide < -360.0f) rotationSide = 0.0f;            
            Matrix rot = Matrix.CreateRotationX( MathHelper.ToRadians(rotationUp) ) * Matrix.CreateRotationY( MathHelper.ToRadians(rotationSide) );
            Position = Vector3.Transform(origPosition,rot);

        }

        public void OrbitUpways(float angle)
        {
            rotationUp += angle;
            rotationUp = MathHelper.Clamp(rotationUp, -89.0f, 89.0f);
            Matrix rot = Matrix.CreateRotationX(MathHelper.ToRadians(rotationUp)) * Matrix.CreateRotationY(MathHelper.ToRadians(rotationSide));
            Position = Vector3.Transform(origPosition,rot);

        }

    }
}
