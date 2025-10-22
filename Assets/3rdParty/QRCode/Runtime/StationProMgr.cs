using System;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using QRCode.Utility;

namespace QRCode
{
    public class StationProMgr
    {
        private Action<string> action;
        private RawImage rawImage = null;
        private WebCamTexture camTexture = null;
        private BarcodeReader barcodeReader;

        public StationProMgr()
        {
        }

        public void Initialize(Action<string> action, RawImage image = null)
        {
            if (barcodeReader != null)
            {
                action.Invoke("数据已经被使用");
            }

            this.rawImage = image;
            barcodeReader = new BarcodeReader();
#if !UNITY_EDITOR
            this.rawImage.transform.localScale = new Vector3(1, -1, 1);
#endif
            this.action = action;
            OpenWebCamDevice();
            VTaskPool.YieldAction(0.2f, DecodeQR);
            // SetAutoFocus();
        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        public async void OpenWebCamDevice()
        {
            await Application.RequestUserAuthorization(UserAuthorization.WebCam);
            Log("打开摄像头");
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                if (devices.Length > 0)
                {
                    string deviceName = devices[0].name;
                    camTexture = new WebCamTexture(deviceName);

                    if (rawImage != null)
                        rawImage.texture = camTexture;

                    camTexture.Play();

                    if (camTexture.isPlaying)
                    {
                        Log("摄像头已开始播放");
                        data = new Color32[camTexture.width * camTexture.height];
                    }
                    else
                    {
                        Log("摄像头播放失败");
                    }
                }
                else
                {
                    Log("未找到摄像头设备");
                }
            }
            else
            {
                Log("摄像头权限未授予");
            }
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void CloseWebCamDevice()
        {
            Log("关闭摄像头");
            VTaskPool.UniTaskCancel();
            camTexture?.Stop();
            this.rawImage = null;
            this.action = null;
            barcodeReader = null;
        }

        private void OnDestroy()
        {
            CloseWebCamDevice();
        }

        private void Log(string msg)
        {
            Debug.Log(msg);
        }

        private Color32[] data;

        public void DecodeQR()
        {
            if (camTexture != null)
            {
                if (camTexture.isPlaying)
                {
                    camTexture.GetPixels32(data);
                    Result result = barcodeReader.Decode(data, camTexture.width, camTexture.height);
                    if (result != null)
                    {
                        action?.Invoke(result.Text);
                    }
                }
            }

            VTaskPool.YieldAction(0.15f, DecodeQR);
        }
    }
}
