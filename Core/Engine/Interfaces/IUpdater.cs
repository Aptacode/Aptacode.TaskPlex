using System;
using Aptacode.TaskPlex.Engine.Enums;

namespace Aptacode.TaskPlex.Engine.Interfaces
{
    public interface IUpdater : IDisposable
    {
        RefreshRate RefreshRate { get; }
        event EventHandler OnUpdate;

        void Start(RefreshRate refreshRate);
        void Start();
        void Stop();
    }
}