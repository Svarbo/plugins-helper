using UnityEditor;
using UnityEngine;

namespace PluginsHelper
{
    [CustomEditor(typeof(PackagesConfig))]
    public class PackagesConfigEditor : Editor
    {
        private float _addPackageButtonHeight = 30f;
        private PackagesConfig _packagesConfig;

        private void OnEnable()
        {
            _packagesConfig = target as PackagesConfig;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add packages to manifest", GUILayout.Height(_addPackageButtonHeight)))
            {
                _packagesConfig.AddPackagesToManifest();
            }
        }
    }
}