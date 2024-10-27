using com.absence.variablesystem;
using System.Collections.Generic;

namespace com.absence.savesystem.variablebanks
{
    [System.Serializable]
    public class TempVariableBankData
    {
        public int IntCount = 0;
        public int FloatCount = 0;
        public int StringCount = 0;
        public int BooleanCount = 0;

        public List<Variable_Integer> Ints = new();
        public List<Variable_Float> Floats = new();
        public List<Variable_String> Strings = new();
        public List<Variable_Boolean> Booleans = new();

        public TempVariableBankData()
        {
            
        }

        public TempVariableBankData(VariableBank copyFrom)
        {
            Ints = new(copyFrom.Ints);
            Floats = new(copyFrom.Floats);
            Strings = new(copyFrom.Strings);
            Booleans = new(copyFrom.Booleans);

            IntCount = Ints.Count;
            FloatCount = Floats.Count;
            StringCount = Strings.Count;
            BooleanCount = Booleans.Count;
        }
    }

}