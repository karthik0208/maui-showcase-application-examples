using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIShowcaseSample.Converters
{
    internal class StringToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Color color = Colors.Black; // Default color
            string? param = parameter as string;
            string? val = value as string;

            if (value == null) return color;            

            switch ((string)value)
            {
                case "Income":
                    if (param == "LabelValue" && Application.Current?.Resources.TryGetValue("Green", out var retGreenObj) == true && retGreenObj is Color retGreen)
                    {
                        color = retGreen;
                    }
                    else if (param == "DataGridBackground" && Application.Current?.Resources.TryGetValue("BackgroundGreen", out var retBackGreenObj) == true && retBackGreenObj is Color retBackGreen)
                    {
                        color = retBackGreen;                                               
                    }
                    else if (param == "DataGridValue" && Application.Current?.Resources.TryGetValue("Green", out var retValGreenObj) == true && retValGreenObj is Color retValGreen)
                    {
                        color = retValGreen;                     
                    }
                    break;

                case "Expense":
                    if (param == "LabelValue" && Application.Current?.Resources.TryGetValue("Red", out var retRedObj) == true && retRedObj is Color retRed)
                    {
                        color = retRed;
                    }
                    else if (param == "DataGridBackground" && Application.Current?.Resources.TryGetValue("BackgroundRed", out var retBackRedObj) == true && retBackRedObj is Color retBackRed)
                    {
                        color = retBackRed;
                    }
                    else if (param == "DataGridValue" && Application.Current?.Resources.TryGetValue("Red", out var retValRedObj) == true && retValRedObj is Color retValRed)
                    {
                        color = retValRed;
                    }

                    break;

                default:
                    if (param == "DataGridBackgrouned" && Application.Current?.Resources.TryGetValue("Surface", out var retBackObj) == true && retBackObj is Color retBack)
                    {
                        color = retBack;
                    }
                    else if (param == "DataGridValue" && Application.Current?.Resources.TryGetValue("BlackText", out var retValObj) == true && retValObj is Color retVal)
                    {
                        color = retVal;
                    }
                        break;
                
            }
            return color;
        }
        /// <summary>
        /// This method is used to convert the color to string.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the string.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
