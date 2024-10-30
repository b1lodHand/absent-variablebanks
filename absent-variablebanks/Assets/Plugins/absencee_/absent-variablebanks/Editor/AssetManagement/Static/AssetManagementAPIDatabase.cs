using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.absence.variablebanks.editor.internals.assetmanagement
{
    [InitializeOnLoad]
    public class AssetManagementAPIDatabase
    {
        const bool DEBUG_MODE = true;

        private static List<APIRegistry> s_apis = new();
        public static List<APIRegistry> APIs => s_apis;

        public static APIRegistry CurrentAPI => s_apis[PackageSettings.instance.AssetManagementAPISelection];

        static AssetManagementAPIDatabase()
        {
            Refresh();
        }

        public static void Refresh()
        {
            s_apis.Clear();
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

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
                        DisplayName = attrRef.displayName
                    };

                    s_apis.Add(reg);
                    if (DEBUG_MODE) Debug.Log(reg.DisplayName);
                });
            });

        }
    }
}
