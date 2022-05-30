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
using System.Windows.Media.Effects;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Reflection;

namespace SlideShow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private object dummyNode = null;
        private string imagesdir="";
        private Image[] toshow=new Image[100];
        private Window _modal ;
        private  string ImageinSlide;
        private string type= "horizontal";
        public string[] ComboxItems = { "vertical", "horizontal", "opacity" };
        int imgpos = 0;

        private DispatcherTimer ChangeIMG;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ComboxItems;

            ChangeIMG = new DispatcherTimer();
            ChangeIMG.Interval = new TimeSpan(0, 0, 2);
            ChangeIMG.Tick += new EventHandler(ChangeIMG_Tick);
            Assembly assembly = Assembly.LoadFrom("");

        }

		private void ChangeIMG_Tick(object sender, EventArgs e)
		{
            if (imgpos == 99) imgpos = 0;
            if (toshow[imgpos+1] == null) imgpos = 0;
            _modal.Content = toshow[imgpos];
            switch (type)
            {
                case "opacity":
                    opacity(null, null, 0, 0);
                    break;
                case "horizontal":
                    horizontal();
                    break;
                case "vertical":
                    vertical();
                    break;

            }
            imgpos++;
            //Modal_Loaded2(null,null);

        }

		private void setVertical(object sender, RoutedEventArgs e)
        {
            type = "vertical";
            Button_Click(null, null);
        }
        private void setHorizontal(object sender, RoutedEventArgs e)
        {
            type = "horizontal";
            Button_Click(null, null);
        }

        private void setOpacity(object sender, RoutedEventArgs e)
        {
            type = "opacity";
            Button_Click(null, null);
        }
        void changeImgDir(string newdir) {
            if (imagesdir.Length == 0) imagesdir = newdir;
            else
            {
                if (newdir.Contains(imagesdir) && newdir.Length > imagesdir.Length)
                {
                    Console.WriteLine("depper directories");
                    imagesdir = newdir;
                    
                }
                if(!imagesdir.Contains(newdir)) {
                    Console.WriteLine("dif directories");
                    imagesdir = newdir;
                    Console.WriteLine(imagesdir);
                    
                }
            }
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

            //
            // if (checkIfPics(item.Tag.ToString())) { item.MouseDown += new MouseButtonEventHandler(dispImg); }

            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {

                try
                {
                    item.Items.Clear();
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                      

                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        if (checkIfPics(s)) { subitem.Selected += dispImg; }
                        //if (!checkIfPics(s)) { subitem.Selected += (i,d) => imgPanel.Children.Clear(); }
                        //subitem.MouseLeftButtonDown += new MouseButtonEventHandler(dispImg);
                        item.Items.Add(subitem);


                    }

                }
                catch
                {
                    //Console.WriteLine(s);
                }
            }
        }

        private void dispImg(object sender, RoutedEventArgs e)
        {
            
            TreeViewItem item = (TreeViewItem)sender;
            Console.WriteLine(item.Tag);
            try {
                changeImgDir(item.Tag.ToString());
                imgPanel.Children.Clear();
                GetImg(imagesdir);
               }catch { }
        }

        private void GetImg(string path)
        {

            if (path == "") return;

            try
            {
               
                DirectoryInfo folder = new DirectoryInfo(path);
                if (folder.Exists)
                {

                    foreach (FileInfo fi in folder.GetFiles())
                    {
                        if (".jpg|.jpeg|.png".Contains(fi.Extension.ToLower()))
                        {

                            AddImage(fi.FullName, fi.Name, fi.Length);
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
        private void closeModal_Click(object sender, RoutedEventArgs e)
        {
            //App.

        }

        private void AddImage(string imgPath, string name, long len)
        {
            if ( new Regex(".jpg$|.jpg$|.jpeg$|.png$").IsMatch(imgPath))
            {

                Console.WriteLine(imgPath);
                Image newimg = new Image();
                StackPanel stack = new StackPanel();
                stack.Background = Brushes.White;
                Border border = new Border();
                DropShadowBitmapEffect shadow = new DropShadowBitmapEffect();
                shadow.Color = Color.FromRgb(0, 0, 0);
                shadow.ShadowDepth = 5;
                shadow.Direction = 270;
                shadow.Softness = 10;
                border.BitmapEffect = shadow;
                border.Height = 141;
                border.Width = 141;


                TextBlock textBlock = new TextBlock();
                textBlock.Text = name;

                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(imgPath, UriKind.Absolute);
                src.EndInit();

                newimg.Source = src;
                newimg.Tag = Math.Floor(src.Height) + "." + Math.Floor(src.Width) + "." + src;
                // newimg.Stretch = Stretch.Uniform;
                newimg.Height = 80;
                newimg.Width = 120;
                newimg.MouseDown += (e, s) =>
                {
                    fileInf.Visibility = Visibility.Hidden;
                    Image item = (Image)e;
                    fileName.Text = name;
                    

                    string[] sizeInfo = item.Tag.ToString().Split('.');
                    fileWi.Text = sizeInfo[0] + "px";
                    fileH.Text = sizeInfo[1] + "px";
                    fileSize.Text = len/1024 + "kb";
                    Imgdata.Visibility = Visibility.Visible;


                };
                border.Child = stack;
                stack.Children.Add(newimg);
               
                stack.Children.Add(textBlock);

                stack.MouseDown += (e, s) =>
                {
                    fileInf.Visibility = Visibility.Hidden;
                    StackPanel stac = (StackPanel)e;
                    Image item =(Image) stac.Children[0];
                    fileName.Text = name;


                    string[] sizeInfo = item.Tag.ToString().Split('.');
                    fileWi.Text = sizeInfo[0] + "px";
                    fileH.Text = sizeInfo[1] + "px";
                    fileSize.Text = len / 1024 + "kb";
                    Imgdata.Visibility = Visibility.Visible;
                };
                imgPanel.Children.Add(border);
            }
        }
        private void openDIR_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult res = dialog.ShowDialog();

            string path = dialog.SelectedPath;
            changeImgDir(path);
            
            GetImg(imagesdir);


        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }


        private bool checkIfPics(string path)
        {
            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                if (folder.Exists)
                {
                    foreach (FileInfo fi in folder.GetFiles())
                    {
                        if (".jpg|.jpeg|.png".Contains(fi.Extension.ToLower()))
                        {

                            return true;
                        }
                    }
                    return false;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }


        private void Button_Click2(object sender, RoutedEventArgs e) {
            var t = (System.Windows.Controls.Button)sender;
            Console.WriteLine(t.Tag.ToString());
            type = ComboxItems[Int32.Parse(t.Tag.ToString())];
            Button_Click(null, null);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
		{
            
            var modal  = new Window()
            {
                Title = "Modal Dialog",
                ShowInTaskbar = false,               // don't show the dialog on the taskbar
               // Topmost = true,                      // ensure we're Always On Top
                ResizeMode = ResizeMode.NoResize,    // remove excess caption bar buttons
                Owner = System.Windows.Application.Current.MainWindow
               
            
            };

            //1024 x 768 px
           
            modal.WindowStyle = WindowStyle.None;
            modal.Width =  1024;
            modal.Height = 768;


            modal.Left = 20;
            modal.Top = 20;

			modal.Loaded += Modal_Loaded2;

            _modal = modal;
			//var myDoubleAnimation = new DoubleAnimation();
			//myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
			//myDoubleAnimation.From = 0;
			//myDoubleAnimation.To = 1024;

			//myStoryboard = new Storyboard();
			//myStoryboard.Children.Add(myDoubleAnimation);


			_modal.ShowDialog();
			//Storyboard.SetTargetName(myDoubleAnimation, "imgShowing");
           // Image inside = (Image)_modal.Content;
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(inside.));



			// Determine if the form is modal.

		}

		private void Modal_Loaded(object sender, RoutedEventArgs e)
		{


            try
            {

                DirectoryInfo folder = new DirectoryInfo(imagesdir);
                if (folder.Exists)
                {

                    FileInfo fi = folder.GetFiles()[1];
                    Console.WriteLine("349"+fi.FullName);

                    if (".jpg|.jpeg|.png".Contains(fi.Extension.ToLower()))
                    {

                        
                        _modal.Content = getAnImage(fi.FullName); ;




                    }

                }
            }
            catch { }
        }
        private void Modal_Loaded2(object sender, RoutedEventArgs e)
        {


            try
            {

                DirectoryInfo folder = new DirectoryInfo(imagesdir);
                if (folder.Exists)
                {



                    int poss = 0;
                    foreach (FileInfo fi in folder.GetFiles())
                    {
                        if (".jpg|.jpeg|.png".Contains(fi.Extension.ToLower()))
                        {

                            toshow[poss] =(getAnImage(fi.FullName));
                           ++poss;
                        }
                    }
                    

                }
            }
            catch { }

            // int pos = 0;
            // opacity();//

            ChangeIMG.IsEnabled = true;




        }

       

        private Image getAnImage(string imgPath)
        {
           
            if (new Regex(".jpg$|.jpg$|.jpeg$|.png$").IsMatch(imgPath))
            {
                Console.WriteLine("378" + imgPath);
                Image newimg = new Image();

                BitmapImage src = new BitmapImage();
                
                src.BeginInit();
                src.UriSource = new Uri(imgPath, UriKind.Absolute);
                src.EndInit();
              
                newimg.Source = src;
                newimg.Height = src.Height < 765 ? src.Height : 765;
                newimg.Width = src.Width < 1020 ? src.Width : 1020;
				newimg.Name = "imgShowing";
				//newimg.Loaded += (oo, ee) =>
				//{
                   
    //            };
				ImageinSlide = newimg.Name;

				System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                menuItem.Header = "Play/Pause";
                menuItem.Click += new System.Windows.RoutedEventHandler((eee, sss) => { ChangeIMG.IsEnabled = !ChangeIMG.IsEnabled; });
                System.Windows.Controls.MenuItem menuItem2 = new System.Windows.Controls.MenuItem();
                menuItem2.Header = "close";
                menuItem2.Click += new System.Windows.RoutedEventHandler((eee, sss) => { _modal.Close(); });

                System.Windows.Controls.ContextMenu modalOptions = new System.Windows.Controls.ContextMenu();
                modalOptions.Items.Add(menuItem);
                modalOptions.Items.Add(menuItem2);

                newimg.ContextMenu = modalOptions;

                return newimg;

            }
            return null;

		}

        private void vertical() {
            Image inside = toshow[imgpos];
            // pos++;
            Storyboard Storyfisrt = new Storyboard();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 768, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(FrameworkElement.HeightProperty));
            ThicknessAnimation Thickness1 = new ThicknessAnimation(new Thickness(0, 768, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 1, 250), 0);
            Storyboard.SetTargetProperty(Thickness1, new PropertyPath(FrameworkElement.MarginProperty));

            Storyboard.SetTarget(Thickness1, inside);
            Storyboard.SetTarget(Animation1, inside);

            Storyboard Story2nd = new Storyboard();
            DoubleAnimation Animation2 = new DoubleAnimation(768, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTarget(Animation2, toshow[imgpos+1]);


            Storyfisrt.Children.Add(Animation1);
            Storyfisrt.Children.Add(Thickness1);
            Storyfisrt.Begin();
            Story2nd.Children.Add(Animation2);
            Story2nd.Begin();
        }

        private void horizontal()
        {
            //Image inside = toshow.Pop();
            Image inside = toshow[imgpos];
            // pos++;
            Storyboard Storyfisrt = new Storyboard();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 1024, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(WidthProperty));
            ThicknessAnimation Thickness1 = new ThicknessAnimation(new Thickness(1024, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 1, 250), 0);
            Storyboard.SetTargetProperty(Thickness1, new PropertyPath(FrameworkElement.MarginProperty));



            Storyboard.SetTarget(Thickness1, inside);
            Storyboard.SetTarget(Animation1, inside);

            Storyboard Story2nd = new Storyboard();
            DoubleAnimation Animation2 = new DoubleAnimation(768, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(FrameworkElement.WidthProperty));
           // Storyboard.SetTarget(Animation2, toshow.Peek());
            Storyboard.SetTarget(Animation2, toshow[imgpos+1]);


            Storyfisrt.Children.Add(Animation1);
            Storyfisrt.Children.Add(Thickness1);
            Storyfisrt.Begin();
            Story2nd.Children.Add(Animation2);
            Story2nd.Begin();
        }

        private void opacity(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
           // Image inside = toshow.Pop();
             Image inside = toshow[imgpos];
             //pos++;
            Storyboard Storyfisrt = new Storyboard();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(Animation1, inside);


            Storyboard Story2nd = new Storyboard();
            DoubleAnimation Animation2 = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(UIElement.OpacityProperty));
          //  Storyboard.SetTarget(Animation2, toshow.Peek());
            Storyboard.SetTarget(Animation2, toshow[imgpos +1]);

            Storyfisrt.Children.Add(Animation1);
            Storyfisrt.Begin();

            Story2nd.Children.Add(Animation2);
            Story2nd.Begin();
        }


    }
}


