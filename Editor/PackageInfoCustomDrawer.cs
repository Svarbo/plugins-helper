#if UNITY_EDITOR
using PluginsHelper.Runtime;
using UnityEditor;
using UnityEngine;
using PackageInfo = PluginsHelper.Runtime.PackageInfo;

namespace PluginsHelper.EditorTools
{
    [CustomPropertyDrawer(typeof(PackageInfo))]
    public class PackageInfoCustomDrawer : PropertyDrawer
    {
        private float _stringsCount = 3f;
        private float _baseSymbolWidth = 10f;
        private float _secondLineAdditionalDistanceY = 10f;
        private float _packagesDownloadPlatformFieldWidth = 0.35f;
        private float _nameFieldWidth = 0.3f;
        private float _urlFieldWidth = 0.45f;
        private float _versionFieldWidth = 0.15f;
        private float _currentPositionY;

        private float BaseFieldHeight => EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isExpanded = property.isExpanded;
            float height = EditorGUIUtility.singleLineHeight;

            if (isExpanded)
            {
                height += EditorGUIUtility.singleLineHeight * _stringsCount;
                height += EditorGUIUtility.standardVerticalSpacing * 7;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, property.FindPropertyRelative(PackageInfo.NameKey).stringValue, true);

            if (property.isExpanded)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                _currentPositionY = position.y;
                _currentPositionY += BaseFieldHeight;
                DrawFirstLine(position, property);

                _currentPositionY += BaseFieldHeight + _secondLineAdditionalDistanceY;
                DrawSecondLine(position, property);

                _currentPositionY += BaseFieldHeight;
                DrawThirdLine(position, property);

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
        }

        private void DrawFirstLine(Rect position, SerializedProperty property)
        {
            float packagesDownloadPlatformWidth = position.width * _packagesDownloadPlatformFieldWidth;

            Rect packagesDownloadPlatformRect = new Rect(position.x, _currentPositionY, packagesDownloadPlatformWidth, BaseFieldHeight);
            EditorGUI.PropertyField(packagesDownloadPlatformRect, property.FindPropertyRelative(PackageInfo.PackagesDownloadPlatformKey), GUIContent.none);
        }

        private void DrawSecondLine(Rect position, SerializedProperty property)
        {
            int packagesDownloadPlatformIndex = property.FindPropertyRelative(PackageInfo.PackagesDownloadPlatformKey).enumValueIndex;

            switch (packagesDownloadPlatformIndex)
            {
                case (int)PackagesDownloadPlatform.Git:
                    DrawGitSecondLine(position);
                    break;
                case (int)PackagesDownloadPlatform.OpenUPM:
                    DrawOpenUPMSecondLine(position);
                    break;
                case (int)PackagesDownloadPlatform.Unity:
                    DrawUnitySecondLine(position);
                    break;
            }
        }

        private void DrawThirdLine(Rect position, SerializedProperty property)
        {
            int packagesDownloadPlatformIndex = property.FindPropertyRelative(PackageInfo.PackagesDownloadPlatformKey).enumValueIndex;

            switch (packagesDownloadPlatformIndex)
            {
                case (int)PackagesDownloadPlatform.Git:
                    DrawGitThirdLine(position, property);
                    break;
                case (int)PackagesDownloadPlatform.OpenUPM:
                    DrawOpenUPMThirdLine(position, property);
                    break;
                case (int)PackagesDownloadPlatform.Unity:
                    DrawUnityThirdLine(position, property);
                    break;
            }
        }

        private void DrawGitSecondLine(Rect position)
        {
            float nameWidth = position.width * _nameFieldWidth;
            float urlWidth = position.width * _urlFieldWidth;
            float versionWidth = position.width * _versionFieldWidth;

            Rect nameRect = new Rect(position.x, _currentPositionY, nameWidth, BaseFieldHeight);
            Rect urlRect = new Rect(position.x + nameWidth + _baseSymbolWidth, _currentPositionY, urlWidth, BaseFieldHeight);
            Rect versionRect = new Rect(position.x + nameWidth + urlWidth + _baseSymbolWidth * 2, _currentPositionY, versionWidth, BaseFieldHeight);

            EditorGUI.LabelField(nameRect, PackageInfo.NameKey);
            EditorGUI.LabelField(urlRect, PackageInfo.URLKey);
            EditorGUI.LabelField(versionRect, PackageInfo.VersionKey);
        }

