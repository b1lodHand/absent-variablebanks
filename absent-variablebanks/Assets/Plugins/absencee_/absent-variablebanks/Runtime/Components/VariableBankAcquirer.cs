using com.absence.variablebanks.internals;
using System;
using UnityEngine;

namespace com.absence.variablebanks
{
    public class VariableBankAcquirer : MonoBehaviour
    {
        [SerializeField] private string m_targetGuid;
        public string TargetGuid => m_targetGuid;

        [SerializeField] private VariableBank m_bank;
        public VariableBank Bank
        {
            get
            {
                if (!Application.isPlaying) throw new Exception("You cannot retrive the bank directly from an acquirer in editor. Use TargetGuid instead.");
                if (!VariableBanksCloningHandler.CloningCompleted) throw new Exception("Cloning has not yet been completed.");

                return m_bank;
            }
        }

        private void Awake()
        {
            VariableBanksCloningHandler.AddCloningCompleteCallbackOrInvoke(FetchFixedBank);
        }

        private void FetchFixedBank()
        {
            m_bank = VariableBank.GetInstance(m_targetGuid);
        }
    }
}