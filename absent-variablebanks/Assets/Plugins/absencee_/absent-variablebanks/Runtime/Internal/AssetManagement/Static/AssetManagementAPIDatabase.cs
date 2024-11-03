using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace com.absence.variablebanks.internals.assetmanagement
{
    public class AssetManagementAPIDatabase
    {
        const bool DEBUG_MODE = true;

        private static List<APIRegistry> s_apis = new();
        public static List<APIRegistry> APIs => s_apis;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Bootstrap()
        {
            Refresh();
        }

        public static void Refresh()
        {
            s_apis.Clear();
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            Dictionary<Type, ConstructorInfo> extensionConstructorDict = new();

            assemblies.ForEach(assembly =>
            {
                List<Type> types = assembly.GetTypes().ToList();

                types.ForEach(type =>
                {
                    if (type.IsAbstract) return;

                    AssetManagementAPIEditorExtensionAttribute extAttrRef =
                        type.GetCustomAttribute<AssetManagementAPIEditorExtensionAttribute>();

                    if (extAttrRef == null) return;

                    Type extRef = type.GetInterface(nameof(IAssetManagementAPIEditorExtension), true);

                    if (extRef == null) return;

                    ConstructorInfo constructor =
                    type.GetConstructors().ToList().FirstOrDefault(cons => cons.GetCustomAttribute<APIConstructor>() != null);

                    if (constructor == null) return;

                    extensionConstructorDict.Add(extAttrRef.targetApiType, constructor);
                });
            });

            assemblies.ForEach(assembly =>
            {
                List<Type> types = assembly.GetTypes().ToList();

                types.ForEach(type =>
                {
                    if (type.IsAbstract) return;

                    AssetManagementAPIAttribute attrRef =
                        type.GetCustomAttribute<AssetManagementAPIAttribute>();

                    if (attrRef == null) return;

                    Type apiRef = type.GetInterface(nameof(IAssetManagementAPI), true);

                    if (apiRef == null) return;

                    ConstructorInfo constructor =
                    type.GetConstructors().ToList().FirstOrDefault(cons => cons.GetCustomAttribute<APIConstructor>() != null);

                    if (constructor == null) return;

                    APIRegistry reg = new()
                    {
                        APIObject = constructor.Invoke(null) as IAssetManagementAPI,
                        DisplayName = attrRef.displayName,
                    };

#if UNITY_EDITOR
                    reg.EditorExtensions =
                    extensionConstructorDict[reg.APIObject.GetType()].Invoke(null)
                    as IAssetManagementAPIEditorExtension;
#endif

                    s_apis.Add(reg);
                    if (DEBUG_MODE) Debug.Log(reg.DisplayName);
                });
            });
        }
    }
}
