namespace AdventOfCode2024.Day9;

public class Day9 : Parser
{
    private static List<int> _diskMap = new();

    public static void Run()
    {
        Console.WriteLine("Day 9");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = 0;

        var i = 0;
        var j = _diskMap.Count - 1;
        var iIndex = 0;
        var moving = false;

        while (i <= j)
        {
            if (moving)
            {
                while (_diskMap[i] > 0)
                {
                    var toBreak = j <= i;
                    while (_diskMap[j] > 0 && !toBreak)
                    {
                        result += iIndex * CalcIndex(j);
                        _diskMap[i]--;
                        _diskMap[j]--;

                        iIndex++;

                        if (_diskMap[i] == 0) toBreak = true;
                    }

                    if (toBreak) break;
                    j -= 2;
                }

                moving = false;
            }
            else
            {
                while (_diskMap[i] > 0)
                {
                    result += iIndex * CalcIndex(i);
                    _diskMap[i]--;
                    iIndex++;
                }

                moving = true;
            }

            i++;
        }
        
        ParseInput(); // restore _diskMap
        Console.WriteLine($"Part1 solution: {result}");
    }

    private static int CalcIndex(int i)
    {
        return (int)Math.Floor((double)i / 2);
    }

    private static void Part2()
    {
        long result = 0;

        var blocks = GenerateBlocks();
        var lastBlockInd = blocks.Count - 1;

        while (lastBlockInd > 0)
        {
            var block = blocks[lastBlockInd];
            for (var i = 0; i < lastBlockInd; i++)
            {
                var gap = blocks[i + 1][0] - blocks[i][1];
                if (gap >= block[1] - block[0])
                {
                    block[1] = blocks[i][1] + block[1] - block[0];
                    block[0] = blocks[i][1];
                    blocks.RemoveAt(lastBlockInd);
                    blocks.Insert(i + 1, block);
                    lastBlockInd++;
                    break;
                }
            }

            lastBlockInd--;
        }

        foreach (var block in blocks)
        {
            for (var i = block[0]; i < block[1]; i++)
            {
                result += i * block[2];
            }
        }


        Console.WriteLine($"Part2 solution: {result}");
    }

    private static List<List<int>> GenerateBlocks()
    {
        var blocks = new List<List<int>>();

        var blockStart = 0;
        for (var i = 0; i < _diskMap.Count; i++)
        {
            if (i % 2 == 0)
            {
                var blockEnd = blockStart + _diskMap[i];
                blocks.Add([blockStart, blockEnd, CalcIndex(i)]);
                blockStart = blockEnd;
            }
            else
            {
                blockStart += _diskMap[i];
            }
        }

        return blocks;
    }

    private static void ParseInput()
    {
        _diskMap = ParseLines(9)[0].Select(c => int.Parse(c.ToString())).ToList();
    }
}