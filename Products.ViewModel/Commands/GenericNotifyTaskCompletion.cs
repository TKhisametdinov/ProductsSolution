﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Products.ViewModel.Interfaces;

namespace Products.ViewModel
{
    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    public sealed class NotifyTaskCompletionImplementation<TResult> : INotifyTaskCompletion<TResult>
    {
        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        public NotifyTaskCompletionImplementation(Task<TResult> task)
        {
            Task = task;
            if (task.IsCompleted)
            {
                TaskCompleted = System.Threading.Tasks.Task.FromResult(true);
                return;
            }

            var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
            TaskCompleted = task.ContinueWith(t =>
            {
                var propertyChanged = PropertyChanged;
                if (propertyChanged == null)
                    return;

                propertyChanged(this, new PropertyChangedEventArgs("Status"));
                propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
                if (t.IsCanceled)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
                }
                else if (t.IsFaulted)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                    propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                    propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                    propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                }
                else
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                    propertyChanged(this, new PropertyChangedEventArgs("Result"));
                }
            },
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            scheduler);
        }

        public Task<TResult> Task { get; private set; }
        Task INotifyTaskCompletion.Task { get { return Task; } }
        public Task TaskCompleted { get; private set; }
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult); } }
        public TaskStatus Status { get { return Task.Status; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }
        public string ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
