using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SignExProj
{
    /// <summary>
    /// Логика взаимодействия для ProcessChoose.xaml
    /// </summary>
    public partial class ProcessChoose : Window
    {
        Process process;
        public ProcessChoose()
        {
            InitializeComponent();
            Timer t = new Timer(500);
            t.Elapsed += (object _, ElapsedEventArgs __) =>
            {
                Dispatcher.Invoke(() =>
                ProcessesList.ItemsSource =
                Process.GetProcesses().Select(x => (x.ProcessName, x.Id)).OrderBy(x => x.ProcessName));
            };
            t.Enabled = true;
        }
        private void CloseP(object sender, MouseButtonEventArgs e) =>
            process.Close();
        private void KillP(object sender, MouseButtonEventArgs e) =>
            process.Kill();
        private void ProcessesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProcessesList.SelectedIndex > -1)
                process = 
                    Process.GetProcessById((((string, int))ProcessesList.SelectedItem).Item2);
            else
            {
                Name.Content = "";
                ID.Content = "";
                Responding.Content = "";
                Start.Content = "";
                Readonly.Content = "";
                return;
            }
            Name.Content = process.ProcessName;
            ID.Content = process.Id;
            Responding.Content = process.Responding;
            try
            {
                Start.Content = process.StartTime;
                Readonly.Content = "false";
            }
            catch
            {
                Start.Content = "";
                Readonly.Content = "true";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Readonly.Content == "true") 
            {
                MessageBox.Show("You can't choose readonly process");
                return;
            }
            if (process == null) return;
            Application.Current.Properties["process"] = process;
            Close();
        }

        private void ProcessesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Readonly.Content == "true")
            {
                MessageBox.Show("You can't choose readonly process");
                return;
            }
            if (process == null) return;
            Application.Current.Properties["process"] = process;
            Close();
        }
    }
}
