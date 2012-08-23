#region using statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XnaGame
{

    /// <summary>
    /// Pretty much a helper class to define custom vertexs put it somewhere and 
    /// draw (and profit!). You just have to override the InitVertices and DrawPrimitves
    /// methods to get your custom model to be drawn.
    /// </summary>
    
    public class VertexModelEntity : SpatialEntity
    {
        protected VertexPositionColor[] vertices;

        protected VertexDeclaration m_pVertDeclaration;

        public VertexModelEntity() { InitVertices(); }

        public VertexModelEntity(string name, Vector3 pos)
            : base(name, pos)
        {
            InitVertices();            
        }

        public virtual void DrawEntity(Matrix view, Matrix projection,
                       string effectTechniqueName, GraphicsDevice device)
        {
            BasicEffect basicEffect = new BasicEffect(device);

            //device.RenderState.DepthBufferEnable = true;

            basicEffect.World = WorldMatrix;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                DrawPrimitives();
            }
            
        }

        #region Must-override methods
        /// <summary>
        /// Init Vertices contains the actual vertices.
        /// </summary>
        protected virtual void InitVertices() { }
        /// <summary>
        /// This is the specific DrawPrimitive call.
        /// </summary>
        protected virtual void DrawPrimitives() { }
        #endregion

    }
}
