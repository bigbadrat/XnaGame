using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    
    public class IntrusiveListItem<Type> 
        where Type : IEntityComponent
    {
        static Type _head = default(Type);
        static Type _tail = default(Type);

        public static Type Head()
        {
            return _head;
        }

        public static bool Empty()
        {
            return _head == null;
        }

        public static void AddToTail(Type item)
        {
            if (Empty())
            {
                _head = item;
                _tail = item;
            }
            else
            {
                item.Prev = _tail;
                _tail.Next = item;                
                _tail = item;
            }
        }

        public static void Remove(Type item)
        {
            if (item.Equals(_head))
            {                
                _head = (Type)item.Next;
                _head.Prev = null;                
            }
            else if (item.Equals(_tail))
            {
                _tail = (Type)item.Prev;
                _tail.Next = null;
            }
            else
            {
                item.Prev.Next = item.Next;
                item.Next.Prev = item.Prev;
            }
        }

        Type _prev;
        Type _next;
        public Type Next
        {
            get { return _next; }
            set { _next = value; }
        }
        public Type Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }

        public IntrusiveListItem()
        {
            Next = default(Type);
            Prev = default(Type);            
        }

    }

    /// <summary>
    /// This is the very basic definition of an interface to group components
    /// </summary>
    public interface IGameEntity
    {
        string Name { get; }
        IEntityComponent GetComponent(string name);
        void AddComponent(IEntityComponent comp);
    }

    public interface IEntityComponent
    {
        IGameEntity Owner { get; set; }
        string Name { get; }
        void Process();

        IEntityComponent Prev { get; set; }
        IEntityComponent Next { get; set; }
    }

    /// <summary>
    /// Simple entity that has a position, orientation and scale associated.
    /// Everything that uses a space in the world should derive frome here
    /// </summary>
    public interface ISpatialEntity
    {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
        Matrix WorldMatrix { get; }

    }

    /// <summary>
    /// Entity that can be drawn. Note that all the drawing code should be
    /// accesible inside this function because this will be called from the manager.
    /// </summary>
    public interface IDrawableEntity : ISpatialEntity
    {
        void DrawEntity(Matrix view, Matrix projection,
            string effectTechniqueName, GraphicsDevice device);
    }

    /// <summary>
    /// Entity that can be updated. Note that all the update code should be
    /// accesible inside this function because this will be called from the manager.
    /// </summary>
    public interface IUpdatableEntity
    {
        void Update(GameTime gameTime);
    }

}
