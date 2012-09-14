using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    
    public class IntrusiveListItem<Type> 
        where Type : IEntityComponent
    {
        static Type head = default(Type);
        static Type tail = default(Type);

        public static Type Head()
        {
            return head;
        }

        public static bool Empty()
        {
            return head == null;
        }

        public static void AddToTail(Type item)
        {
            if (Empty())
            {
                head = item;
                tail = item;
            }
            else
            {
                item.LinkPrev(tail);
                tail.LinkNext(item);                
                tail = item;
            }
        }

        public static void Remove(Type item)
        {
            if (item.Equals(head))
            {                
                head = (Type)item.Next;
                head.LinkPrev(null);                
            }
            else if (item.Equals(tail))
            {
                tail = (Type)item.Prev;
                tail.LinkNext(null);
            }
            else
            {
                item.Prev.LinkNext(item.Next);
                item.Next.LinkPrev(item.Prev);
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

        void LinkPrev(IEntityComponent comp);
        void LinkNext(IEntityComponent comp);
        IEntityComponent Prev { get; }
        IEntityComponent Next { get; }
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
