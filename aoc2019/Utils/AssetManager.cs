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
    }
}
