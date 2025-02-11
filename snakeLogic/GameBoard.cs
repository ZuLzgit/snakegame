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
            Height = height;
            Width = width;
            Rocks = Rock.CreateRocks(10, width, height, Snake);
            Apple = Apple.CreateApple(width, height, Snake, Rocks);

        }
        public int Height { get; }
        public int Width { get; }

        public Snake Snake { get; set; } = new Snake(5, 10, 3);
        public List<Rock> Rocks { get; set; }
        public Apple Apple { get; set; }
        public bool IsGameOver { get; private set; } = false;
        public bool CheckCollisionWithWallsAndRocks(Position newHead)
        {
            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= Width || newHead.Y >= Height)
            {
                return true;
            }
            if (Rocks.Any(rock => rock.X == newHead.X && rock.Y == newHead.Y))
            {
                return true;
            }

            return false;
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

            if (CheckCollisionWithWallsAndRocks(newHead))
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

            if (Apple.CheckCollisionWithApple(newHead))
            {
                Snake.SnakeElements.Insert(0, newHead);

                Apple = Apple.CreateApple(Width, Height, Snake, Rocks);
                return;
            }


            Snake.SnakeElements.Insert(0, newHead); 
            Snake.SnakeElements.RemoveAt(Snake.SnakeElements.Count - 1);
        }

        public Direction CurrentDirection { private get; set; } = Direction.Suspend;


    }
}
