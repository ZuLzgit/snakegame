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
using System.Windows.Threading;
using snakeLogic;
using snakeLogic.Enum;

namespace snakeUI
{

    public partial class MainWindow : Window
    {
        const int GridFactor = 20;
        const int TickSpeedMs = 500;
        const int BorderOffset = 40;
        const int GameBoardHeight = 40;
        const int GameBoardWidth = 40;


        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }
        public DispatcherTimer GameTimer { get; set; }
        public GameBoard GameBoard { get; set; }
        private void DrawGameBoard()
        {
            GameBoardUI.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            GameBoardUI.Children.Clear();
            foreach (var snake in GameBoard.Snakes)
            {
                for (int i = 0; i < snake.SnakeElements.Count; i++)
                {
                    var snakeElement = snake.SnakeElements[i];

                    var rectangle = CreateRectangle(
                        snakeElement.X * GridFactor,
                        snakeElement.Y * GridFactor,
                        (Color) ColorConverter.ConvertFromString(snake.GetHexElementColor(i))
                    );


                    GameBoardUI.Children.Add(rectangle);
                }
            }
            foreach (var rock in GameBoard.Rocks)
            {
                var rockRectangle = CreateRectangle(
                    rock.X * GridFactor,
                    rock.Y * GridFactor,
                    Colors.Gray);
                GameBoardUI.Children.Add(rockRectangle);
            }
            foreach (var apple in GameBoard.Apples)
            {
                var appleRectangle = CreateRectangle(
                    apple.X * GridFactor,
                    apple.Y * GridFactor,
                    Colors.Red);
                GameBoardUI.Children.Add(appleRectangle);
            }
        }
        private Rectangle CreateRectangle(int x, int y, Color color) 
        {
            var rectangle = new Rectangle()
            {
                Margin = new Thickness(x, y, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Visible,
                Fill = new SolidColorBrush(color),
                Width = GridFactor,
                Height = GridFactor,
                //Stroke = new SolidColorBrush(Colors.Black),
            };
            return rectangle;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Width = (GameBoardWidth * GridFactor) + BorderOffset;
            this.Height = (GameBoardHeight * GridFactor) + BorderOffset +21;
            this.GameBoardUI.Height = Height * GridFactor;
            this.GameBoardUI.Width = Width * GridFactor;
            GameTimer = new DispatcherTimer();
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Interval = TimeSpan.FromMilliseconds(TickSpeedMs);
            GameTimer.Start();
            RestartGame();

        }
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            GameTimer.Stop();
            if (GameBoard.IsGameOver)
            {
                GameTimer.Stop(); 
                return;
            }
            GameBoard.NextStep();
            this.Dispatcher.Invoke(() => 
            { 
                DrawGameBoard();               
            });
            GameTimer.Start();
        }
        private void RestartGame()
        {
            GameTimer.Stop();

            GameBoard = new GameBoard(GameBoardHeight, GameBoardWidth, 10, 3, 25, Colors.Black.ToString());

            GameTimer.Start();

            DrawGameBoard();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                RestartGame();
            }
            else
            {
                foreach (var snake in GameBoard.Snakes.Where(w => w.SnakeType == SnakeType.Human))
                {
                    switch (e.Key)
                    {
                        case Key.W:
                            snake.SnakeDirection = Direction.Up;
                            break;
                        case Key.A:
                            snake.SnakeDirection = Direction.Left;
                            break;
                        case Key.S:
                            snake.SnakeDirection = Direction.Down;
                            break;
                        case Key.D:
                            snake.SnakeDirection = Direction.Right;
                            break;
                    }
                }

            }
           
        }
    }
}