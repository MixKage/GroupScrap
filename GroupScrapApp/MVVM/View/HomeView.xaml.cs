using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
        public void OpenDialogError()
        {

        }

        private static void StartScrabPeople(object uri)
        {
            string uriText = (string)uri;
            string err = "";
            using (var s = new StreamReader(@"groupList.txt", Encoding.UTF8))
            {
                if (s.ReadLine() == null)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, EMPTY FILE" + Environment.NewLine;
                    Debug.WriteLine("ERR, EMPTY FILE");
                    return;
                }
                if (uriText == "" || uriText == null)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, EMPTY VAL" + Environment.NewLine;
                    Debug.WriteLine("ERR, EMPTY VAL");
                    return;
                }
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"GetWeb.exe",
                        Arguments = uriText,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, GetWeb.exe" + Environment.NewLine;
                    Debug.WriteLine("ERR, GetWeb.exe");
                }

                var process2 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"Parse.exe",
                        Arguments = "ans.html",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process2.Start();
                process2.WaitForExit();
                if (process2.ExitCode != 0)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, Parse.exe" + Environment.NewLine;
                    Debug.WriteLine("ERR, Parse.exe");
                }

                var process3 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"comparison_engine.exe",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process3.Start();
                process3.WaitForExit();
                if (process3.ExitCode != 0)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, comparison_engine.exe" + Environment.NewLine;
                    Debug.WriteLine("ERR, comparison_engine.exe");
                }
                if (err != "")
                {
                    using (StreamWriter sw = new StreamWriter(@"logFile.txt", true, System.Text.Encoding.UTF8))
                    {
                        sw.Write(err);
                    }
                }
            }
        }

        public void CreateExcel()
        {
            using (ExcelEngine excelEngiene = new ExcelEngine())
            {
                IApplication application = excelEngiene.Excel;

                application.DefaultVersion = ExcelVersion.Excel2013;

                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                workbook.SaveAs("Output.xlsx");
            }
            Process.Start("Output.xlsx");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = textBoxUri.Text.ToString();
            if (s == null || s == "")
                textBlockDialog.Text = "Поле с ссылкой путое!";
            else
                textBlockDialog.Text = "Если Chrome завис - нажмите 'Ctrl +'";
            Thread processThread = new Thread(new ParameterizedThreadStart(StartScrabPeople));
            processThread.Start(textBoxUri.Text.ToString());
            dialogHome.IsOpen = true;
        }

        private void textBoxUri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }
}
