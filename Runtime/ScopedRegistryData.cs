namespace PluginsHelper.Runtime
{
    public class ScopedRegistryData
    {
        public string Name { get; private set; }
        public string URL { get; private set; }

        public ScopedRegistryData(string name, string url)
        {
            Name = name;
            URL = url;
        }
    }
}
