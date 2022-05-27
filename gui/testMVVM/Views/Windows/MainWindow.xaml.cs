using System;
using System.Windows;
using System.Windows.Forms;

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
            dialog.SelectedPath = @"C:\Users\user\source\repos\anomaly-catchers\gui\testMVVM\Data\db1";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                db1_textbox.Text = dialog.SelectedPath;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"C:\Users\user\source\repos\anomaly-catchers\gui\testMVVM\Data\db2";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                db2_textbox.Text = dialog.SelectedPath;
        }
    }
}
