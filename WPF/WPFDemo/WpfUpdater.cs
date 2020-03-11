using System;
using System.Timers;
using System.Windows;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;

namespace WPFDemo
{
    public class WpfUpdater : IUpdater
    {
        private Timer _timer;

        public WpfUpdater(RefreshRate refreshRate = RefreshRate.Normal)
        {
            RefreshRate = refreshRate;
        }

        public RefreshRate RefreshRate { get; private set; }
        public event EventHandler OnUpdate;

        public void Start(RefreshRate refreshRate)
        {
            RefreshRate = refreshRate;
            Start();
        }

        public void Start()
        {
            Dispose();
            _timer = new Timer((int) RefreshRate);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        public void Stop()
        {
            _timer?.Stop();
        }

        public void Dispose()
        {
            Stop();
            if (_timer == null)
            {
                return;
            }

            _timer.Elapsed -= TimerElapsed;
            _timer.Dispose();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() => OnUpdate?.Invoke(this, EventArgs.Empty));
            }
        }
    }
}