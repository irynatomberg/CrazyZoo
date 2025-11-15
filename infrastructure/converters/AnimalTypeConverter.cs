using System;
using System.Globalization;
using System.Windows.Data;

namespace CrazyZoo.infrastructure.converters
{
    public class AnimalTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var type = value.GetType().Name;
            return type switch
            {
                "Cat" => "Kass",
                "Dog" => "Koer",
                "Bird" => "Lind",
                "Horse" => "Hobune",
                "Monkey" => "Ahv",
                "Fox" => "Rebane",
                _ => type
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
