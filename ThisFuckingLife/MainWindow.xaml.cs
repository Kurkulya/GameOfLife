using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Timers;

namespace ThisFuckingLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static Random rnd = new Random();
        World world;
        int wSize;
        private Timer timer = new Timer(50);

        private WriteableBitmap _wbWorld;

        public WriteableBitmap WorldBM
        {
            get { return _wbWorld; }
            set
            {
                _wbWorld = value;
                PropertyChanged(this, new PropertyChangedEventArgs("WorldBM"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _wbWorld = new WriteableBitmap((int)img.Width,
                (int)img.Height, 96, 96, PixelFormats.Bgra32, null);

            DataContext = this;
            wSize = 250;
            CreateWorld();

        }

        private void WorldGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle temp = ((Rectangle)e.OriginalSource);
                Cell C = (Cell)temp.DataContext;
                C.State = !C.State;
               // C.Brush = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256)));
            }
        }

        

        private void NewLife()
        {
            
            Dispatcher.BeginInvoke((Action)(() =>
            {
                int step = (int)(img.Width / wSize);
                Color back = Colors.Black;
                using (_wbWorld.GetBitmapContext())
                {
                    for (int row = 0; row < wSize; row++)
                        for (int column = 0; column < wSize; column++)
                        {
                            if (_wbWorld.GetPixel(row * step + 1, column * step + 1) != (world[row, column] == false ? back : Colors.White))
                            {
                                if (world[row, column] == false)
                                {
                                    _wbWorld.FillRectangle(row * step, column * step, row * step + step, column * step + step, back);
                                    //_wbWorld.DrawRectangle(row * step, column * step, row * step + step, column * step + step, Colors.White);
                                }
                                else
                                {
                                    _wbWorld.FillRectangle(row * step, column * step, row * step + step, column * step + step, Colors.White);
                                    //_wbWorld.DrawRectangle(row * step, column * step, row * step + step, column * step + step, Colors.Gray);
                                }
                                
                            }
                            //if (world[row, column].State == true) _wbWorld.DrawRectangle(row * step, column * step, row * step + step, column * step + step, Colors.Gray);
                            //_wbWorld.DrawLine(row * step + step , 0, row * step + step , 500, Colors.Gray);
                            //_wbWorld.DrawLine(0, column * step + step, _wbWorld.PixelWidth, column * step + step , Colors.Gray);
                        }
                }
            }));
            WorldBM = _wbWorld;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartButton.Content.ToString() == "Start")
            {
                timer.Start();
                StartButton.Content = "Stop";
                StartButton.Background = Brushes.Red;
               
            }
            else
            {
                timer.Stop();
                StartButton.Content = "Start";
                StartButton.Background = Brushes.Green;                
            }
        }

        private void CreateWorld()
        {
            world = new World(wSize, wSize);
            
            RandomizeWorld();
            timer.Elapsed += (sender2, e2) =>
            {
                timer.Stop();
                world.NextGeneration();
                NewLife();
                timer.Start();
            };
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartButton.Content = "Start";
            StartButton.Background = Brushes.Green;
            for (int i = 0; i < world.Height; i++)
            {
                for (int j = 0; j < world.Width; j++)
                {
                    world[i, j] = false;
                }
            }
        }

        private void RandomizeWorld()
        {
            for (int i = 0; i < world.Height; i++)
            {
                for (int j = 0; j < world.Width; j++)
                {
                    world[i, j] = (rnd.Next(0, 2) == 1 ? true : false);             
                }
            }
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartButton.Content = "Start";
            StartButton.Background = Brushes.Green;
            RandomizeWorld();
        }

        private void SetSizeButton_Click(object sender, RoutedEventArgs e)
        {
           timer.Stop();
            StartButton.Content = "Start";
            StartButton.Background = Brushes.Green;
            try
            {
                wSize = int.Parse(WorldSize.Text);
                if (wSize > 500 || wSize < 1)
                {
                    wSize = 50;
                    throw new Exception();
                }               
            }
            catch
            {
                MessageBox.Show("Wrong Value!");
                return;
            }
            CreateWorld();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //timer.Interval = TimerSlider.Value;
        }

        private void SetDarkBackground(object sender, RoutedEventArgs e)
        {
           
        }

        private void SetLightBackground(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewLife();
        }
    }
}
