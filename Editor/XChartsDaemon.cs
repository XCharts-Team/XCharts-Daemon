using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XCharts.Daemon
{
    internal static class XChartsDaemon
    {
        public class XChartsAssetPostprocessor : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                string[] movedFromAssetsPaths)
            {
                foreach (var assetPath in importedAssets)
                {
                    CheckAddedAsset(assetPath);
                }
            }
        }

        public static void CheckAddedAsset(string assetPath)
        {
            var fileName = Path.GetFileName(assetPath);
            if (fileName.Equals("XChartsMgr.cs"))
            {
                CheckAsmdef();
            }
        }

        public static void CheckAsmdef()
        {
#if UNITY_2017_1_OR_NEWER
#if dUI_TextMeshPro
            CheckAsmdefTmpReference(true);
#else
            CheckAsmdefTmpReference(false);
#endif
#elif UNITY_2019_1_OR_NEWER
#if INPUT_SYSTEM_ENABLED
            CheckAsmdefInputSystemReference(true);
#else
            CheckAsmdefInputSystemReference(false);
#endif
#endif
        }

        #region Text mesh pro support
#if UNITY_2017_1_OR_NEWER
        const string SYMBOL_TMP = "dUI_TextMeshPro";
        const string ASMDEF_TMP = "Unity.TextMeshPro";

        public static void CheckAsmdefTmpReference(bool enable)
        {
            if (enable)
            {
                InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_TMP);
                InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_TMP);
            }
            else
            {
                RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_TMP);
                RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_TMP);
            }
        }
#endif
        #endregion

        #region InputSystem Support
#if UNITY_2019_1_OR_NEWER
        //As InputSystem is released in 2019.1+ ,when unity version is 2019.1+ , enable InputSystem Support
        const string SYMBOL_I_S = "INPUT_SYSTEM_ENABLED";
        const string ASMDEF_I_S = "Unity.InputSystem";

        public static void CheckAsmdefInputSystemReference(bool enable)
        {
            if(enable)
            {
                InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_I_S);
                InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_I_S);
            }
            else
            {
                RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_I_S);
                RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_I_S);
            }
        }
#endif
        #endregion

        #region Assistant members
#if UNITY_2017_1_OR_NEWER
        // as text mesh pro is released in 2017.1, so we may use these function and types in 2017.1 or later
        private static void InsertSpecifyReferenceIntoAssembly(Platform platform, string reference)
        {
            var file = GetPackageAssemblyDefinitionPath(platform);
            if (!File.Exists(file))
            {
                Debug.LogError("Can't find assembly definition file in path:" + file);
                return;
            }
            var content = File.ReadAllText(file);
            var data = new AssemblyDefinitionData();
            EditorJsonUtility.FromJsonOverwrite(content, data);
            if (!data.references.Contains(reference))
            {
                data.references.Add(reference);
                var json = EditorJsonUtility.ToJson(data, true);
                File.WriteAllText(file, json);
                AssetDatabase.ImportAsset(GetAsmdefAssetPath(platform, file));
            }
        }

        private static void RemoveSpecifyReferenceFromAssembly(Platform platform, string reference)
        {
            var file = GetPackageAssemblyDefinitionPath(platform);
            if (!File.Exists(file))
            {
                Debug.LogError("Can't find assembly definition file in path:" + file);
                return;
            }
            var content = File.ReadAllText(file);
            var data = new AssemblyDefinitionData();
            EditorJsonUtility.FromJsonOverwrite(content, data);
            if (data.references.Contains(reference))
            {
                data.references.Remove(reference);
                var json = EditorJsonUtility.ToJson(data, true);
                File.WriteAllText(file, json);
                AssetDatabase.ImportAsset(GetAsmdefAssetPath(platform, file));
            }
        }

        private static string GetAsmdefAssetPath(Platform platform, string filePath)
        {
            filePath = filePath.Replace("\\", "/");
            if (filePath.Contains("Assets/"))
            {
                return filePath.Substring(filePath.IndexOf("Assets/"));
            }
            else
            {
                return string.Format("Packages/com.monitor1394.xcharts/{0}/XCharts.{1}.asmdef", platform.ToString(), platform.ToString());
            }
        }

        public enum Platform { Editor, Runtime }
        public static string GetPackageAssemblyDefinitionPath(Platform platform)
        {
            var p = platform == Platform.Editor ? "Editor" : "Runtime";
            var f = "XCharts." + p + ".asmdef";
            var sub = Path.Combine(p, f);
            string packagePath = Path.GetFullPath("Packages/com.monitor1394.xcharts");
            if (!Directory.Exists(packagePath))
            {
                packagePath = AssetDatabase.FindAssets("t:Script")
                                                   .Where(v => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(v)) == "XChartsMgr")
                                                   .Select(id => AssetDatabase.GUIDToAssetPath(id))
                                                   .FirstOrDefault();
                packagePath = Path.GetDirectoryName(packagePath);
                packagePath = packagePath.Substring(0, packagePath.LastIndexOf("Runtime"));
            }
            return Path.Combine(packagePath, sub);
        }

        public static bool IsSpecifyAssemblyExist(string name)
        {
#if UNITY_2018_1_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.Player))
            {
                if (assembly.name.Equals(name)) return true;
            }
#elif UNITY_2017_3_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies())
            {
                if (assembly.name.Equals(name)) return true;
            }
#endif
            return false;
        }

        [Serializable]
        class AssemblyDefinitionData
        {
#pragma warning disable 649
            public string name;
            public List<string> references;
            public List<string> includePlatforms;
            public List<string> excludePlatforms;
            public bool allowUnsafeCode;
            public bool overrideReferences;
            public List<string> precompiledReferences;
            public bool autoReferenced;
            public List<string> defineConstraints;
            public List<string> versionDefines;
            public bool noEngineReferences;
#pragma warning restore 649
        }
#endif
        #endregion
    }
}