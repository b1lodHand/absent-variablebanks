using com.absence.variablebanks.editor.internals.assetmanagement;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The class responsible for drawing a section for this package in Project settings.
    /// </summary>
    public class PackageSettingsProvider : SettingsProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scope"></param>
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

            string[] options = AssetManagementAPIDatabase.APIs.ConvertAll(api => api.DisplayName).ToArray();

            int assetApiIndex = settings.AssetManagementAPISelection;
            int assetApiIndexNew = EditorGUILayout.Popup("", assetApiIndex, options, GUILayout.ExpandWidth(false));

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
