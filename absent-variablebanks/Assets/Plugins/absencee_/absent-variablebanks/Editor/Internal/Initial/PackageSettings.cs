using com.absence.variablebanks.internals;
using com.absence.variablebanks.internals.assetmanagement;
using System.Linq;
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
        [SerializeField] private string m_assetApiSelection = "Resources";

        public APIRegistry CurrentAPI => AssetManagementAPIDatabase.APIs.FirstOrDefault(api => api.DisplayName.Equals(AssetManagementAPIName));

        /// <summary>
        /// Selection index of asset management API selection.
        /// </summary>
        public string AssetManagementAPIName
        {
            get
            {
                return m_assetApiSelection;
            }

            set
            {
                m_assetApiSelection = value;
                Save(true);

                RuntimeSettings.instance.AssetManagementAPIName = value;
                //RuntimeSettings.Save();
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
