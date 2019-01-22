using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication1.ViewModel;

namespace WpfApplication1.View
{
    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();
        }
        
        public bool IsEnter;
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (TxtChineseName.Text.Length == 0 ||
                TxtEnglishName.Text.Length == 0)
            {
                MessageBox.Show("中文名或者英文名不能为空");
                return;
            }
            IsEnter = true;
            this.Close();
        }
    }
}
