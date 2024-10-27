using com.absence.variablebanks.internals;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using static com.absence.variablebanks.editor.PackageSettings;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace com.absence.variablebanks.editor
{
    [InitializeOnLoad]
    public static class SymbolInitializer
    {
        const string k_adressables_package_name = "com.unity.addressables";

        static SymbolInitializer()
        {
            Refresh();
        }

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