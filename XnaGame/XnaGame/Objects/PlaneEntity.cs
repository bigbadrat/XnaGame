using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    
    public class PlaneEntity: VertexModelEntity, IDrawableEntity
    {
        public PlaneEntity() { }

        public PlaneEntity(string name, Vector3 pos)
            : base(name,pos)
        {      
        }

        protected override void InitVertices()
        {
            GraphicsDevice device = MyGame.GetGame().GraphicsDevice;

            //m_pVertDeclaration = new VertexDeclaration(VertexPositionColor.VertexDeclaration);
            vertices = new VertexPositionColor[4];

            //X Axis
            vertices[0] = new VertexPositionColor(new Vector3(-0.5f, 0, -0.5f), Color.ForestGreen);
            vertices[1] = new VertexPositionColor(new Vector3(0.5f, 0, -0.5f), Color.ForestGreen);
            vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0.5f), Color.ForestGreen);
            vertices[3] = new VertexPositionColor(new Vector3(0.5f, 0, 0.5f), Color.ForestGreen);
        }
 
        protected override void  DrawPrimitives()
        {
            GraphicsDevice device = MyGame.GetGame().GraphicsDevice;
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 2);
        }
        

    }
}
