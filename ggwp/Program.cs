using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static int width = 40;
        static int height = 20;
        static List<Point> snake = new List<Point>();
        static Point food;
        static int direction = 1; // 0 - вверх, 1 - вправо, 2 - вниз, 3 - влево
        static bool gameOver = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 2, height + 2);
            Console.SetBufferSize(width + 2, height + 2);

            // Инициализация змеи
            snake.Add(new Point(width / 2, height / 2));
            snake.Add(new Point(width / 2 - 1, height / 2));

            // Создание еды
            GenerateFood();

            while (!gameOver)
            {
                // Проверка ввода
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (direction != 2) direction = 0;
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction != 3) direction = 1;
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction != 0) direction = 2;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (direction != 1) direction = 3;
                            break;
                    }
                }

                // Движение змеи
                MoveSnake();

                // Проверка столкновений
                if (CheckCollision())
                {
                    gameOver = true;
                }
                else
                {
                    // Обновление экрана
                    Draw();
                    Thread.Sleep(150);
                }
            }

            Console.Clear();
            Console.WriteLine("Игра окончена!");
            Console.ReadKey();
        }

        static void GenerateFood()
        {
            Random random = new Random();
            food = new Point(random.Next(1, width), random.Next(1, height));
        }

        static void MoveSnake()
        {
            // Добавить новую голову
            Point newHead = new Point(snake[0].X, snake[0].Y);
            switch (direction)
            {
                case 0:
                    newHead.Y--;
                    break;
                case 1:
                    newHead.X++;
                    break;
                case 2:
                    newHead.Y++;
                    break;
                case 3:
                    newHead.X--;
                    break;
            }

            // Проверка, не съедена ли еда
            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                GenerateFood();
            }
            else
            {
                // Удаление хвоста, если еда не съедена
                snake.RemoveAt(snake.Count - 1);
            }

            // Добавление головы в начало змеи
            snake.Insert(0, newHead);
        }

        static bool CheckCollision()
        {
            // Проверка на столкновение со стеной
            if (snake[0].X < 0 || snake[0].X >= width || snake[0].Y < 0 || snake[0].Y >= height)
            {
                return true;
            }

            // Проверка на столкновение с самой собой
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[0].X == snake[i].X && snake[0].Y == snake[i].Y)
                {
                    return true;
                }
            }

            return false;
        }

        static void Draw()
        {
            Console.Clear();            //Рисование границ
            for (int i = 0; i <= width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("#");
                Console.SetCursorPosition(i, height);
                Console.Write("#");
            }
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(width, i);
                Console.Write("#");
            }

            // Рисование змеи
            foreach (Point point in snake)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write("O");
            }

            // Рисование еды
            Console.SetCursorPosition(food.X, food.Y);
            Console.Write("X");
        }
    }

    struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
