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
        Random Random = new Random();
        public GameBoard
            (
            int height,
            int width,
            int rockCount = 10,
            int appleCount = 3,
            int snakeStartLength = 25,
            string humanSnakeHexHeadColor = "#00a12b",
            string humanSnakeHexBodyColor1 = "#09aa21",
            string humanSnakeHexBodyColor2 = "#0c611e",
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
        public event Action OnAppleEaten;
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
            //first or default anstatt any mit snaketype
            //for schleife ^collision überprüfen und auf eigene schlange checken
            foreach (var snake in Snakes)
            {
                var collisionElement = Snakes.SelectMany(s => s.SnakeElements).FirstOrDefault(segment => segment.X == position.X && segment.Y == position.Y);
                if (collisionElement != null)
                {
                    if (snake.SnakeElements.Contains(collisionElement))
                    {
                        return EndGameReason.SelfCollision;
                    }
                    else
                    {
                        return EndGameReason.SnakeCollision;
                    }
                }
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

                    switch (collisionReason)
                    {
                        case EndGameReason.WallCollision:
                            MessageBox.Show("Game Over: Hit a wall!");
                            break;
                        case EndGameReason.RockCollision:
                            MessageBox.Show("Game Over: Hit a rock!");
                            break;
                        case EndGameReason.SelfCollision:
                            MessageBox.Show("Game Over: Collided with itself!");
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

                    snake.ApplesEaten++;
                    if (OnAppleEaten != null)
                    {
                        OnAppleEaten.Invoke();
                    }
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
        public Position CheckNewPosition(Snake snake)
        {
            var changeX = (snake.SnakeDirection == Direction.Left) ? -1 : (snake.SnakeDirection == Direction.Right) ? +1 : 0;
            var changeY = (snake.SnakeDirection == Direction.Up) ? -1 : (snake.SnakeDirection == Direction.Down) ? +1 : 0;
            var newPosition = new Position(snake.SnakeHead.X + changeX, snake.SnakeHead.Y + changeY);
            return (CheckCollisionEnd(newPosition) == EndGameReason.None) ? newPosition: null;
            
        }
        public Position CalcNewPositionComputerSnake(Snake snake)
        {
            Position newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y);
            var humanSnakeHeads = Snakes.Where(s => s.SnakeType == SnakeType.Human).Select(s => s.SnakeHead);

            var targetApple = Apples
                .OrderBy(apple => humanSnakeHeads.All(hsh => snake.SnakeHead.CalcDistance(apple) <= hsh.CalcDistance(apple)) ? 0 : 1)
                .ThenBy(apple => snake.SnakeHead.CalcDistance(apple))
                .First();

            var possibleDirections1 = new List<Direction>();
            if (targetApple.X < snake.SnakeHead.X) possibleDirections1.Add(Direction.Left);
            if (targetApple.X > snake.SnakeHead.X) possibleDirections1.Add(Direction.Right);
            if (targetApple.Y < snake.SnakeHead.Y) possibleDirections1.Add(Direction.Up);
            if (targetApple.Y > snake.SnakeHead.Y) possibleDirections1.Add(Direction.Down);
            possibleDirections1 = possibleDirections1.OrderBy(p => Random.Next(0, 99999)).ToList();

            var possibleDirections2 = new List<Direction>
            {
                Direction.Left,
                Direction.Right,
                Direction.Up,
                Direction.Down
            };
            possibleDirections2 = possibleDirections2.Except(possibleDirections1).OrderBy(p => Random.Next(0, 99999)).ToList();

            foreach (var possibleDirection in possibleDirections1.Union(possibleDirections2))
            {
                snake.SnakeDirection = possibleDirection;
                newHead = CheckNewPosition(snake);
                if (newHead != null) break;
            }

            //do
            //{
            //    if (targetApple.X < snake.SnakeHead.X)
            //    {
            //        snake.SnakeDirection = Direction.Left;
            //        newHead = CheckNewPosition(snake);
            //        if (newHead != null) break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X + 1, snake.SnakeHead.Y);
            //    if (targetApple.X > snake.SnakeHead.X && CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Right;
            //        break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y - 1);
            //    if (targetApple.Y < snake.SnakeHead.Y && CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Up;
            //        break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y + 1);
            //    if (targetApple.Y > snake.SnakeHead.Y && CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Down;
            //        break;
            //    }

            //    newHead = new Position(snake.SnakeHead.X - 1, snake.SnakeHead.Y);
            //    if (CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Left;
            //        break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X + 1, snake.SnakeHead.Y);
            //    if (CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Right;
            //        break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y - 1);
            //    if (CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Up;
            //        break;
            //    }
            //    newHead = new Position(snake.SnakeHead.X, snake.SnakeHead.Y + 1);
            //    if (CheckCollisionEnd(newHead) == EndGameReason.None)
            //    {
            //        snake.SnakeDirection = Direction.Down;
            //        break;
            //    }
            //} while (false);

            return newHead;
        }

    }
}
