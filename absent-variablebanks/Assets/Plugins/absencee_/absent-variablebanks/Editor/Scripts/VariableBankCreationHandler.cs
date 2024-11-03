using com.absence.variablebanks.internals;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The static class responsible for handling variable bank creation via editor menu.
    /// </summary>
    public static class VariableBankCreationHandler
    {
        static bool m_transferringBank;
        public static bool TransferringBank => m_transferringBank;

        [MenuItem("Assets/Create/absencee_/absent-variablebanks/Variable Bank (For External Use)", priority = 0)]
        static void CreateVariableBankForExternalUse_MenuItem()
        {
            CreateVariableBankAtSelection(true, true);
        }

        public static void ValidateResourcesPath()
        {
            if (!PackageSettings.instance.CurrentAPI.DisplayName.Equals("Resources")) return;

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

                if (forExternalUse) MakeExternal(itemCreated);
                else MakeInternal(itemCreated);

                onEndAction?.Invoke(itemCreated, true);

                EditorGUIUtility.PingObject(itemCreated);
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

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

        public static bool MakeInternal(VariableBank bank)
        {
            if (bank == null) return false;

            m_transferringBank = true;

            bool success = PackageSettings.instance.CurrentAPI.EditorExtensions.ApplyCreationProperties(bank, typeof(VariableBank));

            if (success) bank.ForExternalUse = false;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();

            m_transferringBank = false;

            Selection.activeObject = bank;

            return success;
        }

        public static bool MakeExternal(VariableBank bank)
        {
            if (bank == null) return false;

            m_transferringBank = true;

            bool success = PackageSettings.instance.CurrentAPI.EditorExtensions.ResetCreationProperties(bank, typeof(VariableBank));

            if (success) bank.ForExternalUse = true;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();

            m_transferringBank = false;

            Selection.activeObject = bank;

            return success;
        }

        public static bool MakeInternal(List<VariableBank> banks)
        {
            if (m_transferringBank) return false;
            if (banks == null) return false;
            if (banks.Count == 0) return false;
            if (banks.Count == 1) return MakeInternal(banks.FirstOrDefault());

            m_transferringBank = true;

            bool success = true;
            foreach (var bank in banks)
            {
                bool successLocal = PackageSettings.instance.CurrentAPI.EditorExtensions.ApplyCreationProperties(bank, typeof(VariableBank));
                if (successLocal) bank.ForExternalUse = false;
                else success = false;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();

            m_transferringBank = false;

            Selection.objects = banks.ToArray();

            return success;
        }

        public static bool MakeExternal(List<VariableBank> banks)
        {
            if (m_transferringBank) return false;
            if (banks == null) return false;
            if (banks.Count == 0) return false;
            if (banks.Count == 1) return MakeExternal(banks.FirstOrDefault());

            m_transferringBank = true;

            bool success = true;
            foreach (var bank in banks)
            {
                bool successLocal = PackageSettings.instance.CurrentAPI.EditorExtensions.ResetCreationProperties(bank, typeof(VariableBank));
                if (successLocal) bank.ForExternalUse = true;
                else success = false;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VariableBankDatabase.Refresh();

            m_transferringBank = false;

            Selection.objects = banks.ToArray();

            return success;
        }

        internal static void MakeAllExternal()
        {
            List<VariableBank> allInternalBanks = AssetDatabase.FindAssets("t:VariableBank").ToList().
                ConvertAll(guid => AssetDatabase.LoadAssetAtPath<VariableBank>(AssetDatabase.GUIDToAssetPath(guid))).
                Where(bnk => !bnk.ForExternalUse).ToList();

            allInternalBanks.ForEach(bank =>
            {
                MakeExternal(bank);
            });

            EditorGUIUtility.PingObject(allInternalBanks.LastOrDefault());
        }

        internal class CreateVariableBankEndNameEditAction : EndNameEditAction
        {
            public bool forExternalUse { get; set; }
            public Action<VariableBank, bool> onEndAction { get; set; }

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                VariableBank itemCreated = CreateBankAssetAt(pathName, forExternalUse);

                if (forExternalUse) MakeExternal(itemCreated);
                else MakeInternal(itemCreated);

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