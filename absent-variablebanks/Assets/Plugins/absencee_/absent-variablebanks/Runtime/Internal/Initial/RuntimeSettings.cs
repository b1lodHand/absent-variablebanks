using System.IO;
using UnityEngine;

namespace com.absence.variablebanks.internals
{
    public class RuntimeSettings
    {
        private static RuntimeSettings m_instance = null;
        public static RuntimeSettings instance => m_instance;

        public void Read()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "absent-variablebanks", "RuntimeSettings.json");
            JsonUtility.FromJson<RuntimeSettings>(path);
        }

        public string AssetManagementAPIName;
    }
}
