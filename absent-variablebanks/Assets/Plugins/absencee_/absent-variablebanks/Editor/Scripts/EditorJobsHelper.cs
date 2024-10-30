using com.absence.variablebanks.editor.internals.assetmanagement;
using com.absence.variablebanks.internals;
using com.absence.variablesystem.banksystembase.editor;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The static class responsible for handling the editor-side things of this package.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorJobsHelper
    {
        static EditorJobsHelper()
        {
            if (VariableBanksCloningHandler.CloningCompleted) RefreshDatabase();
            else
            {
                VariableBanksCloningHandler.OnCloningCompleted -= RefreshDatabase;
                VariableBanksCloningHandler.OnCloningCompleted += RefreshDatabase;
            }
        }

        private static void RefreshDatabase()
        {
            VariableBankDatabase.Refresh();
        }

        [MenuItem("absencee_/absent-variablebanks/Refresh VariableBank Database")]
        static void RefreshVBDatabase()
        {
            VariableBankDatabase.Refresh();

            StringBuilder sb = new StringBuilder();
            sb.Append("<b>[VBDATABASE] Banks found in assets: </b>");

            VariableBankDatabase.BanksInAssets.ForEach(bankAsset =>
            {
                sb.Append("\n\t");

                sb.Append("<color=white>");
                sb.Append("-> ");
                sb.Append(bankAsset.name);
                sb.Append("</color>");

                sb.Append($" [Guid: <color=white>{bankAsset.Guid}</color>]");
            });

            Debug.Log(sb.ToString());
        }

#if ABSENT_VB_ADDRESSABLES
        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (Addressables)", priority = 0)]
        static void CreateVariableBankForAddressables_MenuItem()
        {
            VariableBankCreationHandler.CreateVariableBankAtSelection(false, true);
        }
#endif

        [MenuItem("absencee_/absent-variablebanks/Create Variable Bank (Resources)")]
        static void CreateVariableBankForResources_MenuItem()
        {
            VariableBankCreationHandler.ValidateResourcesPath();

            var path = Path.Combine("Assets/Resources", Constants.K_RESOURCES_PATH, "New VariableBank.asset");
            VariableBankCreationHandler.CreateVariableBankAtPath(path, false, true);
        }

#if ABSENT_VB_ADDRESSABLES
        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (Addressables)", validate = true, priority = 0)]
        static bool CreateVariableBankForAddressables_MenuItemValidation()
        {
            return AssetManagementAPIDatabase.CurrentAPI.DisplayName.Equals("Addressables");
        }
#endif

        [MenuItem("absencee_/absent-variablebanks/Create Variable Bank (Resources)", validate = true)]
        static bool CreateVariableBankForResources_MenuItemValidation()
        {
            return AssetManagementAPIDatabase.CurrentAPI.DisplayName.Equals("Resources");
        }
    }
}