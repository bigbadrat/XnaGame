using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
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

        public void Build( ContentManager cm ) 
        {
            if (assetType == AssetType.Model)
                asset = cm.Load<Model>(xnaName);
            else if (assetType == AssetType.Texture)
                asset = cm.Load<Texture>(xnaName);
        }

    }
    
    public class AssetManager: IAssetManager
    {
        List<Asset> _asset_list;
        ContentManager _content;

        public AssetManager(Game game, string root_directory)
        {
            _asset_list = new List<Asset>();
            _content = new ContentManager(game.Services);
            _content.RootDirectory = root_directory;
            game.Services.AddService(typeof(IAssetManager), this);
        }

        public void LoadAssets()
        {           
            foreach (Asset asset in _asset_list)
            {
                asset.Build(_content);
            }
        }
        public void UnloadAssets()
        {
            _content.Unload();
        }

        public void AddAsset(string xnaName, AssetType xnaType)
        {
            Asset a = new Asset();
            a.xnaName = xnaName;
            a.assetType = xnaType;
            a.id = _asset_list.Count;
            _asset_list.Add(a);
        }

        public Asset GetAsset(string name)
        {
            foreach (Asset a in _asset_list)
            {
                if (name == a.xnaName)
                    return a;
            }
            return null;
        }

        public Asset GetAsset(int id)
        {
            foreach (Asset a in _asset_list)
            {
                if (id == a.id)
                    return a;
            }
            return null;
        }

        
    }
}
