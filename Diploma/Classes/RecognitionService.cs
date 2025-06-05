using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Timers;
using Diploma.Services;
using Newtonsoft.Json;

namespace Diploma.Recognition
{
    public sealed class RecognitionService : IDisposable
    {
        #region Поля
        private readonly CameraService _cam;
        private readonly System.Timers.Timer _timer;
        private Bitmap _latest;
        private bool _disposed;
        private volatile bool _running;
        #endregion

        #region ctor
        public RecognitionService(CameraService cam, int interval = 1000)
        {
            _cam = cam;
            _cam.Subscribe(OnFrameArrived);

            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += async (s, e) => await SendFrame();
        }
        #endregion

        #region API
        public void Start()
        {
            _running = true;
            _timer.Start();
        }

        public void Stop()
        {
            _running = false;
            _timer.Stop();
        }
        #endregion

        #region События
        public event EventHandler<RecognitionResult> Recognized;
        #endregion

        #region Внутреннее
        private void OnFrameArrived(object s, Bitmap bmp)
        {
            var old = Interlocked.Exchange(ref _latest, (Bitmap)bmp.Clone());
            old?.Dispose();
        }

        private async System.Threading.Tasks.Task SendFrame()
        {
            if (!_running || _disposed)
            {
                var tmp = Interlocked.Exchange(ref _latest, null);
                tmp?.Dispose();
                return;
            }

            var frame = Interlocked.Exchange(ref _latest, null);
            if (frame == null) return;

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
            frame.Dispose();

            using (var cli = new HttpClient())
            {
                try
                {
                    var mp = new MultipartFormDataContent
                    {
                        { new ByteArrayContent(bytes) { Headers = { ContentType = MediaTypeHeaderValue.Parse("image/jpeg") } }, "frame", "frame.jpg" }
                    };
                    var resp = await cli.PostAsync("http://127.0.0.1:5000/recognize", mp);
                    resp.EnsureSuccessStatusCode();
                    var json = await resp.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<RecognitionResult>(json);
                    if (res?.id != null)
                        Recognized?.Invoke(this, res);
                }
                catch { }
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (_disposed) return;
            _running = false;
            _timer?.Stop();
            _timer?.Dispose();
            _cam.Unsubscribe(OnFrameArrived);
            _latest?.Dispose();
            _disposed = true;
        }
        #endregion
    }

    #region DTO
    public class RecognitionResult
    {
        public string id { get; set; }
        public string face_image { get; set; }
    }
    #endregion
}
