using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Shared.Client.Interfaces
{
    /// <summary>
    /// the interface defines some functions and properties
    /// that will be used by the instance of this interface to
    /// organize the presenters and views that created by it.
    /// </summary>
    public interface IWorkLoader
    {
        /// <summary>
        /// the ui manager instance
        /// </summary>
        IUIManager UIManager { set; get; }

        /// <summary>
        /// the context
        /// </summary>
        IWorkLoaderContext Context { get; set; }

        /// <summary>
        /// the component factory used to generate the 
        /// related views and presenters in this workloader
        /// <remarks>
        /// actually in the workloader,it uses the imported <see cref="Func<object,objec>"/>
        /// to create views and presenters
        /// </remarks>
        /// </summary>
        IComponentFactory ComponentFactory { get; set; }

        /// <summary>
        /// run the instance of <see cref="IWorkLoader"/>
        /// </summary>
        void Run();

        /// <summary>
        /// close the instace of <see cref="IWorkLoader"/>
        /// </summary>
        void Close();
    }
}
