#if ABSENT_VB_ADDRESSABLES

using com.absence.variablesystem.banksystembase;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor;
using System;
using com.absence.variablesystem.banksystembase.editor;
using com.absence.variablebanks.internals.assetmanagement;
using com.absence.variablebanks.internals.assetmanagement.builtin;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [AssetManagementAPIEditorExtension(typeof(AddressablesAPI))]
    public class AddressablesAPIEditorExtensions : IAssetManagementAPIEditorExtension
    {
        [APIConstructor]
        public AddressablesAPIEditorExtensions()
        {
        }

        public bool ShowInSettings()
        {
            return true;
        }

        public bool ApplyCreationProperties(VariableBank bank, Type type)
        {
            string pathName = AssetDatabase.GetAssetPath(bank);

            AddressableAssetSettings addressableSettings = GetSettings();
            AddressableAssetGroup addressableGroup = GetGroup(addressableSettings);

            AddressableAssetEntry addressableEntry = addressableSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(pathName),
                addressableGroup, true, true);

            addressableEntry.SetLabel("variable-banks", true);

            bank.OnDestroyAction += () =>
            {
                string bankPathName = AssetDatabase.GetAssetPath(bank);
                string bankGuid = AssetDatabase.AssetPathToGUID(bankPathName);

                AddressableAssetEntry bankEntry = addressableGroup.GetAssetEntry(bankGuid, true);

                if (bankEntry != null) addressableGroup.RemoveAssetEntry(bankEntry, true);
                VariableBankDatabase.Refresh();
            };

            return true;
        }

        public bool ResetCreationProperties(VariableBank bank, Type type)
        {
            string pathName = AssetDatabase.GetAssetPath(bank);

            AddressableAssetSettings addressableSettings = GetSettings();
            AddressableAssetGroup addressableGroup = GetGroup(addressableSettings);

            AddressableAssetEntry addressableEntry =
                addressableGroup.GetAssetEntry(AssetDatabase.AssetPathToGUID(pathName), true);

            if (addressableEntry != null) addressableGroup.RemoveAssetEntry(addressableEntry, true);

            return true;
        }

        public bool OverrideBankModeChangeDialogMessage(bool internalizing, out string message)
        {
            message = string.Empty;
            return false;
        }

        static AddressableAssetSettings GetSettings()
        {
            AddressableAssetSettings addressableSettings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            return addressableSettings;
        }

        static AddressableAssetGroup GetGroup(AddressableAssetSettings settings)
        {
            AddressableAssetGroup addressableGroup = settings.FindGroup("VariableBanks");
            if (addressableGroup == null || !addressableGroup.ReadOnly)
            {
                addressableGroup = settings.CreateGroup("VariableBanks", false, true, true, null);
            }

            return addressableGroup;
        }
    }
}

#endif