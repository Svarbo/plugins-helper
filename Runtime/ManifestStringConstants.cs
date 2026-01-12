namespace PluginsHelper.Runtime
{
    public class ManifestStringConstants
    {
        public const string URLKey = "#URL#";
        public const string VersionKey = "#Version#";

        public const string Name = "name";
        public const string Scopes = "scopes";
        public const string URL = "url";

        public const string GitManifestDependenceWithVersion = URLKey + "#" + VersionKey;
        public const string GitManifestDependenceWithoutVersion = URLKey;
        public const string OpenUPMManifestDependence = VersionKey;
        public const string UnityManifestDependence = VersionKey;
    }
}