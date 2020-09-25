using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SignExProj
{
    /// <summary>
    /// Логика взаимодействия для ValueChange.xaml
    /// </summary>
    public partial class ValueChange : Window
    {
        IEnumerable<IntPtr> changelist;
        SignEX sc;
        public ValueChange(SignEX s, IEnumerable<IntPtr> list)
        {
            InitializeComponent();
            changelist = list;
            sc = s;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text1.Text)) return;
            byte[] bytes_write;
            int? value;
            if (ValueBox.SelectedIndex == 1)
                try
                {
                    if (BaseBox.SelectedIndex == 1)
                        bytes_write = Text1.Text.Split(' ').Where(x => x != string.Empty).
                                   Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
                    else
                        bytes_write = Text1.Text.Split(' ').Where(x => x != string.Empty).
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
                    {
                        value = int.Parse(Text1.Text, System.Globalization.NumberStyles.HexNumber);
                        bytes_write = BitConverter.GetBytes((int)value);
                    }
                    else
                    {
                        value = int.Parse(Text1.Text);
                        bytes_write = BitConverter.GetBytes((int)value);
                    }
                }
                catch
                {
                    MessageBox.Show("Values can't be parsed", "Error");
                    return;
                }
            foreach(IntPtr adr in changelist)
                sc.WriteBytes(adr, bytes_write);
            Close();
        }
    }
}
