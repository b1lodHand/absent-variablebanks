using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using System;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor;

#if ABSENT_VB_ADDRESSABLES

namespace com.absence.variablebanks.editor.internals.assetmanagement.builtin
{
    [AssetManagementAPI("Addressables")]
    public class AddressablesAPI : IAssetManagementAPI
    {
        [APIConstructor]
        public AddressablesAPI()
        {
        }

        public bool ShowInSettings()
        {
            return true;
        }

        public void ApplyCreationProperties(VariableBank bank, Type type)
        {
            string pathName = AssetDatabase.GetAssetPath(bank);

            AddressableAssetSettings addressableSettings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            AddressableAssetGroup addressableGroup = addressableSettings.FindGroup("VariableBanks");
            if (addressableGroup == null || !addressableGroup.ReadOnly)
            {
                addressableGroup = addressableSettings.CreateGroup("VariableBanks", false, true, true, null);
            }

            AddressableAssetEntry addressableEntry = addressableSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(pathName),
                addressableGroup, true, true);

            addressableEntry.SetLabel("variable-banks", true);

            bank.OnDestroyAction += () =>
            {
                addressableGroup.RemoveAssetEntry(addressableEntry);
                VariableBankDatabase.Refresh();
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();
        }
    }
}

#endif