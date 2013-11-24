using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client.Interfaces;
using System.Windows.Controls;

namespace Library.Test.Views
{
    public class View:UserControl,IView
    {

        #region IView Members

        public object ViewModel
        {
            get;
            set;
        }

        public virtual void Run()
        {
            this.DataContext = ViewModel;
        }

        public virtual void Close()
        {
            
        }

        #endregion
    }
}
