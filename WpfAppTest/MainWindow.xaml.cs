using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfAppTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    enum GAME_STATE { RUNNING, WIN, LOOSE }

    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        //bool gameOver;
        GAME_STATE gameOver;
        int initRect = 5;
        int deltaTimer = 1000;

        public MainWindow()
        {
            InitializeComponent();
            AddRectangles(initRect);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(deltaTimer);
            timer.Tick += new EventHandler(onTick);
            timer.Start();
            gameOver = GAME_STATE.RUNNING;
        }

        private void onTick(object sender, EventArgs e)
        {
            AddRectangles(1);
            if (grid1.Children.Count == 20)
            {
                timer.Stop();
                MessageBox.Show("Вы прoиграли!!!", "Конец игры", 
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                gameOver = GAME_STATE.LOOSE;
            }
        }

        private void AddRectangles(int n)
        {
            Random random = new Random();

            for(int i = 0; i < n; i++)
            {
                Rectangle r = new Rectangle();
                if (random.Next() % 2 == 0)
                {
                    r.Width = 200;
                    r.Height = 50;
                }
                else
                {
                    r.Width = 50;
                    r.Height = 200;
                }
                r.HorizontalAlignment = HorizontalAlignment.Left;
                r.VerticalAlignment = VerticalAlignment.Top;

                int x = random.Next((int)(this.Width - r.Width - 25));
                int y = random.Next((int)(this.Height - r.Height - 50));
                r.Margin = new Thickness(x, y, 0, 0);

                r.Stroke = Brushes.Black;
                byte R = (byte)random.Next(256);
                byte G = (byte)random.Next(256);
                byte B = (byte)random.Next(256);

                r.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));

                r.MouseDown += new MouseButtonEventHandler(onClick);
                grid1.Children.Add(r);
            }
        }

        private void onClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Click!");            
            Rectangle r = (Rectangle)sender;
            Rect R = new Rect(r.Margin.Left, r.Margin.Top,
                r.Width, r.Height);

            int iRect = grid1.Children.IndexOf(r);
            if (iRect == -1)
                return;

            for(int i = iRect + 1; i < grid1.Children.Count; i++)
            {
                Rectangle rr = (Rectangle)grid1.Children[i];
                Rect RR = new Rect(rr.Margin.Left, rr.Margin.Top,
                    rr.Width, rr.Height);
                if (RR.IntersectsWith(R))
                    return;
            }

            grid1.Children.Remove(r);
            if(grid1.Children.Count == 0)
            {
                timer.Stop();
                MessageBox.Show("Победа!!!", "Конец игры",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                gameOver = GAME_STATE.WIN;
            }
        }

        private void grid1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameOver == GAME_STATE.RUNNING)
                return;

            grid1.Children.Clear();

            if (gameOver == GAME_STATE.WIN)
            {
                initRect += 2;
                deltaTimer -= 100;
            }

            AddRectangles(initRect);
            timer.Interval = TimeSpan.FromMilliseconds(deltaTimer);
            timer.Start();

            gameOver = GAME_STATE.RUNNING;
        }
    }
}
