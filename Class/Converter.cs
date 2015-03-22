#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Converter.cs is part of LoLCombo.

 LoLCombo is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 LoLCombo is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with LoLCombo. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

namespace LoLCombo.Class
{
    #region

    using System;
    using System.Globalization;
    using System.Windows.Data;

    #endregion

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool) value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool) value;
            }
            return value;
        }
    }
}