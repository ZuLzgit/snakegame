using System.Numerics;
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
//        const int GridFactor = 20;
        const int TickSpeedMs = 500;
        const int BorderOffset = 40;
        //const int GameBoardHeight = 40;
        //const int GameBoardWidth = 40;
        private int GameBoardHeight = 40;
        private int GameBoardWidth = 40;
        private int SnakeLength = 20;
        private int AppleCount = 20;
        private int RockCount = 100;
        private string HumanSnakeHexHeadColor = "#42270f";
        private string HumanSnakeHexBodyColor1 = "#09aa21";
        private string HumanSnakeHexBodyColor2 = "#0c611e";
        private string ComuterSnakeHexHeadColor = "#420f11";
        private string ComuterSnakeHexBodyColor1 = "#42270f";
        private string ComuterSnakeHexBodyColor2 = "#293012";
        



        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.ResizeMode = ResizeMode.NoResize;
        }
        public DispatcherTimer GameTimer { get; set; }
        public GameBoard GameBoard { get; set; }
        private void DrawGameBoard()
        {
            var gridFactorX = GameBoardUI.ActualWidth / GameBoardWidth;
            var gridFactorY = GameBoardUI.ActualHeight / GameBoardHeight;
            GameBoardUI.Background = new SolidColorBrush(Colors.DarkOliveGreen);
            GameBoardUI.Children.Clear();
            foreach (var snake in GameBoard.Snakes)
            {
                for (int i = 0; i < snake.SnakeElements.Count; i++)
                {
                    var snakeElement = snake.SnakeElements[i];

                    var rectangle = CreateRectangle(
                        snakeElement.X * gridFactorX,
                        snakeElement.Y * gridFactorY,
                        gridFactorX,
                        gridFactorY,
                        (Color) ColorConverter.ConvertFromString(snake.GetHexElementColor(i))
                    );

                    GameBoardUI.Children.Add(rectangle);
                }
            }
            foreach (var rock in GameBoard.Rocks)
            {
                var rockRectangle = CreateRectangle(
                    rock.X * gridFactorX,
                    rock.Y * gridFactorY,
                    gridFactorX,
                    gridFactorY,
                    Colors.Gray);
                GameBoardUI.Children.Add(rockRectangle);
            }
            foreach (var apple in GameBoard.Apples)
            {
                var appleRectangle = CreateRectangle(
                    apple.X * gridFactorX,
                    apple.Y * gridFactorY,
                    gridFactorX,
                    gridFactorY,
                    Colors.Red);
                GameBoardUI.Children.Add(appleRectangle);
            }
        }
        private Rectangle CreateRectangle(double x, double y, double width, double height ,Color color) 
        {
            var rectangle = new Rectangle()
            {
                Margin = new Thickness(x, y, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Visible,
                Fill = new SolidColorBrush(color),
                Width = width,
                Height = height,
            };
            return rectangle;
        }
        


        private void Window_Initialized(object sender, EventArgs e)
        {
            //this.Width = (GameBoardWidth * GridFactor) + BorderOffset - 22;
            //this.Height = (GameBoardHeight * GridFactor) + BorderOffset;
            //this.GameBoardUI.Height = Height * GridFactor;
            //this.GameBoardUI.Width = Width * GridFactor;
            GameTimer = new DispatcherTimer();
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Interval = TimeSpan.FromMilliseconds(TickSpeedMs);
            GameTimer.Start();
            RestartGame(GameBoardHeight, GameBoardWidth);

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
        private void UpdateAppleCounterUI()
        {
            ScoreBoard.Children.Clear();
            ScoreBoard.Children.Add(new TextBlock { Text = "Scores:", FontSize = 16, FontWeight = FontWeights.Bold });

            foreach (var snake in GameBoard.Snakes)
            {
                var snakeScore = new TextBlock
                {
                    Text = $"{(snake.SnakeType == SnakeType.Human ? "Player" : "Computer")} Apples Eaten: {snake.ApplesEaten}",
                    FontSize = 14
                };
                ScoreBoard.Children.Add(snakeScore);
            }
        }
        private void RestartGame(int height, int width)
        {
            GameTimer.Stop();
            GameBoard = new GameBoard
                (
                height,
                width,
                RockCount,
                AppleCount,
                SnakeLength,
                HumanSnakeHexHeadColor,
                HumanSnakeHexBodyColor1,
                HumanSnakeHexBodyColor2,
                ComuterSnakeHexHeadColor,
                ComuterSnakeHexBodyColor1,
                ComuterSnakeHexBodyColor2 );

            GameBoard.OnAppleEaten += UpdateAppleCounterUI;
            GameTimer.Start();
            DrawGameBoard();
            UpdateAppleCounterUI();
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(WidthInput.Text, out int width) && int.TryParse(HeightInput.Text, out int height))
            {
                var minWidth = SnakeLength * 2;
                var minHeight = SnakeLength * 2;
                bool correctSize = false;
                if (width < minWidth)
                {
                    WidthInput.Text = minWidth.ToString();
                    correctSize = true;
                }
                if (height < minHeight)
                {
                    HeightInput.Text = minHeight.ToString();
                    correctSize = true;
                }
                if (correctSize)
                {
                    return;
                }
                GameBoardWidth = width;
                GameBoardHeight = height;
                StartMenu.Visibility = Visibility.Collapsed;
                GameTimer.Start();
                RestartGame(GameBoardHeight, GameBoardWidth);
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for the game board size.");
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                RestartGame(GameBoardHeight, GameBoardWidth);
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