using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;
using MessageBox = System.Windows.MessageBox;

namespace SlideShow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private object dummyNode = null;


        public MainWindow()
        {
            InitializeComponent();
        }

       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Example!", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
               
                Solution.Items.Add(item);
            }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;

            

            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        subitem.MouseDown += new MouseButtonEventHandler(dispImg);
                        subitem.MouseLeftButtonDown += new MouseButtonEventHandler(dispImg);
                        item.Items.Add(subitem);
                        
                    }

                }
                catch (Exception) { }
            }
        }

        private void dispImg(object sender, RoutedEventArgs e) {
            TreeViewItem item = (TreeViewItem)sender;
            GetImg(item.Tag.ToString());
        }

        private void GetImg(string path)
        {
            //imgPanel.Children.Clear();
            if (path == "") return;

            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                if (folder.Exists)
                {
                    
                    foreach (FileInfo fi in folder.GetFiles())
                    {
                        if (".jpg|.jpeg|.png|.bmp".Contains(fi.Extension.ToLower()))
                        {
                            
                            AddImage(fi.FullName, fi.Name,fi.Length);
                        }
                    }
                }
            }
            catch { }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();

        }

        private void AddImage(string imgPath, string name, long len)
        {
            Image newimg = new Image();
            

            BitmapImage src = new BitmapImage();
        
            src.BeginInit();
            src.UriSource = new Uri(imgPath, UriKind.Absolute);
            src.EndInit();

            newimg.Source = src;
            newimg.Tag = Math.Floor(src.Height)+"."+ Math.Floor(src.Width)+"."+src;
            newimg.Stretch = Stretch.Uniform;
            newimg.Height = 100;
            newimg.MouseDown += (e, s) =>
            {
                fileInf.Visibility = Visibility.Hidden;
                Image item = (Image)e;
                fileName.Text = name;

                string[] sizeInfo = item.Tag.ToString().Split('.');
                fileWi.Text = sizeInfo [0]+ "px";
                fileH.Text = sizeInfo[1] + "px";
                fileSize.Text = (Int16.Parse(sizeInfo[0]) * Int16.Parse(sizeInfo[1])).ToString() + "px";
                Imgdata.Visibility = Visibility.Visible;


            };
            // newimg+= 
            imgPanel.Children.Add(newimg);
        }

        private void openDIR_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult res = dialog.ShowDialog();

            string path = dialog.SelectedPath;
            GetImg(path);
           

        }

    }
}


