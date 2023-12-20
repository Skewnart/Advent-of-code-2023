namespace AdventOfCode.Puzzle
{
    internal class Day06 : IDayPuzzle
    {
        public string ExecutePart1()
        {
            Dictionary<int, int> input = new Dictionary<int, int>() { { 42, 284 }, { 68, 1005 }, { 69, 1122 }, { 85, 1341 } };

            int prod = 1;
            foreach(var race in input)
            {
                int sum = 0;
                for(int i = 1; i < race.Key; i++)
                {
                    int distance = i * (race.Key - i);
                    if (distance > race.Value) sum++;
                }
                prod *= sum;
            }

            return prod.ToString();
        }
        public string ExecutePart2()
        {
            //26 479 734
            long racetime = 42686985;
            long pb = 284100511221341;

            int sum = 0;
            for (long i = 1; i < racetime; i++)
            {
                long distance = i * (racetime - i);
                if (distance > pb)
                {
                    sum++;
                    //Console.WriteLine($"{i} : {distance}");
                }
            }
            return sum.ToString();
        }
    }
}
