using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Diploma.Services
{
    public sealed class CameraService : IDisposable
    {
        #region Синглтон
        private static readonly Lazy<CameraService> _instance =
            new Lazy<CameraService>(() => new CameraService());
        public static CameraService Instance => _instance.Value;
        private CameraService() { }
        #endregion

        #region Поля
        private readonly object _sync = new object();
        private FilterInfoCollection _devices;
        private VideoCaptureDevice _device;
        private Bitmap _latest;
        private int _subscribers;
        private bool _disposed;
        #endregion

        #region Публичное API
        public IReadOnlyList<string> Enumerate()
        {
            return EnsureDevices().Cast<FilterInfo>().Select(d => d.Name).ToList();
        }

        public void Subscribe(EventHandler<Bitmap> handler)
        {
            if (handler == null) return;
            lock (_sync)
            {
                ThrowIfDisposed();
                FrameArrived += handler;
                _subscribers++;
                if (_subscribers == 1) StartInternal();
            }
        }

        public void Unsubscribe(EventHandler<Bitmap> handler)
        {
            if (handler == null) return;
            lock (_sync)
            {
                if (_disposed) return;
                FrameArrived -= handler;
                _subscribers--;
                if (_subscribers <= 0) StopInternal();
            }
        }

        public Bitmap GetLatestFrame()
        {
            lock (_sync)
                return _latest == null ? null : (Bitmap)_latest.Clone();
        }
        #endregion

        #region События
        public event EventHandler<Bitmap> FrameArrived;
        public event EventHandler CameraStopped;
        #endregion

        #region ВнутренниеМетоды
        private FilterInfoCollection EnsureDevices()
        {
            if (_devices == null)
                _devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return _devices;
        }

        private void StartInternal()
        {
            if (_device != null && _device.IsRunning) return;

            try
            {
                var devices = EnsureDevices();
                if (devices.Count == 0)
                {
                    MessageBox.Show("Камера не найдена. Подключите устройство и попробуйте снова.",
                                    "Ошибка камеры", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _device = new VideoCaptureDevice(devices[0].MonikerString);
                _device.NewFrame += OnNewFrame;
                _device.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка запуска камеры: " + ex.Message,
                                "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopInternal()
        {
            if (_device == null) return;

            _device.NewFrame -= OnNewFrame;
            if (_device.IsRunning)
            {
                _device.SignalToStop();
                _device.WaitForStop();
            }

            _device = null;
            CameraStopped?.Invoke(this, EventArgs.Empty);
            var old = Interlocked.Exchange(ref _latest, null);
            old?.Dispose();
        }

        private void OnNewFrame(object sender, NewFrameEventArgs e)
        {
            var frame = (Bitmap)e.Frame.Clone();
            frame.RotateFlip(RotateFlipType.RotateNoneFlipX);

            var old = Interlocked.Exchange(ref _latest, (Bitmap)frame.Clone());
            old?.Dispose();

            var handler = FrameArrived;
            handler?.Invoke(this, frame);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CameraService));
        }
        #endregion

        #region Реализация IDisposable
        public void Dispose()
        {
            if (_disposed) return;
            StopInternal();
            _disposed = true;
        }
        #endregion
    }
}
