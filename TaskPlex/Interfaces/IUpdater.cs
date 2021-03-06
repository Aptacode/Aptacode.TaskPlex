﻿using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Interfaces
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