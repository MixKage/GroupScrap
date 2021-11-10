using System;
using System.Collections.Generic;
using System.IO;
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
        bool isAddListDialog = false;

        public SettingsView()
        {
            InitializeComponent();
            CheckBoxHelp.IsChecked = Properties.Settings.Default.Helper;
        }
        private void GroupListButton(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Helper)
                ShowDialogHelper(1);
        }

        private void WebListButton(object sender, RoutedEventArgs e)
        {
            addList();
        }

        private void VarListButton(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Helper)
                ShowDialogHelper(3);
        }

        private void TranslitListButton(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Helper)
                ShowDialogHelper(4);
        }

        private void ConfigListButton(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Helper)
                ShowDialogHelper(5);
        }

        private void CheckBoxHelp_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Helper = (bool)CheckBoxHelp.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void ShowDialogHelper(int tap, List<CheckBox> checkBoxes = null)
        {
            stackPanelDialog.Children.Clear();
            TextBlock text = new TextBlock();
            text.Name = "textDialog";
            text.TextWrapping = TextWrapping.Wrap;
            if (checkBoxes != null)
            {
                foreach (var check in checkBoxes)
                    stackPanelDialog.Children.Add(check);
                Viewbox view = new Viewbox();
                view.Height = 20;
                stackPanelDialog.Children.Add(view);
            }
            switch (tap)
            {
                case 1:
                    text.FontSize = 14;
                    text.Text = "Пиши каждый ФИО с новой строки (без знаков препинания). Принимается только ФИО!";
                    break;
                case 2:
                    text.FontSize = 12;
                    text.Text = "Отметь галочками тех людей, кто был на паре (даже если это не так)";
                    break;
                case 3:
                    text.FontSize = 14;
                    text.Text = "Допустима лишь одна вариативность (писать в верхнем регистре)! Пример записи: АЛЕКСАНДР>САША";
                    break;
                case 4:
                    text.FontSize = 14;
                    text.Text = "Одна буква не может содержать больше 2 транслитных букв (писать в верхнем регистре)! Пример записи: Ж>ZH";
                    break;
                case 5:
                    text.FontSize = 14;
                    text.Text = "Всё что можно изменить - max_difference (максимальное различие между эталонным и полученным с сайта ФИО)";
                    break;
                case 6:
                    text.FontSize = 14;
                    text.Text = "Не введено ни единого ФИО в GroupList!";
                    break;
            }
            stackPanelDialog.Children.Add(text);
            Viewbox view2 = new Viewbox();
            view2.Height = 20;
            stackPanelDialog.Children.Add(view2);
            DialogSettings.IsOpen = true;
        }

        private void addList()
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (string line in File.ReadLines(@"groupList.txt", Encoding.UTF8))
            {
                var check = new CheckBox();
                check.Content = line;
                checkBoxes.Add(check);
            }
            if (checkBoxes.Count == 0)
                ShowDialogHelper(6);
            foreach (string line in File.ReadLines(@"addList.txt", Encoding.UTF8))
            {
                foreach (var check in checkBoxes)
                {
                    if (line != check.Content.ToString())
                        continue;
                    check.IsChecked = true;
                }
            }
            foreach (var check in checkBoxes)
                stackPanelDialog.Children.Add(check);
            isAddListDialog = true;
            ShowDialogHelper(2, checkBoxes);
        }

        private void CloseSettingsDialog_Click(object sender, RoutedEventArgs e)
        {
            if (!isAddListDialog)
                return;
            isAddListDialog = false;

            string textFIO = "";

            foreach (var child in stackPanelDialog.Children)
            {
                if ((child as CheckBox) == null)
                    continue;
                if ((bool)(child as CheckBox).IsChecked)
                    textFIO += (child as CheckBox).Content.ToString() + Environment.NewLine;
            }
            using (StreamWriter sw = new StreamWriter(@"addList.txt", false, System.Text.Encoding.UTF8))
            {
                sw.Write(textFIO);
            }
            
        }
    }
}