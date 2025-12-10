using System.Text.RegularExpressions;

namespace adventOfcode2025;

using Dumpify;
using Microsoft.Z3;
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
            int SolveJoltageProblem(int[] targets, List<int[]> buttons)
            {
                using (Context ctx = new Context())
                {
                    IntExpr[] buttonVars = buttons.Select((b, i) =>
                        ctx.MkIntConst($"btn_{i}")).ToArray();

                    Optimize opt = ctx.MkOptimize();

                    for (int counter = 0; counter < targets.Length; counter++)
                    {
                        ArithExpr sum = ctx.MkInt(0);
                        for (int btnIdx = 0; btnIdx < buttons.Count; btnIdx++)
                        {
                            if (buttons[btnIdx][counter]>0)
                            {
                                sum = ctx.MkAdd(sum, buttonVars[btnIdx]);
                            }
                        }

                        // Constraint: sum must equal target
                        opt.Add(ctx.MkEq(sum, ctx.MkInt(targets[counter])));
                        
                    }
                    // Constraint: each button can only be pressed an positive number of times
                    foreach (var buttonVar in buttonVars)opt.Add(ctx.MkGe(buttonVar, ctx.MkInt(0)));
                    // 4. Objective: minimize total button presses
                    ArithExpr totalPresses = ctx.MkAdd(buttonVars);
                    opt.MkMinimize(totalPresses);
                  //  Console.WriteLine(opt.ToString());
                    if (opt.Check() == Status.SATISFIABLE)
                    {
                        Model model = opt.Model;
                        int total = 0;
                        for (int i = 0; i < buttonVars.Length; i++)
                        {
                            var dai= ((IntNum)model.Evaluate(buttonVars[i])).Int;
                   //    Console.WriteLine($"Button {i}: {dai}");
                            total += dai;
                        }
                        return total;
                    }
                    else
                    {
                        Console.WriteLine("No solution exists");
                        return -1;
                       
                    }
                }
            }

            ans+=SolveJoltageProblem(targets, buttons);
        }
        // solsCases.Dump();

        return ans;
    }
}