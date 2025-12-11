using Dumpify;

namespace adventOfcode2025;

public class Day11 :ISolver
{
    public object Part1(string[] input)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        List<List<int>> graph = new List<List<int>>(input.Length+1);
        dict.Add("you", 0);
        dict.Add("out",input.Length);
        int node = 0;
        for (int i = 0; i < input.Length+1; i++) {
            graph.Add(new List<int>()); 
        }
        foreach (var line in input)
        {
            
            var co = line.Split(':');
         //   co.Dump();
            if (dict.TryAdd(co[0], node+1))
            {
                node++;
            }
            var neighbours = co[1].Split(' ')
                .Where(s => s.Length > 0)
                .Select(s =>
                {
             
                    if (!dict.ContainsKey(s))
                    {
                        dict[s] = ++node;
                    }
                    return dict[s];
                })
                .ToList();
          //  neighbours.Dump();
            graph[dict[co[0]]].AddRange(neighbours);
        }
  //      dict.Dump();
        // graph.Dump();

        long[] dp = new long[input.Length+1];
        
        long[] inDegree = new long[input.Length+1];
        for (int u = 0; u < dict.Count; u++) {
            
            foreach (int v in graph[u]) {
                inDegree[v]++;
            }

        }
      //  Topological sort (Kahn's algorithm)
        Queue<int> q = new Queue<int>();
        for (int u = 0; u < input.Length; u++) {
            if (inDegree[u] == 0) {
                q.Enqueue(u);
            }
        }
        
        dp[0] = 1; // Base case
     //   inDegree.Dump();
        while (q.Count > 0) {
            int u = q.Dequeue();
            foreach (int v in graph[u]) {
             //   Console.WriteLine(v);
        
                dp[v] += dp[u];
                inDegree[v]--;
                if (inDegree[v] == 0) {
              //      Console.WriteLine(v);
                    q.Enqueue(v);
                }
            }
        }
        
     //   dp.Dump();
        return dp[dict["out"]];
    }
    

    public object Part2(string[] input) // Topo sort but need to go to some nobe 
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        Dictionary<int,string> rev_dict = new Dictionary<int,string>();

        List<List<int>> graph = new List<List<int>>(input.Length+1);
        dict.Add("svr", 0); //why this change boh
        rev_dict[0] = "svr";
        dict.Add("out",input.Length);
        rev_dict[input.Length] = "out";
        int node = 0;
        for (int i = 0; i < input.Length+1; i++) {
            graph.Add(new List<int>()); 
        }
        foreach (var line in input)
        {
            
            var co = line.Split(':');
            //   co.Dump();
            if (dict.TryAdd(co[0], node+1))
            {
                node++;
                rev_dict[node] = co[0];
            }
            var neighbours = co[1].Split(' ')
                .Where(s => s.Length > 0)
                .Select(s =>
                {
             
                    if (!dict.ContainsKey(s))
                    {
                        dict[s] = ++node;
                        rev_dict[node] = s;

                    }
                    return dict[s];
                })
                .ToList();
            //  neighbours.Dump();
            graph[dict[co[0]]].AddRange(neighbours);
        }

        long[,] dp = new long[input.Length+1,4];
        dp[0, 0] = 1;
        
        long[] inDegree = new long[input.Length+1];
        for (int u = 0; u < dict.Count; u++) {
            
            foreach (int v in graph[u]) {
                inDegree[v]++;
            }

        }
        //  Topological sort (Kahn's algorithm)
        Queue<int> q = new Queue<int>();
        for (int u = 0; u < input.Length; u++) {
            if (inDegree[u] == 0) {
                q.Enqueue(u);
            }
        }
        while (q.Count > 0) {
            int u = q.Dequeue();
            foreach (int v in graph[u]) {
                // dp[v] += dp[u];
                
                for (int i = 0; i < 4; i++)
                {
                    int newState = i;
                    if (rev_dict[u] .Equals("fft") )newState |= 1;
                    if (rev_dict[u] .Equals("dac") )newState |= 2;
                    dp[v,newState]+=dp[u,i];
                    
                }
                inDegree[v]--;
                if (inDegree[v] == 0) {
                    //      Console.WriteLine(v);
                    q.Enqueue(v);
                }
            }
        }
     //   dp.Dump();
        return dp[dict["out"],3];
    }
}