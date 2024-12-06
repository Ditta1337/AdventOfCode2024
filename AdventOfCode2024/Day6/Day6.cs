namespace AdventOfCode2024.Day6;

public class Day6 : Parser
{
    private static List<string> _map = new();
    private static int _height;
    private static int _width;
    private static Tuple<int, int> _startPos;

    private static readonly int[][] Moves = new int[][]
    {
        [-1, 0],
        [0, 1],
        [1, 0],
        [0, -1]
    };


    public static void Run()
    {
        Console.WriteLine("Day 2");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        Dictionary<Tuple<int, int>, HashSet<int>> visited = new();
        var r = 0;
        var currPos = _startPos;

        while (InBounds(currPos))
        {
            // PrintMap(currPos, visited);
            if (!visited.ContainsKey(currPos)) visited[currPos] = [];
            visited[currPos].Add(r % 4);
            var currMove = Moves[r % 4];
            var nearObject = NearObject(currPos, currMove);
            if (nearObject == null) break;
            if (nearObject == true) r += 1;
            else currPos = Tuple.Create(currPos.Item1 + currMove[0], currPos.Item2 + currMove[1]);
        }

        Console.WriteLine($"Part1 solution: {visited.Count}");
    }

    private static void Part2()
    {
        var result = 0;

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var mapCopy = _map.Select(row => new string(row.ToCharArray())).ToList();
                if (_map[y][x] == '.')
                {
                    var row = _map[y].ToCharArray();
                    row[x] = '#';
                    _map[y] = new string(row);
                }
                else continue;

                Dictionary<Tuple<int, int>, HashSet<int>> visited = new();
                var r = 0;
                var currPos = _startPos;


                while (InBounds(currPos))
                {
                    var currMove = Moves[r % 4];

                    if (!visited.ContainsKey(currPos)) visited[currPos] = [];
                    if (!visited[currPos].Add(r % 4))
                    {
                        result += 1;
                        break;
                    }

                    var nearObject = NearObject(currPos, currMove);
                    if (nearObject == null) break;
                    if (nearObject == true) r += 1;
                    else currPos = Tuple.Create(currPos.Item1 + currMove[0], currPos.Item2 + currMove[1]);
                }

                _map = mapCopy;
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

    private static bool? NearObject(Tuple<int, int> position, int[] nextMove)
    {
        var newY = position.Item1 + nextMove[0];
        var newX = position.Item2 + nextMove[1];
        if (!InBounds(Tuple.Create(newY, newX))) return null;
        return _map[newY][newX] == '#';
    }

    private static void PrintMap(Tuple<int, int> position, Dictionary<Tuple<int, int>, HashSet<int>> visited,
        List<Tuple<int, int>> newObjects = null)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var pos = Tuple.Create(y, x);
                if (newObjects != null && newObjects.Contains(pos)) Console.Write("X ");
                else if (visited.ContainsKey(pos))
                {
                    switch (visited[pos].ToArray().Last())
                    {
                        case 0: Console.Write("\u2191 "); break;
                        case 1: Console.Write("\u2192 "); break;
                        case 2: Console.Write("\u2193 "); break;
                        case 3: Console.Write("\u2190 "); break;
                    }
                }
                else if (y == position.Item1 && x == position.Item2) Console.Write("@ ");
                else Console.Write($"{_map[y][x]} ");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static void ParseInput()
    {
        _map = ParseLines(6).ToList();
        _height = _map.Count;
        _width = _map[0].Length;

        var y = 0;
        while (y < _height)
        {
            var x = _map[y].IndexOf('^');
            if (x != -1)
            {
                _startPos = Tuple.Create(y, x);
                break;
            }

            y += 1;
        }
    }
}