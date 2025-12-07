namespace adventOfcode2025;

public class Day07: ISolver
{
    public object Part1(string[] input)
    {
        HashSet<int> active_Y=new HashSet<int>(),deplated_Y=new HashSet<int>(),to_add_Y=new HashSet<int>();

        int ans = 0;
        active_Y.Add(input[0].IndexOf('S'));
        
        for (int i=1; i < input.Length; i++)
        {
            foreach (var y in active_Y)
                if (input[i][y] == '^')
                {
                    ans++;
                    if (y > 0) to_add_Y.Add(y-1);
                    if (y < input[i].Length-1) to_add_Y.Add(y+1);
                    deplated_Y.Add(y);
                }
            active_Y.ExceptWith(deplated_Y);
            active_Y.UnionWith(to_add_Y);
            to_add_Y.Clear();
            deplated_Y.Clear();
        }
        return ans;
    }

    public object Part2(string[] input)
    {
        long ans = 1;
        List<long> active=new List<long>(input[0].Length);
        for (int i = 0; i<input[0].Length; i++)
        {
            active.Add(0);
        }
        active[input[0].IndexOf('S')] = 1;
        for (int i = 1; i < input.Length; i++)
        {
            List<long> Nxt_active=new List<long>(input[i].Length);
            var splitter=input[i].Select((x,j)=>x=='^'?j:-1).Where(j=>j!=-1).ToArray();
            Nxt_active.AddRange(active);
            foreach (var futures in splitter)
            {
                long act=active[futures];
                Nxt_active[futures] = 0;
                if (futures > 0)
                {
                    // ans += act;
                    Nxt_active[futures - 1] += act;
                }
                if (futures < input[i].Length-1)
                {
                    // ans += act;
                    Nxt_active[futures + 1] += act;
                }
            }
            active=Nxt_active;
        }
        
        return active.Sum();
    }
}