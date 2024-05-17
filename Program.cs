using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Horses
{
    class Program
    {
        static int finishLine = 50;
        static bool raceOver = false;
        static object lockObj = new object();
        static Dictionary<int, int> horsePositions = new Dictionary<int, int>();
        static int[] horseSpeeds = new int[10];
        static Random rand = new Random();

        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Скачки начались!");
            for (int i = 0; i < 10; i++)
            {
                horsePositions[i + 1] = 0; 
                horseSpeeds[i] = rand.Next(1, 5);
                Thread t = new Thread(RunHorse); 
                t.Start(i + 1);
            }

            while (!raceOver) { }

            Console.WriteLine("\nСкачки завершены!");
        }

        static void RunHorse(object horseNumber)
        {
            while (!raceOver)
            {
                Thread.Sleep(rand.Next(50, 200)); 
                lock (lockObj)
                {
                    if (!raceOver)
                    {
                        horsePositions[(int)horseNumber] += horseSpeeds[(int)horseNumber - 1]; 
                        if (horsePositions[(int)horseNumber] >= finishLine) 
                        {
                            horsePositions[(int)horseNumber] = finishLine; 
                            raceOver = true;
                            Console.SetCursorPosition(0, 12);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Лошадь {horseNumber} пришла первой к финишу!");
                        }
                        else
                        {
                            DrawTrack((int)horseNumber);
                        }
                    }
                }
            }
        }

        static void DrawTrack(int horseNumber)
        {
            lock (lockObj)
            {
                Console.SetCursorPosition(0, horseNumber - 1); 
                Console.Write(new string(' ', horsePositions[horseNumber] - 1));
                Console.Write(horseNumber); 
                Console.Write(new string('-', finishLine - horsePositions[horseNumber])); 
            }
        }
    }
}
