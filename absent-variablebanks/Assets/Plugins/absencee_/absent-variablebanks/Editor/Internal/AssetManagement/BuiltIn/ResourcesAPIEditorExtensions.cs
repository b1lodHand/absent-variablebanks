using com.absence.variablebanks.internals;
using com.absence.variablebanks.internals.assetmanagement;
using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using System;
using UnityEditor;
using UnityEngine;
using ResourcesAPI = com.absence.variablebanks.internals.assetmanagement.builtin.ResourcesAPI;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [AssetManagementAPIEditorExtension(typeof(ResourcesAPI))]
    public class ResourcesAPIEditorExtensions : IAssetManagementAPIEditorExtension
    {
        [APIConstructor]
        public ResourcesAPIEditorExtensions() 
        { 
        }

        public bool ShowInSettings()
        {
            return true;
        }


        public bool ApplyCreationProperties(VariableBank bank, Type type)
        {
            VariableBankCreationHandler.ValidateResourcesPath();

            string path = AssetDatabase.GetAssetPath(bank);
            string intendedPath = $"Assets/Resources/{Constants.K_RESOURCES_PATH}/{bank.name}.asset";

            bank.OnDestroyAction += () =>
            {
                VariableBankDatabase.Refresh();
            };

            if (path.Equals(intendedPath)) return true;

            string error = AssetDatabase.MoveAsset(path, intendedPath);

            if (string.IsNullOrWhiteSpace(error)) return true;

            Debug.LogWarning(error);
            return false;
        }

        public bool ResetCreationProperties(VariableBank bank, Type type)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Temp"))
                AssetDatabase.CreateFolder("Assets", "Temp");

            string path = AssetDatabase.GetAssetPath(bank);
            string intendedPath = $"Assets/Temp/{bank.name}.asset";
            if (path.Equals(intendedPath)) return true;

            string error = AssetDatabase.MoveAsset(path, intendedPath);

            if (string.IsNullOrWhiteSpace(error)) return true;

            Debug.LogWarning(error);
            return false;
        }

        public bool OverrideBankModeChangeDialogMessage(bool internalizing, out string message)
        {
            if (internalizing)
            {
                message = Constants.K_DEFAULT_MODE_CHANGE_DIALOG_INT + $"\n(NOTE: Bank will be moved to: 'Assets/Resources/{Constants.K_RESOURCES_PATH}/..')";
                return true;
            }

            else
            {
                message = Constants.K_DEFAULT_MODE_CHANGE_DIALOG_EXT + $"\n(NOTE: Bank will be moved to: 'Assets/Temp/..')";
                return true;
            }
        }
    }
}
