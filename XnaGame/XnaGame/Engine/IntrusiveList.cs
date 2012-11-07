
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
}
