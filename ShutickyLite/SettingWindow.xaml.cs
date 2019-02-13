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
using System.Windows.Shapes;

namespace ShutickyLite
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            //前回の設定情報を復元
            CheckBox_ExecuteAtStartup.IsChecked = Properties.Settings.Default.ExecuteAtStartupSetting;
        }


        private readonly string executeAtStartupKeyTree = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private readonly string applicationName = "ShutickyLite";

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e != null)
                e.Cancel = true;

            this.Visibility = Visibility.Hidden;
        }

        private void Button_ApplySetting_Click(object sender, RoutedEventArgs e)
        {
            //設定を内部データに反映
            Properties.Settings.Default.ExecuteAtStartupSetting = (bool)CheckBox_ExecuteAtStartup.IsChecked;

            //スタートアップへの登録
            if(Properties.Settings.Default.ExecuteAtStartupSetting)
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(executeAtStartupKeyTree,true);
                regkey?.SetValue(applicationName, assembly.Location);
                regkey?.Close();
            }
            else
            {
                var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(executeAtStartupKeyTree, true);
                regkey?.DeleteValue(applicationName, false);
                regkey?.Close();
            }

            this.Visibility = Visibility.Hidden;
        }

        private void Button_CancelSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }


    }
}
