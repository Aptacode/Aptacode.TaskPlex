using System;
using Aptacode.TaskPlex.Engine.Enums;
using Aptacode.TaskPlex.Engine.Interfaces;

namespace Aptacode.TaskPlex.Tests
{
    public class DummyUpdater : IUpdater
    {
        public DummyUpdater(RefreshRate refreshRate = RefreshRate.High)
        {
            RefreshRate = refreshRate;
            IsRunning = false;
        }

        public bool IsRunning { get; set; }

        public void Dispose()
        {
            IsRunning = false;
        }

        public event EventHandler OnUpdate;

        public void Start(RefreshRate refreshRate)
        {
            Start();
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
        }

        public RefreshRate RefreshRate { get; }

        public void Update()
        {
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}