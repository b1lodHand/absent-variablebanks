using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    public class PackageSettingsProvider : SettingsProvider
    {
        static readonly string[] s_assetPopupOptions = { "Resources", "Addressables" };

        public PackageSettingsProvider(string path, SettingsScope scope) : base(path, scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            EditorGUILayout.Space(5);
            EditorGUI.indentLevel++;

            PackageSettings settings = PackageSettings.instance;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Used Asset Management API", GUILayout.Width(200));

            int assetApiIndex = settings.AssetManagementAPISelection;
            int assetApiIndexNew = EditorGUILayout.Popup("", assetApiIndex, s_assetPopupOptions, GUILayout.ExpandWidth(false));

            EditorGUILayout.EndHorizontal();

            if (assetApiIndexNew != assetApiIndex)
            {
                settings.AssetManagementAPISelection = assetApiIndexNew;
            }

            EditorGUI.indentLevel--;
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new PackageSettingsProvider("absencee_/absent-variablebanks", SettingsScope.Project);
        }
    }
}
