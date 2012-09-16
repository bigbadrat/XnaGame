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

        public BaseComponent()
        {
            _link = new IntrusiveListItem<Derived>();
        }

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
            set
            {
                if (value is Derived)
                    _link.Prev = (Derived)value;
            }
        }

        public IEntityComponent Next
        {
            get
            {
                return _link.Next;
            }
            set
            {
                if (value is Derived)
                    _link.Next = (Derived)value;
            }
        }

        public IntrusiveListItem<Derived> ComponentList
        {
            get
            {
                return _link;
            }
        }

        virtual public void Process( Message msg ) { }
    }
    
}
