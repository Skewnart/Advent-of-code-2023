using System.Text;

namespace AdventOfCode.Puzzle.day14
{
    public static partial class Extensions
    {
        public static bool EqualsTo(this string[] board, string[] board2)
        {
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] != board2[i][j]) return false;
                }
            }

            return true;
        }
        //direction : 1 up, 2 west, 3 south, 4 east
        public static string[] GetBoardMoved(this string[] board, int direction)
        {
            if (direction == 1)
            {
                for (int i = 0; i < board[0].Length; i++)
                {
                    var vert = board.GetVertical(i);
                    var splitted = vert.Split("#");
                    string reordered = string.Join("#", splitted.Select(x => string.Join("", x.OrderByDescending(y => y))));

                    for (int j = 0; j < reordered.Length; j++)
                    {
                        board[j] = board[j].Remove(i, 1);
                        board[j] = board[j].Insert(i, reordered[j].ToString());
                    }
                }
            }
            else if (direction == 2)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    var splitted = board[i].Split("#");
                    string reordered = string.Join("#", splitted.Select(x => string.Join("", x.OrderByDescending(y => y))));

                    board[i] = reordered;
                }
            }
            else if (direction == 3)
            {
                for (int i = 0; i < board[0].Length; i++)
                {
                    var vert = board.GetVertical(i);
                    var splitted = vert.Split("#");
                    string reordered = string.Join("#", splitted.Select(x => string.Join("", x.OrderBy(y => y))));

                    for (int j = 0; j < reordered.Length; j++)
                    {
                        board[j] = board[j].Remove(i, 1);
                        board[j] = board[j].Insert(i, reordered[j].ToString());
                    }
                }
            }
            else if (direction == 4)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    var splitted = board[i].Split("#");
                    string reordered = string.Join("#", splitted.Select(x => string.Join("", x.OrderBy(y => y))));

                    board[i] = reordered;
                }
            }
            return board;
        }

        public static string GetVertical(this string[] board, int index)
        {
            StringBuilder res = new StringBuilder();
            foreach (string line in board) res.Append(line[index]);
            return res.ToString();
        }
    }
    public class Day14 : IDayPuzzle
    {
        public string ExecutePart1()
        {
            var input = G.GetInput();

            for (int i = 0; i < input[0].Length; i++)
            {
                string reordered = string.Join("#",
                    input.ToArray().GetVertical(i)
                        .Split("#")
                        .Select(x => string.Join("", x.OrderByDescending(y => y)))
                    );

                for (int j = 0; j < reordered.Length; j++)
                {
                    input[j] = input[j].Remove(i, 1);
                    input[j] = input[j].Insert(i, reordered[j].ToString());
                }
            }

            int sum = 0;
            for (int i = 0; i < input.Count; i++)
                sum += input[i].Count('O'.Equals) * (input.Count - i);

            return sum.ToString();
        }
        public string ExecutePart2()
        {
            var input = G.GetInput();

            var board = input.ToArray();
            var boards = new List<string[]>
            {
                board.ToList().ToArray()
            };
            string[] finalboard;

            for (int inf = 0; inf < 1000000000; inf++)
            {
                board = board.GetBoardMoved(1);
                board = board.GetBoardMoved(2);
                board = board.GetBoardMoved(3);
                board = board.GetBoardMoved(4);

                if (boards.Any(b => b.EqualsTo(board)))
                {
                    int index = boards.IndexOf(boards.First(b => b.EqualsTo(board)));
                    int loopcount = boards.Count - index;
                    int looping = (1000000000 - boards.Count) % loopcount;

                    finalboard = boards[index + looping];
                    int sum = 0;
                    for (int i = 0; i < finalboard.Length; i++)
                        sum += finalboard[i].Count('O'.Equals) * (finalboard.Length - i);

                    return sum.ToString();
                }
                else
                {
                    boards.Add(board.ToList().ToArray());
                }
            }

            return "no answer 🤞";
        }
        public static class G
        {
            
            public static List<string> GetInput()
            {
                List<string> input = new List<string>();
                input.AddRange(INPUT.Split("\r\n"));

                return input;
            }

            public static readonly string INPUTTEST =
    @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";

            public static readonly string INPUT =
    @".O.....##O.O#..O#....##O#.#.#..##O#..O.....#.OO..O.O.O.O...O....#O...OO....##.O..OO.O......O..O.....
....O..#O....#...#OO..#..#..O.....#O......O.O.#..OO..O....#..#.OO....##O...O..##...#.O...O.#...#.O#.
.O.O#.O...O...O....O...O#....#.#O#...O...#.#..........O#...O#O..O..#.OOO..O.O.....O#.........#O...O.
.O....O.OO.#.....#.#O......##O..O.O.##.#..O##O.O.#OO....O.O..O#.OOO..O..#..##.#...#.O.#O.#.#.....O..
..O......O..O.O.#.OO.OO...#.O....#...#O..OO.#.O......O....O.O.....O.#.OO..O.OOO.......O.O...##O...O#
.O.O.........#.O#.#OO##..#OO..OOO#....#....O.O..#.O......#.OO..#..O.O..O.O..#....#.O#..OO###.....#..
.O..#.#.OO.#...........#..O#....O..O.O#.......#.OO.#OO....O.#O.#.....#.OO.#...#..O..O##.O.O##..#.#.O
..O..#...#..OO.#O.#.O.O#O...O.O..O..O#..#..#..O..........O.O#.....O...........O.###..#.#..O#.#.O...#
#..O...#.O..O.O......#O.O##...##...OOO....#.O.O#OO.##...OO.OOO..#.O..##.....O..O.O.#O.O..#.O#O...O.#
OO....#.#...O...O.#...##O..###O.......OO...#.#...O.#..#.O......O....#.O#..#O..O...OO...#..O#..O....O
O#..#O...##O.....#.O.#.O...##......#...#OO#...O.......#.#.OO#.#..#..O#OO...O.......O.#...#.##....O..
#............O#....##.O.O..#O.#.OO.O...OOOO#.......O#O..O.......O#OO#...O.O....#.....#O.#.#..#.OO.O#
#O.O.....O....#O..OO.#O.#....#.##O.O...........O.##.#O##O...O#..O.O.O#......O...O.#....#OO#.#..O..##
.O..#O.O.#....O.........#.....OOO.......O.#O.....#OOOO#.O##O.#.O...O..#OO...O.....O..#.#......OO.O.#
O..O...O......O.O...#..#...#.OO.#.OO.O..O...#...#...O.###....#O#.O..O..#O..#.O.##.......O#..#O......
#..O...#...#OOO.#.###.#....#..O.O.#.#......OO.#.#...#....#.OO.O..#.O...#.#.....O.#O.O.#O..O....O.O..
OOO.#...##.OO.O....#.O.###.#.....O.OO#..#....OO...O#.#.O..#O.#..#.O..O....#O.OO#...O..#.....#O...O.O
.#.#...........#.............O.#..##.#O.#.O......#..O..OO.##O.O##.....#..O.O..O.OO..OO.#O.O.##O.O...
..O......O..##O.O...##......##OO..#O.#..#..OO...#...O.O..O..O.O.O##.#O.O#.#.O#....##.O.....O..#.O...
.#...#O.#.#....O..O..O#.O.....O..O.#.O....O.O...#.O#......O..O.O...#.#...O..##O...#.#...O.##..#..O#.
.O#..O.#....#.OO.#.....#O....O#O.#..#.O.....#......#O....O.........#.#O.OOOO#O.OOOO.#.....O.#.O..#OO
#..#.....O.....#.....##.O....##.O.O#OOO..O....O...#....O.O.....#.O#.O....O.O....#..O....O###.#..O#O.
...O...#..#O.O.O.......O.....O#O....#.#OO#......#..OO.....#.#...#...OO.O#O#O.#.#O....#..O....O...OO.
..#.##.#.#..............#O...#O#..##O.#.....O.#.#......O.....O#..O.O.O.#OOO####O.....O..##.O.OO.#..#
O...O#.O....#.O...........#......##.O.O#.....#.O..OO#...O#.##O..##.....#.O.O.....O#.#O.O..O...OO#...
OO.........##....##.O.O.OO.....O...........O.....O..#..#.OO........#O...OO.....#.O.........#........
#..#.##O#.O#O..#..O...#.#.O.OO...##O..#..O.O........#.#O#..#..O..#.O.....O...O..O.O....#...O#....#..
O.#.......O..O#O...OOOO#OO..O..#....#O..#...OO..#....O......#..#....O.O#.#....O.OO..O....O....OO..#.
O.OO..........#..O.#O#.#.#O...##..#..O##..##O#.OO......O..#.O.O.O.OO....#.....OOO#.O.#.O.#O##.#OO.#O
#..O....##...#.O.OO.O.O......#.OO...#......O........OO.#O..#.......OO..O..O...#.O......O.#O#O#...#OO
#...#..#OOOO#.#O#O.O#.O.#.###...O..#.....O..............O.....O.#.O....O.O....#...........O...O#.O..
.#O.O.OOO.....O...O...#.........#..O..O.#..#.O#....O#O..OO.........OO#.........#..OO.O.......#...O..
..#...O...#O...O..#.....O..O.#.......O...O.O.O.....O......O...#.....##.O.O#..O.#.O..#.O..O..OO......
.O.O..O..#O..O#OO#OO.#....O...#.O#.O.#...###........O.....O..O.OOO.O...#....O.O....O..OO.###...O...O
.#.....O..OOOO..#...#.#.O......O..O..#.##.#...O#..O.#.#.O#.....##...##O.......##..O.O.O.......#..OO#
...O...O...O..O...#..O#....O....#......O..OO#...O...#.#O.##.#..O..#.##O..#.#...O.....#.#O.O..O.O##..
.#OO#.....#O...O.#.OO......OO...O..O..O...#............#.OO##..OO..O.O......O..O...O...O...O...O....
...O....O..#.O.O###..##.O..O.O.#...#.OO.O..O.O.O.O.#.O..#.O##.......O..#......O.O..O.OOO..##O....#..
O...O.#..........O..OO.O#...#OO.OO..#O#.......O..#....O.......O.#OO.O.O.O.O##.OO..##...O#..OO#..#.#.
.O....#.#O............OOO.......#.O..#.#...#O......#....#.##..O.#.##OO..OO.#.#......O.O.....#.OO...O
..O...O#...O..O..#..#O..O#..O#..O.OO..O#.#...O####..O...O#..##......O#....O....O..O...O........O..O.
O.O.#..#.#..##..#.#....#..#..#.###.O#.O....OO...#....O#.....#.........#.###..#.#OO..#O...O...O.#....
..OO.#.O.....O.O..#.......O...O..##.#.#O..#....#OO.O....O......#.OO#....O..#O.O...#.OO#O.#....O..O.#
#...O#.O...O..O..O.O#...O.O.......OO....OO..O#.O..............O..O#.#...O#....OO..OO..O....#.#O.O..#
O.O.##..O#...#O......O...O...OOO.....#....O#...O#.#.#........O..##..##.O..##O.O..O..#............O.#
.....#....O..O.O...####...#.O.#..#.#..#....#...#.OO.#..##...OOO.#..#......O.OOO.....#O#.......OO....
...#.O#.O...#.OOO#.......OO.#..O..O##.#OO.O.#.O.O.O#.....#.O.........O...O....#.O#....#.#O..O..#..#.
...#..O......O..#.O...#.....#....O.....OOO..O..O##O#...O...O.#...O....#OOO##..........O....#O.......
...O.O.#.#...O.O#O#.#O#O....#....O..O....#.O.O#.....#....O..O..............O...##O.O....O..#....#..O
O.....O..#O....O..O##O..#..#.O.OO..#...##........#.#.#.O..###...#O.##..##..#O..O.OOO...O.O.....O....
..##.......#...O.O.O#O.#...O..OO#OOO.#..O....#...O..#....#.....OO..OO.#....O.....#.##...O.#..O...#O#
O##....O.O..##OO.#.#.O#..##............OO.O.O.#.....#..OO...#...O..##O..........#.......O..OOO...O.#
#OO.......O.O..O...#...O#.#...O..O.....O..OO.O...#OO...O....O...O.O...O....O......#.#.O.OO.....#....
...#..#OO..#..O.OO.#..#.....O..#O..#....#..#...O...O...OO..O#..O...O.........#...OO.O.#OO#.#.O..OO.#
..O#.O.O.#.#.....#...O..O#O..#..#..#.#...O..O.#.O.OO..OOO.#..OO.#.#.....OO...OO#O#.#..........O.##O.
...O#..O..O..O..#OO.###..O.........O#...OO.....O..O#....#O.O.O..O#.#...O#OO.O..#..O.O.O...O.OO.....O
.....O.#.O..O...##..#.O#O.#.......#O.OO#...O...##.OO#...OOO....#..#..#O...O.O#O.O..O.#OO...#..#.O#.O
....#...#O.O...##.#...#...##.O....#.#.O.O..##.O...#O...O#OO#..#.O.....#O#O.O.O#O...#.#.O.O..#...O..O
.O...O....O.....#.#O.#.##.##..O#..OO#O..#..O.#..O..O#....O#..#..OOOO.#.O..OO.O.##......#...#......#O
O....O.O...O#.OOO.#..O.......#..#....O#O........###..O..#.O.O.O.......#....O.O.O.##.O..#..O....O..#.
..O..OO...................OO...#........#.#...O..#.O.......#...O.......O..O#...O.#.O#....O#.O...#...
.O..#......O#O#..#......OOO........OOO........O#....O#.OO.#.......O#.OO.......##.....OO.O......O.O..
.#O..#O#O#..###.............##....#..O...#.O#O..O###.O..##..#..#OOO#.#...O.#.O.....O.#...........#..
.....O..O..OO.O.......OO.O.....#..##.##OOO#.#O..O.#....OO#OO...#...##...#.O...O....OO.O.##..O#..###.
OOO#.....#..#.O......O...O...#O..#....O.OO.#...O..........O.....O..O.....OO...#..O..O.#OO.O..OO.#...
O.OO.#...#.#...#...OOO.......O#....O....#....O.#...O.#.##.#OO.O..##......O.##OO#.......#.O...O.OOO..
.....#.........#.............O##.##..O..OOOO....#....O..#..O..#...O...OO.#O....#O.....O#O#......#..#
.#.O#O#.O#OOO.......O.O......O.#.#....#O..O..#..#.......#O.O#.....###..O.#.O......#.#.O#......#O....
..#O#.#.O.#O.OO.#O.O####.#..##.O.....#...#O#..#O#O#....O....OO#....O#.O...O.........#O##.O.O....#...
.OO..#O..O#.....OO....#O.O.....OO#...O.....OO.......#..O#...OO.#.O..O.O.#O.OO#.#O.....O...O..O.O.O..
..##OOO.O.....##..O.#.#.##..#...##.O..O....OO...##O..#.OO....#.O#.......O..O..O.#.#.#....O.O.O#O..O.
O#.#..#.##.O..O.O.....OO#....OOO.O.O.OO.....O...###.......#O...#....#.OOO.#O..OO...O..#..O.O.O.....#
OO#.....OO.O.#OO...#O...O.....O..O.#....O............#O.O.O....#.......O.O..OOO...#...#...#.........
......#..O.#.....O....OO..##..##........O.#O..........#OOOO......O......O.O##..O#O#O.O..#.#O.....#..
..O..OOO.........#.O..O..#..O..O###..O..##...O..#.O.O..#.............O.....##O.OO..#.#..O.##O.##O.O.
..#...#..O........O.O.#..##.O.#O......O.O.#OO..O..#..#O.O.....#.##..##..#......OO..#.O...#..#.O.O#..
..#...#..O......#......#..O.##.#....OO..#...#.....O.O.O..#.##...##..OO......#.OO....O..O..O.O.#.O...
......O...#O......O.O..OO.O...#O..##..#...#..#....##........##O.O......O.O...#O....OO...............
.....O..#.##....OO.O...O..O..O.O...#....O.#.O....#.#..OO......OO#...#.###..#O...O....O..##O.#..OO.O.
...OO...#...##.##.#..#...#.O..#OOO.....#.#..O...O.##O.#.#..#O.#O.#......O#......OOO.O#.....#......O.
OOO.O.O...###...#...#.#...OOO.O...O..O...#....O.OO...O..##..O........#.O.#....#......##.#O..#.#O#O..
....OO#.#.....O..O..#......OO.........O....O.....O..O#.#..##O.....#.#.#....#..O#..#OO.O...#.OO##O.OO
O.O..#..O.......O.O....O..OO.O...OO.O.......O###OOO##.O...O...#O#OO......O..O..O...#....##...O..O...
O#.O#.....O...O.O...#.##.#.......###....O.O.##......O....O..#O..#O#.O..##OO..O.###..O.OO#...#....##O
O..O..O............OO.....O.O..O...#............#O..O..O#...O..O.O##........#.#.....#....O#O.......#
............O....O#.OO...##..OO#O...#.....#OO..O#.......##O.O#...OO....#..O.....#..O...O.OO#O.O...O.
.#...#...OO...###.OO.OO....#..O...#...O.O...O....#........O.#..O#.OO..#....#O..O##.......O...#..##..
#.O.O#.O..#....#...#O#...OO..#...O.O#.OO.##...............O.#......O.....#..O..#..O..#O..O..##.##.O.
O..##.....OO.O##.O..#.O........OOOO...#..##.........#.#..O.O.OO#..O##O#....O.O#.....O.....O#..#...##
O......#.O...#...O#.#...O#........O..#....#..#.O.......O.......#.....#..O......#..O#..O#...#.#O.O...
..........OO###.#.O.O..#...##..##.#.........O#O..O.#O.......O..O#...#..O.O..#.O....#..O..OO.O......O
.OOOO..O....O.....#.....#.#.O.#O.#.O....##...O.O#O.OO.....O.O#.#...#..#.#O.O.##...O...OO..O....OO#O.
O...###O...###O...#...#O...#....#..O#..#O#....O.##..#.....#.#OO#.O...O.##O#...O.....#.......#.O.....
O...#.OO.O##.O.....#.....O..O...#.O...O.#.....#.O....O.#...#OOOO..#.#O...O...##..##..........O....O#
..OOO..O..#...OO....##....O....O...O....#..O..##.O...O.O.....#.#.O.#.#..O#O.#.....#.O.#.#.....O.O#..
#....O.OO##OO....#...#.....#....###.#..O..O.O...#.....#..O.........#O#...#............OO....#.OO..O.
.O.O#.#O#.O..O........#..O#...O.#..##O....O#O..O#O...#.O...#.....#.#......#......O...#...#..O..#....
.#..O..O..#O..OO....OO#.#...O.....O....#....O..O#...OOOO#.#...#..##.O.....O.....#..O.O..#...OO#OO..#
....#O.##.O.....#..O..OOO.#.O..#.O.....#O.O.##..#OO.......O..##...O#.#.OO..O..#...#......#...O.O....
#......#.#.....#O...O..#..#..###..O...##.OOO.O...#.O..#..##.O...#..O..O....O.......#OO.....#....OO..";
        }
    }
}
