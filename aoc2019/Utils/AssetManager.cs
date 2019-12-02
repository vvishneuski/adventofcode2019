using System.IO;

namespace aoc2019.Utils
{
    public static class AssetManager
    {
        public static StreamReader GetAsset(string name)
        {
            var path = Path.Combine(@"..\..\Assets", name);
            return File.OpenText(path);
        }
        
        public static FileStream OpenAsset(string name)
        {
            var path = Path.Combine(@"..\..\Assets", name);
            return File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        }
    }
}
