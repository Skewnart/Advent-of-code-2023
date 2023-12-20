﻿namespace AdventOfCode.Puzzle
{
    public static partial class Extensions
    {
        public static int[] Add(this int[] me, int[] other) { return new int[] { me[0] + other[0], me[1] + other[1] }; }
        public static bool Equalsto(this int[] me, int[] other) { return me[0] == other[0] && me[1] == other[1]; }
    }
    public class Day16 : IDayPuzzle
    {
        public string ExecutePart1()
        {
            var input = G.GetInput();
            int height = input.Count, width = input[0].Length;
            var board = new bool[height, width];
            var alreadyknow = new Dictionary<int[], List<int[]>>();

            nextdirection(alreadyknow, input, board, new int[] { 0, 0 }, G.dR);

            int sum = 0;
            foreach (bool tile in board)
                if (tile) sum++;

            return sum.ToString();
        }
        private void nextdirection(Dictionary<int[], List<int[]>> already, List<string> input, bool[,] board, int[] pos, int[] direction)
        {
            while (pos[0] >= 0 && pos[0] < board.GetLength(0) && pos[1] >= 0 && pos[1] < board.GetLength(1))
            {
                var key = already.Keys.FirstOrDefault(k => k.Equalsto(pos));
                if (key != null && already[key].Contains(direction)) return;
                else
                {
                    if (key == null)
                    {
                        key = pos;
                        already.Add(key, new List<int[]>());
                    }
                    already[key].Add(direction);

                    board[pos[0], pos[1]] = true;

                    if (input[pos[0]][pos[1]] == '.')
                    {
                        pos = pos.Add(direction);
                    }
                    else if (input[pos[0]][pos[1]] == '\\')
                    {
                        if (direction == G.dR)
                        {
                            pos = pos.Add(G.dD);
                            direction = G.dD;
                        }
                        else if (direction == G.dD)
                        {
                            pos = pos.Add(G.dR);
                            direction = G.dR;
                        }
                        else if (direction == G.dL)
                        {
                            pos = pos.Add(G.dU);
                            direction = G.dU;
                        }
                        else if (direction == G.dU)
                        {
                            pos = pos.Add(G.dL);
                            direction = G.dL;
                        }
                    }
                    else if (input[pos[0]][pos[1]] == '/')
                    {
                        if (direction == G.dR)
                        {
                            pos = pos.Add(G.dU);
                            direction = G.dU;
                        }
                        else if (direction == G.dD)
                        {
                            pos = pos.Add(G.dL);
                            direction = G.dL;
                        }
                        else if (direction == G.dL)
                        {
                            pos = pos.Add(G.dD);
                            direction = G.dD;
                        }
                        else if (direction == G.dU)
                        {
                            pos = pos.Add(G.dR);
                            direction = G.dR;
                        }
                    }
                    else if (input[pos[0]][pos[1]] == '|')
                    {
                        if (direction == G.dU || direction == G.dD) pos = pos.Add(direction);
                        else if (direction == G.dR || direction == G.dL)
                        {
                            nextdirection(already, input, board, pos.Add(G.dU), G.dU);

                            pos = pos.Add(G.dD);
                            direction = G.dD;
                        }
                    }
                    else if (input[pos[0]][pos[1]] == '-')
                    {
                        if (direction == G.dR || direction == G.dL) pos = pos.Add(direction);
                        else if (direction == G.dU || direction == G.dD)
                        {
                            nextdirection(already, input, board, pos.Add(G.dR), G.dR);

                            pos = pos.Add(G.dL);
                            direction = G.dL;
                        }
                    }
                }
            }

        }
        public void reset(ref bool[,] board, ref Dictionary<int[], List<int[]>> already, int height, int width)
        {
            board = new bool[height, width];
            already = new Dictionary<int[], List<int[]>>();
        }
        public int calc(bool[,] board)
        {
            int sum = 0;
            foreach (bool tile in board)
                if (tile) sum++;
            return sum;
        }
        public string ExecutePart2()
        {
            var input = G.GetInput();
            int height = input.Count, width = input[0].Length;
            bool[,] board = default(bool[,]);
            Dictionary<int[], List<int[]>> alreadyknow = null;

            int max = 0;

            for (int i = 0; i < height; i++)
            {
                reset(ref board, ref alreadyknow, height, width);
                nextdirection(alreadyknow, input, board, new int[] { i, 0 }, G.dR);
                int res = calc(board);
                if (res > max) max = res;
            }

            max = 0;

            for (int i = 0; i < height; i++)
            {

                reset(ref board, ref alreadyknow, height, width);
                nextdirection(alreadyknow, input, board, new int[] { i, width - 1 }, G.dL);
                int res = calc(board);
                if (res > max) max = res;
            }

            return max.ToString();
        }
        public static class G
        {
            public static int[] dR = new int[] { 0, 1 }, dD = new int[] { 1, 0 }, dL = new int[] { 0, -1 }, dU = new int[] { -1, 0 };
            
            public static List<string> GetInput()
            {
                List<string> input = new List<string>();

                return INPUT.Split("\r\n").ToList();
            }

            public static readonly string INPUTTEST =
    @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";

            public static readonly string INPUT =
    @"\..\.|.........|..|................................................................./.-............./-....../.
............../..--.../...................-....|.......................\../...|..-....\...-|...-...........\..
.......\......................................|..-\.................-..-......................................
........\..............|......................\........./..............................|..............||......
.........|........|......-...........|....../....-.../.....|/.........-..../.../..........................|...
..\............................./\............................................................................
........|....................\............................./.....|\|..-....../............/..-..........\.....
...\.........................\.............|.........../..............-..../.............../-.................
.||....../..............\.|...............-......................|-..-......../.|.............................
...|.|.....-...../.............-............./............\.\/........|.............................\|.-..\.-.
............./....................|.......|.....\..\..............................-.../....|..................
./......./.|...............\................................................||................................
......\......|.................|........../......|\|.......-|..................|.....-.-.||../.\..............
................../...............|.....\........-...-.\...||..\...........\............|.........|...........
...-........|\|........././/............|...........\.../\...|.......-........\....................-..........
..../...........\.........................../......-.|...................................../..\........|......
\...............\/......./........-..............|..............--........../....|..-.\...........-........./.
..........\........\|.........|\.........../........................../.........|............\..........|.....
...........\....|/.|..........-..............-....-|...\..........-.../............./.........................
..\..\......../...\\....................../...../.........................|.....\..............|..\...-.......
...........................-./...................................-/.\.....\...................\.../...........
..-..................|..................-...\.......|../............./|-............/........|.......|..-.....
...........-......................-.................|............../.......-./..........|....../..............
.........|.............././...............|..../..............|.........|.....|..\......................|.....
......./-/...................\\..\......|........................\...\....../.....................|...-.......
.....|.......................|............|...|........|...........-....../..................-...-............
.../................../......|...........................-............|.............-........./............|..
.-.........|.............|-...../..............................\................|........................../..
.........../.........-.......|..-.........................-...............-...-.../...................../.....
../.......-../....................-...\...\........................-.............-...\...../.|.........../....
............-...............|........................................../...\.|....|........-.....\|...........
.....................|\............................-..-....../..-|..../..................\................\...
.\.................-......\...|............................./......./.........-............../................
.\.........././............../|......\/..............\........................|........................|...-..
.............../.........|.................................-.......\\.\-..........-......................./-..
-.|......................|.............|............................/....................|...........-........
../.....-.......|.....-.....\.../\........|.............|.........\./..//\................|.........-.....||..
.\..\.......|......./............\..\.........../........-.../.|..............-..../....\.........../.\.\.....
.............|..........|.........|.|...\-......................-..............|.../............./.........|..
....|................................................../...........-......-/......................-.....-.....
............................./.........-............................./..................\..........\..../.....
.....|.....................-.........../.......|..................-.......\...|\...|......\.....\...|.........
........|............../..../|.|......-................|...../..........|................|...................-
....../.....//..........\.-|......|.....|..................../..............|.................................
\.-...-....\..................\....................|............||............................../.............
............................-|...-...........-.../......-.........\..-...|.|\..........-...............\.|//./
....../......././.|...................|\........../.\.../-...\..........\.-..|\../...........\....\..|.....\..
...............\.../..../.................\.......-...........\..|..................\\................./.\....
............-..........-......................|.......|..|....................-......|......\...........|.....
...............\......|.-............./............\.../.............-|........./...............\|\.....|.....
.\...............|.................|...........\......\................\............\.|...\......./...........
\...|..............-..........|...................|....-.....\......../..........................\........|..|
.........|........./.........|....-........-...................../|./.--.......\........................-..../
..........................-.....-...............-/............................--...../........................
.....................-.\../\..........|.....................\.......\.../..........|.............\............
......................../..\.......\...............................................\./........................
....|......\..|\.-........../......................\|........\...............\.................|..............
............................./.....|..|............-...\.........-...\....|......\.............-..|...........
..................-......-.....................-...........-......\...-|-.-................\............|/....
...|....../............../..\.......................-....\...|.....\..|..\......./.................-.....-....
.........-........|........../..-................................./....-.................-....................
.............................-.......\...............\.....|......-....-.../.................\................
...../......|...................|..............-........./.............\...\....|.............\.......|.......
.......|............./........./......\..../........|...........\................................-............
/...|././.......|-.|..........|................................/\.\.......-......-........-........\..........
../................./|.-...\........................|........|........|...\|.\........................./.....-
................-.........../|..............|.|/...|\...........-.......................-..|../...............
./..||........|....../........................|.................................../..............\..|./-......
/..-...../......................................\-...........-..................\|................../.........
.............|.-......................|................-../.................-............../..............-...
-.....-|-...............\..........-...........\.......\........................\.........../..../...../......
.-..................../..........|.--.............................-..-....|.\................|....|...\......\
......./.-........................................\....|........|...../...........|...\|..-...................
\...............|........|.......\|..-..|...-..|-........|........./.\........................................
/.|./....|....-...../\..|...............................-.../........./.............-|.........-..../.-..\....
../......./...../..|........|../.....\....\.............-.-.................-...\.......|.\...........\.......
...-........./.............../..................\.|................/..//..................|.......-..\......-.
./.............../.-..\..........|.............-....................-....-..|...........-.....................
...........|-..|........................................./\............./-.../.................\........\.....
................................/...........................\.....................|.|.........................
.........|............\........................................-...../..........|........../.../......\...\...
../.........|......./..................|........\.../.\.-...-......../../\......../................../........
.-........./.........|....|.|.............................................|.\............/../.................
./.........................................\....../................../.--.....-..../.....|./.-......|.......\.
.\..................../.-........|.........\............\...........\...............|-../................\....
........................|......./.-....-....\.|...................-......-..........|........\.........../...|
.............-......./.../......................\.......\............/......................................|.
............-..........................-..................-.......\--......-.....-.......|..\..-....|........\
...............|.......\......|....................\.....................-.../.|......|................../....
---..\-.../.........//....\../....|........\....|-.........-.....|..........-|..............|...../..........|
......-.............................-.........................-.........../...../.|.../................-.....|
.........-.........\..............|....\..............-......\......../......\.......-\........-.............\
.................../......................|......|.....\..........................-..-......./....-.....-.....
...\.....................-..-....\.......././....\......./...........................\..............\\........
.................|................./.....................\...../.\.................\.........|...|....|.......
./............-........................\..|...-............................./............./..\................
-............-..............|.................-..............................-..............................\.
................................/.....\........|....-.....||............/.|.............-..|....|....-........
/.........|............-./...|././..............\./-............/......../\.\..././../..\..\..............-...
..../.....................\.........|......-...........|...\/..........//\.......//....-...|/.....\..\........
../............................/...././...-....-............./.|..\..............|...../.........-.....\....|.
.................................\..../...-.....-.....-....|..\.\...../....../.............../.\..............
..................|..|.......-......\.../....../....................................................|.......-.
-./.....|...\.....\../.......\..........................\.....|.............|...................|.\.../.../\..
..................-.....\.............|...|.........-..-............................../............/..|.......
.....................\..../.....|......................................\.\............|/....................\.
.....-..\.........-....\..............-..........................\.....\....................\.........\.....-.
\...\|\...../.\............................\........-.........................../.......-............\........
.............................................-......\.............|..../.......|......../.....................
/.........-............./......|................-........-.............|...-..../.....\.\....\................";
        }
    }
}
