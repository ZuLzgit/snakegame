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
using snakeLogic;

namespace snakeUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameBoard _gameboard;
        public MainWindow()
        {
            InitializeComponent();
            _gameboard = new GameBoard(40, 40);
            DrawGameBoard();
        }
        private void DrawGameBoard() 
        {
            GameBoard.Background = new SolidColorBrush(Colors.DarkOrchid);
            GameBoard.Children.Clear();
            foreach ( var snakeElement in _gameboard.Snake.SnakeElements)
            {
                var rectangle = CreateRectangle(snakeElement.X * 20, snakeElement.Y * 20, Colors.DarkOliveGreen);
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
                Width = 20,
                Height = 20,
            };
            return rectangle;
        }
    }
}