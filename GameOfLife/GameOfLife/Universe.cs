using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    class Universe
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int LiveCells { get; set; }
        List<int[]> continuesToLive;
        Random ran = new Random();
        List<string> allPositions;
        public StatusCell[,] plane;

        // Переопределение конструктора в данном случае
        // необходимо для того, чтобы не создавать лишние методы
        public Universe()
        {
            allPositions = new List<string>();
        }
        
       

        // Распределеить случайно первое поколение
        public void RandomiseLiveCells()
        {
            // лист заполнения живых клеток
            List<int[]> fill = new List<int[]>();
            for (int i = 0; i < LiveCells; i++)
            {
                int r = ran.Next(Rows);
                int c = ran.Next(Columns);
                int[] tuple = new int[] { r, c };
                if (fill.Any(m => CompareArrays(tuple, m)))
                {
                    i--;
                    continue;
                }
                fill.Add(tuple);
            }
            UpdatePlane(fill);
        }

        // Метод устанавливает случайно количество случайнох столбцов, строк, живых клеток
        public void RandomiseCount()
        {
            // количество колонок и строк случайно от 10 до 30
            Columns = ran.Next(10, 30);
            Rows = ran.Next(10, 30);
            // Определить оптимальное число живых кленок первого поколения
            // Само число получается случайно, но в диапазоне произведения rows и colums
            int maxLiveCells = Rows * Columns / 3;
            int minLiveCells = Rows * Columns / 6;
            LiveCells = ran.Next(minLiveCells, maxLiveCells);
            plane = new StatusCell[Rows, Columns];
        }

        // Развитие клеток и сохранение живых
        public bool Develop()
        {
            List<int[]> survived = new List<int[]>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    StatusCell currentCell = plane[i, j];
                    // Живые ячейки. Подсчёт количества
                    int liveNeighbours = CountLiveNeighbours(i, j);
                    StatusCell result = SetСondition(currentCell, liveNeighbours);
                    // Save result
                    if (result == StatusCell.Live)
                        survived.Add(new int[] { i, j });
                }
            }
            // ОЧистить и затем обновить плоскость
            ClearPlane();
            UpdatePlane(survived);
            bool finished = false;
            // Если есть выжившие клетки, то отобразить их 
            // на следующем листе плоскости
            if (this.continuesToLive != null)
            {
                finished = this.continuesToLive.Zip(survived, CompareArrays)
                                        .All(x => x);
            }
            this.continuesToLive = survived;
            return finished;
        }

        // Получить символ, который символизирует положение клетки
        // "+" - клетка жива
        // "-" - клетка мертва
        public string GetStatusCells()
        {
            StringBuilder strBld = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    string el = plane[i, j] == StatusCell.Live ? "+" : "-";
                    strBld.Append(el);
                }
                strBld.Append("\n");
            }
            return strBld.ToString();
        }


        // Проверка на повтор конфигурации. Во время всей игры каждое пространство сохраняется в список
        // для того, чтобы сверить с новыми пространствами и расположением клеток там
        public bool CheckForUniqueness()
        {
            if (allPositions.Count != 0)
            {
                if (allPositions.FirstOrDefault(stringToCheck => stringToCheck.Contains(GetStatusCells()))!=null)
                {
                    return false;
                }
                else
                {
                    allPositions.Add(GetStatusCells());
                    return true;
                }
            }
            else
            {
                allPositions.Add(GetStatusCells());
                return true;
            }
        }

        internal int CountLiveNeighbours(int x, int y)
        {
            // Список для хранения живых соседей
            List<int> neighbours = new List<int>();
            // Проверка соседей на состояние жизни
            for (int i = -1; i < 2; i++)
            {
                int k = x + i;

                for (int j = -1; j < 2; j++)
                {
                    int len = y + j;

                    if (k >= 0 && k < Rows && len >= 0 && len < Columns && (k != x || len != y))
                    {
                        neighbours.Add((int)plane[k, len]);
                    }
                }
            }
            return neighbours.Sum();
        }

        // Задать условия жизни клеток
        internal StatusCell SetСondition(StatusCell currentCell, int liveNeighbours)
        {
            StatusCell status = StatusCell.Dead;
            if ((currentCell == StatusCell.Live && liveNeighbours == 2) || liveNeighbours == 3)
            {
                status = StatusCell.Live;
            }
            return status;
        }

        void ClearPlane()
        {
            Array.Clear(plane, 0, Rows * Columns);
        }

        void UpdatePlane(List<int[]> liveCells)
        {
            foreach (int[] indices in liveCells)
            {
                plane[indices[0], indices[1]] = StatusCell.Live;
            }
        }

        bool CompareArrays(int[] arrayOne, int[] arrayTwo)
        {
            return arrayOne.Zip(arrayTwo, (x, y) => x == y).All(x => x);
        }
    }

    public enum StatusCell { Dead = 0, Live = 1 }
}
