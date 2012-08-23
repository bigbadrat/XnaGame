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
        void Add(SceneEntity ent);
        void Remove(SceneEntity ent);
        void RemoveAll();
        SceneEntity GetEntity(int i);
        SceneEntity GetEntity(string name);
        void Init();
    }

    public interface IRenderer
    {
        void BeginSceneRendering();
        void EndSceneRendering();
        void BeginGuiRendering();
        void EndGuiRendering();
        void AddPostProcess(string effectName);
        void RemovePostProcess(string effectName);
    }
}
