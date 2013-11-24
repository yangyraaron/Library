using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Shared.Client.Interfaces
{
    /// <summary>
    /// this interface defines operations and properties that 
    /// used by this interface instance to control the ui logic,
    /// for this model ,one presenter will use on viewmodel instance
    /// to control one view.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
   public interface IPresenter
    {
       /// <summary>
       /// the view instance controlled by the<see cref="IPresenter<TViewModel>"/>
       /// instance
       /// </summary>
       IView View { get; set; }

       /// <summary>
       /// run the <see cref="IPresenter<TViewModel>"/> instance
       /// </summary>
        void Run();

       /// <summary>
        /// close the <see cref="IPresenter<TViewModel>"/> instance
       /// </summary>
        void Close();

    }
}
