using com.absence.variablebanks.internals;
using com.absence.variablesystem;
using com.absence.variablesystem.editor;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

#if ABSENT_VB_ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The static class responsible for handling variable bank creation via editor menu.
    /// </summary>
    public static class VariableBankCreationHandler
    {
        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (For External Use)", priority = 0)]
        static void CreateVariableBankForExternalUse()
        {
            CreateVariableBank(true);
        }

        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (Addressables)", validate = true)]
        static bool CreateVariableBank_Addressables_Validation()
        {
#if ABSENT_VB_ADDRESSABLES
            return true;
#else
            return false;
#endif
        }

        [MenuItem("absencee_/absent-variablebanks/Create Variable Bank (Resources)", validate = true)]
        static bool CreateVariableBank_ResourcesAPI_Validation()
        {
#if !ABSENT_VB_ADDRESSABLES
            return true;
#else
            return false;
#endif
        }

        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (Addressables)", priority = 0)]
        static void CreateVariableBank_Addressables()
        {
            CreateVariableBank(false, true);
        }

        [MenuItem("absencee_/absent-variablebanks/Create Variable Bank (Resources)")]
        static void CreateVariableBank_ResourcesAPI()
        {
            var path = Path.Combine("Assets/Resources", Constants.K_RESOURCES_PATH, "New VariableBank.asset");
            CreateVariableBankAtPath(path, false, false);
        }

        static void CreateVariableBankAtPath(string path, bool forExternalUse, bool addressable = false)
        {
            CreateVariableBankEndNameEditAction create = ScriptableObject.CreateInstance<CreateVariableBankEndNameEditAction>();
            create.forExternalUse = forExternalUse;
            create.setupForAddressables = addressable;
            var icon = EditorGUIUtility.IconContent("d_ScriptableObject Icon").image as Texture2D;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, create, path, icon, null);
        }
        static void CreateVariableBank(bool forExternalUse, bool addressable = false)
        {
            string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (selectedPath == string.Empty) return;

            while ((!AssetDatabase.IsValidFolder(selectedPath)))
            {
                TrimLastSlash(ref selectedPath);
            }

            var path = Path.Combine(selectedPath, "New VariableBank.asset");

            CreateVariableBankAtPath(path, forExternalUse, addressable);
        }

        private static void TrimLastSlash(ref string path)
        {
            int lastSlashIndex;
            for (lastSlashIndex = path.Length - 1; lastSlashIndex > 0; lastSlashIndex--)
            {
                if (path[lastSlashIndex] == '/') break;
            }

            path = path.Remove(lastSlashIndex, (path.Length - lastSlashIndex));
        }

        internal class CreateVariableBankEndNameEditAction : EndNameEditAction
        {
            public bool forExternalUse { get; set; }
            public bool setupForAddressables { get; set; }

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var itemCreated = ScriptableObject.CreateInstance<VariableBank>();

                AssetDatabase.CreateAsset(itemCreated, pathName);

                itemCreated.ForExternalUse = forExternalUse;

#if ABSENT_VB_ADDRESSABLES
                if (setupForAddressables)
                {
                    AddressableAssetSettings addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

                    AddressableAssetGroup addressableGroup = addressableSettings.FindGroup(Constants.K_RESOURCES_PATH);
                    if (addressableGroup == null || !addressableGroup.ReadOnly)
                    {
                        addressableGroup = addressableSettings.CreateGroup(Constants.K_RESOURCES_PATH, false, true, true, null);
                    }

                    AddressableAssetEntry addressableEntry = addressableSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(pathName), 
                        addressableGroup, true, true);

                    addressableEntry.SetLabel(Constants.K_ADDRESSABLES_TAG, true);

                    itemCreated.OnDestroyAction += () =>
                    {
                        addressableGroup.RemoveAssetEntry(addressableEntry);
                    };
                }
#endif

                itemCreated.OnDestroyAction += () =>
                {
                    VariableBankDatabase.Refresh();
                };

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                VariableBankDatabase.Refresh();

                Selection.activeObject = itemCreated;
            }

            public override void Cancelled(int instanceId, string pathName, string resourceFile)
            {
                VariableBank item = EditorUtility.InstanceIDToObject(instanceId) as VariableBank;
                ScriptableObject.DestroyImmediate(item);

                VariableBankDatabase.Refresh();
            }
        }
    }

}