using com.absence.variablebanks.internals;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using static com.absence.variablebanks.editor.PackageSettings;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The static class responsible for managing the scripting define symbols for this package (in Player Settings).
    /// </summary>
    [InitializeOnLoad]
    public static class SymbolInitializer
    {
        const string k_adressables_package_name = "com.unity.addressables";

        static SymbolInitializer()
        {
            Refresh();
        }

        /// <summary>
        /// Use to check if any define symbols are missing or extra, rewrite and recompile if needed.
        /// </summary>
        public static void Refresh()
        {
            List<PackageInfo> packages = PackageInfo.GetAllRegisteredPackages().ToList();
            bool addressablesExists = packages.Any(package => package.name.Equals(k_adressables_package_name));

            if (!addressablesExists)
            {
                RemoveAddressablesLabelIfExists();
                return;
            }

            if (PackageSettings.instance.AssetManagementAPISelection != AssetManagementConstants.K_ADDRESSABLES_INDEX)
            {
                RemoveAddressablesLabelIfExists();
                return;
            }

            AddAddressablesLabelOnce();
        }

        /// <summary>
        /// Use to remove addressables define symbol from Player Settings.
        /// </summary>
        public static void RemoveAddressablesLabelIfExists()
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup, out string[] defines);
            List<string> symbols = defines.ToList();
            string symbol = Constants.K_SCRIPTING_DEFINE_SYMBOL;

            bool contains = symbols.Any(sym => sym.Equals(symbol));
            if (!contains) return;

            symbols.Remove(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols.ToArray());
        }

        /// <summary>
        /// Use to add addressables define symbol to Player Settings.
        /// </summary>
        public static void AddAddressablesLabelOnce()
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup, out string[] defines);
            List<string> symbols = defines.ToList();
            string symbol = Constants.K_SCRIPTING_DEFINE_SYMBOL;

            bool contains = symbols.Any(sym => sym.Equals(symbol));
            if (contains) return;

            symbols.Add(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols.ToArray());
        }
    }

}