using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14;

public class Day14 : Parser
{
    private static Dictionary<(int, int), List<(int, int)>> _positions = new();
    private const string Pattern = @"-?\d+";
    private const int Height = 103;
    private const int Width = 101;
    private const int Time = 100;

    public static void Run()
    {
        Console.WriteLine("Day 14");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        for (var t = 0; t < Time; t++)
        {
            var newPositions = new Dictionary<(int, int), List<(int, int)>>();

            foreach (var position in _positions.ToList())
            {
                foreach (var robot in position.Value)
                {
                    var newX = (position.Key.Item1 + robot.Item1 + Width) % Width;
                    var newY = (position.Key.Item2 + robot.Item2 + Height) % Height;

                    if (!newPositions.ContainsKey((newX, newY)))
                        newPositions[(newX, newY)] = [];

                    newPositions[(newX, newY)].Add((robot.Item1, robot.Item2));
                }
            }

            _positions = newPositions;
        }

        ParseInput(); // Reset positions

        Console.WriteLine($"Part1 solution: {GetSafetyFactor()}");
    }


    private static int GetSafetyFactor()
    {
        var middleY = Height / 2;
        var middleX = Width / 2;
        List<int> quadrants = [0, 0, 0, 0];

        foreach (var position in _positions.Keys)
        {
            var x = position.Item1;
            var y = position.Item2;

            if (y > middleY && x > middleX) quadrants[0] += _positions[position].Count;
            if (y > middleY && x < middleX) quadrants[1] += _positions[position].Count;
            if (y < middleY && x < middleX) quadrants[2] += _positions[position].Count;
            if (y < middleY && x > middleX) quadrants[3] += _positions[position].Count;
        }

        Console.WriteLine($"{quadrants[0]} {quadrants[1]} {quadrants[2]} {quadrants[3]}");

        return quadrants.Aggregate(1, (current, value) => current * value);
    }

    private static void Print()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Console.Write(_positions.ContainsKey((x, y)) ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static void Part2()
    {
        var result = 0;
        
        for (var t = 0;; t++)
        {
            if (HasTree())
            {
                Print();
                result = t;
                break;
            }

            var newPositions = new Dictionary<(int, int), List<(int, int)>>();

            foreach (var position in _positions.ToList())
            {
                foreach (var robot in position.Value)
                {
                    var newX = (position.Key.Item1 + robot.Item1 + Width) % Width;
                    var newY = (position.Key.Item2 + robot.Item2 + Height) % Height;

                    if (!newPositions.ContainsKey((newX, newY)))
                        newPositions[(newX, newY)] = [];

                    newPositions[(newX, newY)].Add((robot.Item1, robot.Item2));
                }
            }

            _positions = newPositions;
        }
        
        Console.WriteLine($"Part2 solution: {result}");
    }


    private static bool HasTree()
    {
        var blockWidth = 3;
        var blockHeight = 5;

        for (var y = 0; y < Height - blockHeight; y++)
        {
            for (var x = 0; x < Width - blockWidth; x++)
            {
                var hasTree = true;

                for (var i = 0; i < blockHeight; i++)
                {
                    for (var j = 0; j < blockWidth; j++)
                    {
                        if (!_positions.ContainsKey((x + j, y + i)))
                        {
                            hasTree = false;
                            break;
                        }
                    }
                }

                if (hasTree) return true;
            }
        }

        return false;
    }

    private static void ParseInput()
    {
        var regex = new Regex(Pattern);

        foreach (var line in ParseLines(14))
        {
            var matches = regex.Matches(line);

            var x = int.Parse(matches[0].Value);
            var y = int.Parse(matches[1].Value);
            var vX = int.Parse(matches[2].Value);
            var vY = int.Parse(matches[3].Value);

            if (!_positions.ContainsKey((x, y))) _positions[(x, y)] = [];
            _positions[(x, y)].Add((vX, vY));
        }
    }
}