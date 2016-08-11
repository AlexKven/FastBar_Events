using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FastBar_Events.Converters
{
    public class LoginPage_AdaptiveUIConverter : IValueConverter
    {
        public static LoginPage_AdaptiveUIConverter Instance { get; private set; }

        static LoginPage_AdaptiveUIConverter()
        {
            Instance = new LoginPage_AdaptiveUIConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.IsNumber())
            {
                bool isNarrow = (double)value < 500;
                switch (parameter?.ToString())
                {
                    case "NarrowRow.Height":
                        return isNarrow ? GridLength.Auto : new GridLength(0);
                    case "WideColumn.Width":
                        return isNarrow ? new GridLength(0) : new GridLength(210, GridUnitType.Absolute);
                    case "TitlePanel.(StackPanel.Orientation)":
                        return isNarrow ? StackOrientation.Vertical : StackOrientation.Horizontal;
                    case "*Button.Margin":
                        return new Thickness(isNarrow ? 10 : 0, 4, 10, 4);
                    case "LogoImage.WidthRequest":
                        return isNarrow ? 200 : 400;
                    case "LogoImage.HeightRequest":
                        return isNarrow ? 55 : 110;
                    case "DescriptionText.FontSize":
                        return isNarrow ? 24 : 32;
                    case "MovingLayout.(Grid.Row)":
                        return isNarrow ? 1 : 0;
                    case "MovingLayout.(Grid.Column)":
                        return isNarrow ? 0 : 1;
                }
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
