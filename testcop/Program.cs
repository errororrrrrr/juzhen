

//写一个贪吃蛇游戏
using System;
using System.Collections.Generic;
using System.Threading;

namespace testcop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random randomNumbersGenerator = new Random();
            int score = 0;
            int gameSpeed = 100;
            bool gameover = false;

            //定义蛇的初始位置和长度
            List<int> snakeXPosition = new List<int>() { 10, 9, 8 };
            List<int> snakeYPosition = new List<int>() { 10, 10, 10 };
            int foodXPosition = randomNumbersGenerator.Next(0, screenWidth);
            int foodYPosition = randomNumbersGenerator.Next(0, screenHeight);

            //隐藏光标
            Console.CursorVisible = false;

            //绘制蛇和食物
            DrawSnake(snakeXPosition, snakeYPosition);
            DrawFood(foodXPosition, foodYPosition);

            while (!gameover)
            {
                //移动蛇
                MoveSnake(snakeXPosition, snakeYPosition);

                //检查是否吃到食物
                bool snakeAteFood = DidSnakeEatFood(snakeXPosition[0], snakeYPosition[0], foodXPosition, foodYPosition);

                if (snakeAteFood)
                {
                    //增加分数
                    score++;

                    //生成新的食物位置
                    foodXPosition = randomNumbersGenerator.Next(0, screenWidth);
                    foodYPosition = randomNumbersGenerator.Next(0, screenHeight);

                    //绘制食物
                    DrawFood(foodXPosition, foodYPosition);

                    //增加蛇的长度
                    ExtendSnake(snakeXPosition, snakeYPosition);
                }

                //检查是否碰到边界或自身
                gameover = DidSnakeHitBoundary(snakeXPosition, snakeYPosition) || DidSnakeHitItself(snakeXPosition, snakeYPosition);

                //延迟游戏速度
                Thread.Sleep(gameSpeed);
            }

            //游戏结束，显示分数
            Console.SetCursorPosition(screenWidth / 2 - 4, screenHeight / 2);
            Console.WriteLine("Game Over!");
            Console.SetCursorPosition(screenWidth / 2 - 4, screenHeight / 2 + 1);
            Console.WriteLine("Score: " + score);
        }

        static void DrawSnake(List<int> snakeXPosition, List<int> snakeYPosition)
        {
            for (int i = 0; i < snakeXPosition.Count; i++)
            {
                Console.SetCursorPosition(snakeXPosition[i], snakeYPosition[i]);
                if (i == 0)
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write("o");
                }
            }
        }

        static void DrawFood(int foodXPosition, int foodYPosition)
        {
            Console.SetCursorPosition(foodXPosition, foodYPosition);
            Console.Write("@");
        }

        static void MoveSnake(List<int> snakeXPosition, List<int> snakeYPosition)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        snakeXPosition[0]--;
                        break;
                    case ConsoleKey.RightArrow:
                        snakeXPosition[0]++;
                        break;
                    case ConsoleKey.UpArrow:
                        snakeYPosition[0]--;
                        break;
                    case ConsoleKey.DownArrow:
                        snakeYPosition[0]++;
                        break;
                }
            }

            for (int i = snakeXPosition.Count - 1; i > 0; i--)
            {
                snakeXPosition[i] = snakeXPosition[i - 1];
                snakeYPosition[i] = snakeYPosition[i - 1];
            }
        }

        static bool DidSnakeEatFood(int snakeXPosition, int snakeYPosition, int foodXPosition, int foodYPosition)
        {
            return snakeXPosition == foodXPosition && snakeYPosition == foodYPosition;
        }

        static void ExtendSnake(List<int> snakeXPosition, List<int> snakeYPosition)
        {
            int lastXPosition = snakeXPosition[snakeXPosition.Count - 1];
            int lastYPosition = snakeYPosition[snakeYPosition.Count - 1];

            snakeXPosition.Add(lastXPosition);
            snakeYPosition.Add(lastYPosition);
        }

        static bool DidSnakeHitBoundary(List<int> snakeXPosition, List<int> snakeYPosition)
        {
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int snakeHeadX = snakeXPosition[0];
            int snakeHeadY = snakeYPosition[0];

            return snakeHeadX >= screenWidth || snakeHeadX < 0 || snakeHeadY >= screenHeight || snakeHeadY < 0;
        }

        static bool DidSnakeHitItself(List<int> snakeXPosition, List<int> snakeYPosition)
        {
            int snakeHeadX = snakeXPosition[0];
            int snakeHeadY = snakeYPosition[0];

            for (int i = 1; i < snakeXPosition.Count; i++)
            {
                if (snakeHeadX == snakeXPosition[i] && snakeHeadY == snakeYPosition[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
