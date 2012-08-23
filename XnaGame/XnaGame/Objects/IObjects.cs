using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{

    /// <summary>
    /// Simple entity that has a position, orientation and scale associated.
    /// Everything that uses a space in the world should derive frome here
    /// </summary>
    public interface ISpatialEntity
    {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
        Matrix WorldMatrix { get; }

    }

    /// <summary>
    /// Entity that can be drawn. Note that all the drawing code should be
    /// accesible inside this function because this will be called from the manager.
    /// </summary>
    public interface IDrawableEntity : ISpatialEntity
    {
        void DrawEntity(Matrix view, Matrix projection,
            string effectTechniqueName, GraphicsDevice device);
    }

    /// <summary>
    /// Entity that can be updated. Note that all the update code should be
    /// accesible inside this function because this will be called from the manager.
    /// </summary>
    public interface IUpdatableEntity
    {
        void Update(GameTime gameTime);
    }

}