        private void DrawOpenUPMSecondLine(Rect position)
        {
            float nameWidth = position.width * _nameFieldWidth;
            float versionWidth = position.width * _versionFieldWidth;

            Rect nameRect = new Rect(position.x, _currentPositionY, nameWidth, BaseFieldHeight);
            Rect versionRect = new Rect(position.x + nameWidth + _baseSymbolWidth, _currentPositionY, versionWidth, BaseFieldHeight);

            EditorGUI.LabelField(nameRect, PackageInfo.NameKey);
            EditorGUI.LabelField(versionRect, PackageInfo.VersionKey);
        }

        private void DrawUnitySecondLine(Rect position)
        {
            float nameWidth = position.width * _nameFieldWidth;
            float versionWidth = position.width * _versionFieldWidth;

            Rect nameRect = new Rect(position.x, _currentPositionY, nameWidth, BaseFieldHeight);
            Rect versionRect = new Rect(position.x + nameWidth + _baseSymbolWidth, _currentPositionY, versionWidth, BaseFieldHeight);

            EditorGUI.LabelField(nameRect, PackageInfo.NameKey);
            EditorGUI.LabelField(versionRect, PackageInfo.VersionKey);
        }

        private void DrawGitThirdLine(Rect position, SerializedProperty property)
        {
            float nameLabelWidth = position.width * _nameFieldWidth;
            float urlLabelWidth = position.width * _urlFieldWidth;
            float versionLabelWidth = position.width * _versionFieldWidth;

            Rect nameLabelRect = new Rect(position.x, _currentPositionY, nameLabelWidth, BaseFieldHeight);
            Rect urlLabelRect = new Rect(position.x + nameLabelWidth + _baseSymbolWidth, _currentPositionY, urlLabelWidth, BaseFieldHeight);
            Rect versionLabelRect = new Rect(position.x + nameLabelWidth + urlLabelWidth + _baseSymbolWidth * 2, _currentPositionY, versionLabelWidth, BaseFieldHeight);

            EditorGUI.PropertyField(nameLabelRect, property.FindPropertyRelative(PackageInfo.NameKey), GUIContent.none);
            EditorGUI.PropertyField(urlLabelRect, property.FindPropertyRelative(PackageInfo.URLKey), GUIContent.none);
            EditorGUI.PropertyField(versionLabelRect, property.FindPropertyRelative(PackageInfo.VersionKey), GUIContent.none);

            //Rect ñolonRect = nameRect;
            //Rect sharpRect = urlRect;
            //Rect quotationMarksRect = versionRect;

            //ñolonRect.x += nameWidth;
            //sharpRect.x += urlWidth;
            //quotationMarksRect.x += versionWidth;

            //EditorGUI.LabelField(ñolonRect, @""": """);
            //EditorGUI.LabelField(sharpRect, @"#");
            //EditorGUI.LabelField(quotationMarksRect, @"""");
        }

        private void DrawOpenUPMThirdLine(Rect position, SerializedProperty property)
        {
            float nameLabelWidth = position.width * _nameFieldWidth;
            float versionLabelWidth = position.width * _versionFieldWidth;

            Rect nameLabelRect = new Rect(position.x, _currentPositionY, nameLabelWidth, BaseFieldHeight);
            Rect versionLabelRect = new Rect(position.x + nameLabelWidth + _baseSymbolWidth, _currentPositionY, versionLabelWidth, BaseFieldHeight);

            EditorGUI.PropertyField(nameLabelRect, property.FindPropertyRelative(PackageInfo.NameKey), GUIContent.none);
            EditorGUI.PropertyField(versionLabelRect, property.FindPropertyRelative(PackageInfo.VersionKey), GUIContent.none);
        }

        private void DrawUnityThirdLine(Rect position, SerializedProperty property)
        {
            float nameLabelWidth = position.width * _nameFieldWidth;
            float versionLabelWidth = position.width * _versionFieldWidth;

            Rect nameLabelRect = new Rect(position.x, _currentPositionY, nameLabelWidth, BaseFieldHeight);
            Rect versionLabelRect = new Rect(position.x + nameLabelWidth + _baseSymbolWidth, _currentPositionY, versionLabelWidth, BaseFieldHeight);

            EditorGUI.PropertyField(nameLabelRect, property.FindPropertyRelative(PackageInfo.NameKey), GUIContent.none);
            EditorGUI.PropertyField(versionLabelRect, property.FindPropertyRelative(PackageInfo.VersionKey), GUIContent.none);
        }
    }
}
#endif