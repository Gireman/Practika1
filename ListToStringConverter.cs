using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp1.Models
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return "NULL";

            if (value is List<string> list && list.Count > 0)
                return string.Join(", ", list);

            return "NULL";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}