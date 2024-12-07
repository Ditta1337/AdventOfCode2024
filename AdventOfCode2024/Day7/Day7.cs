namespace AdventOfCode2024.Day7;

public class Day7 : Parser
{
    private static List<List<long>> _equations = new();

    public static void Run()
    {
        Console.WriteLine("Day 7");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = 0;
        foreach (var equation in _equations)
        {
            var equationResult = equation[0];
            var numbersLength = equation.Count - 1;
            foreach (var mask in GenerateBitMasks(numbersLength - 1, 2))
            {
                var index = 1;
                var calculated = equation[index];
                foreach (var bit in mask)
                {
                    index += 1;
                    if (bit == '0') calculated += equation[index];
                    else calculated *= equation[index];
                }

                if (calculated == equationResult)
                {
                    result += equationResult;
                    break;
                }
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        long result = 0;
        foreach (var equation in _equations)
        {
            var equationResult = equation[0];
            var numbersLength = equation.Count - 1;
            foreach (var mask in GenerateBitMasks(numbersLength - 1, 3))
            {
                var index = 1;
                var calculated = equation[index];
                foreach (var bit in mask)
                {
                    index += 1;

                    switch (bit)
                    {
                        case '0': calculated += equation[index]; break;
                        case '1': calculated *= equation[index]; break;
                        case '2': calculated = long.Parse(calculated.ToString() + equation[index]); break;
                    }
                }

                if (calculated == equationResult)
                {
                    result += equationResult;
                    break;
                }
            }
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static List<string> GenerateBitMasks(int length, int bitBase)
    {
        var options = Math.Pow(bitBase, length);
        var masks = new List<string>();

        for (var i = 0; i < options; i++)
        {
            var bin = ConvertToBase(i, bitBase);
            masks.Add(bin.PadLeft(length, '0'));
        }

        return masks;
    }

    private static string ConvertToBase(int value, int toBase)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (toBase < 2 || toBase > chars.Length) throw new ArgumentException("Invalid Base.");

        var result = "";
        do
        {
            result = chars[value % toBase] + result;
            value /= toBase;
        } while (value > 0);

        return result;
    }

    private static void ParseInput()
    {
        foreach (var equation in ParseLines(7))
        {
            var splitEquation = equation.Split(':');
            var parsedEquation = new List<long> { long.Parse(splitEquation[0]) };
            parsedEquation.AddRange(splitEquation[1].Trim().Split(' ').Select(long.Parse));

            _equations.Add(parsedEquation);
        }
    }
}