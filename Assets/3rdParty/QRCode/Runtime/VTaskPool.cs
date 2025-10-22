using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace QRCode.Utility
{
    public static class VTaskPool
    {
        #region UniTask
        private static CancellationTokenSource cancellationTokenSource = null;
        public static async void YieldAction(float time, Action action)
        {
            UniTaskCancel();
            cancellationTokenSource = new CancellationTokenSource();
            bool cancel = await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationTokenSource.Token).SuppressCancellationThrow();

            if (!cancel)
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                action?.Invoke();
            }

        }
        public static void UniTaskCancel()
        {
            if (cancellationTokenSource != null)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                    cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
        #endregion
    }
}