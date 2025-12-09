using System.Diagnostics;

namespace adventOfcode2025;

class Program
{
    static void Main(string[] args)
    {
        // CHANGE THIS TO RUN A DIFFERENT DAY. Set to 0 to generate README with benchmarks.
        var dayNumber = 0;

        if (dayNumber == 0)
        {
            GenerateReadme();
            return;
        }

        Console.WriteLine($"--- Running Day {dayNumber} ---");

        // 1. Locate the Input File
        var inputFileName = $"day{dayNumber:D2}.txt";
        var inputPath = Path.Combine(AppContext.BaseDirectory, "Inputs", inputFileName);

        if (!File.Exists(inputPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: Input file not found at '{inputPath}'.");
            Console.WriteLine(
                $"Please ensure 'Inputs/{inputFileName}' exists and is set to 'Copy to Output Directory'.");
            Console.ResetColor();
            return;
        }

        // 2. Instantiate the Solver
        ISolver solver;
        try 
        {
            solver = GetSolver(dayNumber);
        }
        catch (ArgumentException)
        {
             Console.WriteLine($"No solver found for Day {dayNumber}");
             return;
        }

        // 3. Run Parts
        RunPart("Part 1", () => solver.Part1(File.ReadAllLines(inputPath)));
        RunPart("Part 2", () => solver.Part2(File.ReadAllLines(inputPath)));
    }

    static void GenerateReadme()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("# Advent of Code 2025");
        sb.AppendLine();
        sb.AppendLine("Solutions for Advent of Code 2025 in C#.");
        sb.AppendLine();
        sb.AppendLine("## Runtimes");
        sb.AppendLine();
        sb.AppendLine("| Day | Part 1 | Part 2 | Total |");
        sb.AppendLine("| --- | --- | --- | --- |");

        for (int i = 1; i <= 12; i++)
        {
            try
            {
                var solver = GetSolver(i);
                var inputFileName = $"day{i:D2}.txt";
                var inputPath = Path.Combine(AppContext.BaseDirectory, "Inputs", inputFileName);
                
                if (!File.Exists(inputPath)) continue;

                var input = File.ReadAllLines(inputPath);

                Console.Write($"Running Day {i}... ");
                var (p1Time, p1Res) = Measure(() => solver.Part1(input));
                var (p2Time, p2Res) = Measure(() => solver.Part2(input));
                Console.WriteLine("Done.");

                var total = p1Time + p2Time;
                
                string FormatTime(double ms) => ms < 1 ? "<1ms" : $"{ms:F2}ms";

                sb.AppendLine($"| Day {i} | {FormatTime(p1Time)} | {FormatTime(p2Time)} | {FormatTime(total)} |");
            }
            catch (ArgumentException)
            {
                // No solver for this day
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Day {i}: {ex.Message}");
            }
        }

        var readmePath = Path.Combine(AppContext.BaseDirectory, "../../../../README.md");
        File.WriteAllText(readmePath, sb.ToString());
        Console.WriteLine($"README.md generated at {Path.GetFullPath(readmePath)}");
    }

    static (double ms, object result) Measure(Func<object> action)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = action();
            sw.Stop();
            return (sw.Elapsed.TotalMilliseconds, result);
        }
        catch
        {
            return (0, "Error");
        }
    }

    static ISolver GetSolver(int dayNumber) => dayNumber switch
    {
        1 => new Day01(),
        2 => new Day02(),
        3 => new Day03(),
        4 => new Day04(),
        5 => new Day05(),
        6 => new Day06(),
        7 => new Day07(),
        8 => new Day08(),
        9 => new Day09(),
        _ => throw new ArgumentException($"No solver found for Day {dayNumber}")
    };

    static void RunPart(string partName, Func<object> action)
    {
        Console.Write($"{partName}: ");
        var sw = Stopwatch.StartNew();
        try
        {
            var result = action();
            sw.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{result} ({sw.Elapsed.TotalMilliseconds:F4} ms)");
        }
        catch (NotImplementedException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Not Implemented");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error - {ex.Message}");
        }
        finally
        {
            Console.ResetColor();
        }
    }
}

// --- Core Interfaces & Classes ---

public interface ISolver
{
    object Part1(string[] input);
    object Part2(string[] input);
}