using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Library.Client.Wpf.Converters
{
   public class MarginNegativeConverter:IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            Thickness tk = (Thickness) value;

            if (parameter == null)
                return new Thickness(0 - tk.Left, tk.Top, tk.Right, tk.Bottom);

            double offset = 0;
            double.TryParse(parameter.ToString(), out offset);
            
            return new Thickness(0 - tk.Left - offset, tk.Top, tk.Right, tk.Bottom);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            Thickness tk = (Thickness)value;

            if (parameter == null)
                return new Thickness(0 - tk.Left, tk.Top, tk.Right, tk.Bottom);

            double offset = 0;
            double.TryParse(parameter.ToString(), out offset);

            return new Thickness(0 - tk.Left - offset, tk.Top, tk.Right, tk.Bottom);
        }

        #endregion
    }
}
