using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    /// <summary>
    /// Simple behavior to get a signal after x time
    /// </summary>
    public class BehaviorTimer : BehaviorBase
    {
        public GameEntity pOwner;
        float m_fElapsed;
        float m_fTimeTarget;

        public BehaviorTimer(GameEntity o, float time)
            : base(o)
        {
            pOwner = o;
            m_fElapsed = 0;
            m_fTimeTarget = time;
        }

        protected override void RegisterBehaviorEvents(GameEntity b)
        {
            //b.RegisterEvent(AbyssEventType.EVT_Timer);
        }

        public override void Update(GameTime gametime)
        {
            m_fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (m_fElapsed >= m_fTimeTarget)
            {
                //pOwner.RaiseLocalEvent(AbyssEventType.EVT_Timer, new EventArgs());
                m_fElapsed -= m_fTimeTarget;
            }
        }

    }

    /// <summary>
    /// Easy way to get a function called after x seconds without having 
    /// to write the proper event descriptor. Also contains a built-in 
    /// way to loop the function call.
    /// </summary>
    public class BehaviorDelayedCall : BehaviorBase
    {
        public delegate void DelayedFunction();

        public GameEntity pOwner;
        float m_fElapsed;
        float m_fTimeTarget;
        DelayedFunction m_pFunction;
        bool m_bLooping;
        bool m_bCompleted;

        public BehaviorDelayedCall(GameEntity o, float time, bool loop, DelayedFunction f)
            : base(o)
        {
            pOwner = o;
            m_fElapsed = 0;
            m_fTimeTarget = time;
            m_bLooping = loop;
            m_pFunction = f;
            m_bCompleted = false;
        }

        public override void Update(GameTime gametime)
        {
            m_fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (m_fElapsed >= m_fTimeTarget)
            {
                m_pFunction();
                m_fElapsed -= m_fTimeTarget;
                if (!m_bLooping)
                    m_bCompleted = true;
            }
        }

        public override bool IsComplete() { return m_bCompleted; }
    }
}
