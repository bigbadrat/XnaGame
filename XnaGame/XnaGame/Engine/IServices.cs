using Microsoft.Xna.Framework;

namespace XnaGame
{

    public interface IAssetManager
    {
        void AddAsset(string xnaName, AssetType xnaType);
        Asset GetAsset(string name);
        Asset GetAsset(int id);
        void LoadAssets();
        void UnloadAssets();
    }

    public interface ICamera
    {
        Vector3 Position { get; set; }
        Vector3 LookAt { get; set; }
        Matrix ViewMatrix { get; }
        Matrix ProjectionMatrix { get; }
    }

    public interface IObjectManager
    {
        void Add(GameEntity ent);
        void Remove(GameEntity ent);
        void RemoveAll();
        GameEntity GetEntity(int i);
        GameEntity GetEntity(string name);
    }

    public interface ISpriteManager
    {
        void AddSprite(SpriteBasic sb);
        SpriteBasic GetSprite(int i);
        SpriteBasic GetSprite(string name);
    }

    public interface IRenderer
    {
        void BeginSceneRendering();
        void EndSceneRendering();
        void BeginSpriteRendering();
        void EndSpriteRendering();
        void BeginGuiRendering();
        void EndGuiRendering();
        void AddPostProcess(string effectName);
        void RemovePostProcess(string effectName);
    }

    public interface IInputHandler
    {
        void SuscribeToInput(InputEventHandler inputhandler);
    }
}
