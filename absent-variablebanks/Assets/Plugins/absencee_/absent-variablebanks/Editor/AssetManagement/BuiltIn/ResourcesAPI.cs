using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using System;
using UnityEditor;

namespace com.absence.variablebanks.editor.internals.assetmanagement.builtin
{
    [AssetManagementAPI("Resources")]
    public class ResourcesAPI : IAssetManagementAPI
    {
        [APIConstructor]
        public ResourcesAPI()
        {
        }

        public bool ShowInSettings()
        {
            return true;
        }

        public void ApplyCreationProperties(VariableBank bank, Type type)
        {
            bank.OnDestroyAction += () =>
            {
                VariableBankDatabase.Refresh();
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();
        }
    }
}
