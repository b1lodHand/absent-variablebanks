using com.absence.variablesystem.banksystembase;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ABSENT_VB_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace com.absence.variablebanks.internals
{
    /// <summary>
    /// The static class responsible for cloning the banks at startup.
    /// </summary>
    public static class VariableBanksCloningHandler
    {
        private static Dictionary<string, VariableBank> m_bankTable = new();

        /// <summary>
        /// Action which will get invoked when cloning process gets completed successfully. It gets cleared automatically after invoking.
        /// </summary>
        public static event Action OnCloningCompleted;

        /// <summary>
        /// Use to check if the cloning process got completed successfully.
        /// </summary>
        public static bool CloningCompleted { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void CloneAll()
        {
#pragma warning disable CS0162 // Unreachable code detected
            if (!Constants.K_CLONE_AUTOMATICALLY) return;
#pragma warning restore CS0162 // Unreachable code detected

            m_bankTable.Clear();
            CloningCompleted = false;

            List<VariableBank> originalBanks = null;

#if ABSENT_VB_ADDRESSABLES
            Addressables.LoadAssetsAsync<VariableBank>(Constants.K_ADDRESSABLES_TAG, null, true)
                .Completed += asyncOperationHandle =>
            {
                if (asyncOperationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
                {
                    throw new Exception("Failed to load variable banks.");
                }
                else if (asyncOperationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    originalBanks = asyncOperationHandle.Result.ToList();

                    TraceList();
                    CleanUp();
                    InvokeEvents();

                    Addressables.Release(asyncOperationHandle);
                }
            };
#else
            originalBanks = Resources.LoadAll<VariableBank>(internals.Constants.K_RESOURCES_PATH).ToList(); 

            TraceList();

            originalBanks.ForEach(bank => Resources.UnloadAsset(bank));

            CleanUp();
            InvokeEvents();
#endif

            return;

            void TraceList()
            {
                originalBanks.ForEach(bank =>
                {
                    if (bank.IsClone) return;
                    if (bank.ForExternalUse) return;

                    VariableBank clonedBank = bank.Clone();

                    m_bankTable.Add(bank.Guid, clonedBank);
                    if (Constants.K_DEBUG_MODE) Debug.Log(clonedBank.name);
                });
            }

            void CleanUp()
            {
                originalBanks.Clear();
                originalBanks = null;
            }

            void InvokeEvents()
            {
                CloningCompleted = true;

                OnCloningCompleted?.Invoke();
                OnCloningCompleted = null;
            }
        }

        /// <summary>
        /// Adds the action passed to <see cref="OnCloningCompleted"/> if the cloning process is not ended yet. If it is ended already,
        /// the action passed gets invoked instantly.
        /// </summary>
        /// <param name="callbackContext"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Use to get a clone bank with a specific Guid. <b>Runtime only.</b>
        /// </summary>
        /// <param name="guidOfOriginalBank">Target Guid.</param>
        /// <returns>The clone bank or null.</returns>
        internal static VariableBank GetCloneWithGuid(string guidOfOriginalBank)
        {
            if (!Application.isPlaying) throw new Exception("You cannot call GetCloneWithGuid in editor!");
            if (!CloningCompleted) throw new Exception("Make sure the cloning process has ended successfully before calling GetCloneWithGuid() function.");

            if (!m_bankTable.ContainsKey(guidOfOriginalBank)) throw new Exception("Specified variablebank is not cloned by the internal system. " +
                "Check if it's included in the Addressables menu. And also check it's AvoidCloning property.");

            return m_bankTable[guidOfOriginalBank];
        }
    }

}