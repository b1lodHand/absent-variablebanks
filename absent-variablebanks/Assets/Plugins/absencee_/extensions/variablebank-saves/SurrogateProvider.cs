using System.Runtime.Serialization;

namespace com.absence.savesystem.variablebanks
{
    static class SurrogateProvider
    {
        [SurrogateProviderMethod("Provider for VariableBanks")]
        public static void Provide(SurrogateSelector targetSelector)
        {
            VariableBankSerializationSurrogate variableBankSerializationSurrogate = new VariableBankSerializationSurrogate();
            targetSelector.AddSurrogate(typeof(VariableBankSerializationSurrogate), new StreamingContext(StreamingContextStates.All), variableBankSerializationSurrogate);
        }
    }

}