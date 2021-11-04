using GroupScrapApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GroupScrapApp.MVVM.ViewModel
{
    class HomeViewModel
    {
        public RelayCommand StartProgrammCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal val;
        public decimal Val
        {
            get { return val; }
            set
            {
                val = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            StartProgrammCommand = new RelayCommand(o =>
            {
                //var uri2 = new System.Uri(@"\StorrageFold\groupList.txt");
                using (var s = new StreamReader(@"StorrageFold\groupList.txt", Encoding.UTF8))
                {
                    if(s.ReadLine() == null)
                    {
                        Debug.WriteLine("ERR, EMPTY FILE");
                    }
                    Process p = Process.Start(@"C:\Users\shish\Source\Repos\MixKage\GroupScrapApp\GroupScrapApp\bin\Debug\StorrageFold\GetWeb.exe", "https://events.webinar.ru/3865279/9409989/0efbf93da511bae0786a831977663834");
                }
            });
            checkIsFirstStart();
        }

        void checkIsFirstStart()
        {
            if (Properties.Settings.Default.FirstStart == true)
            {
                Properties.Settings.Default.FirstStart = false;
                Debug.WriteLine("YES");
            }
            else 
                Debug.WriteLine("NO");
        }
    }
}
