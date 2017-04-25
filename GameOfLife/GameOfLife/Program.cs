using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GameOfLife
{
    /// <summary>
    /// При запуске приложения задаются случайные значения
    /// строк, столбцов и живых клеток. И затем уже идёт "Игра в жизнь"
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GenerateInConsole();
            Console.ReadKey();
        }

        // Используется API из класса Universe
        static void GenerateInConsole()
        {
            Universe world = new Universe();
            // Вызов метода для распределения 
            bool isLive=true;
            world.RandomiseCount();
            world.RandomiseLiveCells();
            while (isLive)
            {
                Thread.Sleep(100);
                Console.Clear();
                if (world.CheckForUniqueness())
                {
                    Console.Write(world.GetStatusCells());
                }
                else
                {
                    Console.Write(world.GetStatusCells());
                    Console.WriteLine("Конфигурация повторяет себя");
                    isLive = false;
                    break;
                }
                if (world.Develop())
                {
                    isLive = false;
                    break;
                }
            }
            Console.WriteLine("\nИгра прекращена. Дальнейшего развития нет.");
        }
    }
}
