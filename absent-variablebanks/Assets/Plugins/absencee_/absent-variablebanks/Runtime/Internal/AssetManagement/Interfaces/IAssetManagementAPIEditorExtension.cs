using com.absence.variablesystem.banksystembase;
using System;

namespace com.absence.variablebanks.internals.assetmanagement
{
    public interface IAssetManagementAPIEditorExtension
    {
        bool ShowInSettings();
        bool ApplyCreationProperties(VariableBank bank, Type type);
        bool ResetCreationProperties(VariableBank bank, Type type);
        bool OverrideBankModeChangeDialogMessage(bool internalizing, out string message);
    }
}
