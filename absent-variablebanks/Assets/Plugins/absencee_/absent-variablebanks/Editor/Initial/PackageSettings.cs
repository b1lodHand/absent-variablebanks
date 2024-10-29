using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The scriptable singleton responsible for holding the settings (can be set in Project Settings) of this package.
    /// </summary>
    [FilePath("ProjectSettings/absent-variablebanks-settings.assets", FilePathAttribute.Location.ProjectFolder)]
    public class PackageSettings : ScriptableSingleton<PackageSettings>
    {
        /// <summary>
        /// Constants for asset management.
        /// </summary>
        public static class AssetManagementConstants
        {
            public const int K_RESOURCES_INDEX = 0;
            public const int K_ADDRESSABLES_INDEX = 1;
        }

        [SerializeField] private int m_assetApiSelection = 0;
        
        /// <summary>
        /// Selection index of asset management API selection.
        /// </summary>
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
