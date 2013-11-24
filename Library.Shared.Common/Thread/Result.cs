using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Library.Common
{
    /// <summary>
    /// the result interface
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// excute the task
        /// </summary>
        void Execute();
        /// <summary>
        /// the completed event when the task is completed then fire
        /// </summary>
        event EventHandler<ResultCompletionArgs> Completed;
    }

    /// <summary>
    /// the cancel interface to indicate if the task could be cancel
    /// </summary>
    public interface ICancel
    {
        /// <summary>
        /// cancel the task
        /// </summary>
        void Cancel();
    }

    /// <summary>
    /// the complete argument
    /// </summary>
    public class ResultCompletionArgs : EventArgs
    {
        /// <summary>
        /// constructor for create a has result ResultCompletionArgs instance
        /// </summary>
        /// <param name="result">the result </param>
        public ResultCompletionArgs(object result)
        {
            this.Result = result;
        }
        /// <summary>
        /// constructor for create a error or cancelled ResultCompletionArgs instance
        /// </summary>
        /// <param name="error">the error </param>
        /// <param name="isCancelled">if it is cancelled</param>
        public ResultCompletionArgs(Exception error, bool isCancelled)
            : this(null)
        {
            this.Error = error;
            this.IsCancelled = isCancelled;
        }

        /// <summary>
        /// the exception
        /// </summary>
        public Exception Error { get; private set; }
        /// <summary>
        /// if it has been cancelled
        /// </summary>
        public bool IsCancelled { get; private set; }
        /// <summary>
        /// the task final result
        /// </summary>
        public object Result { get; private set; }
        /// <summary>
        /// if it has error
        /// </summary>
        public bool HasError { get { return Error != null; } }
    }

    /// <summary>
    /// execute task synchronousely
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : IResult
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="action"></param>
        public Result(Func<T> action)
        {
            this._execute = action;
        }

        private Func<T> _execute;

        #region IResult Members

        /// <summary>
        /// execute the task
        /// </summary>
        public virtual void Execute()
        {
            var result = _execute();
            Completed(this, new ResultCompletionArgs(result));
        }

        /// <summary>
        /// the completed event
        /// </summary>
        public event EventHandler<ResultCompletionArgs> Completed = delegate { };

        #endregion
    }

    /// <summary>
    /// the result executes the task asynchronousely
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncResult<T> : IResult, ICancel
    {
        private Func<T> _execute;
        private Action<ResultCompletionArgs> _callback;
        private CancellationTokenSource _source;
        private CancellationToken _token;
        private TaskScheduler _schedular;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="action"></param>
        public AsyncResult(Func<T> action)
        {
            this._callback = t => Completed(this, t);
            this._source = new CancellationTokenSource();
            this._token = this._source.Token;
            this._execute = action;
            this._schedular = TaskScheduler.FromCurrentSynchronizationContext();
        }


        #region IResult Members

        /// <summary>
        /// excute the task
        /// </summary>
        public void Execute()
        {
            Task<T> task = Task.Factory.StartNew(() =>
            {
                _token.ThrowIfCancellationRequested();
                return _execute();
            }, _token,TaskCreationOptions.None,TaskScheduler.Default);

            //if executes successfully
            task.ContinueWith(t=>_callback(new ResultCompletionArgs(t.Result)), 
                                            this._token,
                                            TaskContinuationOptions.OnlyOnRanToCompletion,
                                            this._schedular);
            //else failed
            task.ContinueWith(t => _callback(new ResultCompletionArgs(t.Exception.InnerException, false)), 
                                                this._token,
                                                TaskContinuationOptions.OnlyOnFaulted,
                                                this._schedular);
        }

        /// <summary>
        /// cancel the task
        /// </summary>
        public void Cancel()
        {
            this._source.Cancel();
        }

        /// <summary>
        /// the event when the task has been completed then fire it
        /// </summary>
        public event EventHandler<ResultCompletionArgs> Completed = delegate { };

        #endregion
    }
}
