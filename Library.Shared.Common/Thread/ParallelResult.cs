using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Common
{
    /// <summary>
    /// the result exccutes the tasks parallel
    /// </summary>
    public class ParallelResult:IResult
    {
        private IEnumerable<IResult> _tasks;
        private IList<Object> _result;
        private IList<IResult> _executingTasks;
        private bool _isCompleted = false;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tasks">all the tasks</param>
        public ParallelResult(IEnumerable<IResult> tasks)
        {
            this._tasks = tasks;
            this._result = new List<object>();
            this._executingTasks = new List<IResult>();
        }

        #region IResult Members
        /// <summary>
        /// exucte all the tasks
        /// </summary>
        public void Execute()
        {
            try
            {
                foreach (var t in this._tasks)
                {
                    t.Completed += new EventHandler<ResultCompletionArgs>(Current_Completed);
                    t.Execute();

                    this._executingTasks.Add(t);
                }
            }
            catch (Exception ex)
            {
                OnError(ex,false);
            }

        }

        void Current_Completed(object sender, ResultCompletionArgs e)
        {
            IResult result = sender as IResult;
            if (result == null)
                return;

            result.Completed -= new EventHandler<ResultCompletionArgs>(Current_Completed);
            this._executingTasks.Remove(result);

            //if the process is set to complete then do not  keep going
            //only when there is any error occurs 
            if (this._isCompleted)
                return;

            if (e.HasError || e.IsCancelled)
            {
                //if the current task failed then cancel all other tasks
                CancelAllTasks();
                OnError(e.Error, e.IsCancelled);
                return;
            }
            else
            {
                this._result.Add(e.Result);
            }

            //if  all task executes completely
            if(!this._executingTasks.Any())
                OnCompleted();
        }

        private void CancelAllTasks()
        {
            foreach (var t in this._executingTasks)
            {
                ICancel cancellableTask = t as ICancel;
                if (cancellableTask == null)
                    return;
                else
                    cancellableTask.Cancel();
            }
        }

        /// <summary>
        /// the completed event when all the tasks executes completely
        /// </summary>
        public event EventHandler<ResultCompletionArgs> Completed = delegate { };

        private void OnError(Exception e,bool isCancelled)
        {
            //if there is erro then set the executing process to completed
            this._isCompleted = true;
            Completed(this, new ResultCompletionArgs(e, isCancelled));
        }
        private void OnCompleted()
        {
            this._isCompleted = true;
            Completed(this, new ResultCompletionArgs(this._result));
        }

        #endregion
    }
}
