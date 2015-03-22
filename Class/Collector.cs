#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Collector.cs is part of LoLCombo.

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
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    internal class Collector
    {
        public static List<string> Collect(string region, int page, bool filterSpecialCharacters)
        {
            return LoLSummoners(region, page, filterSpecialCharacters);
        }

        private static List<string> LoLSummoners(string region, int page, bool filterSpecialCharacters)
        {
            var names = new List<string>();
            using (var webClient = new WebClient {Encoding = Encoding.UTF8})
            {
                var matches =
                    new Regex(@"<td class=""name""><a href=""/leagues/.*"">(.*)</a></td>").Matches(
                        webClient.DownloadString(string.Format("http://www.lolsummoners.com/ladders/{0}/{1}", region,
                            page)));
                foreach (Match m in matches)
                {
                    if (filterSpecialCharacters)
                    {
                        if (m.Groups[1].Value.All(c => Char.IsLetterOrDigit(c) && (c < 128)))
                        {
                            names.Add(m.Groups[1].Value.Replace(" ", string.Empty));
                        }
                    }
                    else
                    {
                        names.Add(m.Groups[1].Value.Replace(" ", string.Empty));
                    }
                }
                names.AddRange(from Match m in matches
                    select m.Groups[1].Value.Replace(" ", string.Empty).Replace(" ", string.Empty));
            }

            return names;
        }
    }
}