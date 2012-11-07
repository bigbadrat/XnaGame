using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{

    public class Message
    {
        MsgType _type;

        public MsgType MessageType { get { return _type; } set { _type = value; } }

    }

    public enum MsgType
    {
        Move = 1,
        MoveTo = 2,
        Move2D = 3,
        MoveTo2D = 4
    }

    /// <summary>
    /// This is the very basic definition of an interface to group components.
    /// The GameEntities by themselves don´t do much except gather a group of
    /// components that compose a specific actor. 
    /// The extensible method to add functionality is via messages. The GameEntity
    /// will transfer the message to components and they will do as needed.
    /// </summary>
    public interface IGameEntity
    {
        string Name { get; }
        IEntityComponent GetComponent(string name);
        void AddComponent(IEntityComponent comp);
        void ReceiveMessage(Message msg);
    }

    /// <summary>
    /// The components ar the different pieces of functionality that put together
    /// form an entity. Each component knows only of its own resposability and at
    /// most knows of other components he requires to operate.
    /// The components are responsible of processing messages that are relevant 
    /// for their responsibility.
    /// </summary>
    public interface IEntityComponent
    {
        IGameEntity Owner { get; set; }
        string Name { get; }
        void Process(Message msg);

        IEntityComponent Prev { get; set; }
        IEntityComponent Next { get; set; }
    }

}