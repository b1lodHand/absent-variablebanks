namespace com.absence.variablebanks.internals
{
    /// <summary>
    /// The static class responsible for holding the constants variables of the package.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The resources path of variable banks if you're using <b>Resources API</b> as the asset management tool.
        /// </summary>
        public const string K_RESOURCES_PATH = "VariableBanks";

        /// <summary>
        /// The addressables label of variable banks if you're using <b>Addressables</b> as the asset management tool.
        /// </summary>
        public const string K_ADDRESSABLES_TAG = "variable-banks";

        /// <summary>
        /// The scripting define symbol used in Player Settings to compile the code associated with the Addressables Package without errors.
        /// </summary>
        public const string K_SCRIPTING_DEFINE_SYMBOL = "ABSENT_VB_ADDRESSABLES";

        /// <summary>
        /// If true, all VariableBanks (except the ones marked as 'For External Use') will get cloned right before the splash screen.
        /// </summary>
        public const bool K_CLONE_AUTOMATICALLY = true;

        /// <summary>
        /// If true, some internal information will get printed on console when specific events occur.
        /// </summary>
        public const bool K_DEBUG_MODE = true;
    }
}
