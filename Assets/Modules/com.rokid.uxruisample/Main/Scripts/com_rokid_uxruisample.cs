using Cysharp.Threading.Tasks;
using rokid.armaz.module;
using Rokid.UXR.Interaction;
using UnityEngine;

namespace com.rokid.uxruisample
{
    public class com_rokid_uxruisample : ModuleBase
    {
        private GameObject left;
        private GameObject right;

        public async override UniTask OnEnterAsync(object param)
        {
            InputModuleManager.Instance.ForceActiveModule(InputModuleType.Gesture);
            await UniTask.Delay(500);
            var rayVisuals = InputModuleManager.Instance.transform.GetComponentsInChildren<RayVisual>(true);
            foreach (var item in rayVisuals)
            {
                item.gameObject.SetActive(true);
                item.transform.GetComponent<LineRenderer>().enabled = true;
            }
        }

        public async override UniTask OnExitAsync()
        {
            var rayVisuals = InputModuleManager.Instance.transform.GetComponentsInChildren<RayVisual>(true);
            foreach (var item in rayVisuals)
            {
                item.gameObject.SetActive(false);
                item.transform.GetComponent<LineRenderer>().enabled = false;
            }
        }
    }
}