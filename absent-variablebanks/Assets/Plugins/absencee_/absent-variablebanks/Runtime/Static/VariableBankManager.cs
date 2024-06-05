using com.absence.variablebanks.internals;
using com.absence.variablesystem;

namespace com.absence.variablebanks
{
    /// <summary>
    /// The static class responsible for wrapping internal functions into simpler ones.
    /// </summary>
    public static class VariableBankManager
    {
        /// <summary>
        /// Use to get a clone bank with a specific Guid.
        /// </summary>
        /// <param name="withGuid">Target guid.</param>
        /// <returns>The clone bank.</returns>
        public static VariableBank GetInstance(string withGuid) => VariableBanksCloningHandler.GetCloneWithGuid(withGuid);
    }
}