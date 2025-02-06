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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int GridFactor = 20;
        const int tickSpeedMs = 500;

        private GameBoard _gameBoard;
        private DispatcherTimer GameTimer;
        public MainWindow()
        {
            InitializeComponent();
            _gameBoard = new GameBoard(40, 40);

        }
        private void DrawGameBoard() 
        {
            GameBoard.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            GameBoard.Children.Clear();
            var snakeHead = _gameBoard.Snake.SnakeHead;
            foreach ( var snakeElement in _gameBoard.Snake.SnakeElements)
            {
                var rectangle = CreateRectangle(snakeElement.X * GridFactor, snakeElement.Y * GridFactor, (snakeElement == snakeHead) ? Colors.LightSkyBlue : Colors.LawnGreen);
                GameBoard.Children.Add(rectangle);
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
            _gameBoard.NextStep();
            this.Dispatcher.Invoke(() => 
            { 
                DrawGameBoard();               
            });
            GameTimer.Start();
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
            }
        }
    }
}