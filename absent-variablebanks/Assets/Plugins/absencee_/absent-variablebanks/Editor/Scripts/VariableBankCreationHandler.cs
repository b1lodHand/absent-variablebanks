using com.absence.variablebanks.internals;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using System;
using com.absence.variablebanks.editor.internals.assetmanagement;


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
        static void CreateVariableBankForExternalUse_MenuItem()
        {
            CreateVariableBankAtSelection(true, true);
        }

        public static void ValidateResourcesPath()
        {
            if (!AssetManagementAPIDatabase.CurrentAPI.DisplayName.Equals("Resources")) return;

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder($"Assets/Resources/{Constants.K_RESOURCES_PATH}"))
                AssetDatabase.CreateFolder("Assets/Resources", Constants.K_RESOURCES_PATH);
        }

        public static void CreateVariableBankAtPath(string path, bool forExternalUse, bool nameEdit = true, Action<VariableBank, bool> onEndAction = null)
        {
            if (!nameEdit)
            {
                VariableBank itemCreated = CreateBankAssetAt(path, forExternalUse);
                if (!forExternalUse) ApplyProperties(itemCreated);
                return;
            }

            var icon = EditorGUIUtility.IconContent("d_ScriptableObject Icon").image as Texture2D;

            CreateVariableBankEndNameEditAction create = ScriptableObject.CreateInstance<CreateVariableBankEndNameEditAction>();
            if (onEndAction != null) create.onEndAction = onEndAction;
            create.forExternalUse = forExternalUse;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, create, path, icon, null);
        }

        public static void CreateVariableBankAtSelection(bool forExternalUse, bool nameEdit = true, Action<VariableBank, bool> onEndAction = null)
        {
            string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (selectedPath == string.Empty) return;

            while ((!AssetDatabase.IsValidFolder(selectedPath)))
            {
                TrimLastSlash(ref selectedPath);
            }

            var path = Path.Combine(selectedPath, "New VariableBank.asset");

            CreateVariableBankAtPath(path, forExternalUse, nameEdit, onEndAction);
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

        public static void ApplyProperties(VariableBank bank)
        {
            AssetManagementAPIDatabase.CurrentAPI.APIObject.ApplyCreationProperties(bank, typeof(VariableBank));
        }

        public static VariableBank CreateBankAssetAt(string pathName, bool forExternalUse)
        {
            VariableBank bank = ScriptableObject.CreateInstance<VariableBank>();

            AssetDatabase.CreateAsset(bank, pathName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            bank.ForExternalUse = forExternalUse;

            return bank;
        }

        public static bool DestroyBankAssetAt(string pathName)
        {
            VariableBank bank = AssetDatabase.LoadAssetAtPath<VariableBank>(pathName);

            if (bank == null) return false;

            ScriptableObject.DestroyImmediate(bank);
            VariableBankDatabase.Refresh();

            return true;
        }

        public static bool DestroyBankAssetWithId(int instanceId)
        {
            VariableBank bank = EditorUtility.InstanceIDToObject(instanceId) as VariableBank;

            if (bank == null) return false;

            ScriptableObject.DestroyImmediate(bank);
            VariableBankDatabase.Refresh();

            return true;
        }

        internal class CreateVariableBankEndNameEditAction : EndNameEditAction
        {
            public bool forExternalUse { get; set; }
            public Action<VariableBank, bool> onEndAction { get; set; }

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                VariableBank itemCreated = CreateBankAssetAt(pathName, forExternalUse);

                ApplyProperties(itemCreated);

                Selection.activeObject = itemCreated;

                onEndAction?.Invoke(itemCreated, true);
            }

            public override void Cancelled(int instanceId, string pathName, string resourceFile)
            {
                DestroyBankAssetWithId(instanceId);

                onEndAction?.Invoke(null, false);
            }
        }
    }

}