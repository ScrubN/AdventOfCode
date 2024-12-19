using System.Diagnostics;

namespace Day2;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt")
            .Select(x => x.Split().Select(int.Parse).ToArray())
            .ToArray();

        // var safe1 = Part1(input);
        // var safe2 = Part2(input);
        var (safe1, safe2) = Part1And2(input);

        Console.WriteLine($"Part 1: {safe1}");
        Console.WriteLine($"Part 2: {safe2}");
    }

    private static int Part1(int[][] input) {
        var safe = 0;
        foreach (var numbers in input) {
            if (CheckSafety(numbers)) {
                safe++;
            }
        }

        return safe;
    }

    private static int Part2(int[][] input) {
        var safe = 0;
        foreach (var numbers in input) {
            if (CheckSafety(numbers)) {
                safe++;
                continue;
            }

            var span = new int[numbers.Length - 1].AsSpan();
            var numberSpan = numbers.AsSpan();
            for (var i = 0; i < numbers.Length; i++) {
                numberSpan[..i].CopyTo(span);
                numberSpan[(i + 1)..].CopyTo(span[i..]);

                if (CheckSafety(span)) {
                    safe++;
                    break;
                }
            }
        }

        return safe;
    }

    private static (int safe1, int safw2) Part1And2(int[][] input) {
        Span<int> stackSpace = stackalloc int[16];

        var safe1 = 0;
        var safe2 = 0;
        foreach (var numbers in input) {
            if (CheckSafety(numbers)) {
                safe1++;
                safe2++;
                continue;
            }

            Debug.Assert(numbers.Length - 1 <= stackSpace.Length);

            var span = stackSpace[..(numbers.Length - 1)];
            var numberSpan = numbers.AsSpan();
            for (var i = 0; i < numbers.Length; i++) {
                numberSpan[..i].CopyTo(span);
                numberSpan[(i + 1)..].CopyTo(span[i..]);

                if (CheckSafety(span)) {
                    safe2++;
                    break;
                }
            }
        }

        return (safe1, safe2);
    }

    private static bool CheckSafety(ReadOnlySpan<int> numbers) {
        var ascending = numbers[0] < numbers[1];
        for (var i = 0; i < numbers.Length - 1; i++) {
            var a = numbers[i];
            var b = numbers[i + 1];

            if (a == b) {
                return false;
            }

            if (ascending) {
                if (a > b) {
                    return false;
                }
            }
            else {
                if (a < b) {
                    return false;
                }
            }

            if (Math.Abs(a - b) > 3) {
                return false;
            }
        }

        return true;
    }
}