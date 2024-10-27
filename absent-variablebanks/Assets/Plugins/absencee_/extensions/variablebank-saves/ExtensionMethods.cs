using com.absence.variablesystem;

namespace com.absence.savesystem.variablebanks
{
    public static class ExtensionMethods
    {
        public static void Load(this VariableBank bank, TempVariableBankData dataToLoad)
        {
            if (!bank.IsClone) throw new System.Exception("You cannot load data to a non-clone variable bank.");

            bank.Ints = new(dataToLoad.Ints);
            bank.Floats = new(dataToLoad.Floats);
            bank.Strings = new(dataToLoad.Strings);
            bank.Booleans = new(dataToLoad.Booleans);
        }
    }

}