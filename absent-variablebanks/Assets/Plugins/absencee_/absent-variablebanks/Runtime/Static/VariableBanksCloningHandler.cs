using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.absence.variablebanks.internals
{
    public static class VariableBanksCloningHandler
    {
        private static Dictionary<string, VariableBank> m_bankTable = new();
        public static event Action OnCloningCompleted;
        public static bool CloningCompleted { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void CloneAllBanks()
        {
            m_bankTable.Clear();
            CloningCompleted = false;

            UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<VariableBank>(internals.Constants.K_ADDRESSABLES_TAG, null, true).Completed += asyncOperationHandle =>
            {
                if (asyncOperationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
                {
                    throw new Exception("Failed to load variable banks.");
                }

                else if (asyncOperationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    List<VariableBank> originalBanks = asyncOperationHandle.Result.ToList();

                    originalBanks.ForEach(bank =>
                    {
                        if (bank.IsClone) return;
                        if (bank.ForExternalUse) return;

                        VariableBank clonedBank = bank.Clone();

                        m_bankTable.Add(bank.GUID, clonedBank);

                        Debug.Log(clonedBank.name);
                    });

                    UnityEngine.AddressableAssets.Addressables.Release(asyncOperationHandle);

                    CloningCompleted = true;

                    OnCloningCompleted?.Invoke();
                    OnCloningCompleted = null;
                }
            };
        }

        public static bool AddCloningCompleteCallbackOrInvoke(Action callbackContext)
        {
            if (CloningCompleted)
            {
                callbackContext?.Invoke();
                return true;
            }

            OnCloningCompleted += callbackContext;
            return false;
        }

        internal static VariableBank GetCloneWithGuid(string guidOfOriginalBank)
        {
            if (!m_bankTable.ContainsKey(guidOfOriginalBank)) throw new Exception("Specified variablebank is not cloned by the internal system. " +
                "Check if it's included in the Addressables menu. And also check it's AvoidCloning property.");

            return m_bankTable[guidOfOriginalBank];
        }
    }

}