using com.absence.variablebanks.internals;

namespace com.absence.variablebanks
{
    [System.Serializable]
    public sealed class VariableComparer : BaseVariableComparer
    {
        public override bool HasFixedBank => false;

        public void SetBankGuid(string newBankGuid)
        {
            m_targetBankGuid = newBankGuid;
        }

        public VariableComparer Clone(string overrideBankGuid)
        {
            VariableComparer clone = new();
            
            clone.m_boolValue = m_boolValue;
            clone.m_floatValue = m_floatValue;
            clone.m_intValue = m_intValue;
            clone.m_stringValue = m_stringValue;

            clone.m_comparisonType = m_comparisonType;
            clone.m_targetVariableName = m_targetVariableName;

            clone.SetBankGuid(overrideBankGuid);

            return clone;
        }
        public VariableComparer Clone()
        {
            VariableComparer clone = new();

            clone.m_boolValue = m_boolValue;
            clone.m_floatValue = m_floatValue;
            clone.m_intValue = m_intValue;
            clone.m_stringValue = m_stringValue;

            clone.m_comparisonType = m_comparisonType;
            clone.m_targetVariableName = m_targetVariableName;

            clone.SetBankGuid(m_targetBankGuid);

            return clone;
        }
    }
}