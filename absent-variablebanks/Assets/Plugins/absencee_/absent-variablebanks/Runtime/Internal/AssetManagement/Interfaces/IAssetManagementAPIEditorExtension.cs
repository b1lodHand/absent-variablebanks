using com.absence.variablesystem.banksystembase;
using System;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    public interface IAssetManagementAPIEditorExtension
    {
        bool ShowInSettings();
        void ApplyCreationProperties(VariableBank bank, Type type);
        void ResetCreationProperties(VariableBank bank, Type type);
        bool OverrideBankModeChangeDialogMessage(bool internalizing, out string message);
    }
}
