using System.Text.RegularExpressions;

namespace Day3;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt");

        var sum = 0;
        foreach (var match in Regex.EnumerateMatches(input, @"(?<=mul\()\d+,\d+(?=\))")) {
            var text = input.Substring(match.Index, match.Length);

            var a = int.Parse(text.AsSpan(0, text.IndexOf(',')));
            var b = int.Parse(text.AsSpan(text.IndexOf(',') + 1));
            sum += a * b;
        }

        Console.WriteLine(sum);
    }
}