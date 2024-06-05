using com.absence.variablebanks.internals;

namespace com.absence.variablebanks
{
    [System.Serializable]
    public sealed class VariableSetter : BaseVariableSetter
    {
        public override bool HasFixedBank => false;

        public void SetBankGuid(string newBankGuid)
        {
            m_targetBankGuid = newBankGuid;
        }

        public VariableSetter Clone(string overrideBankGuid)
        {
            VariableSetter clone = new();

            clone.m_boolValue = m_boolValue;
            clone.m_floatValue = m_floatValue;
            clone.m_intValue = m_intValue;
            clone.m_stringValue = m_stringValue;

            clone.m_setType = m_setType;
            clone.m_targetVariableName = m_targetVariableName;

            clone.SetBankGuid(overrideBankGuid);

            return clone;
        }
        public VariableSetter Clone()
        {
            VariableSetter clone = new();

            clone.m_boolValue = m_boolValue;
            clone.m_floatValue = m_floatValue;
            clone.m_intValue = m_intValue;
            clone.m_stringValue = m_stringValue;

            clone.m_setType = m_setType;
            clone.m_targetVariableName = m_targetVariableName;

            clone.SetBankGuid(m_targetBankGuid);

            return clone;
        }
    }

}