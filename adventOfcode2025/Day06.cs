using System.Text.RegularExpressions;

namespace adventOfcode2025;

public class Day06 : ISolver
{
    public object Part1(string[] input)
    {
        return 0;// dovrei creare un boilerplate migliore, inquino array di input facendo cosi
        List<Tuple<long, long>> Mahts = new List<Tuple<long, long>>(input[0].Length);
        Mahts.AddRange(Enumerable.Repeat(Tuple.Create(0L, 1L), input[0].Length));
        int i = 0;
        for (; i < input.Length; i++)
        {
            input[i] = Regex.Replace(input[i], "\\s+", " ").Trim();
            var r = input[i].Split(" ");
            if (r[0] == "*" || r[0] == "+") break;
            for (int j = 0; j < r.Length; j++)
            {
                Mahts[j] = Tuple.Create(Mahts[j].Item1 + long.Parse(r[j]), Mahts[j].Item2 * long.Parse(r[j]));
            }
        }

        long ans = 0;
        var ops = input[i].Split(" ");
        for (int j = 0; j < ops.Length; j++)
        {
            if (ops[j] == "*") ans += Mahts[j].Item2;
            else ans += Mahts[j].Item1;
        }

        return ans;
    }

   public object Part2(string[] input)
    {
        var lines = input.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        int maxWidth = lines.Max(x => x.Length);
        var grid = lines.Select(l => l.PadRight(maxWidth)).ToList();

        string opLine = grid.Last();
        var numLines = grid.Take(grid.Count - 1).ToList();

        long ans = 0;
        
        
        List<long> currentBlockNumbers = new List<long>();
        char currentBlockOp = ' ';

        for (int col = 0; col < maxWidth; col++)
        {
            
            bool isSeparator = true;
            for (int row = 0; row < grid.Count; row++)
            {
                if (grid[row][col] != ' ')
                {
                    isSeparator = false;
                    break;
                }
            }

            if (isSeparator)
            {
                if (currentBlockNumbers.Count > 0)
                {
                    ans += CalculateBlock(currentBlockNumbers, currentBlockOp);
                    currentBlockNumbers.Clear();
                    currentBlockOp = ' '; }
            }
            else
            {
                // It is a data column
                
                // A. Build the number from this column (Top to Bottom)
                string numStr = "";
                foreach (var line in numLines)
                {
                    if (char.IsDigit(line[col]))
                    {
                        numStr += line[col];
                    }
                }

                if (numStr.Length > 0)
                {
                    currentBlockNumbers.Add(long.Parse(numStr));
                }

                if (opLine[col] == '+' || opLine[col] == '*')
                {
                    currentBlockOp = opLine[col];
                }
            }
        }

        if (currentBlockNumbers.Count > 0)
        {
            ans += CalculateBlock(currentBlockNumbers, currentBlockOp);
        }

        return ans;
    }

    private long CalculateBlock(List<long> nums, char op)
    {
        if (nums.Count == 0) return 0;
        
        if (op == '*')
        {
            return nums.Aggregate(1L, (acc, val) => acc * val);
        }
        else
        {
            return nums.Sum();
        }
    }
}