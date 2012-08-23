#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace XnaGame
{
    public class CameraStatic : GameComponent,ICamera
    {
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public CameraStatic(Game game, Vector3 pos, Vector3 look)
            : base(game)
        {
            Position = pos;
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

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(120),
                                                        aspectRatio,
                                                        0.5f, 100.0f);

            base.Initialize();
        }

    }
}
