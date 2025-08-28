using Cysharp.Threading.Tasks;
using UnityEngine;

namespace rokid.armaz.module
{
    public static class ARMazModuleEngine
    {
        private static bool s_inited;
        public static void Init()
        {
            if (!s_inited)
            {
                IResourceLoader resourceLoader = null;
                #if UNITY_EDITOR
                    resourceLoader = new ModuleResourcesLoaderExtend();
                #else
                    resourceLoader = new ModuleResourcesLoaderDefault();
                #endif
                ModuleEngine.Init(resourceLoader);
                ModulePackageManager.LoadEditorModules();
                s_inited = true;
            }
        }

        public static async UniTask Open(string moduleIdent, Transform parentTrans)
        {
            if (s_inited)
            {
                await AMP.AMPEngine.Open(moduleIdent, moduleIdent,parentTrans, Vector3.zero, Vector3.zero, Vector3.one, null, null);
            }
        }

        public static async void Close(string moduleIdent)
        {
            if (s_inited)
            {
                await AMP.AMPEngine.Close(moduleIdent);
            }
        }
    }
}
