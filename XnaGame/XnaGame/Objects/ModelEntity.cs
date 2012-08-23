
#region using statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion


namespace XnaGame
{
    /// <summary>
    /// Entity that has a model.
    /// </summary>
    
    public class ModelEntity : SpatialEntity, IDrawableEntity
    {
        
        public Model Model3D { get;set; }
        public Color AmbientColor { get; set; }
        public string ModelName { get; set; }

        public ModelEntity() { }

        public ModelEntity(string entname, string modelName, Vector3 entpos)
            : base(entname, entpos)
        {
            ModelName = modelName;
        }

        public override void Init()
        {
            Asset a = Game1.GetService<IAssetManager>().GetAsset(ModelName);
            Model3D = a.GetAssetAs<Model>();
            //if (Model3D == null)
            //    Abyss.Log("error creating" + ModelName);
        }

        /// <summary>
        /// Helper for drawing the model
        /// </summary>
        public void DrawEntity( Matrix view, Matrix projection,
                       string effectTechniqueName, GraphicsDevice device)
        {
            // Set suitable renderstates for drawing a 3D model.
            /*RenderState renderState = device.RenderState;

            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;*/

            // Look up the bone transform matrices.
            Matrix[] transforms = new Matrix[Model3D.Bones.Count];

            Model3D.CopyAbsoluteBoneTransformsTo(transforms);
            device.DepthStencilState = DepthStencilState.Default;
            // Draw the model.
            foreach (ModelMesh mesh in Model3D.Meshes)
            {
                Matrix localWorld = transforms[mesh.ParentBone.Index] * WorldMatrix;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = AmbientColor.ToVector3() ;
                    // Specify which effect technique to use.
                    //effect.CurrentTechnique = effect.Techniques[effectTechniqueName];

                    effect.World = localWorld;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }



    }

   


}
 