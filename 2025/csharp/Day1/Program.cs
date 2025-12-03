using System.Diagnostics;

namespace Day1;

internal static class Program {
    public enum Direction {
        Left,
        Right
    }

    public record struct Turn(Direction Direction, int Count);

    public static void Main(string[] args) {
        var turns = GetTurns();

        var part1 = CountZeroes(turns);
        var part2 = CountZeroClicks(turns);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static List<Turn> GetTurns() {
        var turns = new List<Turn>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            turns.Add(new Turn(
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

    public static int CountZeroes(List<Turn> turns) {
        var zeroes = 0;

        var dial = 50;
        foreach (var (direction, count) in turns) {
            dial +=
                direction == Direction.Right
                    ? count
                    : -count;

            while (dial > 99) {
                dial -= 100;
            }

            while (dial < 0) {
                dial += 100;
            }

            if (dial == 0) {
                zeroes++;
            }
        }

        return zeroes;
    }

    public static int CountZeroClicks(List<Turn> turns) {
        var zeroes = 0;

        var dial = 50;
        foreach (var (direction, count) in turns) {
            for (var i = 0; i < count; i++) {
                var turn =
                    direction == Direction.Right
                        ? 1
                        : -1;

                dial += turn;

                if (dial > 99) {
                    dial -= 100;
                }
                else if (dial < 0) {
                    dial += 100;
                }

                if (dial == 0) {
                    zeroes++;
                }
            }
        }

        return zeroes;
    }
}