using com.absence.variablebanks.editor.internals.assetmanagement;
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
        [SerializeField] private bool m_disableModeSwitchPrompt = false;
        [SerializeField] private bool m_disableMultiModeSwitchPrompt = false;
        [SerializeField] private int m_assetApiSelection = 0;

        public APIRegistry CurrentAPI => AssetManagementAPIDatabase.APIs[AssetManagementAPISelection];

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

        /// <summary>
        /// If true, a dialog will show up when you try to switch mode of a bank in the editor.
        /// </summary>
        public bool DisableSingleBankModeSwitchPrompt
        {
            get
            {
                return m_disableModeSwitchPrompt;
            }

            set
            {
                m_disableModeSwitchPrompt = value;
                Save(true);
            }
        }

        /// <summary>
        /// If true, a dialog will show up when you try to switch mode of a bank in the editor.
        /// </summary>
        public bool DisableMultiBankModeSwitchPrompt
        {
            get
            {
                return m_disableMultiModeSwitchPrompt;
            }

            set
            {
                m_disableMultiModeSwitchPrompt = value;
                Save(true);
            }
        }
    }
}
