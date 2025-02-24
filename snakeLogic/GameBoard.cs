using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using snakeLogic.Enum;
using snakeLogic.Elements;
using System.Windows.Documents;
using System.Windows.Media;


namespace snakeLogic
{
    public class GameBoard
    {

        public GameBoard
            (
            int height,
            int width,
            int rockCount = 10,
            int appleCount = 3,
            int snakeStartLength = 25,
            string humanSnakeHexHeadColor = "#03adfc",
            string humanSnakeHexBodyColor1 = "#C11A30",
            string humanSnakeHexBodyColor2 = "#ffeeee",
            string comuterSnakeHexHeadColor = "#420f11",
            string comuterSnakeHexBodyColor1 = "#42270f",
            string comuterSnakeHexBodyColor2 = "#293012"
            )
        {
            Height = height;
            Width = width;
            Snakes.Add(new Snake(width / 2, height / 2, snakeStartLength, SnakeType.Human, humanSnakeHexHeadColor, humanSnakeHexBodyColor1, humanSnakeHexBodyColor2));
            Snakes.Add(new Snake(10 / 2, height / 2, snakeStartLength, SnakeType.Computer, comuterSnakeHexHeadColor, comuterSnakeHexBodyColor1, comuterSnakeHexBodyColor2));
            Rocks = new List<Rock>();
            Apples = new List<Apple>();
            ObjectGenerator.CreateItems(rockCount, width, height, Snakes, ObjectType.Stone, Rocks);
            ObjectGenerator.CreateItems(appleCount, width, height, Snakes, ObjectType.Apple, Rocks, Apples);
        }
        public int Height { get; }
        public int Width { get; }
        public List<Snake> Snakes { get; set; } = new List<Snake>();
        public List<Rock> Rocks { get; set; }
        public List<Apple> Apples { get; set; }
        public bool IsGameOver { get; private set; } = false;
        public EndGameReason CheckCollisionEnd(Position position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
            {
                return EndGameReason.WallCollision;
            }
            if (Rocks.Any(rock => position.CheckCollision(rock)))
            {
                return EndGameReason.RockCollision;
            }
            if (Snakes.SelectMany(s => s.SnakeElements).Any(segment => segment.X == position.X && segment.Y == position.Y))
            {
                return EndGameReason.SelfCollision;
            }
            return EndGameReason.None;
        }

        public Apple CheckCollisionApple(Position position)
        {
            return Apples.FirstOrDefault(apple => position.CheckCollision(apple));
        }
        public void NextStep()
        {
            foreach (var snake in Snakes)
            {
                if ((snake.SnakeDirection == Direction.Suspend || IsGameOver) && snake.SnakeType == SnakeType.Human)
                {
                    return;
                }
                var newHead = (snake.SnakeType == SnakeType.Human)
                    ? CalcNewPositionHumanSnake(snake)
                    : CalcNewPositionComputerSnake(snake);

                var collisionReason = CheckCollisionEnd(newHead);

                if (collisionReason != EndGameReason.None)
                {
                    IsGameOver = true;
                    string message;


                    switch (collisionReason)
                    {
                        case EndGameReason.WallCollision:
                            MessageBox.Show("Game Over: Hit a wall!");
                            break;
                        case EndGameReason.RockCollision:
                            MessageBox.Show("Game Over: Hit a rock!");
                            break;
                        case EndGameReason.SelfCollision:
                            MessageBox.Show("Game Over: Collided with itself or another snake!");
                            break;
                        case EndGameReason.SnakeCollision:
                            MessageBox.Show("Game Over: Collided with another snake!");
                            break;
                        default:
                            MessageBox.Show("Game Over!");
                            break;
                    }
                }

                var eatenApple = CheckCollisionApple(newHead);
                if (eatenApple != null)
                {
                    snake.SnakeElements.Insert(0, newHead);
                    Apples.Remove(eatenApple);
                    ObjectGenerator.CreateItems(1, Width, Height, Snakes, ObjectType.Apple, Rocks, Apples);
                }
                else
                {
                    snake.SnakeElements.Insert(0, newHead);
                    snake.SnakeElements.RemoveAt(snake.SnakeElements.Count - 1);
                }
            }
        }
        public Position CalcNewPositionHumanSnake(Snake snake)
        {
            Position newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y);

            switch (snake.SnakeDirection)
            {
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
            }

            return newHead;
        }
        public Position CalcNewPositionComputerSnake(Snake snake)
        {
            Position newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y);
            Snake humanSnake = Snakes.FirstOrDefault(s => s.SnakeType == SnakeType.Human);

            Apple targetApple;
            var validApples = Apples
                .Where(apple => snake.SnakeHead.CalcDistance(apple) <= humanSnake.SnakeHead.CalcDistance(apple))
                .ToList();

            if (validApples.Any())
            {
                targetApple = validApples.OrderBy(apple => snake.SnakeHead.CalcDistance(apple)).First();
            }
            else
            {
                targetApple = Apples.OrderBy(apple => snake.SnakeHead.CalcDistance(apple)).First();
            }

            do
            {
                newHead = new Position(snake.SnakeHead.X - 1, snake.SnakeHead.Y);
                if (targetApple.X < snake.SnakeHead.X && CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Left;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X + 1, snake.SnakeHead.Y);
                if (targetApple.X > snake.SnakeHead.X && CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Right;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y - 1);
                if (targetApple.Y < snake.SnakeHead.Y && CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Up;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y + 1);
                if (targetApple.Y > snake.SnakeHead.Y && CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Down;
                    break;
                }

                newHead = new Position(snake.SnakeHead.X - 1, snake.SnakeHead.Y);
                if (CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Left;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X + 1, snake.SnakeHead.Y);
                if (CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Right;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y - 1);
                if (CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Up;
                    break;
                }
                newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y + 1);
                if (CheckCollisionEnd(newHead) == EndGameReason.None)
                {
                    snake.SnakeDirection = Direction.Down;
                    break;
                }
            } while (false);

            return newHead;
        }

    }
}
