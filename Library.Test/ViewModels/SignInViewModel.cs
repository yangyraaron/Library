using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client;
using Library.Shared;
using Library.Client.Wpf.Commands;
using System.ComponentModel.Composition;

namespace Library.Test.ViewModels
{
    public class SignInViewModel:ViewModel
    {
        private string _account;
        public event ValueChangedHandler<String> AccountChanged;
        public string Account
        {
            get
            { return _account; }
            set {
                string oldValue = _account;
                _account = value;
            OnPropertyChanged("Account");
            HandlerManager.FireEvent(AccountChanged, oldValue, _account);
            }
        }

        private string _password;
        public event ValueChangedHandler<string> PasswordChanged;
        public string Password
        {
            get { return _password; }
            set {
                string oldValue = _password;
                _password = value;
                OnPropertyChanged("Password");
                HandlerManager.FireEvent(PasswordChanged, oldValue, _password);
            }
        }

        public DelegateCommand<object> SignInCommand { get; set; }
    }
}
