namespace AdventOfCode2024.Day21;

public class Day21 : Parser
{
    private static List<string> _codes = [];
    private static (int, int) _numericKeypadStart = (3, 2);
    private static Dictionary<(char, char), string> _numericMemo = new();
    private const int NumericHeigth = 4;

    private const int NumericWidth = 3;

    private static List<string> NumericKeypad =
    [
        "789",
        "456",
        "123",
        "#0A"
    ];

    private static (int, int) _directionalKeypadStart = (0, 2);
    private static Dictionary<(char, char), string> _directionalMemo = new();
    private static bool _directionalMemoCreated = false;
    private const int DirectionalHeigth = 2;
    private const int DirectionalWidth = 3;

    private static List<string> DirectionalKeypad =
    [
        "#^A",
        "<v>"
    ];

    private static Dictionary<(int, int), char> Moves = new()
    {
        [(0, -1)] = '<',
        [(0, 1)] = '>',
        [(-1, 0)] = '^',
        [(1, 0)] = 'v'
    };


    public static void Run()
    {
        Console.WriteLine("Day 21");
        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;
        string? totalBestPermutation = null;

        var moves = Moves.ToList();
        foreach (var code in _codes)
        {
            var smallestPath = int.MaxValue;
            string bestPermutation = "";
            foreach (var permutation in GetPermutations(moves, moves.Count))
            {
                var permutedMoves = permutation.ToDictionary(x => x.Key, x => x.Value);

                var firstRobotMoves = GenerateMoves(code, _numericKeypadStart, NumericKeypad, NumericHeigth,
                    NumericWidth, permutedMoves);
                var secondRobotMoves = GenerateMoves(firstRobotMoves, _directionalKeypadStart, DirectionalKeypad,
                    DirectionalHeigth, DirectionalWidth, permutedMoves);
                var meMoves = GenerateMoves(secondRobotMoves, _directionalKeypadStart, DirectionalKeypad,
                    DirectionalHeigth, DirectionalWidth, permutedMoves);

                // smallestPath = Math.Min(smallestPath, meMoves.Length);
                if (smallestPath > meMoves.Length)
                {
                    smallestPath = meMoves.Length;
                    bestPermutation = string.Join("", permutation.Select(x => x.Value));
                }
            }

            if (totalBestPermutation == null)
            {
                totalBestPermutation = bestPermutation;
            }
            else if (totalBestPermutation == bestPermutation)
            {
                Console.WriteLine("Same permutation");
            }
            else
            {
                Console.WriteLine($"Different permutation: {totalBestPermutation} vs {bestPermutation}");
            }

            Console.WriteLine("Best permutation: ");
            result += smallestPath * int.Parse(code[..^1]);
        }

        Console.WriteLine($"Part1 solution: {result}");
    }
    
    private static void Part2()
    {
        var result = 0;

        var moves = Moves.ToList();
        foreach (var code in _codes)
        {
            var smallestPath = int.MaxValue;
            foreach (var permutation in GetPermutations(moves, moves.Count))
            {
                var permutedMoves = permutation.ToDictionary(x => x.Key, x => x.Value);

                var robotMoves = GenerateMoves(code, _numericKeypadStart, NumericKeypad, NumericHeigth,
                    NumericWidth, permutedMoves);
                for (var i = 0; i < 25; i++)
                {
                    Console.WriteLine("Current robot: " + i);
                    Console.WriteLine("Current robot moves length: " + robotMoves.Length);
                    robotMoves = GenerateMoves(robotMoves, _numericKeypadStart, NumericKeypad, NumericHeigth,
                        NumericWidth, permutedMoves, _directionalMemo);
                }

                var meMoves = GenerateMoves(robotMoves, _directionalKeypadStart, DirectionalKeypad,
                    DirectionalHeigth, DirectionalWidth, permutedMoves, _directionalMemo);


                smallestPath = Math.Min(smallestPath, meMoves.Length);
            }

            result += smallestPath * int.Parse(code[..^1]);
        }

        Console.WriteLine($"Part2 solution: {result}");
    }


