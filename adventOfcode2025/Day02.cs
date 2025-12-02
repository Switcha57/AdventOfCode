namespace adventOfcode2025;

public class Day02 : ISolver
{
    private string[] _input;

    void SplitInput(string[] input)
    {
        _input = input[0].Split(',');
    }

    long CheckValidity(int cifre, long min, long max)
    {
        long current = 0;
        
        for (long i = Convert.ToInt64(Math.Pow(10,cifre/2-1)); ; i++)
        {
            long ii = long.Parse($"{i}{i}");
            if (ii >= min && ii <= max) current+=ii;
            if (ii > max) break;

        }

        return current;
    }
    long InvalidID(string IDrange)
    {
        // mi interessano solo numeri di lunghezza pari
        int minRange = (IDrange.Split('-')[0]).Length % 2 == 0
            ? (IDrange.Split('-')[0]).Length
            : (IDrange.Split('-')[0]).Length + 1;
        int maxRange = (IDrange.Split('-')[1]).Length % 2 == 0
            ? (IDrange.Split('-')[1]).Length
            : (IDrange.Split('-')[1]).Length - 1;
        long min = long.Parse(IDrange.Split('-')[0]);
        long max = long.Parse(IDrange.Split('-')[1]);
        long currentSam=0;
        for (; minRange <= maxRange; minRange += 2)
        {
            currentSam+=CheckValidity(minRange,min,max);
        }
        return currentSam;
    }

    public object Part1(string[] input)
    {
        long solve = 0;
        SplitInput(input);
        foreach (var IDrange in _input)
        {
            solve += InvalidID(IDrange);
        }

        return solve;
    }

    public object Part2(string[] input)
    {
        return "Ciao";
    }
}