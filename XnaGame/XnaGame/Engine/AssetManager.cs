using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XnaGame
{
    
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