    private static string GenerateMoves(string code, (int, int) start, List<string> keypad, int height, int width,
        Dictionary<(int, int), char> permutedMoves, Dictionary<(char, char), string>? memo = null)
    {
        var memoCounter = 0;
        var finalPath = "";
        var currPos = start;
        foreach (var c in code)
        {
            var startChar = keypad[currPos.Item1][currPos.Item2];
            // if (memo?.ContainsKey((startChar, c)) == true)
            // {
            //     memoCounter++;
            //     finalPath += memo[(startChar, c)];
            //     continue;
            // }
            //
            // if (_directionalMemoCreated)
            // {
            //     finalPath += _directionalMemo[(startChar, c)];
            //     continue;
            // }
            
            var path = "";
            var lastMove = (-1, -1);
            

            var visited = CreateVisited(height, width);
            var parents = CreateParents(width, height);

            visited[currPos.Item1][currPos.Item2] = true;
            var queue = new Queue<((int, int), (int, int))>();
            queue.Enqueue((currPos, lastMove));

            while (queue.Count > 0)
            {
                (currPos, lastMove) = queue.Dequeue();

                if (keypad[currPos.Item1][currPos.Item2] == c) break;

                var moves = permutedMoves.Keys.ToList();
                if (lastMove != (-1, -1))
                {
                    moves.Remove(lastMove);
                    moves.Insert(0, lastMove);
                }

                foreach (var move in moves)
                {
                    var newPos = (currPos.Item1 + move.Item1, currPos.Item2 + move.Item2);
                    if (InBounds(newPos, width, height)
                        && keypad[newPos.Item1][newPos.Item2] != '#'
                        && !visited[newPos.Item1][newPos.Item2])
                    {
                        visited[newPos.Item1][newPos.Item2] = true;
                        parents[newPos.Item1][newPos.Item2] = currPos;
                        queue.Enqueue((newPos, move));
                    }
                }
            }

            foreach (var parent in GetPath(currPos, parents).AsEnumerable().Reverse())
            {
                path += permutedMoves[parent];
            }
            path += "A";
            // if (memo != null)
            // {
            //     memo[(startChar, c)] = path;
            // }
            finalPath += path;
        }

        // if (!_directionalMemoCreated && memoCounter == code.Length) _directionalMemoCreated = true;
        return finalPath;
    }
    
    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(List<T> list, int length)
    {
        if (length == 1)
            return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat([t2]));
    }

    private static List<(int, int)> GetPath((int, int) pos, List<List<(int, int)?>> parents)
    {
        var curr = pos;
        List<(int, int)> path = [];
        while (parents[curr.Item1][curr.Item2] != null)
        {
            var newCurr = parents[curr.Item1][curr.Item2]!.Value;
            var move = (curr.Item1 - newCurr.Item1, curr.Item2 - newCurr.Item2);
            path.Add(move);
            curr = newCurr;
        }

        return path;
    }

    private static bool InBounds((int, int) position, int height, int width)
    {
        return position.Item1 >= 0
               && position.Item1 < width
               && position.Item2 >= 0
               && position.Item2 < height;
    }

    private static List<List<bool>> CreateVisited(int height, int width)
    {
        var visited = new List<List<bool>>();
        for (var y = 0; y < height; y++)
        {
            visited.Add([]);
            for (var x = 0; x < width; x++)
            {
                visited[y].Add(false);
            }
        }

        return visited;
    }

    private static List<List<(int, int)?>> CreateParents(int width, int height)
    {
        var visited = new List<List<(int, int)?>>();
        for (var i = 0; i < height; i++)
        {
            visited.Add([]);
            for (var j = 0; j < width; j++)
            {
                visited[i].Add(null);
            }
        }

        return visited;
    }
    
    private static void ParseInput()
    {
        _codes = ParseLines(21).ToList();
    }
}