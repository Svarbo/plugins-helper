using System.Collections.Generic;
using UnityEngine;

namespace PluginsHelper
{
    [CreateAssetMenu(fileName = "PackagesConfig", menuName = "ScriptableObjects/PackagesConfig")]
    public class PackagesConfig : ScriptableObject
    {
        [SerializeField] private List<PackageInfo> _packagesInfo;

        public void AddPackagesToManifest()
        {
            MinifestModifier.AddPackagesToManifest(_packagesInfo);
        }
    }
}