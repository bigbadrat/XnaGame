using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGame
{
    public enum AssetType
    {
        Model,
        Texture
    }

    public class Asset
    {
        public int id { get; set; }
        public string xnaName { get; set; }
        public AssetType assetType { get; set; }

        public Object asset;

        public Object GetAsset() { return asset; }
        public T GetAssetAs<T>() { return (T)asset; }

        public void Build(ContentManager cm)
        {
            if (assetType == AssetType.Model)
                asset = cm.Load<Model>(xnaName);
            else if (assetType == AssetType.Texture)
                asset = cm.Load<Texture>(xnaName);
        }

    }
}
