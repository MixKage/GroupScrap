using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupScrapApp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            CheckBoxHelp.IsChecked = Properties.Settings.Default.Helper;
        }
        private void GroupListButton(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Helper)
                DialogSettings.IsOpen = true;
        }

        private void CheckBoxHelp_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Helper = (bool)CheckBoxHelp.IsChecked;
            Properties.Settings.Default.Save();
        }
    }
}
