namespace AdventOfCode2024.Day10;

public class Day10 : Parser
{
    private static List<List<int>> _map = [];
    private static List<Tuple<int, int>> _trailHeads = [];
    private static int _height;
    private static int _width;

    private static readonly int[][] Moves = new int[][]
    {
        [0, 1],
        [1, 0],
        [0, -1],
        [-1, 0]
    };

    public static void Run()
    {
        Console.WriteLine("Day 9");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;

        foreach (var start in _trailHeads)
        {
            var visited = new HashSet<Tuple<int, int>> { start };
            var queue = new Stack<Tuple<int, int>>();
            queue.Push(start);

            while (queue.Count != 0)
            {
                var pos = queue.Pop();
                var posHeight = _map[pos.Item1][pos.Item2];
                if (posHeight == 9) result += 1;

                foreach (var move in Moves)
                {
                    var newPos = Tuple.Create(pos.Item1 + move[0], pos.Item2 + move[1]);
                    if (InBounds(newPos) && !visited.Contains(newPos) &&
                        _map[newPos.Item1][newPos.Item2] == posHeight + 1)
                    {
                        visited.Add(newPos);
                        queue.Push(newPos);
                    }
                }
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;

        foreach (var start in _trailHeads)
        {
            var queue = new Stack<Tuple<int, int>>();
            queue.Push(start);

            while (queue.Count != 0)
            {
                var pos = queue.Pop();
                var posHeight = _map[pos.Item1][pos.Item2];
                if (posHeight == 9) result += 1;

                foreach (var move in Moves)
                {
                    var newPos = Tuple.Create(pos.Item1 + move[0], pos.Item2 + move[1]);
                    if (InBounds(newPos) &&
                        _map[newPos.Item1][newPos.Item2] == posHeight + 1)
                    {
                        queue.Push(newPos);
                    }
                }
            }
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static bool InBounds(Tuple<int, int> position)
    {
        return position.Item1 >= 0 &&
               position.Item1 < _height &&
               position.Item2 >= 0 &&
               position.Item2 < _width;
    }

    private static void ParseInput()
    {
        var lines = ParseLines(10);
        _height = lines.Length;
        _width = lines[0].Length;

        for (var y = 0; y < _height; y++)
        {
            var strip = new List<int>();
            for (var x = 0; x < _width; x++)
            {
                var symbol = lines[y][x];
                strip.Add((int)char.GetNumericValue(symbol));
                if (symbol == '0') _trailHeads.Add(Tuple.Create(y, x));
            }

            _map.Add(strip);
        }
    }
}