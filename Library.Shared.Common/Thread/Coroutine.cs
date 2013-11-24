using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Common
{
    /// <summary>
    /// the corountine class 
    /// </summary>
    public static class Coroutine
    {
        /// <summary>
        /// execute all tasks in sequence
        /// </summary>
        /// <param name="tasks">the tasks are provided to coroutine to execute</param>
        /// <param name="onSucceed">the action is going to be executed when all tasks were completed</param>
        /// <param name="onError">the action is going to be executed when there is any task failed</param>
        public static void Start(IEnumerable<IResult> tasks, Action<IEnumerable<object>> onSucceed,
            Action<Exception> onError = null)
        {
            var enumerator = new SequentialResult(tasks.GetEnumerator());

            enumerator.Completed += (o, e) => OnStartCompleted(o, e, onSucceed, onError);
            enumerator.Execute();
        }

        /// <summary>
        /// when all the tasks were executed completely by start function
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <param name="OnSucceed"></param>
        /// <param name="OnError"></param>
        /// <param name="OnCancelled"></param>
        private static void OnStartCompleted(object obj, ResultCompletionArgs result, Action<IEnumerable<object>> OnSucceed,
            Action<Exception> OnError = null, Action<IEnumerable<object>> OnCancelled = null)
        {
            var enumerator = (SequentialResult)obj;
            var results = (IEnumerable<object>)result.Result;
            if (enumerator != null)
                enumerator.Completed -= (o, e) => OnStartCompleted(o, e, OnSucceed, OnError);

            if (result.HasError)
                OnError(result.Error);
            else
                OnSucceed(results);
        }

        /// <summary>
        /// excutes all tasks parallel
        /// </summary>
        /// <param name="tasks">the task list need to execute</param>
        /// <param name="onSucceed">the action for success</param>
        /// <param name="onError">the action for when there is  any error during the executing process</param>
        public static void ParallelStart(IEnumerable<IResult> tasks, Action<IEnumerable<object>> onSucceed,
            Action<Exception> onError = null)
        {
            var presult = new ParallelResult(tasks);
            presult.Completed += (o, e) => OnParallelStartCompleted(o, e, onSucceed, onError);

            presult.Execute();
        }

        /// <summary>
        /// complete function for parallel start
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <param name="OnSucceed"></param>
        /// <param name="OnError"></param>
        private static void OnParallelStartCompleted(object obj, ResultCompletionArgs result, Action<IEnumerable<object>> OnSucceed,
            Action<Exception> OnError = null)
        {
            var enumerator = (ParallelResult)obj;
            var results = (IEnumerable<object>)result.Result;
            if (enumerator != null)
                enumerator.Completed -= (o, e) => OnParallelStartCompleted(o, e, OnSucceed, OnError);

            if (result.HasError)
                OnError(result.Error);
            else
                OnSucceed(results);
        }

    }
}
