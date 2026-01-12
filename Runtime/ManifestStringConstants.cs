namespace PluginsHelper.Runtime
{
    public class ManifestStringConstants
    {
        public const string URLKey = "#URL#";
        public const string VersionKey = "#Version#";

        public const string GitManifestDependenceWithVersion = URLKey + "#" + VersionKey;
        public const string GitManifestDependenceWithoutVersion = URLKey;
        public const string OpenUPMManifestDependence = VersionKey;
        public const string UnityManifestDependence = VersionKey;

    }
}