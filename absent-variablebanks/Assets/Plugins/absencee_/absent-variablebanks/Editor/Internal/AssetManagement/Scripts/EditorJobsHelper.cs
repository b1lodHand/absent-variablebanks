using com.absence.variablebanks.internals.assetmanagement;
using UnityEditor;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [InitializeOnLoad]
    public static class EditorJobsHelper
    {
        static EditorJobsHelper()
        {
            AssetManagementAPIDatabase.Refresh();
        }
    }
}
