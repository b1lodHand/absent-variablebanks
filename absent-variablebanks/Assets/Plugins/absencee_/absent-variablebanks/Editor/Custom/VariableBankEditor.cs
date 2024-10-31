using com.absence.variablebanks.editor;
using com.absence.variablebanks.editor.internals.assetmanagement;
using com.absence.variablebanks.internals;
using com.absence.variablesystem.banksystembase;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks
{
    [CustomEditor(typeof(VariableBank), true, isFallback = false)]
    public class VariableBankEditor : Editor
    {
        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            VariableBank bank = (VariableBank)target;

            DrawModeSelectionButton();

            return;

            //void DrawAPIChangeButton()
            //{

            //}

            void DrawModeSelectionButton()
            {
                Rect rect = new Rect(490, 28, 62, 18);

                GUIStyle style = new(GUI.skin.button);
                style.alignment = TextAnchor.LowerCenter;
                style.fontStyle = FontStyle.Bold;

                bool forExternalUse = bank.ForExternalUse;

                Color prevColor = GUI.backgroundColor;
                GUI.backgroundColor = forExternalUse ? Color.red : Color.green;

                string buttonText = forExternalUse ? "External" : "Internal";
                string buttonTooltip = forExternalUse ? "Switch to internal usage" : "Switch to external usage";

                GUIContent buttonContent = new GUIContent()
                {
                    text = buttonText,
                    tooltip = buttonTooltip,
                };

                bool systemBusy = VariableBankCreationHandler.TransferringBank;
                if (systemBusy) GUI.enabled = false;

                IAssetManagementAPI api = AssetManagementAPIDatabase.CurrentAPI.APIObject;
                if (GUI.Button(rect, buttonContent, style))
                {
                    string title = forExternalUse ? "Mark this bank as 'Internal' ?" :
                                                    "Mark this bank as 'External' ?";

                    string originalMessage = forExternalUse ? Constants.K_DEFAULT_MODE_CHANGE_DIALOG_INT :
                        Constants.K_DEFAULT_MODE_CHANGE_DIALOG_EXT;

                    string message = api.OverrideBankModeChangeDialogMessage(forExternalUse, out string customMessage) ?
                        customMessage : originalMessage;

                    bool confirm = EditorUtility.DisplayDialog(title,
                        message,
                        "Ok", "Cancel");

                    if (confirm)
                    {
                        if (forExternalUse) VariableBankCreationHandler.MakeInternal(bank);
                        else VariableBankCreationHandler.MakeExternal(bank);

                        EditorGUIUtility.PingObject(bank);
                    }
                }

                GUI.enabled = true;
                GUI.backgroundColor = prevColor;
            }
        }

        public override void OnInspectorGUI()
        {
            VariableBank bank = (VariableBank)target;

            GUI.enabled = false;

            EditorGUILayout.LabelField($"Name: {bank.name}");
            EditorGUILayout.LabelField($"Guid: {bank.Guid}");

            GUI.enabled = true;

            SerializedObject serializedObject = new SerializedObject(bank);
            serializedObject.Update();

            SerializedProperty intListProp = serializedObject.FindProperty("m_ints");
            SerializedProperty floatListProp = serializedObject.FindProperty("m_floats");
            SerializedProperty stringListProp = serializedObject.FindProperty("m_strings");
            SerializedProperty booleanListProp = serializedObject.FindProperty("m_booleans");

            GUIContent intListHeader = new GUIContent()
            {
                text = intListProp.displayName,
                tooltip = intListProp.tooltip,
            };

            GUIContent floatListHeader = new GUIContent()
            {
                text = floatListProp.displayName,
                tooltip = floatListProp.tooltip,
            };

            GUIContent stringListHeader = new GUIContent()
            {
                text = stringListProp.displayName,
                tooltip = stringListProp.tooltip,
            };

            GUIContent booleanListHeader = new GUIContent()
            {
                text = booleanListProp.displayName,
                tooltip = booleanListProp.tooltip,
            };

            Undo.RecordObject(bank, "Variable Bank (Editor)");
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(intListProp, intListHeader, true);
            EditorGUILayout.PropertyField(floatListProp, floatListHeader, true);
            EditorGUILayout.PropertyField(stringListProp, stringListHeader, true);
            EditorGUILayout.PropertyField(booleanListProp, booleanListHeader, true);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(bank);
            }
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}
