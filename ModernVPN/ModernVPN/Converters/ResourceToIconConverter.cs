using ModernVPN.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ModernVPN.Converters
{
    internal class ResourceToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            switch ((WindowIcons)value)
            {
                case WindowIcons.WindowClose:
                    return Application.Current.TryFindResource("2ColIconSm");
                case WindowIcons.WindowMinimize:
                    return Application.Current.TryFindResource("SensorgramIconSm");
                case WindowIcons.WindowMaximize:
                    return Application.Current.TryFindResource("PlotSm");
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
