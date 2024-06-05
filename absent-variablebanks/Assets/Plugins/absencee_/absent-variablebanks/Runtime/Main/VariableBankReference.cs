using com.absence.variablebanks.internals;
using System;
using UnityEngine;

namespace com.absence.variablebanks
{
    [System.Serializable]
    public class VariableBankReference
    {
        [SerializeField] private string m_targetGuid;
        public string TargetGuid => m_targetGuid;

        public VariableBank Bank
        {
            get
            {
                if (!Application.isPlaying) throw new Exception("You cannot retrive the bank directly from a reference in editor. Use TargetGuid instead.");
                if (!VariableBanksCloningHandler.CloningCompleted) throw new Exception("Cloning has not yet been completed.");

                return VariableBank.GetInstance(m_targetGuid);
            }
        }
    }

}