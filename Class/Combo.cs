#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Combo.cs is part of LoLCombo.

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
    using System.Collections.Generic;
    using System.Linq;
    using Data;

    #endregion

    internal class Combo
    {
        private readonly List<ConfigPasswordsAdd> _adds = new List<ConfigPasswordsAdd>();
        private readonly List<ConfigPasswordsAppend> _appends = new List<ConfigPasswordsAppend>();
        private readonly Dictionary<char, char> _replaceDic = new Dictionary<char, char>();

        public Combo(ConfigPasswords config)
        {
            foreach (
                var replace in
                    config.Replaces.Where(replace => replace.Enabled && !_replaceDic.ContainsKey(replace.Key[0])))
            {
                try
                {
                    if (replace.IgnoreCase)
                    {
                        _replaceDic[Char.ToUpper(replace.Key[0])] = replace.Value[0];
                        _replaceDic[Char.ToLower(replace.Key[0])] = replace.Value[0];
                    }
                    else
                    {
                        _replaceDic[replace.Key[0]] = replace.Value[0];
                    }
                }
                catch
                {
                }
            }
            if (config.Adds != null && config.Adds.Count > 0)
                _adds.AddRange(config.Adds.ToList());

            if (config.Appends != null && config.Appends.Count > 0)
                _appends.AddRange(config.Appends.ToList());
        }

        public List<string> Create(string value)
        {
            var output = new List<string> {value};
            var run = true;
            while (run)
            {
                var newItem = false;
                for (int i = 0, il = output.Count; i < il; i++)
                {
                    for (int j = 0, jl = output[i].Length; j < jl; j++)
                    {
                        if (_replaceDic.ContainsKey(output[i][j]))
                        {
                            var chars = output[i].ToCharArray();
                            chars[j] = _replaceDic[output[i][j]];
                            var val = new string(chars);
                            if (!output.Contains(val))
                            {
                                output.Add(val);
                                newItem = true;
                            }
                        }
                    }
                }
                run = newItem;
            }
            output.AddRange(from a in _appends where a.Enabled select value + a.Value);
            output.AddRange(from a in _adds where a.Enabled select a.Value);
            return output;
        }
    }
}