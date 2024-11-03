using System;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetManagementAPIEditorExtensionAttribute : Attribute
    {
        public Type targetApiType;

        public AssetManagementAPIEditorExtensionAttribute(Type targetApiType)
        {
            this.targetApiType = targetApiType;
        }
    }
}
