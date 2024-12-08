namespace AdventOfCode2024.Day8;

public class Day8 : Parser
{
    private static Dictionary<char, List<Tuple<int, int>>> _antenasMap = new();
    private static int _height;
    private static int _width;

    public static void Run()
    {
        Console.WriteLine("Day 8");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var antinodes = new HashSet<Tuple<int, int>>();
        foreach (var positions in _antenasMap.Values)
        {
            for (var i = 0; i < positions.Count - 1; i++)
            {
                for (var j = i + 1; j < positions.Count; j++)
                {
                    var (first, second) = (positions[i], positions[j]);
                    int[] forwardVec = [second.Item1 - first.Item1, second.Item2 - first.Item2];
                    int[] backwardVec = [-forwardVec[0], -forwardVec[1]];
                    var firstAntinode = Tuple.Create(second.Item1 + forwardVec[0], second.Item2 + forwardVec[1]);
                    var secondAntinode = Tuple.Create(first.Item1 + backwardVec[0], first.Item2 + backwardVec[1]);
                    if (InBounds(firstAntinode)) antinodes.Add(firstAntinode);
                    if (InBounds(secondAntinode)) antinodes.Add(secondAntinode);
                }
            }
        }

        Console.WriteLine($"Part1 solution: {antinodes.Count}");
    }

    private static void Part2()
    {
        var antinodes = new HashSet<Tuple<int, int>>();
        foreach (var positions in _antenasMap.Values)
        {
            for (var i = 0; i < positions.Count - 1; i++)
            {
                for (var j = i + 1; j < positions.Count; j++)
                {
                    var (first, second) = (positions[i], positions[j]);
                    antinodes.Add(first);
                    antinodes.Add(second);

                    int[] forwardVec = [second.Item1 - first.Item1, second.Item2 - first.Item2];
                    int[] backwardVec = [-forwardVec[0], -forwardVec[1]];

                    var firstAntinode = Tuple.Create(second.Item1 + forwardVec[0], second.Item2 + forwardVec[1]);
                    while (InBounds(firstAntinode))
                    {
                        antinodes.Add(firstAntinode);
                        firstAntinode = Tuple.Create(firstAntinode.Item1 + forwardVec[0],
                            firstAntinode.Item2 + forwardVec[1]);
                    }

                    var secondAntinode = Tuple.Create(first.Item1 + backwardVec[0], first.Item2 + backwardVec[1]);
                    while (InBounds(secondAntinode))
                    {
                        antinodes.Add(secondAntinode);
                        secondAntinode = Tuple.Create(secondAntinode.Item1 + backwardVec[0],
                            secondAntinode.Item2 + backwardVec[1]);
                    }
                }
            }
        }

        Console.WriteLine($"Part2 solution: {antinodes.Count}");
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
        var lines = ParseLines(8);
        _height = lines.Length;
        _width = lines[0].Length;

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var symbol = lines[y][x];
                if (symbol == '.') continue;
                if (!_antenasMap.ContainsKey(symbol)) _antenasMap[symbol] = [];
                _antenasMap[symbol].Add(Tuple.Create(y, x));
            }
        }
    }
}