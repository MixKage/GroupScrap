using GroupScrapApp.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GroupScrapApp.MVVM.ViewModel
{
    class SettingsViewModel
    {
        public RelayCommand OpenListFile { get; set; }

        public RelayCommand OpenVarListFile { get; set; }

        public RelayCommand OpenTranslitListFile { get; set; }

        public RelayCommand OpenConfListFile { get; set; }

        public RelayCommand OpenSiteNNC { get; set; }
        public SettingsViewModel()
        {
            OpenListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"groupList.txt");
            });

            OpenVarListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"variabilityName.txt");
            });

            OpenTranslitListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"alfavitTranslit.txt");
            });

            OpenConfListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"configEngine.txt");
            });

            OpenSiteNNC = new RelayCommand(o =>
            {
                var uri = new Uri("http://group-hw.ru/");
                System.Diagnostics.Process.Start("www.nncompany.site");
            });
        }
    }
}
