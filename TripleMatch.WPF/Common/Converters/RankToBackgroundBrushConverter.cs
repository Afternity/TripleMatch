using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TripleMatch.WPF.Common.Converters
{
    public class RankToBackgroundBrushConverter
        : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is int rank)
            {
                return rank switch
                {
                    1 => Brushes.Gold,
                    2 => Brushes.Silver,    // Серебряный
                    3 => Brushes.SandyBrown,
                    _ => Brushes.White
                };
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}