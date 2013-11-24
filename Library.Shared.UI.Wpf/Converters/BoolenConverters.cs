using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Markup;

namespace Library.Client.Wpf.Converters
{
    public class EnumBoolenConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool type = (bool)value;
            if (type)
                return Enum.Parse(targetType, parameter.ToString());
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    [ValueConversion(typeof(int), typeof(Boolean))]
    public class IntBoolenConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            int threhold = -1;
            if (parameter == null)//if the paramete is not setted,then set the threhold to 0
                threhold = 0;
            if (!int.TryParse(parameter.ToString(), out threhold))//if the parameter type is not int 
                threhold = 0;

            int c = int.Parse(value.ToString());
            if (c > threhold)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
