using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Test.ViewModels;
using Library.Shared.Client;
using System.ComponentModel.Composition;
using Library.Shared.Client.Interfaces;
using Library.Test.Attributes;
using Library.Client.Wpf;

namespace Library.Test.Presenters
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportPresenter(Key=PresenterType.SignInPresneter)]
    public class SignInPresenter : Presenter<SignInViewModel>
    {
        public SignInPresenter()
        {
            _viewMode = new SignInViewModel();
            
        }
        private SignInViewModel _viewMode;

        public override SignInViewModel ViewModel
        {
            get
            {
                return _viewMode;
            }
            set
            {
                _viewMode = value;
            }
        }

        public override Shared.ValidationResult<object> ValidateViewModelMember(string columnName, object parameter)
        {
            return base.ValidateViewModelMember(columnName, parameter);
        }
    }
}
