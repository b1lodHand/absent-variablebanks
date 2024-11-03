using com.absence.variablebanks.internals.assetmanagement;
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

            List<string> options = AssetManagementAPIDatabase.APIs.ConvertAll(api => api.DisplayName);

            string assetApiName = settings.AssetManagementAPIName;

            bool unknownApi = !options.Contains(assetApiName);
            if (unknownApi)
            {
                settings.AssetManagementAPIName = "Resources";
                return;
            }

            int assetApiIndex = options.IndexOf(assetApiName);
            int assetApiIndexNew = EditorGUILayout.Popup("", assetApiIndex, options.ToArray(), GUILayout.ExpandWidth(false));

            string assetApiNameNew = options[assetApiIndexNew];

            GUILayout.Space(10);

            GUIContent refreshButtonContent = new()
            {
                image = EditorGUIUtility.IconContent("Refresh").image,
                tooltip = "Refresh API database."
            };

            if (GUILayout.Button(refreshButtonContent, GUILayout.ExpandWidth(false)))
            {
                AssetManagementAPIDatabase.Refresh();
            }

            EditorGUILayout.EndHorizontal();

            if (assetApiName != assetApiNameNew)
            {
                bool confirm = EditorUtility.DisplayDialog("Change API selection?",
                    "You are changing the used API. All of the" +
                    " internal banks will automatically get marked as 'External'." +
                    " If you want to had internal banks, you should go and manually change" +
                    " their mode.\n\nYou can't undo this action. ", 
                    "Ok", "Cancel");

                if (confirm)
                {
                    VariableBankCreationHandler.MakeAllExternal();
                    settings.AssetManagementAPIName = assetApiNameNew;
                }
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
