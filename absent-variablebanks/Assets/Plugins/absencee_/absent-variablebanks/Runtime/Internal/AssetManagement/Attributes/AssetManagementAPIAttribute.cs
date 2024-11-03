using System;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AssetManagementAPIAttribute : Attribute
    {
        public string displayName { get; private set; }

        public AssetManagementAPIAttribute(string displayName)
        {
            this.displayName = displayName;
        }
    }
}
