using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GroupScrapApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int countTap = 0;
        public MainWindow()
        {
            InitializeComponent();
            MVVM.ViewModel.MainViewModel vm = new MVVM.ViewModel.MainViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
            IsFirstStart();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void IsFirstStart()
        {
            if (Properties.Settings.Default.FirstStart)
            {
                MyDialog.IsOpen = true;
            }
        }

        private void ButtonDialog_Click(object sender, RoutedEventArgs e)
        {
            countTap++;
            switch (countTap)
            {
                case 1:
                    BackButton.Visibility = Visibility.Visible;
                    TextInDialog.Text = "Цель программы - подключиться к вебинару (в прямом эфере или записи) и получить список присутствующих.";
                    break;
                case 2:
                    TextInDialog.Text = "Для этого, она сравнивает списки сайта и твою группу. Программа умеет распознавать транслит и учитывать вариативность имён.";
                    break;
                case 3:
                    TextInDialog.Text = "Абсолютно всё настраивается (алфавит транслита, список вариативных имён, список предварительно присутствующих, ФИО, ИФО, ИФ, ФИ). Все данные хранятся в .txt файлах.";
                    break;
                case 4:
                    TextInDialog.Text = "Доступ к ним можно получить через вкладку настроек. Очень важно соблюдать правила, по которым эти файлы были записаны.";
                    break;
                case 5:
                    TextInDialog.Text = "Файлы уже заполнены тестовой информацией. Удачи! :)";
                    ContinueButton.Content = "Закрыть";
                    break;
                case 6:
                    MyDialog.IsOpen = false;
                    Properties.Settings.Default.FirstStart = false;
                    Properties.Settings.Default.Save();
                    break;
                default:
                    break;
            }
        }

        private void ButtonDialog2_Click(object sender, RoutedEventArgs e)
        {
            countTap--;
            switch (countTap)
            {
                case 0:
                    BackButton.Visibility = Visibility.Collapsed;
                    TextInDialog.Text = "Добро пожаловать в GroupScrap! Сейчас, мы поможем разобраться в программе.";
                    break;
                case 1:
                    TextInDialog.Text = "Цель программы - подключиться к вебинару (в прямом эфере или записи) и получить список присутствующих.";
                    break;
                case 2:
                    TextInDialog.Text = "Для этого, она сравнивает списки сайта и твоей группы (необхоодим Chrome). Программа умеет распозновать транслит и учитывать вариативность имён.";
                    break;
                case 3:
                    TextInDialog.Text = "Обсалютно всё настраивается (алфавит транслита, список вариативных имён, список якобы присутствующих). Все данные хранятся в .txt файлах.";
                    break;
                case 4:
                    TextInDialog.Text = "Доступ к ним можно получить через вкладку настроек. Очень важно соблюдать правила, по которым эти файлы были записаны.";
                    ContinueButton.Content = "Продолжить";
                    break;
                default:
                    break;
            }
        }
    }
}
