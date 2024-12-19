using System.Text.RegularExpressions;

namespace Day3;

internal static partial class Program {
    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex Part1Regex { get; }

    [GeneratedRegex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)")]
    private static partial Regex Part2Regex { get; }

    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt");

        // var sum1 = Part1(input);
        // var sum2 = Part2(input);
        var (sum1, sum2) = Part1And2(input);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static int Part1(string input) {
        var sum = 0;
        foreach (var match in Part1Regex.EnumerateMatches(input)) {
            var span = input.AsSpan(match.Index, match.Length);

            var separator = span.IndexOf(',');
            var a = int.Parse(span[4..separator]);
            var b = int.Parse(span[(separator + 1)..^1]);
            sum += a * b;
        }

        return sum;
    }

    private static int Part2(string input) {
        var sum = 0;
        var enabled = true;
        foreach (var match in Part2Regex.EnumerateMatches(input)) {
            var span = input.AsSpan(match.Index, match.Length);
            switch (span) {
                case "don't()":
                    enabled = false;
                    continue;
                case "do()":
                    enabled = true;
                    continue;
            }

            if (!enabled) {
                continue;
            }

            var separator = span.IndexOf(',');
            var a = int.Parse(span[4..separator]);
            var b = int.Parse(span[(separator + 1)..^1]);
            sum += a * b;
        }

        return sum;
    }

    private static (int sum1, int sum2) Part1And2(string input) {
        var sum1 = 0;
        var sum2 = 0;
        var enabled = true;
        foreach (var match in Part2Regex.EnumerateMatches(input)) {
            var span = input.AsSpan(match.Index, match.Length);
            switch (span) {
                case "don't()":
                    enabled = false;
                    continue;
                case "do()":
                    enabled = true;
                    continue;
            }

            var separator = span.IndexOf(',');
            var a = int.Parse(span[4..separator]);
            var b = int.Parse(span[(separator + 1)..^1]);

            sum1 += a * b;
            if (enabled) {
                sum2 += a * b;
            }
        }

        return (sum1, sum2);
    }
}