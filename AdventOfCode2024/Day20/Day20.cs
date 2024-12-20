namespace AdventOfCode2024.Day20;

public class Day20 : Parser
{
    private static List<string> _maze;
    private static int _height;
    private static int _width;
    private static (int, int) _start;
    private static (int, int) _end;
    private static Dictionary<(int, int), int> _memo = new();

    private const int SaveThreshold = 100;
    private static readonly List<(int, int)> Moves = [(0, 1), (0, -1), (1, 0), (-1, 0)];


    public static void Run()
    {
        Console.WriteLine("Day 20");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;
        var cheatTime = 2;

        var cheatMask = GenerateCheatMask(cheatTime);

        var (fairRun, saved) = Race(cheatMask);

        foreach (var save in saved)
        {
            if (fairRun - save >= SaveThreshold) result++;
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        var cheatTime = 20;

        var cheatMask = GenerateCheatMask(cheatTime);

        var (fairRun, saved) = Race(cheatMask);

        foreach (var save in saved)
        {
            if (fairRun - save >= SaveThreshold) result++;
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static List<(int, int)> GenerateCheatMask(int cheatTime)
    {
        var mask = new List<(int, int)>();
        for (var y = -cheatTime; y <= cheatTime; y++)
        {
            for (var x = -cheatTime; x <= cheatTime; x++)
            {
                if (Math.Abs(y) + Math.Abs(x) <= cheatTime)
                {
                    mask.Add((y, x));
                }
            }
        }

        return mask;
    }

    private static (int, List<int>) Race(List<(int, int)> cheatMask)
    {
        var saved = new List<int>();
        var fairRun = int.MaxValue;

        var visited = CreateVisited();
        visited[_start.Item1][_start.Item2] = 0;
        var queue = new PriorityQueue<((int, int), int), int>();
        queue.Enqueue((_start, 0), 0);

        while (queue.Count > 0)
        {
            var (currPos, currCost) = queue.Dequeue();

            if (currPos == _end)
            {
                fairRun = Math.Min(fairRun, currCost);
                continue;
            }

            foreach (var move in cheatMask)
            {
                var newPos = (currPos.Item1 + move.Item1, currPos.Item2 + move.Item2);

                if (Moves.Contains(move)
                    && InBounds(newPos)
                    && _maze[newPos.Item1][newPos.Item2] != '#'
                    && visited[newPos.Item1][newPos.Item2] == int.MaxValue)
                {
                    visited[newPos.Item1][newPos.Item2] = currCost + 1;
                    queue.Enqueue((newPos, currCost + 1), currCost + 1);
                }

                if (InBounds(newPos)
                    && _maze[newPos.Item1][newPos.Item2] != '#'
                    && Moves.Any(m => _maze[currPos.Item1 + m.Item1][currPos.Item2 + m.Item2] == '#'))
                {
                    if (!_memo.ContainsKey(newPos)) _memo[newPos] = RaceAfterCheat(newPos);
                    saved.Add(_memo[newPos] + currCost + Math.Abs(move.Item1) + Math.Abs(move.Item2));
                }
            }
        }

        return (fairRun, saved);
    }

    private static int RaceAfterCheat((int, int) newStart)
    {
        var visited = CreateVisited();
        visited[newStart.Item1][newStart.Item2] = 0;
        var queue = new Queue<((int, int), int)>();
        queue.Enqueue((newStart, 0));

        while (queue.Count > 0)
        {
            var (currPos, currCost) = queue.Dequeue();

            if (currPos == _end) return currCost;

            foreach (var move in Moves)
            {
                var newPos = (currPos.Item1 + move.Item1, currPos.Item2 + move.Item2);

                if (InBounds(newPos)
                    && _maze[newPos.Item1][newPos.Item2] != '#'
                    && visited[newPos.Item1][newPos.Item2] == int.MaxValue)
                {
                    currCost++;
                    visited[newPos.Item1][newPos.Item2] = currCost;
                    queue.Enqueue((newPos, currCost));
                }
            }
        }

        return int.MaxValue;
    }

    private static bool InBounds((int, int) position)
    {
        return position.Item1 >= 0 &&
               position.Item1 < _height &&
               position.Item2 >= 0 &&
               position.Item2 < _width;
    }

    private static List<List<int>> CreateVisited()
    {
        var visited = new List<List<int>>();
        for (var i = 0; i < _height; i++)
        {
            visited.Add([]);
            for (var j = 0; j < _width; j++)
            {
                visited[i].Add(int.MaxValue);
            }
        }

        return visited;
    }

    private static void ParseInput()
    {
        _maze = ParseLines(20).ToList();
        _height = _maze.Count;
        _width = _maze[0].Length;

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (_maze[y][x] == 'S')
                {
                    _start = (y, x);
                }
                else if (_maze[y][x] == 'E')
                {
                    _end = (y, x);
                }
            }
        }
    }
}