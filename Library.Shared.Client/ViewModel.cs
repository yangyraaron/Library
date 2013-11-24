using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Library.Shared;

namespace Library.Shared.Client
{
    /// <summary>
    /// abscract base class of viewmodel,it provides the property changed event
    /// and validating properties
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// fire the property changed event,specifically for wpf
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            HandlerManager.FireEvent(PropertyChanged, new object[]{this,new PropertyChangedEventArgs(propertyName)});
        }


        #endregion

        #region IDataErrorInfo Members

        /// <summary>
        /// error string
        /// </summary>
        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            //get { return HandlerManager.FireEvent<string>(ValidateEvent, columnName, null); }
            get { return OnValidateMember(columnName); }
        }

        protected virtual string OnValidateMember(string name)
        {
            return null;
        }

        #endregion

        public event ValidateHandler<object, object> ValidateEvent;

    }
}
