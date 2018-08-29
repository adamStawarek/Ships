using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SimpleGame.Helpers
{
    public class BoolToVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((string)parameter=="Inverse")
                return value != null && (bool)value ?  Visibility.Hidden:Visibility.Visible;
            return value != null && (bool) value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
