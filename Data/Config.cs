#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Config.cs is part of LoLCombo.

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

namespace LoLCombo.Data
{
    #region

    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Xml.Serialization;

    #endregion

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Config : INotifyPropertyChanged
    {
        private ConfigCollector _collector;
        private ConfigPasswords _passwords;

        public ConfigPasswords Passwords
        {
            get { return _passwords; }
            set
            {
                _passwords = value;
                OnPropertyChanged("Passwords");
            }
        }

        public ConfigCollector Collector
        {
            get { return _collector; }
            set
            {
                _collector = value;
                OnPropertyChanged("Collector");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class ConfigPasswords : INotifyPropertyChanged
    {
        private ObservableCollection<ConfigPasswordsAdd> _adds;
        private ObservableCollection<ConfigPasswordsAppend> _appends;
        private ObservableCollection<ConfigPasswordsReplace> _replaces;

        [XmlArrayItem("Add", IsNullable = false)]
        public ObservableCollection<ConfigPasswordsAdd> Adds
        {
            get { return _adds; }
            set
            {
                _adds = value;
                OnPropertyChanged("Adds");
            }
        }

        [XmlArrayItem("Replace", IsNullable = false)]
        public ObservableCollection<ConfigPasswordsReplace> Replaces
        {
            get { return _replaces; }
            set
            {
                _replaces = value;
                OnPropertyChanged("Replaces");
            }
        }

        [XmlArrayItem("Append", IsNullable = false)]
        public ObservableCollection<ConfigPasswordsAppend> Appends
        {
            get { return _appends; }
            set
            {
                _appends = value;
                OnPropertyChanged("Appends");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class ConfigPasswordsAdd : INotifyPropertyChanged
    {
        private bool _enabled;
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [XmlAttribute]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class ConfigPasswordsReplace : INotifyPropertyChanged
    {
        private bool _enabled;
        private bool _ignoreCase;
        private string _key;
        private string _value;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [XmlAttribute]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        [XmlAttribute]
        public bool IgnoreCase
        {
            get { return _ignoreCase; }
            set
            {
                _ignoreCase = value;
                OnPropertyChanged("IgnoreCase");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class ConfigPasswordsAppend : INotifyPropertyChanged
    {
        private bool _enabled;
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [XmlAttribute]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class ConfigCollector : INotifyPropertyChanged
    {
        private readonly string[] _regions =
        {
            "North America", "Europe West", "Europe Nordic", "Brazil", "LA North", "LA South", "Oceania", "Russia",
            "Turkey", "Korea"
        };

        private bool _addPassword;
        private bool _filterSpecialCharacters;
        private int _pageFrom;
        private int _pageTo;
        private string _region;

        public int PageFrom
        {
            get { return _pageFrom; }
            set
            {
                _pageFrom = value;
                OnPropertyChanged("PageFrom");
            }
        }

        public int PageTo
        {
            get { return _pageTo; }
            set
            {
                _pageTo = value;
                OnPropertyChanged("PageTo");
            }
        }

        public bool AddPassword
        {
            get { return _addPassword; }
            set
            {
                _addPassword = value;
                OnPropertyChanged("AddPassword");
            }
        }

        public bool FilterSpecialCharacters
        {
            get { return _filterSpecialCharacters; }
            set
            {
                _filterSpecialCharacters = value;
                OnPropertyChanged("FilterSpecialCharacters");
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged("Region");
            }
        }

        public string[] Regions
        {
            get { return _regions; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}