using System;

namespace PluginsHelper
{
    [Serializable]
    public class PackageInfo
    {
        public const string PackagesDownloadPlatformKey = nameof(PackagesDownloadPlatform);
        public const string NameKey = nameof(Name);
        public const string VersionKey = nameof(Version);
        public const string URLKey = nameof(URL);

        public PackagesDownloadPlatform PackagesDownloadPlatform;
        public string Name;
        public string Version;
        public string URL;
    }
}