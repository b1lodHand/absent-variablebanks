using com.absence.variablebanks.internals;
using com.absence.variablesystem.editor;
using UnityEditor;

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
    }
}