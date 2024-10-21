using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaUI.Ribbon;

public class NotConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool booleanValue)
            throw new InvalidOperationException();
        return !booleanValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}