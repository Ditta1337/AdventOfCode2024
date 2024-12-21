namespace AdventOfCode2024.Day17;

public class Day17 : Parser
{
    private static long _A;
    private static long _B;
    private static long _C;
    private static List<(int, int)> _intstructions = [];
    private static string _originalInstruction = "";

    public static void Run()
    {
        Console.WriteLine("Day 17");
        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        Console.WriteLine($"Part1 solution: {CalculateOutput()}");
    }

    private static void Part2()
    {
        Console.WriteLine($"Part2 solution: {ReverseCompute(_originalInstruction, 0)}");
    }

    private static string CalculateOutput()
    {
        List<long> output = [];
        var pointer = 0;

        while (pointer < _intstructions.Count)
        {
            Compute(ref pointer, output);
        }

        return string.Join(",", output);
    }

    private static void Compute(ref int pointer, List<long> output)
    {
        var instruction = _intstructions[pointer];
        var literal = instruction.Item2;
        var combo = GetComboValue(literal);

        switch (instruction.Item1)
        {
            case 0: _A = (int)Math.Floor(_A / Math.Pow(2, combo)); break;
            case 1: _B ^= literal; break;
            case 2: _B = combo % 8; break;
            case 3: if (_A != 0) pointer = (int)Math.Floor((double)literal / 2) - 1; break;
            case 4: _B ^= _C; break;
            case 5: output.Add(combo % 8); break;
            case 6: _B = (int)Math.Floor(_A / Math.Pow(2, combo)); break;
            case 7: _C = (int)Math.Floor(_A / Math.Pow(2, combo)); break;
        }

        pointer++;
    }

    private static long ReverseCompute(string target, long searchedA)
    {
        if (string.IsNullOrEmpty(target))
        {
            return searchedA;
        }

        for (long i = 0; i < 8; i++)
        {
            _A = searchedA << 3 | i;
            _B = 0;
            _C = 0;

            foreach (var instruction in _intstructions)
            {
                var literal = instruction.Item2;
                var combo = GetComboValue(literal);
                long output = -1;

                switch (instruction.Item1)
                {
                    case 0: break;
                    case 1: _B ^= literal; break;
                    case 2: _B = combo % 8; break;
                    case 3: break;
                    case 4: _B ^= _C; break;
                    case 5: output = combo % 8; break;
                    case 6: _B = (long)Math.Floor(_A / Math.Pow(2, combo)); break;
                    case 7: _C = (long)Math.Floor(_A / Math.Pow(2, combo)); break;
                }

                if (output == int.Parse(target[^1].ToString()))
                {
                    var sub = ReverseCompute(target[..^1], _A);
                    if (sub != -1) return sub;
                }
            }
        }

        return -1;
    }

    private static long GetComboValue(int literal)
    {
        return literal switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => _A,
            5 => _B,
            6 => _C,
            _ => throw new InvalidOperationException("Unrecognized combo operand.")
        };
    }

    private static void ParseInput()
    {
        var lines = ParseLines(17);
        _A = int.Parse(lines[0].Split(": ")[1]);
        _B = int.Parse(lines[1].Split(": ")[1]);
        _C = int.Parse(lines[2].Split(": ")[1]);
        var instructions = lines[4].Split(": ")[1].Split(",");
        _originalInstruction = string.Join("", instructions);

        for (var i = 0; i <= instructions.Length - 2; i += 2)
        {
            _intstructions.Add((int.Parse(instructions[i]), int.Parse(instructions[i + 1])));
        }
    }
}