using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaGame
{
    public class InputEventArgs:EventArgs
    {
        public float elapsedTime;
        public int x, y;
        public bool shoot;
        public bool switchWeapon;
    }

    public delegate void InputEventHandler(InputEventArgs args);
    
    class InputManager: GameComponent, IInputHandler
    {
        InputState input;
        
        private event InputEventHandler InputEvent;

        public InputManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IInputHandler), this);
            input = new InputState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            input.Update();

            PlayerIndex aux;
            bool shoot = false;
            bool switchweapon = false;
            int x = 0, y = 0;
            if (input.IsKeyPressed(Keys.Left, null, out aux))
                x--;
            if (input.IsKeyPressed(Keys.Right, null, out aux))
                x++;
            if (input.IsKeyPressed(Keys.Up, null, out aux))
                y--;
            if (input.IsKeyPressed(Keys.Down, null, out aux))
                y++;
            if (input.IsNewKeyPress(Keys.Space, null, out aux))
                shoot = true;
            if (input.IsNewKeyPress(Keys.LeftControl, null, out aux))
                switchweapon = true;
            if ( x != 0 || y != 0 || shoot || switchweapon)
                if (InputEvent != null)
                {
                    InputEventArgs args = new InputEventArgs();
                    args.x = x;
                    args.y = y;
                    args.shoot = shoot;
                    args.switchWeapon = switchweapon;
                    args.elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
                    InputEvent(args);
                }

        }

        public void SuscribeToInput(InputEventHandler inputhandler)
        {
            InputEvent += inputhandler;
        }

    }
}
