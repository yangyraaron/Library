using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Common
{
    /// <summary>
    /// executes the tasks in sequence
    /// </summary>
    public class SequentialResult : IResult
    {
        private IEnumerator<IResult> _tasks;
        private IList<object> _results;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tasks"></param>
        public SequentialResult(IEnumerator<IResult> tasks)
        {
            this._tasks = tasks;
            this._results = new List<object>();
        }

        #region IResult Members
        /// <summary>
        /// execute all tasks
        /// </summary>
        public void Execute()
        {
            //begin to excute tasks
            this.ExcuteTask(null, new ResultCompletionArgs(null, false));
        }

        /// <summary>
        /// excute the task sequentially
        /// <remarks>
        /// this method is being called when the previous task excutes complete.
        /// </remarks>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ExcuteTask(object sender, ResultCompletionArgs args)
        {
            var previous = sender as IResult;
            if (previous != null)
                previous.Completed -= new EventHandler<ResultCompletionArgs>(ExcuteTask);

            //if the previous task has error or indicates to cancel the task the complete
            if (args.HasError || args.IsCancelled)
            {
                OnError(args.Error, args.IsCancelled);
                return;
            }

            //only the succeed result would be added in
            if(previous!=null)
                this._results.Add(args.Result);

            var moveNext = false;
            try
            {
                moveNext = this._tasks.MoveNext();
            }
            catch (Exception e)
            {
                //if exceptional then return
                OnError(e, false);
                return;
            }

            if (moveNext)//move next to get current task and excute it
            {
                try
                {
                    var current = this._tasks.Current as IResult;
                    current.Completed += new EventHandler<ResultCompletionArgs>(ExcuteTask);
                    current.Execute();
                }
                catch (Exception e)
                {
                    OnError(e, false);
                    return;
                }
            }
            else
                OnCompleted();
        }
        /// <summary>
        /// the completed event when all the tasks executes completely
        /// </summary>
        public event EventHandler<ResultCompletionArgs> Completed = delegate { };

        #endregion

        private void OnError(Exception error, bool isCancelled)
        {
            _tasks.Dispose();

            var action = this.Completed;
            if (action != null)
                action(this, new ResultCompletionArgs(error, isCancelled));
        }

        private void OnCompleted()
        {
            _tasks.Dispose();

            var action = this.Completed;
            if (action != null)
                action(this, new ResultCompletionArgs(this._results));
        }
    }
}
