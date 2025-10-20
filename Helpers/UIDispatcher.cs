using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace StartButtonLauncher.Helpers
{
    public static class UIDispatcher
    {
        /// <summary>
        ///     Whether or not the calling thread has access to the resource guarded by this dispatcher.
        /// </summary>
        /// <returns>
        ///     Usually a dispatcher is used to guard access to a resource, for example a FrameworkElement.
        ///     When a method doesn't have control over the thread it's called from (for example because it's
        ///     an event delegate), then it might want to check if if can immediately access the resource,
        ///     or if it should delegate the access via <see cref="Invoke(Action)"/>.
        /// </returns>
        /// <returns>True when the calling thread has access to the resource, otherwise false.</returns>
        public static bool HasAccess => Thread.CurrentThread == Application.Current.Dispatcher?.Thread;

        /// <summary>
        ///     Adds the given action to the list of actions to be executed later on.
        /// </summary>
        /// <param name="action"></param>
        public static void Invoke(Action action) =>
            Invoke(action, DispatcherPriority.Normal);

        /// <summary>
        ///     Adds the given action to the list of actions to be executed later on.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="priority"></param>
        public static void Invoke(Action action, DispatcherPriority priority)
            => Application.Current.Dispatcher?.Invoke(action, priority);

        /// <summary>
        ///     Adds the given action to the list of actions to be executed later on.
        ///     Returns a task that is completed once the actual action has finished executing.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task InvokeAsync(Action action)
            => InvokeAsync(action, DispatcherPriority.Normal);

        /// <summary>
        ///     Adds the given action to the list of actions to be executed later on.
        ///     Returns a task that is completed once the actual action has finished executing.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static Task InvokeAsync(Action action, DispatcherPriority priority)
            => Application.Current.Dispatcher?.BeginInvoke(action, priority).Task;
    }
}