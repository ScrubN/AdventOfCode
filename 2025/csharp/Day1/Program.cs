using System.Diagnostics;

namespace Day1;

internal static class Program {
    public enum Direction {
        Left,
        Right
    }

    public static void Main(string[] args) {
        var data = GetTurns();

        var part1 = CountZeroes(data);
        var part2 = CountZeroClicks(data);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static List<(Direction, int)> GetTurns() {
        var turns = new List<(Direction, int)>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            turns.Add((
                line[0] switch {
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    _ => throw new UnreachableException()
                },
                int.Parse(line.AsSpan(1))
            ));
        }

        return turns;
    }

    public static int CountZeroes(List<(Direction, int)> data) {
        var zeroes = 0;

        var dial = 50;
        foreach (var (direction, count) in data)
        {
            dial +=
                direction == Direction.Right
                ? count
                : -count;

            while (dial > 99)
                dial -= 100;

            while (dial < 0)
                dial += 100;

            if (dial == 0) {
                zeroes++;
            }
        }

        return zeroes;
    }

    public static int CountZeroClicks(List<(Direction, int)> data) {
        var zeroes = 0;

        var dial = 50;
        foreach (var (direction, count) in data) {
            for (var i = 0; i < count; i++) {
                var turn = direction == Direction.Right
                    ? 1
                    : -1;

                dial += turn;

                while (dial > 99)
                    dial -= 100;

                while (dial < 0)
                    dial += 100;

                if (dial == 0) {
                    zeroes++;
                }
            }
        }

        return zeroes;
    }
}