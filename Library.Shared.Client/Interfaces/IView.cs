using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Shared.Client.Interfaces
{
    /// <summary>
    /// View interface which abstracts ui operations and properties
    /// that will be used by view to run the view
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public interface IView
    {
        /// <summary>
        /// the viewmodel instance which contains informations that 
        /// view need to interact with view elemnets
        /// </summary>
        object ViewModel { get; set; }

        /// <summary>
        /// run the view instance
        /// </summary>
        void Run();

        /// <summary>
        /// close the view instance
        /// </summary>
        void Close();
    }
}
