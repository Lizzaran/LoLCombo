#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 MainWindow.xaml.cs is part of LoLCombo.

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

namespace LoLCombo.Views
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Class;
    using Data;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    #endregion

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private int _startMaximum;
        private int _startProgress;

        public MainWindow()
        {
            Application.Current.DispatcherUnhandledException +=
                delegate
                {
                    try
                    {
                        if (Config != null)
                            Utility.MapClassToXmlFile(typeof (Config), Config, "config.xml");
                    }
                    catch
                    {
                    }
                };

            Utility.CreateFileFromResource("config.xml", "LoLCombo.Resources.config.xml");
            Config = ((Config) Utility.MapXmlFileToClass(typeof (Config), "config.xml"));
            InitializeComponent();
            DataContext = this;
            Title = string.Format("{0} {1}.{2}", Title, Assembly.GetExecutingAssembly().GetName().Version.Major,
                Assembly.GetExecutingAssembly().GetName().Version.Minor);
            if (Config.Collector.Regions.Contains(Config.Collector.Region))
                RegionDropdown.SelectedIndex = Array.IndexOf(Config.Collector.Regions, Config.Collector.Region);
        }

        public int StartMaximum
        {
            get { return _startMaximum; }
            set
            {
                _startMaximum = value;
                OnPropertyChanged("StartMaximum");
            }
        }

        public int StartProgress
        {
            get { return _startProgress; }
            set
            {
                _startProgress = value;
                OnPropertyChanged("StartProgress");
            }
        }

        public Config Config { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private async void Credits_OnClick(object sender, RoutedEventArgs e)
        {
            await
                this.ShowMessageAsync("Credits",
                    "GitHub: Lizzaran" + Environment.NewLine + "Forum: Lizzaran" + Environment.NewLine + "IRC: Appril");
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var dataGrid = (DataGrid) sender;
            if (dataGrid != null)
            {
                if (dataGrid.SelectedItems.Count == 0)
                {
                    e.Handled = true;
                }
                else if (dataGrid.CanUserAddRows)
                {
                    if (dataGrid.Items.IndexOf(dataGrid.CurrentItem) >= dataGrid.Items.Count - 1)
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem) sender;
            if (menuItem != null)
            {
                var contextMenu = (ContextMenu) menuItem.Parent;
                if (contextMenu != null)
                {
                    var dataGrid = (DataGrid) contextMenu.PlacementTarget;
                    if (dataGrid != null)
                    {
                        var rows = dataGrid.SelectedItems;
                        for (var i = rows.Count; i-- > 0;)
                        {
                            switch (dataGrid.Name)
                            {
                                case "PasswordReplaceDataGrid":
                                    Config.Passwords.Replaces.Remove((ConfigPasswordsReplace) rows[i]);
                                    break;

                                case "PasswordAddDataGrid":
                                    Config.Passwords.Adds.Remove((ConfigPasswordsAdd) rows[i]);
                                    break;

                                case "PasswordAppendDataGrid":
                                    Config.Passwords.Appends.Remove((ConfigPasswordsAppend) rows[i]);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            Utility.MapClassToXmlFile(typeof (Config), Config, "config.xml");
        }

        private void OnProgressFinish(Button button, string content)
        {
            foreach (TabItem tabItem in TabControl.Items)
            {
                tabItem.IsEnabled = true;
            }
            button.IsEnabled = true;
            button.Content = content;
        }

        private void OnProgressStart<T>(Button button, Expression<Func<T>> progress)
        {
            foreach (TabItem tabItem in TabControl.Items)
            {
                tabItem.IsEnabled = false;
            }

            var propertyInfo = ((MemberExpression) progress.Body).Member as PropertyInfo;
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, 0);
            }

            button.IsEnabled = false;
            button.Content = new ProgressRing {IsActive = true, Height = 15, Width = 15, Foreground = Brushes.White};
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void StartButton_OnClick(object s, RoutedEventArgs e)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                StartProgress = 0;
                StartMaximum = Config.Collector.PageTo - Config.Collector.PageFrom;
                Application.Current.Dispatcher.Invoke(
                    () => this.OnProgressStart(this.StartButton, () => this.StartProgress));

                var combo = new Combo(Config.Passwords);
                var result = new List<string>();
                var region = Utility.LongRegionToShort(Config.Collector.Region);
                if (region != null)
                {
                    Parallel.For(Config.Collector.PageFrom, Config.Collector.PageTo, i =>
                    {
                        try
                        {
                            if (Config.Collector.AddPassword)
                            {
                                var tmp = Collector.Collect(region, i, Config.Collector.FilterSpecialCharacters);
                                result.AddRange(from name in tmp
                                    let cbo = combo.Create(name)
                                    from c in cbo
                                    select name + ":" + c);
                            }
                            else
                            {
                                result.AddRange(Collector.Collect(region, i, Config.Collector.FilterSpecialCharacters));
                            }
                        }
                        catch
                        {
                        }
                        Application.Current.Dispatcher.Invoke(() => this.StartProgress++);
                    });
                    File.WriteAllLines(
                        string.Format("accounts{1}-{0:yyyy-MM-dd_hh-mm-ss-tt}_{2}.txt", DateTime.Now,
                            Config.Collector.AddPassword ? "+pw" : string.Empty, result.Count), result);
                }
            };
            bw.RunWorkerCompleted += (sender, args) => { OnProgressFinish(StartButton, "Start"); };
            bw.RunWorkerAsync();
        }

        private void RegionDropdown_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Config.Collector.Region = ((SplitButton) sender).SelectedItem.ToString();
        }
    }
}