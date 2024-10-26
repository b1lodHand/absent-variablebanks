using com.absence.variablebanks.internals;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace com.absence.variablebanks.editor
{
    [InitializeOnLoad]
    public static class SymbolInitializer
    {
        static SymbolInitializer()
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