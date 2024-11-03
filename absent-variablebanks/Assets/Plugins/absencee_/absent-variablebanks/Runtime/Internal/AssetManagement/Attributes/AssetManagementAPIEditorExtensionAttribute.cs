using System;

namespace com.absence.variablebanks.internals.assetmanagement
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AssetManagementAPIEditorExtensionAttribute : Attribute
    {
        public Type targetApiType;

        public AssetManagementAPIEditorExtensionAttribute(Type targetApiType)
        {
            this.targetApiType = targetApiType;
        }
    }
}
