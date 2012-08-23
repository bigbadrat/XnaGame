#region File description
/// This file contains the the backbone of the rendering in the engine
#endregion
#region Using
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace XnaGame
{
    public class Renderer : DrawableGameComponent, IRenderer
    {
        VertexPositionTexture[] ppVertices;
        //VertexDeclaration ppVertexDeclaration;
        Effect composeEffect;

        public List<string> effectList;

        public RenderTarget2D sceneTarget;
        public RenderTarget2D guiTarget;
        public Texture2D sceneTexture;
        public Texture2D guiTexture;

        SpriteBatch screenDrawer;
        Rectangle rect;

        public RenderTarget2D postProcessTarget;
        int frame = 0;
        float m_timer;


        public Renderer(Game game) :
            base(game)
        {
            game.Services.AddService(typeof(IRenderer), this);
        }

        public override void Initialize()
        {
            base.Initialize();

            ppVertices = new VertexPositionTexture[4];
            int i = 0;
            ppVertices[i++] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 0));
            ppVertices[i++] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 0));
            ppVertices[i++] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 1));
            ppVertices[i++] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 1));

            //ppVertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionTexture.VertexElements);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            sceneTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat,DepthFormat.Depth24Stencil8);
            guiTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight/*, false, pp.BackBufferFormat, pp.DepthStencilFormat*/);
            postProcessTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight/*, false, pp.BackBufferFormat, pp.DepthStencilFormat*/);

            composeEffect = Game.Content.Load<Effect>("hlsl/SimpleTechniques");
            composeEffect.Parameters["iSeed"].SetValue(1338);
            composeEffect.Parameters["fNoiseAmount"].SetValue(0.01f);

            effectList = new List<string>();
            //effectList.Add("Noise");
            //effectList.Add("Grayscale");
            m_timer = 0.0f;

            screenDrawer = new SpriteBatch(GraphicsDevice);
            rect = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GraphicsDevice device = GraphicsDevice;
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            frame++;
            m_timer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice device = Game.GraphicsDevice;

            device.SetRenderTarget(null);

            sceneTexture = sceneTarget;
            guiTexture = guiTarget;

            PostProcessScene(effectList);

            screenDrawer.Begin();

            screenDrawer.Draw(sceneTexture, rect, Color.White);
            screenDrawer.Draw(guiTexture, rect, Color.White);

            screenDrawer.End();

        }

        public void PostProcessScene(List<string> ppEffectsList)
        {
            GraphicsDevice device = Game.GraphicsDevice;
            
            for (int i = 0; i < ppEffectsList.Count; i++)
            {
                //We have to swap between the scene and post process target after each postprocess
                //because if we have more than 1 postprocess, we can't read and write from the same
                //texture (input for the second pass should be output of the first pass)
                device.SetRenderTarget( (i%2==0)?postProcessTarget:sceneTarget );

                composeEffect.CurrentTechnique = composeEffect.Techniques[ppEffectsList[i]];

                composeEffect.Parameters["sceneTexture"].SetValue((i % 2 == 0) ? sceneTarget : postProcessTarget );
                composeEffect.Parameters["fTimer"].SetValue(m_timer);

                foreach (EffectPass pass in composeEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();                    
                    device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, ppVertices, 0, 2);                    
                }                             
            }
            if (ppEffectsList.Count > 0)
            {
                device.SetRenderTarget(null);
                sceneTexture = (ppEffectsList.Count%2==0)?sceneTarget : postProcessTarget;
            }

        }
        #region Helper methods for other components to signal begining/ending of rendering stages
        public void BeginSceneRendering()
        {
            Game.GraphicsDevice.SetRenderTarget( sceneTarget );
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        public void EndSceneRendering()
        {
            Game.GraphicsDevice.SetRenderTarget(null);
            sceneTexture = sceneTarget;
        }
        public void BeginGuiRendering()
        {
            Game.GraphicsDevice.SetRenderTarget(guiTarget);
        }
        public void EndGuiRendering()
        {
            Game.GraphicsDevice.SetRenderTarget(null);
            guiTexture = guiTarget;
        }
        #endregion


        public void AddPostProcess(string effectName)
        {
            if ( !effectList.Contains(effectName) )
            {
                effectList.Add(effectName);
            }
        }
        public void RemovePostProcess(string effectName)
        {
            if (effectList.Contains(effectName))
            {
                effectList.Remove(effectName);
            }
        }
    }

}


