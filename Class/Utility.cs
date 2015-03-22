#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Utility.cs is part of LoLCombo.

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
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    #endregion

    public class Utility
    {
        public static void CreateFileFromResource(string path, string resource, bool overwrite = false)
        {
            if (!overwrite && File.Exists(path))
            {
                return;
            }
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            {
                                sw.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <exception cref="RegexMatchTimeoutException">
        ///     A time-out occurred. For more information about time-outs, see the Remarks
        ///     section.
        /// </exception>
        public static string MakeValidFileName(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, "_");
        }

        public static void MapClassToXmlFile(Type type, object obj, string path)
        {
            var serializer = new XmlSerializer(type);
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, obj);
                }
            }
            catch
            {
            }
        }

        /// <exception cref="FileNotFoundException">The file cannot be found. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        public static object MapXmlFileToClass(Type type, string path)
        {
            var serializer = new XmlSerializer(type);
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                return serializer.Deserialize(reader);
            }
        }

        public static bool OverwriteFile(string file, string path)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (dir != null)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.Move(file, path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadResourceString(string resource)
        {
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        public static bool RenameFileIfExists(string file, string path)
        {
            try
            {
                var counter = 1;
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileExtension = Path.GetExtension(file);
                var newPath = path;
                var pathDirectory = Path.GetDirectoryName(path);
                if (pathDirectory != null)
                {
                    if (!Directory.Exists(pathDirectory))
                    {
                        Directory.CreateDirectory(pathDirectory);
                    }
                    while (File.Exists(newPath))
                    {
                        var tmpFileName = string.Format("{0} ({1})", fileName, counter++);
                        newPath = Path.Combine(pathDirectory, tmpFileName + fileExtension);
                    }
                    File.Move(file, newPath);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static string LongRegionToShort(string region)
        {
            switch (region)
            {
                case "North America":
                    return "na";

                case "Europe West":
                    return "euw";

                case "Europe Nordic":
                    return "eune";

                case "Brazil":
                    return "br";

                case "LA North":
                    return "lan";

                case "LA South":
                    return "las";

                case "Oceania":
                    return "oce";

                case "Russia":
                    return "ru";

                case "Turkey":
                    return "tr";

                case "Korea":
                    return "kr";
            }
            return null;
        }
    }
}