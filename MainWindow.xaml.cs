using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SignExProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process process;
        public MainWindow()
        {
            InitializeComponent();
            System.Timers.Timer t = new System.Timers.Timer(2000);
            t.Elapsed += (object _, ElapsedEventArgs __) =>
            {
                Dispatcher.Invoke(() => 
                ProcessesList.ItemsSource = 
                Process.GetProcesses().Select(x => (x.ProcessName, x.Id)).OrderBy(x => x.ProcessName));
            };
            t.Enabled = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckLength()) return;
            byte[] bytes_scan;
            byte[] bytes_write;
            if (BaseBox.SelectedIndex == 0)
                try
                {
                    bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).Select(byte.Parse).ToArray();
                    bytes_write = Text2.Text.Split(' ').Where(x => x != string.Empty).Select(byte.Parse).ToArray();
                }
                catch { MessageBox.Show("Some values can't be parsed","Error"); return; }
            else
            {
                try
                {
                    bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).
                        Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                    bytes_write = Text2.Text.Split(' ').Where(x => x != string.Empty).
                        Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                }
                catch { MessageBox.Show("Some values can't be parsed", "Error"); return; }
            }
            SignEX s = new SignEX(process);
            if (s.WriteBytes(bytes_scan, bytes_write, true))
                MessageBox.Show("Bytes were succesfully injected", "OK");
            else MessageBox.Show("Error");
        }
        private bool CheckLength()
        {
            int a = Text1.Text.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int b = Text2.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (a > b)
            {
                if (MessageBox.Show("Bytes lengths should be equal\nDo you want to add nops?",
                                "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    for (int i = 0; i < a - b; i++)
                        if (BaseBox.SelectedIndex == 0) Text2.Text += " 144";
                        else Text2.Text += " 90";
            }
            else if (a < b)
            {
                if (MessageBox.Show("Bytes for scan can't be less than bytes for write",
                                "Error", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    return false;
            }
            return true;
        }

        private void RefreshP(object sender, MouseButtonEventArgs e)
        {
         
        }
        private void CloseP(object sender, MouseButtonEventArgs e) =>
            process.Close();
        private void KillP(object sender, MouseButtonEventArgs e) =>
            process.Kill();
        private void ProcessesList_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            process = Process.GetProcessById((((string, int))ProcessesList.SelectedItem).Item2);
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string s = Text1.Text;
            Text1.Text = Text2.Text;
            Text2.Text = s;
        }
    }
}
