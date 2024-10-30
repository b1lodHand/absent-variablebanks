using com.absence.variablesystem.banksystembase;
using System;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    public abstract class AssetManagementAPI : IAssetManagementAPI
    {
        public AssetManagementAPI()
        {
        }

        public abstract bool ShowInSettings();
        public abstract void ApplyCreationProperties(VariableBank bank, Type type);
        public abstract void ApplyDestroyingProperties(VariableBank bank, Type type);
    }
}
