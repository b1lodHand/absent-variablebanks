using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    [FilePath("ProjectSettings/absent-variablebanks-settings.assets", FilePathAttribute.Location.ProjectFolder)]
    public class PackageSettings : ScriptableSingleton<PackageSettings>
    {
        public static class AssetManagementConstants
        {
            public const int K_RESOURCES_INDEX = 0;
            public const int K_ADDRESSABLES_INDEX = 1;
        }

        [SerializeField] private int m_assetApiSelection = 0;
        
        public int AssetManagementAPISelection
        {
            get
            {
                return m_assetApiSelection;
            }

            set
            {
                m_assetApiSelection = value;
                Save(true);

                SymbolInitializer.Refresh();
            }
        }
    }
}
