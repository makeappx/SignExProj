using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
        Process process = null;
        SignEX s;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_processes(object sender, RoutedEventArgs e)
        {
            (new ProcessChoose()).ShowDialog();
            try
            {
                process = (Process)Application.Current.Properties["process"];
                ProcessLabel.Content = process.ProcessName;
            }
            catch { }
            GC.Collect();
        }
        private void Text1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            double size = window.ActualHeight;
            foreach (StackPanel k in sp.Children)
                size -= k.ActualHeight;
            size -= 80;
            if (size < 0) window.Height -= size;
        }
        private void Button_search(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            if (process == null)
            {
                MessageBox.Show("You have to select process first");
                return;
            }
            if (string.IsNullOrWhiteSpace(Text1.Text)) return;
            s = new SignEX(process);
            byte[] bytes_scan = null;
            int? value = null;
            if (ValueBox.SelectedIndex == 1)
                try
                {
                    if (BaseBox.SelectedIndex == 1)
                        bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).
                                   Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                    else
                        bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).
                                   Select(byte.Parse).ToArray();
                }
                catch
                {
                    MessageBox.Show("Values can't be parsed", "Error");
                    return;
                }
            else
                try
                {
                    if (BaseBox.SelectedIndex == 1)
                        value = int.Parse(Text1.Text, System.Globalization.NumberStyles.HexNumber);
                    else
                        value = int.Parse(Text1.Text);
                    bytes_scan = BitConverter.GetBytes((int)value);
                }
                catch
                {
                    MessageBox.Show("Values can't be parsed", "Error");
                    return;
                }
            if (BaseBox.SelectedIndex == 1)
                addresses_list.ItemsSource = s.SignScan(bytes_scan).Select(x =>
                ((IntPtr)x, value == null ? Text1.Text : value.ToString()));
            else
                addresses_list.ItemsSource = s.SignScan(bytes_scan).Select(x =>
                ((IntPtr)x, value == null ? Text1.Text : value.ToString()));
            Updatelist();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
        private void Adresses_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (addresses_list.Items.Count != 0) 
            (new ValueChange(s, addresses_list.SelectedItems.Cast<(IntPtr, string)>().Select(x => x.Item1))).ShowDialog();
        }
        private void Updatelist()
        {
            address_count.Content = "Count: " + addresses_list.Items.Count;
        }
        private byte[] GetBytes((IntPtr, string) value)
        {
            byte[] current;
            if (ValueBox.SelectedIndex == 1)
                if (BaseBox.SelectedIndex == 1)
                    current = value.Item2.Split(' ').Where(x => x != string.Empty).
                              Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                else
                    current = value.Item2.Split(' ').Where(x => x != string.Empty).
                              Select(byte.Parse).ToArray();
            else
            {
                int val;
                if (BaseBox.SelectedIndex == 1)
                    val = int.Parse(Text1.Text, System.Globalization.NumberStyles.HexNumber);
                else
                    val = int.Parse(Text1.Text);
                current = BitConverter.GetBytes(val);
            }
            return current;
        }
        private void Button_unchanged(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            List<(IntPtr, string)> result = new List<(IntPtr, string)>();
            foreach ((IntPtr, string) value in addresses_list.Items)
            {
                try
                {
                    byte[] current = GetBytes(value);
                    if (current.SequenceEqual(s.CheckValue(value.Item1, current.Length)))
                        result.Add(value);
                }
                catch { }
            }
            addresses_list.ItemsSource = result;
            Updatelist();
            GC.Collect();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
        private void Button_changed(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            List<(IntPtr, string)> result = new List<(IntPtr, string)>();
            foreach ((IntPtr, string) value in addresses_list.Items)
            {
                try
                {
                    byte[] current = GetBytes(value);
                    byte[] changed = s.CheckValue(value.Item1, current.Length);
                    if (!current.SequenceEqual(changed))
                    {
                        if (ValueBox.SelectedIndex == 1)
                            result.Add((value.Item1, string.Join("",
                                changed.Select(x => x.ToString(BaseBox.SelectedIndex == 1 ? "X" : "") + " "))));
                    }
                }
                catch { }
            }
            addresses_list.ItemsSource = result;
            Updatelist();
            GC.Collect();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
        private void Button_next(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            List<(IntPtr, string)> result = new List<(IntPtr, string)>();
            byte[] bytes_scan;
            int? value; 
            if (string.IsNullOrWhiteSpace(Text1.Text)) return;
            if (ValueBox.SelectedIndex == 1)
                try
                {
                    if (BaseBox.SelectedIndex == 1)
                        bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).
                                   Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                    else
                        bytes_scan = Text1.Text.Split(' ').Where(x => x != string.Empty).
                                   Select(byte.Parse).ToArray();
                }
                catch
                {
                    MessageBox.Show("Values can't be parsed", "Error");
                    return;
                }
            else
                try
                {
                    if (BaseBox.SelectedIndex == 1)
                        value = int.Parse(Text1.Text, System.Globalization.NumberStyles.HexNumber);
                    else
                        value = int.Parse(Text1.Text);
                    bytes_scan = BitConverter.GetBytes((int)value);
                }
                catch
                {
                    MessageBox.Show("Values can't be parsed", "Error");
                    return;
                }
            foreach ((IntPtr, string) val in addresses_list.Items)
                if (bytes_scan.SequenceEqual(s.CheckValue(val.Item1, bytes_scan.Length)))
                    result.Add((val.Item1, Text1.Text));
            addresses_list.ItemsSource = result;
            Updatelist();
            GC.Collect();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
        private void Button_increased(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            List<(IntPtr, string)> result = new List<(IntPtr, string)>();
            foreach ((IntPtr, string) value in addresses_list.Items)
            {
                try {
                    byte[] current = GetBytes(value);
                    byte[] changed = s.CheckValue(value.Item1, current.Length);
                    if (BitConverter.ToInt32(changed, 0) > BitConverter.ToInt32(current, 0))
                    {
                        if (ValueBox.SelectedIndex == 1)
                            result.Add((value.Item1, string.Join("",
                                changed.Select(x => x.ToString(BaseBox.SelectedIndex == 1 ? "X" : "") + " "))));
                    }
                }
                catch
                {}
            }
            addresses_list.ItemsSource = result;
            Updatelist();
            GC.Collect();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
        private void Button_decreased(object sender, RoutedEventArgs e)
        {
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            List<(IntPtr, string)> result = new List<(IntPtr, string)>();
            foreach ((IntPtr, string) value in addresses_list.Items)
            {
                try
                {
                    byte[] current = GetBytes(value);
                    byte[] changed = s.CheckValue(value.Item1, current.Length);
                    if (BitConverter.ToInt32(changed, 0) < BitConverter.ToInt32(current, 0))
                    {
                        if (ValueBox.SelectedIndex == 1)
                            result.Add((value.Item1, string.Join("",
                                changed.Select(x => x.ToString(BaseBox.SelectedIndex == 1 ? "X" : "") + " "))));
                    }
                }
                catch {}
            }
            addresses_list.ItemsSource = result;
            Updatelist();
            GC.Collect();
            //TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        }
    }
}
