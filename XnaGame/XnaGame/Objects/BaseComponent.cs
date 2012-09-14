using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaGame
{
    public class BaseComponent<Derived> 
        where Derived: IEntityComponent
    {
        IGameEntity _owner;
        public IntrusiveListItem<Derived> _link;

        public IGameEntity Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                OnOwnerChanged();
            }
        }

        virtual public void OnOwnerChanged()
        { }

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

        public IntrusiveListItem<Derived> ComponentList
        {
            get
            {
                return _link;
            }
        }

        virtual public void Process() { }
    }
}
