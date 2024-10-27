using com.absence.variablesystem;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.absence.savesystem.variablebanks
{
    public class VariableBankSerializationSurrogate : ISerializationSurrogate
    {
        const string k_name = "vbname";
        const string k_ints = "ints";
        const string k_floats = "floats";
        const string k_strings = "strings";
        const string k_booleans = "booleans";

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            TempVariableBankData vbData = (TempVariableBankData)obj;

            int intCount = vbData.IntCount;
            int floatCount = vbData.FloatCount;
            int stringCount = vbData.StringCount;
            int booleanCount = vbData.BooleanCount;

            info.AddValue(k_ints, intCount);
            info.AddValue(k_floats, floatCount);
            info.AddValue(k_strings, stringCount);
            info.AddValue(k_booleans, booleanCount);

            int pointer = 0;
            vbData.Ints.ForEach(variable =>
            {
                info.AddValue(pointer.ToString(), variable.Value);
                info.AddValue($"{pointer.ToString()}:name", variable.Name);
                pointer++;
            });

            vbData.Floats.ForEach(variable =>
            {
                info.AddValue(pointer.ToString(), variable.Value);
                info.AddValue($"{pointer.ToString()}:name", variable.Name);
                pointer++;
            });

            vbData.Strings.ForEach(variable =>
            {
                info.AddValue(pointer.ToString(), variable.Value);
                info.AddValue($"{pointer.ToString()}:name", variable.Name);
                pointer++;
            });

            vbData.Booleans.ForEach(variable =>
            {
                info.AddValue(pointer.ToString(), variable.Value);
                info.AddValue($"{pointer.ToString()}:name", variable.Name);
                pointer++;
            });
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            TempVariableBankData vbData = (TempVariableBankData)obj;

            string name = (string)info.GetValue(k_name, typeof(string));
            int intCount = (int)info.GetValue(k_ints, typeof(int));
            int floatCount = (int)info.GetValue(k_floats, typeof(int));
            int stringCount = (int)info.GetValue(k_strings, typeof(int));
            int booleanCount = (int)info.GetValue(k_booleans, typeof(int));

            vbData.IntCount = intCount;
            vbData.FloatCount = floatCount;
            vbData.StringCount = stringCount;
            vbData.BooleanCount = booleanCount;

            int sum1 = intCount + floatCount;
            int sum2 = intCount + floatCount + stringCount;
            int sum3 = intCount + floatCount + stringCount + booleanCount;

            List<Variable_Integer> ints = new();
            List<Variable_Float> floats = new();
            List<Variable_String> strings = new();
            List<Variable_Boolean> booleans = new();

            ReadValues();

            vbData.Ints = ints;
            vbData.Floats = floats;
            vbData.Strings = strings;
            vbData.Booleans = booleans;

            return vbData;

            void ReadValues()
            {
                if (sum3 == 0) return;

                // ints.
                for (int pointer = 0; pointer < intCount; pointer++)
                {
                    string name = (string)info.GetValue($"{pointer.ToString()}:name", typeof(string));
                    int value = (int)info.GetValue(pointer.ToString(), typeof(int));

                    ints.Add(new Variable_Integer(name, value));
                }

                if (intCount == sum3) return;

                // floats.
                for (int pointer = intCount; pointer < sum1; pointer++)
                {
                    string name = (string)info.GetValue($"{pointer.ToString()}:name", typeof(string));
                    float value = (float)info.GetValue(pointer.ToString(), typeof(float));

                    floats.Add(new Variable_Float(name, value));
                }

                if (sum1 == sum3) return;

                // strings.
                for (int pointer = sum1; pointer < sum2; pointer++)
                {
                    string name = (string)info.GetValue($"{pointer.ToString()}:name", typeof(string));
                    string value = (string)info.GetValue(pointer.ToString(), typeof(string));

                    strings.Add(new Variable_String(name, value));
                }

                if (sum2 == sum3) return;

                // booleans.
                for (int pointer = sum2; pointer < sum3; pointer++)
                {
                    string name = (string)info.GetValue($"{pointer.ToString()}:name", typeof(string));
                    bool value = (bool)info.GetValue(pointer.ToString(), typeof(bool));

                    booleans.Add(new Variable_Boolean(name, value));
                }
            }
        }
    }

}