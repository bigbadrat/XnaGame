using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XnaGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        static MyGame _singleton_ref = null;
        int fishx;        

        public MyGame()
        {
            _singleton_ref = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            
            
            AssetManager asm = new AssetManager(this, "Content");

            CameraOrbit cam = new CameraOrbit(this, new Vector3(0, 0, 150), Vector3.Zero);
            cam.OrbitUpways(80);
            Components.Add(cam);
            Components.Add(new FrameRateCounter(this));
            Components.Add(new ObjectManager(this));
            Components.Add(new SpriteManager(this));
            Components.Add(new Renderer(this));
            Components.Add(new InputManager(this));

            
            fishx = 0;
            GetService<IInputHandler>().SuscribeToInput(UpdateJellyfish);
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            IAssetManager asm = GetServiceAs<IAssetManager>();
            asm.AddAsset("pics/fish", AssetType.Texture);
            asm.AddAsset("pics/jellyfish", AssetType.Texture);
            asm.AddAsset("models/Cube", AssetType.Model);
            asm.LoadAssets();

            //ModelEntity m = new ModelEntity("ship", "models/Cube", Vector3.Zero);
            SpriteBasic sb1 = new SpriteBasic("jelly","pics/jellyfish");
            SpriteBasic sb2 = new SpriteBasic("fish", "pics/fish");
            sb2.Position = new Vector2(400, 50);
            sb1.Position = new Vector2(100, 100);
            sb1.Layer = 100;
            sb2.Layer = 10;

            //IObjectManager obj = GetServiceAs<IObjectManager>();
            //obj.Init();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            Vector2 pos = new Vector2(0, 0);
            pos.X += fishx;
            if (fishx < 700)
                ++fishx;
            else
                fishx = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void UpdateJellyfish(InputEventArgs input)
        {
            SpriteBasic jelly = GetServiceAs<ISpriteManager>().GetSprite("jelly");
            Vector2 jellypos = jelly.Position;

            if (input.x > 0)
                jellypos.X += 5;
            else if (input.x < 0)
                jellypos.X -= 5;
            if (input.y > 0)
                jellypos.Y += 5;
            else if (input.y < 0)
                jellypos.Y -= 5;

            
            jelly.Position = jellypos;

        }
        
        static public MyGame GetGame()
        {
            return _singleton_ref;
        }

        static public T GetService<T>()
        {
            return (T)_singleton_ref.Services.GetService(typeof(T));
        }

        public T GetServiceAs<T>()
        {
            return (T)Services.GetService(typeof(T));
        }
    }

}
