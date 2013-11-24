using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client.Interfaces;

namespace Library.Shared.Client
{
    /// <summary>
    /// defines abstract framework operations and properties as 
    /// basic of the workloader class
    /// </summary>
    public abstract class WorkLoader:IWorkLoader
    {
        private IUIManager _uiManager;
        private IWorkLoaderContext _context;
        private IComponentFactory _componentFactory;

        #region IWorkLoader Members

        /// <summary>
        /// ui manager instance
        /// <remarks>
        /// during this property's setter excuting process it subscribes the
        /// uimanger's run event and close event
        /// </remarks>
        /// </summary>
        public IUIManager UIManager
        {
            get { return _uiManager; }
            set { _uiManager = value;
            OnUIManagerChanged();
            }
        }

        public IComponentFactory ComponentFactory
        {
            get
            {
                return _componentFactory;
            }
            set
            {
                _componentFactory = value;
            }
        }

        private void OnUIManagerChanged()
        {
            if (_uiManager == null)
                return;
        }

        /// <summary>
        /// run the workloader
        /// </summary>
        public virtual void Run()
        {
          
        }

        /// <summary>
        /// close the workloader
        /// <remarks>
        /// in this method by default remove the subscriptions to
        /// the ui manager's run event and close event,so if you do
        /// not remove these subscriptions in your overrid method,make
        /// sure that you call base's close method.
        /// </remarks>
        /// </summary>
        public virtual void Close()
        {
        }


        public IWorkLoaderContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        #endregion
    }
}
