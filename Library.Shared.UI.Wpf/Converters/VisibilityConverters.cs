using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Collections;
using Library.Common.Extension;

namespace Library.Client.Wpf.Converters
{

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolenVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            bool result;
            bool isSuccess = bool.TryParse(value.ToString(), out result);

            if (!isSuccess)
                return Visibility.Collapsed;

            if (!result)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class PercentVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Hidden;

            int result;
            bool isSuccess = int.TryParse(value.ToString(), out result);

            if (!isSuccess)
                return Visibility.Hidden;

            if (result == 0 || result > 100)
                return Visibility.Hidden;

            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class EnumVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null)
                return Visibility.Collapsed;

            if(parameter == null)
                return Visibility.Collapsed;
            
            if (parameter is IEnumerable)
            {
                var instances = parameter.To<IEnumerable>();
                foreach (var instance in instances)
                {
                    if (instance.Equals(value))
                        return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }

            return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
