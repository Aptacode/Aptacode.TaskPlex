using System;
using System.Timers;
using Aptacode.TaskPlex.Engine.Enums;
using Aptacode.TaskPlex.Engine.Interfaces;

namespace Aptacode.TaskPlex.Engine
{
    public class SystemTimerUpdater : IUpdater
    {
        private Timer _timer;

        public SystemTimerUpdater(RefreshRate refreshRate = RefreshRate.Normal)
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
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}