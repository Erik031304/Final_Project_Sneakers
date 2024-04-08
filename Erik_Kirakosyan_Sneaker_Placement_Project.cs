using System;
using System.Collections.Generic;
using System.Linq;

namespace SneakerPlacement
{
    enum SneakerColor
    {
        White,
        Black,
        Red,
        Blue,
        Green,
        Yellow
    }

    class Sneaker
    {
        public string Brand { get; }
        public char Size { get; }
        public int Price { get; }
        public SneakerColor Color { get; }

        public Sneaker(string brand, char size, int price, SneakerColor color)
        {
            Brand = brand;
            Size = size;
            Price = price;
            Color = color;
        }
    }

    class Stand
    {
        public const int Rows = 7;
        public const int Cols = 7;
        private Sneaker[,] grid = new Sneaker[Rows, Cols];
        private int placedSneakers = 0;

        public void PlaceSneaker(Sneaker sneaker)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    if (grid[row, col] == null)
                    {
                        grid[row, col] = sneaker;
                        placedSneakers++;
                        return;
                    }
                }
            }
            Console.WriteLine("Stand is full! Cannot place the sneaker.");
        }

        public void DisplayStand()
        {
            var groupedSneakers = grid.Cast<Sneaker>()
                .Where(s => s != null)
                .GroupBy(s => s.Brand)
                .OrderBy(g => g.Key);

            Console.WriteLine("┌───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");

            foreach (var brandGroup in groupedSneakers)
            {
                var sortedSneakers = MergeSort(brandGroup.ToList(), CompareSneakers);
                int sneakersCount = sortedSneakers.Count;

                int rows = (sneakersCount + Cols - 1) / Cols;

                // Print brand name
                Console.WriteLine($"│ {brandGroup.Key.ToUpper().PadRight(15)}".PadRight(154) + "│");

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        int index = row * Cols + col;
                        if (index < sneakersCount)
                        {
                            var sneaker = sortedSneakers[index];
                            string size = sneaker.Size.ToString().PadLeft(3);
                            string price = sneaker.Price.ToString().PadLeft(3);
                            string color = sneaker.Color.ToString().PadRight(7);
                            Console.Write($" {size}${price} {color} ".PadRight(17));
                        }
                        else
                        {
                            Console.Write("".PadRight(17));
                        }
                    }
                    Console.WriteLine("");
                }
                if (brandGroup != groupedSneakers.Last())
                    Console.WriteLine("├───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┤");
            }

            Console.WriteLine("└───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘");
        }

        public bool IsFilled()
        {
            return placedSneakers == Rows * Cols;
        }

        private int CompareSneakers(Sneaker s1, Sneaker s2)
        {
            int sizeComparison = GetSizeOrder(s1.Size).CompareTo(GetSizeOrder(s2.Size));
            if (sizeComparison != 0)
                return sizeComparison;
            int priceComparison = s1.Price.CompareTo(s2.Price);
            if (priceComparison != 0)
                return priceComparison;
            return s1.Color.CompareTo(s2.Color);
        }

        private List<Sneaker> MergeSort(List<Sneaker> arr, Comparison<Sneaker> comparison)
        {
            if (arr.Count <= 1)
                return arr;

            int mid = arr.Count / 2;
            List<Sneaker> left = arr.GetRange(0, mid);
            List<Sneaker> right = arr.GetRange(mid, arr.Count - mid);

            left = MergeSort(left, comparison);
            right = MergeSort(right, comparison);

            return Merge(left, right, comparison);
        }

        private List<Sneaker> Merge(List<Sneaker> left, List<Sneaker> right, Comparison<Sneaker> comparison)
        {
            List<Sneaker> result = new List<Sneaker>();
            int leftPtr = 0;
            int rightPtr = 0;

            while (leftPtr < left.Count && rightPtr < right.Count)
            {
                if (comparison(left[leftPtr], right[rightPtr]) <= 0)
                {
                    result.Add(left[leftPtr]);
                    leftPtr++;
                }
                else
                {
                    result.Add(right[rightPtr]);
                    rightPtr++;
                }
            }

            while (leftPtr < left.Count)
            {
                result.Add(left[leftPtr]);
                leftPtr++;
            }

            while (rightPtr < right.Count)
            {
                result.Add(right[rightPtr]);
                rightPtr++;
            }

            return result;
        }

        private int GetSizeOrder(char size)
        {
            switch (size)
            {
                case 'S': return 0;
                case 'M': return 1;
                case 'L': return 2;
                default: throw new ArgumentException("Invalid size");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stand stand = new Stand();
            stand.DisplayStand();
            List<Sneaker> sneakers = new List<Sneaker> {
                new Sneaker("Nike", 'M', 50, SneakerColor.Black),
                new Sneaker("Adidas", 'L', 30, SneakerColor.Blue),
                new Sneaker("Reebok", 'S', 70, SneakerColor.Red),
                new Sneaker("Nike", 'M', 40, SneakerColor.Blue),
                new Sneaker("Adidas", 'L', 60, SneakerColor.Red),
                new Sneaker("New Balance", 'S', 80, SneakerColor.White),
                new Sneaker("Adidas", 'L', 70, SneakerColor.Black),
                new Sneaker("Reebok", 'S', 50, SneakerColor.Green),
                new Sneaker("Nike", 'M', 60, SneakerColor.White),
                new Sneaker("New Balance", 'L', 40, SneakerColor.Yellow),
                new Sneaker("Nike", 'S', 90, SneakerColor.Black),
                new Sneaker("Adidas", 'M', 80, SneakerColor.Green),
                new Sneaker("Reebok", 'L', 20, SneakerColor.Red),
                new Sneaker("New Balance", 'S', 100, SneakerColor.White),
                new Sneaker("Eghvard", 'S', 120, SneakerColor.Green),
                new Sneaker("Eghvard", 'M', 110, SneakerColor.Yellow),
                new Sneaker("Eghvard", 'M', 150, SneakerColor.Yellow),
                new Sneaker("Eghvard", 'L', 180, SneakerColor.Yellow),
                new Sneaker("Eghvard", 'L', 180, SneakerColor.Black),
                new Sneaker("Eghvard", 'L', 180, SneakerColor.Green),
                new Sneaker("Eghvard", 'L', 280, SneakerColor.Blue),

            };

            foreach (Sneaker sneaker in sneakers)
            {
                if (!stand.IsFilled())
                    stand.PlaceSneaker(sneaker);
                else
                    break;
            }

            stand.DisplayStand();
        }
    }
}
