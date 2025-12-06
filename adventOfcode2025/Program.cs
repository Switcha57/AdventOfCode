using System.Diagnostics;

namespace adventOfcode2025;

class Program
{
    static void Main(string[] args)
    {
        // CHANGE THIS TO RUN A DIFFERENT DAY
        var dayNumber = 6;

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
        ISolver solver = dayNumber switch
        {
            1 => new Day01(),
            // Add cases for new days here:
            2 => new Day02(),
            3 => new Day03(),
            4 => new Day04(),
            5 => new Day05(),
            
            6 => new Day06(),
            _ => throw new ArgumentException($"No solver found for Day {dayNumber}")
        };

        // 3. Run Parts
        RunPart("Part 1", () => solver.Part1(File.ReadAllLines(inputPath)));
        RunPart("Part 2", () => solver.Part2(File.ReadAllLines(inputPath)));
    }

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