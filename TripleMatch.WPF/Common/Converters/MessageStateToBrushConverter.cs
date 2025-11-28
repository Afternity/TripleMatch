using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TripleMatch.Shered.Contracts.MessageVMs;

namespace TripleMatch.WPF.Common.Converters
{
    public class MessageStateToBrushConverter
       : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is MessageState state)
            {
                return state switch
                {
                    MessageState.Success => Brushes.Green,
                    MessageState.Error => Brushes.Red,
                    MessageState.Warning => Brushes.Orange,
                    MessageState.Info => Brushes.Blue,
                    MessageState.Fail => Brushes.DarkRed,
                    _ => Brushes.Black
                };
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
