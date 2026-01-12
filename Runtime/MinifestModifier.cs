using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace PluginsHelper.Runtime
{
    public static partial class MinifestModifier
    {
        private const string ManifestPath = "Packages/manifest.json";
        private const string Dependencies = "dependencies";
        private const string ScopedRegistries = "scopedRegistries";

        private static Dictionary<PackagesDownloadPlatform, ScopedRegistryData> ScopedRegiestryDatas = new()
        {
            { PackagesDownloadPlatform.OpenUPM, new ScopedRegistryData("package.openupm.com", "https://package.openupm.com") }
        };

        public static void AddPackagesToManifest(List<PackageInfo> packagesInfo)
        {
            try
            {
                string manifestJson = File.ReadAllText(ManifestPath);
                JObject manifest = JObject.Parse(manifestJson);
                JObject dependencies = (JObject)manifest[Dependencies];

                AddScopedRegiestries(packagesInfo, ref manifest);

                PackageInfo packageInfo;
                string packageInfoName;
                string libraryReference;
                string previousLibraryReference;

                for (int i = 0; i < packagesInfo.Count; i++)
                {
                    packageInfo = packagesInfo[i];
                    packageInfoName = packageInfo.Name;

                    if (dependencies.ContainsKey(packageInfoName))
                    {
                        previousLibraryReference = dependencies[packageInfoName].ToString();
                    }
                    else
                    {
                        previousLibraryReference = "";
                    }

                    libraryReference = GetLibraryReference(packageInfo);
                    dependencies[packageInfoName] = libraryReference;

                    if (dependencies.ContainsKey(packageInfoName))
                    {
                        if (libraryReference != previousLibraryReference)
                        {
                            Debug.Log($"Package {packageInfoName} version was modified");
                        }
                        else
                        {
                            Debug.Log($"Package {packageInfoName} already exists");
                        }
                    }
                    else
                    {
                        Debug.Log($"Package {packageInfoName} was added");
                    }
                }

                File.WriteAllText(ManifestPath, manifest.ToString(Newtonsoft.Json.Formatting.Indented));
                AssetDatabase.Refresh();
                Client.Resolve();

                Debug.Log("Packages added successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to modify manifest: {e.Message}");
            }
        }

        private static string GetLibraryReference(PackageInfo packageInfo)
        {
            string libraryReference = "";

            switch (packageInfo.PackagesDownloadPlatform)
            {
                case PackagesDownloadPlatform.Git:
                    libraryReference = GetGitHubPackageStringTemplate(packageInfo);
                    break;
                case PackagesDownloadPlatform.OpenUPM:
                    libraryReference = ManifestStringConstants.OpenUPMManifestDependence;
                    break;
                case PackagesDownloadPlatform.Unity:
                    libraryReference = ManifestStringConstants.UnityManifestDependence;
                    break;
            }

            libraryReference = libraryReference.Replace(ManifestStringConstants.URLKey, packageInfo.URL);
            libraryReference = libraryReference.Replace(ManifestStringConstants.VersionKey, packageInfo.Version);

            return libraryReference;
        }

        private static string GetGitHubPackageStringTemplate(PackageInfo packageInfo)
        {
            string packageStringTemplate = "";

            if (packageInfo.Version != string.Empty && packageInfo.Version != null)
            {
                packageStringTemplate = ManifestStringConstants.GitManifestDependenceWithVersion;
            }
            else
            {
                packageStringTemplate = ManifestStringConstants.GitManifestDependenceWithoutVersion;
            }

            return packageStringTemplate;
        }

        private static void AddScopedRegiestries(List<PackageInfo> packagesInfo, ref JObject manifest)
        {
            JArray scopedRegiestries = new JArray();

            JObject scopedRegiestry;

            JProperty name;
            JArray scopesArray;
            JProperty scopes;
            JProperty url;

            foreach (var scopedRegiestryData in ScopedRegiestryDatas)
            {
                name = new JProperty(ManifestStringConstants.Name, scopedRegiestryData.Value.Name);
                url = new JProperty(ManifestStringConstants.URL, scopedRegiestryData.Value.URL);

                IEnumerable<PackageInfo> jArray = packagesInfo.Where(p => p.PackagesDownloadPlatform == scopedRegiestryData.Key);

                scopesArray = new JArray();

                foreach (PackageInfo packageInfo in jArray)
                {
                    scopesArray.Add(packageInfo.Name);
                }

                scopes = new JProperty(ManifestStringConstants.Scopes, scopesArray);

                scopedRegiestry = new JObject(name, scopes, url);
                scopedRegiestries.Add(scopedRegiestry);
            }

            manifest[ScopedRegistries] = scopedRegiestries;
        }
    }
}