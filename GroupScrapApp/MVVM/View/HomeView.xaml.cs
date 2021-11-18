using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private void StartScrabPeople(object uri, object isExcel)
        {
            string uriText = (string)uri;
            string emailText = textBoxEmail.Text;
            string err = "";
            using (var s = new StreamReader(@"groupList.txt", Encoding.UTF8))
            {
                if (s.ReadLine() == null)
                {
                    //Отошёл он
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, EMPTY FILE" + Environment.NewLine;
                    Debug.WriteLine("ERR, EMPTY FILE");
                    Dispatcher.Invoke((() =>
                    {
                        textBlockDialog.Text = "Ошибка, пустой файл группы";
                    }));
                    return;
                }
                if (uriText == "" || uriText == null)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, EMPTY VAL" + Environment.NewLine;
                    Debug.WriteLine("ERR, EMPTY VAL");
                    Dispatcher.Invoke(() =>
                    {
                        textBlockDialog.Text = "Ошибка, путая строка";
                    });
                    return;
                }
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"GetWeb.exe",
                        Arguments = uriText + " " + emailText,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, GetWeb.exe : " + process.ExitCode + Environment.NewLine;
                    Debug.WriteLine("ERR, GetWeb.exe");
                    Dispatcher.Invoke(() =>
                    {
                        textBlockDialog.Text = "Ошибка, GetWeb.exe : " + process.ExitCode;
                    });
                    return;
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
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, Parse.exe : " + process2.ExitCode + Environment.NewLine;
                    Debug.WriteLine("ERR, Parse.exe");
                    Dispatcher.Invoke(() =>
                    {
                        textBlockDialog.Text = "ERR, Parse.exe : " + process2.ExitCode;
                    });
                    return;
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
                    err += DateTime.Now.ToString("HH:mm:ss") + " : " + "ERR, comparison_engine.exe : " + process3.ExitCode + Environment.NewLine;
                    Debug.WriteLine("ERR, comparison_engine.exe");
                    Dispatcher.Invoke(() =>
                    {
                        textBlockDialog.Text = "ERR, comparison_engine.exe : " + process3.ExitCode;
                    });
                    return;
                }
                if (err != "")
                {
                    using (StreamWriter sw = new StreamWriter(@"logFile.txt", true, System.Text.Encoding.UTF8))
                    {
                        sw.Write(err);
                    }
                }
                if ((bool)isExcel)
                    CreateExcel();
                else
                    Process.Start("outputList.txt");
                Dispatcher.Invoke(() =>
                    {
                        dialogHome.IsOpen = false;
                    });
            }
        }

        public static void CreateExcel()
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Посещаемость");
            IRow row = sheet.CreateRow(0);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("ФИО");
            cell = row.CreateCell(1);
            cell.SetCellValue(DateTime.Now.ToString("dd.MM"));

            int index = 0;
            List<string> outputList = new List<string>();
            foreach (string line in File.ReadLines(@"outputList.txt", Encoding.UTF8)) { outputList.Add(line); }

            foreach (string line in File.ReadLines(@"groupList.txt", Encoding.UTF8))
            {
                row = sheet.CreateRow(index + 1);
                cell = row.CreateCell(0);
                cell.SetCellValue(line);
                cell = row.CreateCell(1);
                ICellStyle styleMiddle = workbook.CreateCellStyle();
                styleMiddle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                cell.CellStyle = styleMiddle;
                cell.SetCellValue(outputList[index]);
                index++;
            }

            for (int i = 0; i <= 20; i++) sheet.AutoSizeColumn(i);
            using (FileStream fs = new FileStream("Output.xlsx", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            Process.Start("Output.xlsx");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = textBoxUri.Text.ToString();
            if (s == null || s == "")
            {
                Dispatcher.Invoke((() =>
                {
                    textBlockDialog.Text = "Ошибка, пустая ссылка!";
                    dialogHome.IsOpen = true;
                }));
                return;
            }
            else
                textBlockDialog.Text = "Если Chrome завис - нажмите 'Ctrl +'";
            s = textBoxEmail.Text.ToString();
            if (s == null || s == "" || !s.Contains("@"))
            {
                Dispatcher.Invoke((() =>
                {
                    textBlockDialog.Text = "Ошибка, почты!";
                    dialogHome.IsOpen = true;
                }));
                return;
            }
            else
                textBlockDialog.Text = "Если Chrome завис - нажмите 'Ctrl +'";
            bool isExcel = true;
            try
            {
                using (FileStream fs = new FileStream("Output.xlsx", FileMode.Create, FileAccess.Write)) { }
            }
            catch
            {
                isExcel = false;
                textBlockDialog.Text = "Если Chrome завис - нажмите 'Ctrl +'\nExcel открыт и не даёт открыть файл Output.xlsx на запись, будет открыт .txt";
            }
            try
            {
                Task.Factory.StartNew(() =>
                {
                    StartScrabPeople(s, isExcel);
                });
            }
            catch
            {
                Debug.WriteLine("ERR, NewThread");
            }

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
