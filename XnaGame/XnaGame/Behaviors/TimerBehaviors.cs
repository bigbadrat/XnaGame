using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{

    /// <summary>
    /// Base behavior that checks if a given amount of time has passed
    /// and calls a function (expected to be overriden) to take action
    /// as the derived function sees fit.
    /// </summary>
    public class BaseTimerBehavior : IBehavior
    {
        IGameEntity _owner;
        float _elapsed;
        float _target_time;
        protected bool _complete;
        protected bool _loop;

        public BaseTimerBehavior(float time, bool loop = false)
        {   
            _target_time = time;
            _loop = loop;
            _complete = false;
            _elapsed = 0;            
        }

        public void Update(GameTime gametime)
        {
            _elapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (_elapsed >= _target_time)
            {                
                _elapsed -= _target_time;
                OnTimer();
                if (!_loop)
                    _complete = true;
            }
        }

        public void AttachTo(IGameEntity entity)
        {
            _owner = entity;
        }

        public bool IsComplete() { return _complete; }

        virtual public void OnTimer() { }
    }

    /// <summary>
    /// Simple behavior to get a signal after x time
    /// </summary>
    public class TimerBehavior : BaseTimerBehavior
    { 
        public TimerBehavior(float time) :
            base(time)
        {
        }

        public override void OnTimer()
        {
            //TODO: Hook this when the rest of the message system is working
            //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Timer, new EventArgs());
        }

    }

    /// <summary>
    /// Easy way to get a function called after x seconds without having 
    /// to write the proper event descriptor. Also contains a built-in 
    /// way to loop the function call.
    /// </summary>
    public class DelayedCallBehavior : BaseTimerBehavior
    {
        public delegate void DelayedFunction();

        DelayedFunction _function;


        public DelayedCallBehavior(float time, bool loop, DelayedFunction f)
            : base(time)
        {
            _loop  = loop;
            _function = f;
        }

        public override void OnTimer()
        {
            _function();
        } 

    }
}
