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

namespace snakeUI
{

    public partial class MainWindow : Window
    {
        const int GridFactor = 20;
        const int tickSpeedMs = 200;
        const int BorderOffset = 10;

        private GameBoard _gameBoard;
        private DispatcherTimer GameTimer;
        public MainWindow()
        {
            InitializeComponent();
            _gameBoard = new GameBoard(40, 40);
            this.Width = (_gameBoard.Width * GridFactor) + BorderOffset * 2;
            this.Height = (_gameBoard.Height * GridFactor) + BorderOffset * 2;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void DrawGameBoard()
        {
            GameBoard.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            GameBoard.Children.Clear();
            var snakeHead = _gameBoard.Snake.SnakeHead;
            foreach (var snakeElement in _gameBoard.Snake.SnakeElements)
            {
                var rectangle = CreateRectangle(snakeElement.X * GridFactor, snakeElement.Y * GridFactor, (snakeElement == snakeHead) ? Colors.LightSkyBlue : Colors.LawnGreen);
                GameBoard.Children.Add(rectangle);
            }
            foreach (var rock in _gameBoard.Rocks)
            {
                var rockRectangle = CreateRectangle(
                    rock.X * GridFactor,
                    rock.Y * GridFactor,
                    Colors.Gray);
                GameBoard.Children.Add(rockRectangle);
            }
            foreach (var apple in _gameBoard.Apples)
            {
                var appleRectangle = CreateRectangle(
                    apple.X * GridFactor,
                    apple.Y * GridFactor,
                    Colors.Red);
                GameBoard.Children.Add(appleRectangle);
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
                Stroke = new SolidColorBrush(Colors.Black),
            };
            return rectangle;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            GameTimer = new DispatcherTimer();
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Interval = TimeSpan.FromMilliseconds(tickSpeedMs);
            GameTimer.Start();
        }
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            GameTimer.Stop();
            if (_gameBoard.IsGameOver)
            {
                GameTimer.Stop(); 
                return;
            }
            _gameBoard.NextStep();
            this.Dispatcher.Invoke(() => 
            { 
                DrawGameBoard();               
            });
            GameTimer.Start();
        }
        private void RestartGame()
        {
            GameTimer.Stop();

            _gameBoard = new GameBoard(40, 40);

            GameTimer.Start();

            DrawGameBoard();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _gameBoard.CurrentDirection = Direction.Up;
                    break;
                case Key.A:
                    _gameBoard.CurrentDirection = Direction.Left;
                    break;
                case Key.S:
                    _gameBoard.CurrentDirection = Direction.Down;
                    break;
                case Key.D:
                    _gameBoard.CurrentDirection = Direction.Right;
                    break;
                case Key.R:
                    RestartGame();
                    break;
            }
        }
    }
}