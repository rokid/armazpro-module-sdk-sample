using Cysharp.Threading.Tasks;
using rokid.armaz.module;
using System.Collections;
using UnityEngine;

namespace com.rokid.armaz.wenhuaqiang
{
    public class com_rokid_armaz_wenhuaqiang : ModuleBase
    {
        private Coroutine m_Coroutine;
        public async override UniTask OnEnterAsync(object param)
        {
            var cubeTrans = transform.Find("Cube");
            var renderer = cubeTrans.GetComponent<MeshRenderer>();
            m_Coroutine = StartCoroutine(RandomColor(renderer.material));
        }

        public IEnumerator RandomColor(Material meta)
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                var color = new Color(Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                meta.color = color;
            }
        }

        public async override UniTask OnExitAsync()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
            }
        }
    }
}
