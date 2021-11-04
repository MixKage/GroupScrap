using GroupScrapApp.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupScrapApp.MVVM.ViewModel
{
    class SettingsViewModel
    {
        public RelayCommand OpenListFile { get; set; }

        public RelayCommand OpenAddListFile { get; set; }

        public RelayCommand OpenVarListFile { get; set; }

        public RelayCommand OpenTranslitListFile { get; set; }

        public RelayCommand OpenConfListFile { get; set; }

        public SettingsViewModel()
        {
            OpenListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"StorrageFold\groupList.txt");
            });

            OpenAddListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"StorrageFold\addList.txt");
            });

            OpenVarListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"StorrageFold\variabilityName.txt");
            });

            OpenTranslitListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"StorrageFold\alfavitTranslit.txt");
            });

            OpenConfListFile = new RelayCommand(o =>
            {
                Process p = Process.Start("notepad.exe", @"StorrageFold\configEngine.txt");
            });
        }
    }
}
