using System;

namespace com.absence.variablebanks.internals.assetmanagement
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AssetManagementAPIAttribute : Attribute
    {
        public string displayName { get; private set; }

        public AssetManagementAPIAttribute(string displayName)
        {
            this.displayName = displayName;
        }
    }
}
