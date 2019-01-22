using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace WpfApplication1.View
{
    /// <summary>
    /// ImportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();
            InitGroupBox();
        }

        public bool isEnter;
        private void InitGroupBox()
        {
            if (RbFurniture.IsChecked == true)
            {
                GdProperty.Visibility = Visibility.Visible;
            }
            else
            {
                GdProperty.Visibility = Visibility.Hidden;
            }
            GbDisFromFloor.Visibility = Visibility.Hidden;
        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if (TxtSourcePath.Text.Length != 0)
            {

                //DirectoryInfo TheFolder = new DirectoryInfo(TxtSourcePath.Text);
                //foreach (FileInfo NextFile in TheFolder.GetFiles())
                //{
                //    if (NextFile.Name.Substring(NextFile.Name.LastIndexOf(".")) == ".obj")
                //    {
                //        //xml文件，其实的也类型。也可以改成lamda表达式。
                //        count++;
                //    }
                //}

                //if (count > 0)
                //{
                if (TxtModelEnglishName.Text.Length == 0 || TxtModelEnglishName.Text.Length == 0)
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
                    isEnter = true;
                    this.Close();
                    count = 0;
                //}
                //else
                //{
                //    MessageBox.Show("您选择的路径并没有相关的材质或者模型导入");
                //    return;
                //}

            }
        }

        private void RbFurniture_Checked(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility = Visibility.Visible;
        }

        private void RbFurniture_Unchecked(object sender, RoutedEventArgs e)
        {
            GdProperty.Visibility = Visibility.Hidden;
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {           
            string oldstrpath = "";
            string defaultfilePath = "";
            FolderBrowserDialog folderdlg = new FolderBrowserDialog();
            if (defaultfilePath != "")
            {
                //设置此次默认目录为上一次选中目录  
                folderdlg.SelectedPath = defaultfilePath;
            }
            if (folderdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                oldstrpath = folderdlg.SelectedPath;
                defaultfilePath = folderdlg.SelectedPath;
                TxtSourcePath.Text = defaultfilePath;
                try
                {
                    string namefolder = null;
                    string str = defaultfilePath;
                    string[] s = str.Split(new char[] { '\\' });
                    namefolder = s[s.Length - 1];
                    int andCount = 0;
                    for (int i = 0; i < namefolder.Length; i++)
                    {
                        if (namefolder[i] == '&')
                        {
                            andCount++;
                        }
                    }

                    if (andCount == 1)
                    {
                        string[] sname = namefolder.Split(new char[] { '&' });

                       TxtModelChineseName.Text = sname[1];
                        TxtModelEnglishName.Text = sname[0];
                    }
                    else
                    {
                        TxtSourcePath.Text = defaultfilePath;

                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("并不能导入:  "+ex);
                    throw;
                }              
                //DirectoryInfo TheFolder = new DirectoryInfo(oldstrpath);
                //foreach (FileInfo NextFile in TheFolder.GetFiles())
                //{
                //    if (
                //        NextFile.Name.Substring(NextFile.Name.LastIndexOf(".")) ==
                //        ".obj" ||
                //        NextFile.Name.Substring(NextFile.Name.LastIndexOf(".")) ==
                //        ".mtl" ||
                //        NextFile.Name.Substring(NextFile.Name.LastIndexOf(".")) ==
                //        "D2Geometry.xml")
                //    {

                //    }                                 
                //}
            }
        }

        private void TxtDefDisFromFloor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = @"(^[1-9]\d{0,2}$)|(^(10000)$)";
            Match m = Regex.Match(this.TxtDefDisFromFloor.Text, pattern);   // 匹配正则表达式

            string param1;
            if (!m.Success)   // 输入的不是数字
            {
                //param1 = "0";
                //this.TxtDefDisFromFloor.Text = param1;   // textBox内容不变

                // 将光标定位到文本框的最后
                this.TxtDefDisFromFloor.SelectionStart = this.TxtDefDisFromFloor.Text.Length;
            }
            else   // 输入的是数字
            {
                param1 = this.TxtDefDisFromFloor.Text;   // 将现在textBox的值保存下来
            }
        }

        private void TxtDefDisFromFloor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CheckNumber(sender, e);
        }
        private void CheckNumber(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
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

        private void RbFloorMove_Checked(object sender, RoutedEventArgs e)
        {
            GbDisFromFloor.Visibility = Visibility.Visible;
        }

        private void RbFloorMove_Unchecked(object sender, RoutedEventArgs e)
        {
            GbDisFromFloor.Visibility = Visibility.Hidden;
        }
    }
}
