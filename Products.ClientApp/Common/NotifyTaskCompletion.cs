using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Products.ClientApp.Interfaces;

namespace Products.ClientApp.Common
{
    /// <summary>
    /// Factory for task completion notifiers.
    /// </summary>
    public static class NotifyTaskCompletion
    {
        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <returns>A new task notifier watching the specified task.</returns>
        public static INotifyTaskCompletion Create(Task task)
        {
            return new NotifyTaskCompletionImplementation(task);
        }

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="task">The task to watch.</param>
        /// <returns>A new task notifier watching the specified task.</returns>
        public static INotifyTaskCompletion<TResult> Create<TResult>(Task<TResult> task)
        {
            return new NotifyTaskCompletionImplementation<TResult>(task);
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <returns>A new task notifier watching the returned task.</returns>
        public static INotifyTaskCompletion Create(Func<Task> asyncAction)
        {
            return Create(asyncAction());
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <returns>A new task notifier watching the returned task.</returns>
        public static INotifyTaskCompletion<TResult> Create<TResult>(Func<Task<TResult>> asyncAction)
        {
            return Create(asyncAction());
        }

        /// <summary>
        /// Watches a task and raises property-changed notifications when the task completes.
        /// </summary>
        private sealed class NotifyTaskCompletionImplementation : INotifyTaskCompletion
        {
            /// <summary>
            /// Initializes a task notifier watching the specified task.
            /// </summary>
            /// <param name="task">The task to watch.</param>
            public NotifyTaskCompletionImplementation(Task task)
            {
                Task = task;
                if (task.IsCompleted)
                {
                    TaskCompleted = Task.FromResult(true);
                    return;
                }

                var scheduler = (SynchronizationContext.Current == null)
                    ? TaskScheduler.Current
                    : TaskScheduler.FromCurrentSynchronizationContext();
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
                        }
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    scheduler);
            }

            public Task Task { get; private set; }
            public Task TaskCompleted { get; private set; }

            public TaskStatus Status
            {
                get { return Task.Status; }
            }

            public bool IsCompleted
            {
                get { return Task.IsCompleted; }
            }

            public bool IsNotCompleted
            {
                get { return !Task.IsCompleted; }
            }

            public bool IsSuccessfullyCompleted
            {
                get { return Task.Status == TaskStatus.RanToCompletion; }
            }

            public bool IsCanceled
            {
                get { return Task.IsCanceled; }
            }

            public bool IsFaulted
            {
                get { return Task.IsFaulted; }
            }

            public AggregateException Exception
            {
                get { return Task.Exception; }
            }

            public Exception InnerException
            {
                get { return (Exception == null) ? null : Exception.InnerException; }
            }

            public string ErrorMessage
            {
                get { return (InnerException == null) ? null : InnerException.Message; }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

    }
}
