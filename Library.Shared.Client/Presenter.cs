using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client.Interfaces;

namespace Library.Shared.Client
{
    /// <summary>
    /// this class abstracts some basic logic for presenter
    /// </summary>
    /// <typeparam name="TViewModel">the viewmodel type</typeparam>
    public abstract class Presenter<TViewModel>:IPresenter where TViewModel:ViewModel
    {

        #region Fields

        private IView _view;

        #endregion

        public abstract TViewModel ViewModel
        {
            get;
            set;
        }

        #region IPresenter<TViewModel> Members

        public IView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public virtual void Run()
        {
            SubscribeEvent();
            _view.ViewModel = ViewModel;
            _view.Run();
        }

        public virtual void Close()
        {
            UnSubscribeEvent();
        }

        #endregion

        private void SubscribeEvent()
        {
            if (ViewModel == null)
                return;
            ViewModel.ValidateEvent += new ValidateHandler<object, object>(ValidateViewModelMember);
        }

        private void UnSubscribeEvent()
        {
            if (ViewModel == null)
                return;
            ViewModel.ValidateEvent -= new ValidateHandler<object, object>(ValidateViewModelMember);
        }

        public virtual ValidationResult<object> ValidateViewModelMember(string columnName,object parameter)
        {
            return null;
        }
    }
}
