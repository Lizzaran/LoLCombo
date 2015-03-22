#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Validation.cs is part of LoLCombo.

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

    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Data;

    #endregion

    public class PasswordsReplaceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var error = new StringBuilder();
            var bindingGroup = (BindingGroup) value;
            if (bindingGroup != null)
            {
                foreach (var replace in
                    from object item in bindingGroup.Items select item as ConfigPasswordsReplace)
                {
                    if (replace == null)
                    {
                        error.Append("The row can't be empty. ");
                        continue;
                    }
                    if (replace.Key.Length != 1)
                    {
                        error.Append("The \"Search\" field must be a char. ");
                    }
                    if (replace.Value.Length != 1)
                    {
                        error.Append("The \"Replace\" field must be a char. ");
                    }
                }
            }
            return !string.IsNullOrWhiteSpace(error.ToString())
                ? new ValidationResult(false, error.ToString())
                : ValidationResult.ValidResult;
        }
    }

    public class PasswordsAddValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var error = new StringBuilder();
            var bindingGroup = (BindingGroup) value;
            if (bindingGroup != null)
            {
                foreach (var add in
                    from object item in bindingGroup.Items select item as ConfigPasswordsAdd)
                {
                    if (add == null)
                    {
                        error.Append("The row can't be empty. ");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(add.Value))
                    {
                        error.Append("The \"Add\" can't be empty. ");
                    }
                }
            }
            return !string.IsNullOrWhiteSpace(error.ToString())
                ? new ValidationResult(false, error.ToString())
                : ValidationResult.ValidResult;
        }
    }

    public class PasswordsAppendValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var error = new StringBuilder();
            var bindingGroup = (BindingGroup) value;
            if (bindingGroup != null)
            {
                foreach (var append in
                    from object item in bindingGroup.Items select item as ConfigPasswordsAppend)
                {
                    if (append == null)
                    {
                        error.Append("The row can't be empty. ");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(append.Value))
                    {
                        error.Append("The \"Append\" can't be empty. ");
                    }
                }
            }
            return !string.IsNullOrWhiteSpace(error.ToString())
                ? new ValidationResult(false, error.ToString())
                : ValidationResult.ValidResult;
        }
    }
}