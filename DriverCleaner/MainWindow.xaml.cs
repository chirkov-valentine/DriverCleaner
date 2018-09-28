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
using DriverCleaner.Helper;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace DriverCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string driverDirFullName = string.Empty;
        private string zipFileName = string.Empty;
        private string setupCfg = "setup.cfg";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog();
            openDlg.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";

            if (openDlg.ShowDialog() == true)
            {
                zipFileName = openDlg.FileName;
                edtFileName.Text = openDlg.FileName;
                driverDirFullName = Path.Combine(Path.GetDirectoryName(openDlg.FileName),
                                        Path.GetFileNameWithoutExtension(openDlg.FileName));
                edtLog.AppendText("Директория:\n");
                edtLog.AppendText(driverDirFullName +"\n");
                setupCfg = driverDirFullName + "\\" + setupCfg;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            edtLog.AppendText("Распаковка\n");
            ZipHelper.ExtractFileToDirectory(zipFileName, driverDirFullName);
            edtLog.AppendText("Корректировка Setup.cfg\n");
            ZipHelper.CleanCfg(setupCfg);
        }
    }
}
