using System;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace GameOfLife
{
    class Program
    {
        private const int Rows = 30;
        private const int Columns = 30;
        private const char LiveSymbol = '\u2B1C';
        private const char DeadSymbol = '\u2B1B';
        static bool _runningSim = true;

        static void Main(string[] args)
        {
            var grid = new Status[Rows, Columns];
            
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    grid[row, column] = (Status) RandomNumberGenerator.GetInt32(0, 2);
                }
            }

            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

            Console.Clear();

            while (_runningSim)
            {
                Print(grid);
                grid = NextGen(grid);
            }
        }

        protected static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            _runningSim = false;
            Console.WriteLine("\nSimulation ended");
        }
        
        public enum  Status
        {
        Dead,
        Alive
        }

        private static Status[,] NextGen(Status[,] currentGrid)
        {
            var nextGen = new Status[Rows, Columns];

            for (var row = 1; row < Rows - 1; row++)
            {
                for (var column = 1; column < Columns - 1; column++)
                {
                    var aliveNeighbors = 0;
                    for (var i = -1; i <= 1; i++)
                    {
                        for (var j = -1; j <= 1; j++)
                        {
                            aliveNeighbors += currentGrid[row + i, column + j] == Status.Alive ? 1 : 0;
                        }
                    }

                    var currentCell = currentGrid[row, column];

                    aliveNeighbors -= currentCell == Status.Alive ? 1 : 0;
                    
                    if (currentCell == Status.Alive && aliveNeighbors < 2)
                    {
                        nextGen[row, column] = Status.Dead;
                    }
                    
                    else if (currentCell == Status.Alive && aliveNeighbors > 3)
                    {
                        nextGen[row, column] = Status.Dead;
                    }

                    else if (currentCell == Status.Dead && aliveNeighbors == 3)
                    {
                        nextGen[row, column] = Status.Alive;
                    }

                    else
                    {
                        nextGen[row, column] = currentCell;
                    }
                }
            }
            return nextGen;
        }

        private static void Print(Status[,] future, int timeout = 500)
        {
            var stringBuilder = new StringBuilder();
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    var cell = future[row, column];
                    stringBuilder.Append(cell == Status.Alive ? LiveSymbol : DeadSymbol);
                }

                stringBuilder.Append("\n");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder.ToString());
            Thread.Sleep(timeout);
        }
    }
}