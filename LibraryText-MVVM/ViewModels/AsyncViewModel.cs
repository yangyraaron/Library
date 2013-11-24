using Library.Shared.Client;
using System.ComponentModel.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Client.Wpf;
using System.Threading;
using Library.Client.Wpf.Commands;

namespace LibraryText_MVVM.ViewModels
{
    [Export]
    public class AsyncViewModel:ViewModel
    {
        public AsyncViewModel()
        {
            this.LoadCommand = new DelegateCommand<object>(
                (o) =>
                {
                    Initilize();
                }

                , (o) =>
                {
                    return !(IsListBusy & IsCurrentBusy);
                }
                );

        }

        protected override string OnValidateMember(string name)
        {
            if (name == "CurrentStr")
            {
                if (string.IsNullOrEmpty(this.CurrentStr))
                    return null;
                return  "Also check out ";
            }
                
            return null;
        }

        private void Initilize()
        {
            InitializeList();
            InitializeCurrent();
        }

        private void InitializeList()
        {
            this.IsListBusy = true;

            AsyncExcutor.Process<IEnumerable<string>,object>(
                (strs) =>
                {
                    this.Strings = new List<string>(strs);
                    this.IsListBusy = false;
                    this.LoadCommand.RaiseCanExecuteChanged();
                },
                (pa) =>
                {
                    Thread.Sleep(new TimeSpan(0, 0, 5));
                    return new string[] { "1", "2", "3", "4", "5", "6" };
                }, null
                );
        }

        private void InitializeCurrent()
        {
            this.IsCurrentBusy = true;

            AsyncExcutor.Process<string,object>(
                (str) =>
                {
                    this.CurrentStr = str;
                    this.IsCurrentBusy = false;
                    this.LoadCommand.RaiseCanExecuteChanged();
                },
                (pa) =>
                {
                    Thread.Sleep(new TimeSpan(0, 0, 5));
                    return "1";
                },
                  null
                );
        }

        private IList<string> _strings; 
        public IList<string> Strings
        {
            get { return _strings; }
            set { _strings = value;
            OnPropertyChanged("Strings");
            }
        }

        private String _currentStr;
        public String CurrentStr
        {
            get { return _currentStr; }
            set
            {
                if (_currentStr == value)
                    return;
                _currentStr = value;
                OnPropertyChanged("CurrentStr");
            }
        }

        private bool _isListBusy;
        public bool IsListBusy
        {
            get { return _isListBusy; }
            set
            {
                if (_isListBusy == value)
                    return;
                _isListBusy = value;
                OnPropertyChanged("IsListBusy");
            }
        }

        

        private bool _isCurrentBusy;
        public bool IsCurrentBusy
        {
            get { return _isCurrentBusy; }
            set
            {
                if (_isCurrentBusy == value)
                    return;
                _isCurrentBusy = value;
                OnPropertyChanged("IsCurrentBusy");
            }
        }       

        public DelegateCommand<object> LoadCommand { get; set; }
                
    }
}
