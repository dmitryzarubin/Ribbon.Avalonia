using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace AvaloniaUI.Ribbon;

public class BoundsPointToAdjustedPointConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double x = 0;
        double y = 0;

        if (value is Rect rect && parameter != null)
        {
            var paramParts = parameter.ToString().Replace(" ", string.Empty).Split(',');

            var pt = paramParts[2];
            var ptX = pt[1];
            var ptY = pt[0];

            if (ptX == 'R')
                x = rect.Width;
            else if (ptX == 'C')
                x = rect.Width / 2;

            if (ptY == 'B')
                y = rect.Height;
            else if (ptY == 'C')
                y = rect.Height / 2;

            /*x = rect.Width;
            y = rect.Height;*/

            if (double.TryParse(paramParts[0], out var xAdjust))
                x += xAdjust;

            if (double.TryParse(paramParts[1], out var yAdjust))
                y += yAdjust;
        }


        return new Point(x, y);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}