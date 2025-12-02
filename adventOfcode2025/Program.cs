using System.Diagnostics;

namespace adventOfcode2025;

class Program
{
    static void Main(string[] args)
    {
        // CHANGE THIS TO RUN A DIFFERENT DAY
        var dayNumber = 1;

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

        var input = File.ReadAllLines(inputPath);

        // 2. Instantiate the Solver
        ISolver solver = dayNumber switch
        {
            1 => new Day01(),
            // Add cases for new days here:
            // 2 => new Day02(),
            _ => throw new ArgumentException($"No solver found for Day {dayNumber}")
        };

        // 3. Run Parts
        RunPart("Part 1", () => solver.Part1(input));
        RunPart("Part 2", () => solver.Part2(input));
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

public class Day01 : ISolver
{
    private class Dial
    {
        private int _position = 0;
        private int _count = 0;
        public void setCurrentPosition(int position) => _position = position;
        public void incrementCount() => _count++;
        public int getCurrentPosition() => _position;
        public int getCount() => _count;
        public int turnRight(int v) => (_position + v) % 100;
        public int turnLeft(int v) => (_position - v + 100) % 100;
    }

    public object Part1(string[] input)
    {
        Dial lockeDial = new();
        lockeDial.setCurrentPosition(50);

        foreach (string line in input)
        {
            if (line.StartsWith("L")) lockeDial.setCurrentPosition(lockeDial.turnLeft(int.Parse(line.Substring(1))));
            else
            {
                lockeDial.setCurrentPosition(lockeDial.turnRight(int.Parse(line.Substring(1))));
            }

            if (lockeDial.getCurrentPosition() == 0) lockeDial.incrementCount();
        }

        //Console.WriteLine(lockeDial.getCount());
        return lockeDial.getCount();
    }

    private class Dial2
    {
        private int _position = 0;
        private int _count = 0;
        public void setCurrentPosition(int position) => _position = position;
        private void incrementCount(int v) => _count += v;
        private int getCurrentPosition() => _position;
        public int getCount() => _count;
        private int turnRight(int v) => (_position + v) % 100;
        private int turnLeft(int v) => (_position - v + 100) % 100;

        public void CheckTurn(char turn, int v)
        {
            int old = getCurrentPosition();
            int numberOfFullRot = v / 100;
            incrementCount(numberOfFullRot);
            v -= numberOfFullRot * 100;
            if (v == 0) return;
            if (turn == 'R')
            {
                setCurrentPosition(turnRight(v));
                if (old > getCurrentPosition()|| getCurrentPosition() == 0)
                {
                    incrementCount(1);
                }
            }
            else
            {
                setCurrentPosition(turnLeft(v));
                if (old!=0&&(old < getCurrentPosition()|| getCurrentPosition() == 0))
                {
                    incrementCount(1);
                }
            }

            return;
        }
    }

    public object Part2(string[] input)
    {

        Dial2 lockeDial = new();
        lockeDial.setCurrentPosition(50);

        foreach (var line in input)
        {
            lockeDial.CheckTurn(line[0], int.Parse(line.Substring(1)));
        }

        // Console.WriteLine(lockeDial.getCount());
        return lockeDial.getCount();
    }
}