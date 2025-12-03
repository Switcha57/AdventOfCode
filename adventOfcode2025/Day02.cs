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
    long ConcatNumber(long number, int rep)
    {
        int digits = (int)Math.Floor(Math.Log10(number)) + 1;
        long multiplier = (long)Math.Pow(10, digits);
    
        long result = 0;
        for (int i = 0; i < rep; i++)
        {
            result = result * multiplier + number;
        }
        return result;
    }
    bool OneWay(long n, int cifre)
    {
        // int totalDigits = (int)Math.Floor(Math.Log10(n)) + 1;
        for (int i = 1; i < cifre; i++)
        {
            int totalDigits = (int)Math.Floor(Math.Log10(n)) + 1;
            if (totalDigits%i!=0) continue;
            int rep = totalDigits/i;
            if(ConcatNumber(n / (long)Math.Pow(10, totalDigits - i),rep)==n) return false;
        }
        
        return true;
    }
    long CheckValidity2(int cifre, long min, long max)
    {
        long current = 0;

        for (long i=Convert.ToInt64(Math.Pow(10,cifre-1)); i < Convert.ToInt64(Math.Pow(10,cifre)); i++)
        {
            
            long ii = long.Parse($"{i}{i}");
            while (ii.ToString().Length <= max.ToString().Length)
            {
                if (ii >= min && ii <= max&&OneWay(ii,cifre)) {current+=ii;}
                if (ii > max) break;
                ii=long.Parse($"{ii}{i}");
   
            }
        }
        return current;
    }
    
    long InvalidID2(string IDrange)
    {
        // mi interessano solo numeri di lunghezza pari
        int maxRange = (IDrange.Split('-')[1]).Length % 2 == 0
            ? (IDrange.Split('-')[1]).Length
            : (IDrange.Split('-')[1]).Length - 1;
        long min = long.Parse(IDrange.Split('-')[0]);
        long max = long.Parse(IDrange.Split('-')[1]);
        long currentSam=0;
        for (int minR=1; minR <= maxRange/2; minR += 1)
        {
            currentSam+=CheckValidity2(minR,min,max);
        }
        return currentSam;
    }
    public object Part2(string[] input)
    {
        
        long solve = 0;
        SplitInput(input);
        foreach (var IDrange in _input)
        {
            solve += InvalidID2(IDrange);
        }

        return solve;
    }
}