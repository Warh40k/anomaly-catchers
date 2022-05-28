using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Text.Json;
using testMVVM.ViewModels;

namespace testMVVM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"C:\Users\user\Documents\Data\db1";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                db1_textbox.Text = dialog.SelectedPath;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"C:\Users\user\Documents\Data\db2";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                db2_textbox.Text = dialog.SelectedPath;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string exepath = "";
            //System.Diagnostics.Process.Start(exepath, " '" + db1_textbox.Text + "'" + " " + "'" + db2_textbox.Text + "'");
            Thread.Sleep(2000);

            string json = System.IO.File.ReadAllText(@"C:\Users\user\source\repos\anomaly-catchers\gui\testMVVM\Data\result.json");
            //string result = JsonSerializer.Deserialize<string>(json);
            //notify_listview.Items.Add($"{MainWindowViewModel.dict[0].Id} + {MainWindowViewModel.dict[0].Description}");
        }
    }
}
