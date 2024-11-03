namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [System.Serializable]
    public class APIRegistry
    {
        public string DisplayName;
        public IAssetManagementAPI APIObject;
#if UNITY_EDITOR
        public IAssetManagementAPIEditorExtension EditorExtensions;
#endif
    }
}
