using System.Text.RegularExpressions;

namespace Day3;

internal static partial class Program {
    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex Part1Regex { get; }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")]
    private static partial Regex Part2Regex { get; }

    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt");

        var sum1 = Part1(input);
        var sum2 = Part2(input);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static int Part1(string input) {
        var sum = 0;
        foreach (Match match in Part1Regex.Matches(input)) {
            var a = int.Parse(match.Groups[1].ValueSpan);
            var b = int.Parse(match.Groups[2].ValueSpan);
            sum += a * b;
        }

        return sum;
    }

    private static int Part2(string input) {
        var sum = 0;
        var enabled = true;
        foreach (Match match in Part2Regex.Matches(input)) {
            if (match.ValueSpan.StartsWith("don't()")) {
                enabled = false;
                continue;
            }

            if (match.ValueSpan.StartsWith("do()")) {
                enabled = true;
                continue;
            }

            if (!enabled) {
                continue;
            }

            var a = int.Parse(match.Groups[1].ValueSpan);
            var b = int.Parse(match.Groups[2].ValueSpan);
            sum += a * b;
        }

        return sum;
    }
}