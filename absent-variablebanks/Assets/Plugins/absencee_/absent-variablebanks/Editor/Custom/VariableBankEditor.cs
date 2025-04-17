using com.absence.variablebanks.internals;
using com.absence.variablebanks.internals.assetmanagement;
using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    [CustomEditor(typeof(VariableBank), true, isFallback = false)]
    public class VariableBankEditor : Editor
    {
        List<ModeSwitchOperationHandle> m_awaitingOperations = new();
        Editor m_internalEditor;

        private void OnEnable()
        {
            Editor.CreateCachedEditor(target, typeof(VariableBankEditorBase), ref m_internalEditor);
        }

        private void OnDisable()
        {
            Editor.DestroyImmediate(m_internalEditor);
        }

        protected override void OnHeaderGUI()
        {
            if (m_awaitingOperations.Count > 0)
            {
                GUI.enabled = false;
                m_awaitingOperations.ForEach(op =>
                {
                    if (op.ToExternal) VariableBankCreationHandler.MakeExternal(op.Targets);
                    else VariableBankCreationHandler.MakeInternal(op.Targets);

                    op.Targets.ForEach(opTarget => EditorUtility.SetDirty(opTarget));

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    EditorGUIUtility.PingObject(op.Targets.LastOrDefault());
                });

                m_awaitingOperations.Clear();
                GUI.enabled = true;
            }

            base.OnHeaderGUI();

            VariableBank singleSelectedBank = (VariableBank)target;

            List<VariableBank> banksSelected = 
                targets.ToList().ConvertAll(target => (VariableBank)target).ToList();

            List<SerializedObject> serializedObjects = banksSelected.
                ConvertAll(bank => new SerializedObject(bank));
            
            SerializedObject serializedObject = serializedObjects.FirstOrDefault();

            serializedObjects.ForEach(obj => obj.Update());

            bool multipleBanksSelected = targets.Length > 1;
            bool systemBusy = VariableBankCreationHandler.TransferringBank;

            if (systemBusy) GUI.enabled = false;

            if (multipleBanksSelected) DrawMultiModeSwitchButton();
            else DrawSingleModeSwitchButton();

            if (systemBusy) GUI.enabled = true;

            return;

            void DrawMultiModeSwitchButton()
            {
                Rect total = EditorGUILayout.GetControlRect();
                EditorGUI.DrawRect(total, Color.white);
                Rect rect = new Rect(490, 28, 62, 18);

                GUIStyle style = new(GUI.skin.button);
                style.alignment = TextAnchor.LowerCenter;
                style.fontStyle = FontStyle.Bold;

                bool allExternal = banksSelected.All(bank => bank.ForExternalUse);
                bool allInternal = banksSelected.All(bank => !bank.ForExternalUse);
                bool mixed = (!allExternal) && (!allInternal);

                Color prevColor = GUI.backgroundColor;

                if (allExternal) GUI.backgroundColor = Color.red;
                else if (allInternal) GUI.backgroundColor = Color.green;
                else GUI.backgroundColor = Color.yellow;

                string buttonText = "Mixed";
                if (allExternal) buttonText = "External";
                else if (allInternal) buttonText = "Internal";

                string buttonTooltip = "Switch multiple banks' mode";

                GUIContent buttonContent = new GUIContent()
                {
                    text = buttonText,
                    tooltip = buttonTooltip,
                };

                IAssetManagementAPI api = PackageSettings.instance.CurrentAPI.APIObject;
                if (GUI.Button(rect, buttonContent, style))
                {
                    GenericMenu menu = new GenericMenu();

                    if (allInternal) menu.AddDisabledItem(new GUIContent("Make all Internal"));
                    else menu.AddItem(new GUIContent("Make all Internal"), false, MultiModeSwitch, false);

                    if (allExternal) menu.AddDisabledItem(new GUIContent("Make all External"));
                    else menu.AddItem(new GUIContent("Make all External"), false, MultiModeSwitch, true);

                    menu.ShowAsContext();
                }

                GUI.backgroundColor = prevColor;
            }

            void DrawSingleModeSwitchButton()
            {
                Rect rect = new Rect(490, 28, 62, 18);

                GUIStyle style = new(GUI.skin.button);
                style.alignment = TextAnchor.LowerCenter;
                style.fontStyle = FontStyle.Bold;

                bool forExternalUse = singleSelectedBank.ForExternalUse;

                Color prevColor = GUI.backgroundColor;
                GUI.backgroundColor = forExternalUse ? Color.red : Color.green;

                string buttonText = forExternalUse ? "External" : "Internal";
                string buttonTooltip = forExternalUse ? "Switch to internal usage" : "Switch to external usage";

                GUIContent buttonContent = new GUIContent()
                {
                    text = buttonText,
                    tooltip = buttonTooltip,
                };

                if (GUI.Button(rect, buttonContent, style))
                {
                    SingleModeSwitch(forExternalUse);
                }

                GUI.backgroundColor = prevColor;
            }

            void SingleModeSwitch(bool forExternalUse)
            {
                IAssetManagementAPIEditorExtension api = 
                    PackageSettings.instance.CurrentAPI.EditorExtensions;

                if (PackageSettings.instance.DisableSingleBankModeSwitchPrompt)
                {
                    AddOpHandle(singleSelectedBank, !forExternalUse);
                    return;
                }

                string title = forExternalUse ? "Mark this bank as 'Internal' ?" :
                                                    "Mark this bank as 'External' ?";

                string originalMessage = forExternalUse ? Constants.K_DEFAULT_MODE_CHANGE_DIALOG_INT :
                    Constants.K_DEFAULT_MODE_CHANGE_DIALOG_EXT;

                string message = api.OverrideBankModeChangeDialogMessage(forExternalUse, out string customMessage) ?
                    customMessage : originalMessage;

                int dialogOutput = EditorUtility.DisplayDialogComplex(title,
                    message,
                    "Ok", 
                    "Cancel",
                    "Don't ask again");

                switch (dialogOutput)
                {
                    case 0:
                        AddOpHandle(singleSelectedBank, !forExternalUse);
                        break;
                    case 2:
                        AddOpHandle(singleSelectedBank, !forExternalUse);
                        PackageSettings.instance.DisableSingleBankModeSwitchPrompt = true;
                        break;
                    default:
                        break;
                }
            }

            void MultiModeSwitch(object data)
            {
                bool toExternal = (bool)data;

                if (PackageSettings.instance.DisableMultiBankModeSwitchPrompt)
                {
                    AddOpHandle(banksSelected, toExternal);
                    return;
                }

                string originalMessage = "All VariableBanks selected will switch to the mode you've selected. Will you proceed?" +
                    "\n\nYou can't undo this action.";

                int dialogOutput = EditorUtility.DisplayDialogComplex("Switch modes of multiple banks?",
                    originalMessage,
                    "Ok", 
                    "Cancel",
                    "Don't ask again");

                switch (dialogOutput)
                {
                    case 0:
                        AddOpHandle(banksSelected, toExternal);
                        break;
                    case 2:
                        AddOpHandle(banksSelected, toExternal);
                        PackageSettings.instance.DisableMultiBankModeSwitchPrompt = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (m_internalEditor != null)
                m_internalEditor.OnInspectorGUI();
        }

        void AddOpHandle(List<VariableBank> targets, bool toExternal)
        {
            ModeSwitchOperationHandle handle = new()
            {
                ToExternal = toExternal,
                Targets = new(targets)
            };

            m_awaitingOperations.Add(handle);
        }

        void AddOpHandle(VariableBank target, bool toExternal)
        {
            ModeSwitchOperationHandle handle = new()
            {
                ToExternal = toExternal,
                Targets = new() { target }
            };

            m_awaitingOperations.Add(handle);
        }
    }

    [System.Serializable]
    public class ModeSwitchOperationHandle
    {
        public List<VariableBank> Targets;
        public bool ToExternal;
    }
}
