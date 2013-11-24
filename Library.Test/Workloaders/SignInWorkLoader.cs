using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client;
using Library.Shared.Client.Interfaces;
using Library.Test.Attributes;
using System.ComponentModel.Composition;

namespace Library.Test.Workloaders
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportWorkLoader(Key=WorkLoaderType.SignInWorkLoader)]
    public class SignInWorkLoader:WorkLoader
    {
        [Import(typeof(Func<object,IPresenter>))]
        public Func<object, IPresenter> GetPresentrFunction { get; set; }
        private IPresenter _presenter;
        public IPresenter Presenter
        {
            get
            {
                if (_presenter == null)
                {
                    if (GetViewFunction != null)
                        _presenter = GetPresentrFunction(PresenterType.SignInPresneter);
                 //_presenter = ComponentFactory.GetComponent<IPresenter>(PresenterType.SignInPresneter);
                
                }
                return _presenter;
            }
            set { _presenter = value ; }
        }

        [Import(typeof(Func<object, IView>))]
        public Func<object, IView> GetViewFunction { get; set; }

        private IView _view;
        public IView View
        {
            get {
                if (_view == null)
                {
                 //_view = ComponentFactory.GetComponent<IView>(ViewType.SignInView);
                    if (GetViewFunction != null)
                        _view = GetViewFunction(ViewType.SignInView);
                }
                return _view;
              }
            set { _view = value; }
        }

        public override void Run()
        {
            if (Presenter == null)
                return;
            Presenter.View = View;
            Presenter.Run();

            UIManager.SetContent(View);
        }

        public override void Close()
        {
            base.Close();

            if (Presenter == null)
                return;
            Presenter.Close();

            if (View == null)
                return;
            View.Close();
        }
    }
}
