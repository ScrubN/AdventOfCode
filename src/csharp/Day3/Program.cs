using System.Text.RegularExpressions;

namespace Day3;

internal static partial class Program {
    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")]
    private static partial Regex MulRegex();

    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt");

        var sum = 0;
        var enabled = true;
        foreach (Match match in MulRegex().Matches(input)) {
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

        Console.WriteLine(sum);
    }
}