using Mapsui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mapsui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadedMap();
            LoadScreen();
        }
        private void AddScreen(object sender, RoutedEventArgs e)
        {
            Image img = new Image();
            img.Height = 20;
            img.Width = 20;

            img.Source = new BitmapImage(new Uri(@"../com.png", UriKind.Relative));
            img.MouseMove += Image_MouseMove;
            img.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            img.MouseLeftButtonUp += Image_MouseLeftButtonUp;
            Canvas.SetLeft(img, 50);
            Canvas.SetTop(img, 50);

            can.Children.Add(img);
        }

        private void EnlargeMap(object sender, RoutedEventArgs e)
        {
            map.Width = map.Width * 1.2;
            map.Height = map.Height * 1.2;
            Canvas.SetLeft(map, 0);
            Canvas.SetTop(map, 0);
            foreach (UIElement ele in can.Children)
            {
                if (ele.GetType() == typeof(System.Windows.Controls.Image))
                {
                    Point point = ele.TranslatePoint(map.TranslatePoint(new Point(), can), map);
                    Canvas.SetLeft(ele, point.X * 1.2);
                    Canvas.SetTop(ele, point.Y * 1.2);
                }
            }
        }
        private void NarrowMap(object sender, RoutedEventArgs e)
        {
            map.Width = map.Width / 1.2;
            map.Height = map.Height / 1.2;
            Canvas.SetLeft(map, 0);
            Canvas.SetTop(map, 0);
            foreach (UIElement ele in can.Children)
            {
                if (ele.GetType() == typeof(System.Windows.Controls.Image))
                {
                    Point point = ele.TranslatePoint(map.TranslatePoint(new Point(), can), map);
                    Canvas.SetLeft(ele, point.X / 1.2);
                    Canvas.SetTop(ele, point.Y / 1.2);
                }
            }
        }

        public void LoadScreen()
        {
            var screens = GetScreenData();
            foreach (var sc in screens)
            {
                Image img = new Image();
                img.Width = 20;
                img.Height = 20;

                img.Name = sc.Name;
                img.Source = GetScreenSource();

                Canvas.SetLeft(img, sc.X);
                Canvas.SetTop(img, sc.Y);

                img.MouseMove += Image_MouseMove;
                img.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                img.MouseLeftButtonUp += Image_MouseLeftButtonUp;

                can.Children.Add(img);
            }
        }
        private void LoadedMap()
        {
            map.Source = GetCurrentMapSource();
        }

        Point pos = new Point();
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Image image = (Image)sender;
                double dx = e.GetPosition(null).X - image.Width / 2;
                double dy = e.GetPosition(null).Y - image.Height / 2;
                Canvas.SetLeft(image, dx);
                Canvas.SetTop(image, dy);
                pos = e.GetPosition(map);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            pos = e.GetPosition(null);
            image.CaptureMouse();
            image.Cursor = Cursors.Hand;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            image.ReleaseMouseCapture();
        }
        public List<MyScreen> GetScreenData()
        {
            return new List<MyScreen>
            {
                new MyScreen{Name="img1",Ip="192.168.1.3",X=100,Y=120,Floor=1},
                new MyScreen{Name="img2",Ip="192.168.1.3",X=130,Y=120,Floor=1},
                new MyScreen{Name="img3",Ip="192.168.1.3",X=160,Y=120,Floor=1},
                new MyScreen{Name="img4",Ip="192.168.1.4",X=120,Y=220,Floor=2},
                new MyScreen{Name="img5",Ip="192.168.1.4",X=150,Y=220,Floor=2}
            }.Where(t => t.Floor == (int)CurrentFloor).ToList();
        }

        public BitmapImage GetCurrentMapSource()
        {
            return new BitmapImage(new Uri(GetCurrentImageUrl(), UriKind.Relative));
        }

        public BitmapImage GetScreenSource()
        {
            return new BitmapImage(new Uri(@"../com.png", UriKind.Relative));
        }

        public Floor CurrentFloor { get; set; } = Floor.F1;
        public int CurrentBei { get; set; } = 1;
        public string GetCurrentImageUrl()
        {
            return "../" + CurrentFloor.ToString() + CurrentBei + ".jpg";

        }

        private void ChangeFloor1(object sender, RoutedEventArgs e)
        {
            CurrentFloor = Floor.F1;
            LoadedMap();
            ClearFloorScreen();
            LoadScreen();
        }

        private void ChangeFloor2(object sender, RoutedEventArgs e)
        {
            CurrentFloor = Floor.F2;
            LoadedMap();
            ClearFloorScreen();
            LoadScreen();
        }
         
        public void ClearFloorScreen()
        {
            int index = 0;
            foreach (UIElement ele in can.Children)
            {
                if (ele.GetType() == typeof(System.Windows.Controls.Image))
                {
                    var im = ele as FrameworkElement;
                    if (im.Name != "map")
                    {
                        index=can.Children.IndexOf(im);
                        break;
                    }
                }
            }
            if(index!=0)
            {
                can.Children.RemoveRange(index, can.Children.Count - index);
            }

        }

        public enum Floor
        {
            F1=1,
            F2=2
        }
    }
}
