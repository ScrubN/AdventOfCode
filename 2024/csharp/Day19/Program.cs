using System.Text.RegularExpressions;

namespace Day19;

internal static class Program {
    internal static void Main(string[] args) {
        AppContext.SetData("REGEX_NONBACKTRACKING_MAX_AUTOMATA_SIZE", 99999);

        var (patterns, designs) = GetInput();

        var sum = Part1(patterns, designs);

        Console.WriteLine(sum);
    }

    private static (string[] patterns, string[] designs) GetInput() {
        var lines = File.ReadAllLines("Inputs.txt");

        var patterns = lines[0].Split(",", StringSplitOptions.TrimEntries);
        var designs = lines.Skip(2).ToArray();

        return (patterns, designs);
    }

    private static long Part1(string[] patterns, string[] designs) {
        var sum = 0L;

        var regexStr = $"^(?:{string.Join('|', patterns)})+$";
        var patternRegex = new Regex(regexStr, RegexOptions.Compiled | RegexOptions.NonBacktracking);
        foreach (var design in designs) {
            if (patternRegex.IsMatch(design)) {
                sum++;
            }
        }

        return sum;
    }
}