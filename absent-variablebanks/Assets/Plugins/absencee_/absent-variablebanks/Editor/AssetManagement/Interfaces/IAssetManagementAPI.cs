using com.absence.variablesystem.banksystembase;
using System;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    public interface IAssetManagementAPI
    {
        bool ShowInSettings();
        void ApplyCreationProperties(VariableBank bank, Type type);
    }
}
