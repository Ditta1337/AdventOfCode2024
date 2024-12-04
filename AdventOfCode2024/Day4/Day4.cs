using System.Runtime.InteropServices.JavaScript;

namespace AdventOfCode2024.Day4;

public class Day4 : Parser
{
    private static List<string> _puzzle = new();
    private static int _height;
    private static int _width;

    private static readonly int[][] Moves = new int[][]
    {
        [0, 1],
        [1, 0],
        [0, -1],
        [-1, 0],
        [1, 1],
        [1, -1],
        [-1, -1],
        [-1, 1]
    };

    private static readonly int[][] XPattern = new int[][]
    {
        [-1, -1],
        [-1, 1],
        [0, 0],
        [1, -1],
        [1, 1]
    };


    public static void Run()
    {
        Console.WriteLine("Day 4");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;
        var keyword = "XMAS";

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                if (_puzzle[i][j] == keyword[0])
                {
                    result += CheckSection(i, j, keyword, Moves);
                }
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        var codes = new[]
        {
            "MSAMS",
            "SMASM",
            "MMASS",
            "SSAMM"
        };

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                if (_puzzle[i][j] == 'A')
                {
                    result += CheckPattern(i, j, codes, XPattern);
                }
            }
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static int CheckSection(int i, int j, string keyword, int[][] moves)
    {
        var foundKeywords = 0;
        foreach (var move in moves)
        {
            var (currI, currJ) = (i, j);
            var k = 1;
            while (k < keyword.Length)
            {
                currI += move[0];
                currJ += move[1];
                if (currI < 0 ||
                    currI >= _height ||
                    currJ < 0 ||
                    currJ >= _height ||
                    _puzzle[currI][currJ] != keyword[k]
                   ) break;
                k += 1;
            }

            if (k == keyword.Length) foundKeywords += 1;
        }

        return foundKeywords;
    }

    private static int CheckPattern(int i, int j, string[] codes, int[][] pattern)
    {
        var code = "";
        foreach (var position in pattern)
        {
            var (currI, currJ) = (i, j);
            currI += position[0];
            currJ += position[1];
            if (currI < 0 ||
                currI >= _height ||
                currJ < 0 ||
                currJ >= _height
               ) break;
            code += _puzzle[currI][currJ];
        }

        return (codes.Contains(code)) ? 1 : 0;
    }

    private static void ParseInput()
    {
        _puzzle = ParseLines(4).ToList();
        _height = _puzzle.Count;
        _width = _puzzle[0].Length;
    }
}