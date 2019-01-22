using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1.View
{
    /// <summary>
    /// EditWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow1 : Window
    {
        public EditWindow1()
        {
            InitializeComponent();
            GbDisFromFloor.Visibility = Visibility.Hidden;
        }

        public bool IsEnter;
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            if (TxtModelChineseName.Text.Length==0 || TxtModelEnglishName.Text.Length == 0)
            {
                MessageBox.Show("中文名或者英文名不能为空");
                return;
            }
            if (RbFurniture.IsChecked == false && RbMaterial.IsChecked == false)
            {
                MessageBox.Show("请选择模型类型");
                return;
            }
            if (RbFurniture.IsChecked == true)
            {
                if (RbOnWall.IsChecked == false && RbInWall.IsChecked == false)
                {
                    MessageBox.Show("家具靠墙属性不能为空");
                    return;
                }
                else if (RbFloorMove.IsChecked == false &&
                     RbInCelling.IsChecked == false &&
                     RbOnFloor.IsChecked == false && RbInFloor.IsChecked == false &&
                     RbOnCelling.IsChecked == false)
                {
                    MessageBox.Show("家具垂直属性不能为空");
                    return;
                }
                else if (RbTransparent.IsChecked == false &&
                    RbTranslucent.IsChecked == false)
                {
                    MessageBox.Show("家具透明度不能为空");
                    return;
                }
            }
            this.Close();
            IsEnter = true;
        }

        private void RbFurniture_Click(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility=Visibility.Visible;
            
        }

        private void RbFurniture_Checked(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility = Visibility.Visible;
        }

        private void RbFurniture_Unchecked(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility = Visibility.Hidden;
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RbMaterial_Checked(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility = Visibility.Hidden;
        }

        private void TxtDefaultFromFloor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = @"(^[1-9]\d{0,2}$)|(^(10000)$)";
           
            Match m = Regex.Match(this.TxtDefaultFromFloor.Text, pattern);   // 匹配正则表达式

            string param1;
            if (!m.Success)   // 输入的不是数字
            {
                //param1 = "0";
                ////param1 =this.TxtDefaultFromFloor.Text ;   // textBox内容不变
                //this.TxtDefaultFromFloor.Text = param1;
                //// 将光标定位到文本框的最后
                //this.TxtDefaultFromFloor.SelectionStart = this.TxtDefaultFromFloor.Text.Length;
                e.Handled = true;
            }
            else   // 输入的是数字
            {
                param1 = this.TxtDefaultFromFloor.Text;   // 将现在textBox的值保存下来
            }
        }
        private void CheckNumber(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键txt.Text.Contains(".") &&
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                      e.Key == Key.OemPeriod) &&
                     e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtDefaultFromFloor_KeyDown(object sender, KeyEventArgs e)
        {
            CheckNumber(sender, e);
        }

        private void RbFloorMove_Unchecked(object sender, RoutedEventArgs e)
        {
            GbDisFromFloor.Visibility = Visibility.Hidden;
        }

        private void RbFloorMove_Checked(object sender, RoutedEventArgs e)
        {
            GbDisFromFloor.Visibility = Visibility.Visible;
        }
    }
}
