using System;
using System.Timers;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;

namespace Aptacode.TaskPlex
{
    public class SystemTimerUpdater : IUpdater
    {
        public RefreshRate RefreshRate { get; private set; }
        public event EventHandler OnUpdate;

        private Timer _timer;

        public SystemTimerUpdater(RefreshRate refreshRate = RefreshRate.Normal)
        {
            RefreshRate = refreshRate;
        }

        public void Start(RefreshRate refreshRate)
        {
            RefreshRate = refreshRate;
            Start();
        }

        public void Start()
        {
            Dispose();
            _timer = new Timer((int)RefreshRate);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnUpdate?.Invoke(this, EventArgs.Empty);
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
    }
}