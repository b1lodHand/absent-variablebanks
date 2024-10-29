using com.absence.variablebanks.internals;
using com.absence.variablesystem.banksystembase.editor;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor
{
    /// <summary>
    /// The static class responsible for handling the editor-side things of this package.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorJobsHelper
    {
        static EditorJobsHelper()
        {
            if (VariableBanksCloningHandler.CloningCompleted) RefreshDatabase();
            else
            {
                VariableBanksCloningHandler.OnCloningCompleted -= RefreshDatabase;
                VariableBanksCloningHandler.OnCloningCompleted += RefreshDatabase;
            }
        }

        private static void RefreshDatabase()
        {
            VariableBankDatabase.Refresh();
        }

        [MenuItem("absencee_/absent-variablebanks/Refresh VariableBank Database")]
        static void RefreshVBDatabase()
        {
            VariableBankDatabase.Refresh();

            StringBuilder sb = new StringBuilder();
            sb.Append("<b>[VBDATABASE] Banks found in assets: </b>");

            VariableBankDatabase.BanksInAssets.ForEach(bankAsset =>
            {
                sb.Append("\n\t");

                sb.Append("<color=white>");
                sb.Append("-> ");
                sb.Append(bankAsset.name);
                sb.Append("</color>");

                sb.Append($" [Guid: <color=white>{bankAsset.Guid}</color>]");
            });

            Debug.Log(sb.ToString());
        }
    }
}