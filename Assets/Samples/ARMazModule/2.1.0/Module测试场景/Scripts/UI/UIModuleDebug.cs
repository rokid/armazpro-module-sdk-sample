using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace rokid.armaz.module
{
    public class UIModuleDebug : MonoBehaviour
    {
        [SerializeField] private Dropdown m_moduleIdDropdown;
        [SerializeField] private Transform m_moduleRootTrans;
        private DirectoryInfo[] m_moduleDirs;
        private void Awake()
        {
            InitModuleDropdown();
            ARMazModuleEngine.Init();
        }

        public async void OnOpenBtn()
        {
            var dirInfo = m_moduleDirs[m_moduleIdDropdown.value];
            await ARMazModuleEngine.Open(dirInfo.Name, m_moduleRootTrans);
        }

        public void OnCloseBtn() 
        {
            var dirInfo = m_moduleDirs[m_moduleIdDropdown.value];
            ARMazModuleEngine.Close(dirInfo.Name);
        }

        private void InitModuleDropdown()
        {
            m_moduleDirs = GetAllModulePath();
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            if (m_moduleDirs == null || m_moduleDirs.Length <= 0)
            {
                m_moduleIdDropdown.value = -1;
                m_moduleIdDropdown.options = optionDatas;
                return;
            }
            foreach (var directoryInfo in m_moduleDirs)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                optionData.text = directoryInfo.Name;
                optionDatas.Add(optionData);
            }
            m_moduleIdDropdown.options = optionDatas;
            m_moduleIdDropdown.value = 0;
        }

        private DirectoryInfo[] GetAllModulePath()
        {
#if UNITY_EDITOR
            DirectoryInfo moduleDir = new DirectoryInfo($"{Application.dataPath}/Modules");
#else
            //DirectoryInfo moduleDir = new DirectoryInfo(ModuleSettings.Instance.ModulesPath());
            DirectoryInfo moduleDir = new DirectoryInfo($"{Application.persistentDataPath}/ARMazModuleData/Modules/Android");
#endif
            if (moduleDir.Exists)
            {
                var dirs = moduleDir.GetDirectories();
                return dirs;
            }
            return null;
        }
    }
}
