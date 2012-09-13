using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    class StaticModelComponent: IEntityComponent
    {

        public Model Model3D { get; set; }
        public Color AmbientColor { get; set; }
        public string ModelName { set { UpdateModel(value); } }
        IntrusiveListItem<StaticModelComponent> _link;

        SpatialComponent _spatial;

        public StaticModelComponent(string model)
        {
            UpdateModel(model);            
            _link = new IntrusiveListItem<StaticModelComponent>(this);
        }

        public string Name { get { return "StaticModel"; } }

        IGameEntity _owner;
        public IGameEntity Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _spatial = (SpatialComponent)value.GetComponent("Spatial");
                _owner = value;
            }
        }

        public void LinkPrev(IEntityComponent comp)
        {
            _link.Prev = (StaticModelComponent)comp;
            comp.LinkNext(this);
        }
        public void LinkNext(IEntityComponent comp)
        {
            _link.Next = (StaticModelComponent)comp;
            comp.LinkPrev(this);
        }
        public IEntityComponent Prev
        {
            get
            {
                return _link.Prev;
            }
        }

        public IEntityComponent Next
        {
            get
            {
                return _link.Next;
            }
        }

        public void Process() { }

        void UpdateModel(string model)
        {
            Asset a = MyGame.GetService<IAssetManager>().GetAsset(model);
            if (a == null)
                throw new Exception("No model" + model);
            Model3D = a.GetAssetAs<Model>();
        }

        /// <summary>
        /// Helper for drawing the model
        /// </summary>
        public void Draw(Matrix view, Matrix projection,
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
                Matrix localWorld = transforms[mesh.ParentBone.Index] * _spatial.WorldMatrix;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = AmbientColor.ToVector3();
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
