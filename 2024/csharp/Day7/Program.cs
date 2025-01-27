using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Day7;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt")
            .Select(ParseLine)
            .ToArray();

        var sum1 = 0UL;
        foreach (var (expected, parameters) in input) {
            sum1 += Part1(expected, CollectionsMarshal.AsSpan(parameters));
        }

        var sum2 = 0UL;
        foreach (var (expected, parameters) in input) {
            sum2 += Part2(expected, CollectionsMarshal.AsSpan(parameters));
        }

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static (ulong expected, List<ulong> parameters) ParseLine(string line) {
        var colonIndex = line.IndexOf(": ", StringComparison.Ordinal);
        Debug.Assert(colonIndex != -1);

        var expected = ulong.Parse(line.AsSpan(0, colonIndex));

        List<ulong> parameters = [];
        var span = line.AsSpan(colonIndex + 2);
        foreach (var range in span.Split(' ')) {
            var span2 = span[range];
            Debug.Assert(!span2.IsEmpty);
            parameters.Add(ulong.Parse(span2));
        }

        return (expected, parameters);
    }

    private static ulong Part1(ulong expected, ReadOnlySpan<ulong> parameters) {
        var maskMax = Math.ScaleB(1, parameters.Length);
        for (var mask = 0L; mask < maskMax; mask++) {
            var runningTotal = parameters[0];

            var workingMask = mask;
            for (var i = 1; i < parameters.Length; i++) {
                var param = parameters[i];

                if ((workingMask & 1) == 1) {
                    runningTotal *= param;
                }
                else {
                    runningTotal += param;
                }

                // Strangely, this makes it 5-10% slower
                // if (runningTotal > expected) {
                //     break;
                // }

                workingMask >>= 1;
            }

            if (runningTotal == expected) {
                return expected;
            }
        }

        return 0;
    }

    private static ulong Part2(ulong expected, ReadOnlySpan<ulong> parameters) {
        var maskMax = Math.Pow(3, parameters.Length);
        for (var mask = 0L; mask < maskMax; mask++) {
            var runningTotal = parameters[0];

            var workingMask = mask;
            for (var i = 1; i < parameters.Length; i++) {
                var param = parameters[i];

                switch (workingMask % 3) {
                    case 0:
                        runningTotal += param;
                        break;
                    case 1:
                        runningTotal *= param;
                        break;
                    case 2:
                        // For loop is faster than Math.Pow
                        var digits = CountDigits(param);
                        for (var j = 0; j < digits; j++) {
                            runningTotal *= 10;
                        }

                        runningTotal += param;
                        break;
                }

                if (runningTotal > expected) {
                    break;
                }

                workingMask /= 3;
            }

            if (runningTotal == expected) {
                return expected;
            }

            mask++;
        }

        return 0;
    }

    private static int CountDigits(ulong number) {
        Debug.Assert(number > 0);

        // Not supporting 0 is faster
        var digits = 0;
        while (number > 0) {
            digits++;
            number /= 10;
        }

        return digits;
    }
}