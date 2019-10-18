using System;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.EventArgs;

namespace Aptacode.TaskPlex.Tasks
{
    public interface IBaseTask
    {
        TimeSpan Duration { get; set; }
        event EventHandler<System.EventArgs> OnStarted;
        event EventHandler<System.EventArgs> OnFinished;
        event EventHandler<TaskCancellationEventArgs> OnCancelled;
        /// <summary>
        ///     Returns true if the specified task collides with the instance
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CollidesWith(IBaseTask item);
        /// <summary>
        ///     Start the task
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        ///     Start the task with the given CancellationToken
        /// </summary>
        /// <returns></returns>
        Task StartAsync(CancellationTokenSource cancellationToken);

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        void Cancel();
    }
}