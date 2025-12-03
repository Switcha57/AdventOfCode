namespace adventOfcode2025;

public class MyFancyIntComparer : IComparer<int>
{
    public int Compare(int x, int y)
    {
        return x.CompareTo(y) * -1;
    }
}

public class Day03 : ISolver
{
    public object Part1(string[] input)
    {
        int sumMax = 0;

        foreach (var pack in input)
        {
            int currMax = 0;
            SortedDictionary<char, int> pqFirst = new();
            SortedDictionary<char, int> pqSecond = new();

            for (int i = 0; i < pack.Length; i++)
            {
                if (!pqSecond.TryAdd(pack[i], 1))
                {
                    pqSecond[pack[i]]++;
                }

                ;
            }

            for (int i = 0; i < pack.Length - 1; i++)
            {
                if (!pqFirst.TryAdd(pack[i], 1))
                {
                    pqFirst[pack[i]]++;
                }

                ;
                if (pqSecond[pack[i]] == 1)
                {
                    pqSecond.Remove(pack[i]);
                }
                else
                {
                    pqSecond[pack[i]]--;
                }

                ;
                currMax = Math.Max(currMax, int.Parse(pqFirst.Last().Key.ToString() + pqSecond.Last().Key.ToString()));
            }

            sumMax += currMax;
        }

        return sumMax;
    }

    private long maxNumber;
    private long[] maxNumberPerDigit;
    string currPack;
    Dictionary<int, long> memo; //taken digits, number

    private long dp(long number, int i, int taken, int remSkip)
    {
        if (i == currPack.Length || taken == 12)
        {
           // Console.WriteLine(number);
            maxNumber = Math.Max(maxNumber, number);
            memo[taken] = maxNumber;
            return memo[taken];
        }

        if (memo[taken]>number) return maxNumber;
        memo[taken]=number;
        long result;
        if (remSkip == 0)
            result = dp(number * 10 + long.Parse($"{currPack[i]}"), i + 1, taken + 1, 0);
        else
            result = Math.Max(dp(number * 10 + long.Parse($"{currPack[i]}"), i + 1, taken + 1, remSkip),
                dp(number, i + 1, taken, remSkip-1));
       
        return result;
    }

    public object Part2(string[] input) //dp
    {
        long sumMax = 0;

        foreach (var pack in input)
        {
            maxNumber = 0;
            currPack = pack;
            memo = new Dictionary<int, long>();
            for(int i=0;i<13;i++) memo.Add(i,0);
            
            sumMax += dp(0, 0, 0, pack.Length - 12);
        }

        return sumMax;
    }
}