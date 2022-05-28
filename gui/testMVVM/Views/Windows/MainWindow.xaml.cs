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
    }
}
