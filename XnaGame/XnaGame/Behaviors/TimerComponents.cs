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
    public class TimerBehavior : BaseComponent<TimerBehavior>, IEntityComponent
    {        
        float m_fElapsed;
        float m_fTimeTarget;

        public TimerBehavior(float time)
            : base()
        {
            m_fElapsed = 0;
            m_fTimeTarget = time;
        }

        public string Name { get { return "Timer"; } }

        public void Update(GameTime gametime)
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
    public class DelayedCallCoponent : BaseComponent<TimerBehavior>, IEntityComponent
    {
        public delegate void DelayedFunction();

        DelayedFunction m_pFunction;
        float m_fElapsed;
        float m_fTimeTarget;        
        bool m_bLooping;
        bool m_bCompleted;

        public DelayedCallCoponent(float time, bool loop, DelayedFunction f)
            : base()
        {
            m_fElapsed = 0;
            m_fTimeTarget = time;
            m_bLooping = loop;
            m_pFunction = f;
            m_bCompleted = false;
        }

        public string Name { get { return "DelayedCall"; } }

        public void Update(GameTime gametime)
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

    }
}
