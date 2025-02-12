using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;


namespace snakeLogic
{
    public class GameBoard
    {
        
        public GameBoard(int height, int width)
        {
            const int rockCount = 10;
            const int appleCount = 3;
            const int snakeLength = 25;
            Height = height;
            Width = width;
            Snake = new Snake(width/2, height/2, snakeLength);
            Rocks = new List<Rock>();
            Apples = new List<Apple>();
            ObjectGenerator.CreateItems(rockCount, width, height, Snake, ObjectType.Stone, Rocks);
            ObjectGenerator.CreateItems(appleCount, width, height, Snake, ObjectType.Apple, Rocks, Apples);
        }
        public int Height { get; }
        public int Width { get; }
        public Snake Snake { get; set; }
        public List<Rock> Rocks { get; set; }
        public List<Apple> Apples { get; set; }
        public bool IsGameOver { get; private set; } = false;
        public bool CheckCollisionEnd(Position position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
            {
                return true;
            }
            if (Rocks.Any(rock => position.CheckCollision(rock)))
            {
                return true;
            }

            return false;
        }
        public Apple CheckCollisionApple(Position position)
        {
            return Apples.FirstOrDefault(apple => position.CheckCollision(apple));
        }
        public void NextStep()
        {
            if (CurrentDirection == Direction.Suspend || IsGameOver)
            {
                return;
            }
            var snakeHead = Snake.SnakeHead;
            int newHeadX = snakeHead.X;
            int newHeadY = snakeHead.Y;
            switch (CurrentDirection)
            {
                case Direction.Left:
                    newHeadX--;
                    break;
                case Direction.Up:
                    newHeadY--;
                    break;
                case Direction.Right:
                    newHeadX++;
                    break;
                case Direction.Down:
                    newHeadY++;
                    break;
            }
            Position newHead = new Position(newHeadX, newHeadY);
            if (CheckCollisionEnd(newHead))
            {
                IsGameOver = true;
                MessageBox.Show("Game Over: Hit a wall or rock!");
                return;
            }
            if (Snake.SnakeElements.Any(segment => segment.X == newHead.X && segment.Y == newHead.Y))
            {
                IsGameOver = true;
                MessageBox.Show("Game Over: Collided with itself!");
                return;
            }
            var eatenApple = CheckCollisionApple(newHead);
            if (eatenApple != null)
            { 
                Snake.SnakeElements.Insert(0, newHead);
                Apples.Remove(eatenApple); // Remove the eaten apple

                // Generate a new apple to maintain count
                ObjectGenerator.CreateItems(1, Width, Height, Snake, ObjectType.Apple, Rocks, Apples);

                return;
            }

            Snake.SnakeElements.Insert(0, newHead);
            Snake.SnakeElements.RemoveAt(Snake.SnakeElements.Count - 1);
        }
        public Direction CurrentDirection { private get; set; } = Direction.Suspend;
    }
}
