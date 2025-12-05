namespace adventOfcode2025;

public class Day05:ISolver
{
    private List<Tuple<long, long>> ListofID = new List<Tuple<long, long>>();
    private List<long> Querys = new List<long>();

    void ParseInput(string[] input)
    {
     
        long i = 0;
        for (; i < input.Length&&! String.IsNullOrWhiteSpace(input[i]); i++)
        {
           // Console.WriteLine(input[i]);
            var rID = input[i].Split("-");
            ListofID.Add(new Tuple<long, long>(long.Parse(rID[0]), long.Parse(rID[1])));
        }

        i++;
        for (; i < input.Length; i++) Querys.Add(long.Parse(input[i]));
        
    }
    public object Part1(string[] input)
    {
        ParseInput(input);
        long count = 0;
        foreach (var query in Querys)
        {
            count += ListofID.Any(x => x.Item1 <= query && query <= x.Item2) ? 1 : 0;
        }
        return count;
    }

    public object Part2(string[] input)
    {
        //ParseInput(input);
        ListofID = ListofID.OrderBy(x => x.Item1).ThenBy(x=>x.Item2).ToList();
        
        long ans=0;
        long curr = ListofID[0].Item1;
        long currE = ListofID[0].Item2;
        for (int i = 1; i < ListofID.Count; i++)
        {
            var rr = ListofID[i];
            if (rr.Item1 <= currE +1) currE = Math.Max(rr.Item2, currE);
            else
            {
                ans += currE - curr+1;
                curr = rr.Item1;
                currE = rr.Item2;
            }
        }
        return ans+ currE - curr+1;
    }
}