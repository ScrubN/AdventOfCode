using System.Buffers;
using System.Text.RegularExpressions;

namespace Day3;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt");

        var searchValues = SearchValues.Create(["mul(", "do()", "don't()"], StringComparison.Ordinal);

        long sum = 0;
        var @do = true;
        int index;
        while ((index = input.AsSpan().IndexOfAny(searchValues)) != -1) {
            input = input[index..];
            if (input.StartsWith("don't()")) {
                @do = false;
                input = input[7..];
                continue;
            }

            if (input.StartsWith("do()")) {
                @do = true;
                input = input[4..];
                continue;
            }

            var match = Regex.Match(input, @"^mul\((\d+),(\d+)\)");
            if (@do && match.Success) {
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                sum += a * b;

                input = input[match.Length..];
                continue;
            }

            input = input[1..];
        }

        Console.WriteLine(sum);
    }
}