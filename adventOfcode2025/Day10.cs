using System.Text.RegularExpressions;

namespace adventOfcode2025;

using Dumpify;

public class Day10 : ISolver
{
    public object Part1(string[] input)
    {
        // List<int> solsCases=new List<int>();
        int ans = 0;
        foreach (var machine in input)
        {
            Match diagramMatch = Regex.Match(machine, @"\[([.#]+)\]");
            string diagram = diagramMatch.Groups[1].Value;
            int target = 0;
            for (int i = 0; i < diagram.Length; i++)
            {
                if (diagram[i] == '#')
                {
                    target |= (1 << (diagram.Length - 1 - i)); // Store MSB first for readability
                }
            } // diagramList.Dump();

            int lightCount = diagram.Length;
            List<int> edges = new List<int>();
            foreach (Match match in Regex.Matches(machine, @"\(([^)]+)\)"))
            {
                int mask = 0;
                foreach (string indexStr in match.Groups[1].Value.Split(','))
                {
                    // Console.WriteLine(indexStr);
                    if (int.TryParse(indexStr, out int index))
                    {
                        mask |= (1 << (lightCount - 1 - index));
                    }
                }

                edges.Add(mask);
            }

            int Bfs()
            {
                var queue = new Queue<(int state, int presses)>();
                var visited = new HashSet<int>();
                int initialState = 0;
                queue.Enqueue((initialState, 0));
                visited.Add(initialState);

                while (queue.Count > 0)
                {
                    var (currentState, presses) = queue.Dequeue();
                    if (currentState == target)
                        return presses;
                    foreach (var v in edges)
                    {
                        int newState = currentState ^ v;
                        if (!visited.Contains(newState))
                        {
                            visited.Add(newState);
                            queue.Enqueue((newState, presses + 1));
                        }
                    }
                }

                return -1;
            }

            ans += Bfs();
        }

        // solsCases.Dump();
        // Match joltageMatch = Regex.Match(machine, @"\{([^}]+)\}");
        // string joltage = joltageMatch.Groups[1].Value;
        return ans;
    }


    public object Part2(string[] input)
    {
        long ans = 0;
        foreach (var machine in input)
        {
            Match joltageMatch = Regex.Match(machine, @"\{([^}]+)\}");
            int[] targets = joltageMatch.Groups[1].Value
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            List<int[]> buttons = new List<int[]>();
            foreach (Match match in Regex.Matches(machine, @"\(([^)]+)\)"))
            {
                int[] button = new int[targets.Length];
                var toAdd = match.Groups[1].Value
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                foreach (var i in toAdd)
                {
                    button[i]++;
                }

                buttons.Add(button);
            }

            int Bfs()
            {
                var queue = new Queue<(int[] state, int presses)>();
                var visited = new HashSet<int[]>();
                var initialState = new int[targets.Length];
                queue.Enqueue((initialState, 0));
                visited.Add(initialState);

                while (queue.Count > 0)
                {
                    var (currentState, presses) = queue.Dequeue();
                    if (currentState.SequenceEqual(targets))
                        return presses;
                    if (currentState.Select((x, i) => new { pressed = x, indexs = i })
                        .Any(x => targets[x.indexs] < x.pressed))
                    {
                        continue;
                    }

                    foreach (var v in buttons)
                    {
                        int[] newState = currentState.Select((x, i) => x + v[i]).ToArray();
                        if (!visited.Contains(newState))
                        {
                            visited.Add(newState);
                            queue.Enqueue((newState, presses + 1));
                        }
                    }
                }

                return -1;
            }

            ans += Bfs();
        }
        // solsCases.Dump();

        return ans;
    }
}