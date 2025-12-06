using System.Diagnostics;

namespace Day6;

internal static class Program {
    public enum Operation {
        None,
        Add,
        Multiply
    }

    public static void Main(string[] args) {
        var worksheet = GetWorksheet();

        var part1 = Part1(worksheet);
        // var part2 = Part2(ranges);

        Console.WriteLine($"Part 1: {part1}");
        // Console.WriteLine($"Part 2: {part2}");
    }

    private static List<(List<int> numbers, Operation operation)> GetWorksheet() {
        var worksheet = new List<(List<int> numbers, Operation operation)>();

        var firstRun = true;
        var lines = File.ReadAllLines("Inputs.txt");
        foreach (var line in lines.SkipLast(1)) {
            var cols = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < cols.Length; i++) {
                if (firstRun) {
                    worksheet.Add(([], Operation.None));
                }

                worksheet[i].numbers.Add(int.Parse(cols[i]));
            }

            firstRun = false;
        }

        foreach (var line in lines.TakeLast(1)) {
            var cols = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < cols.Length; i++) {
                worksheet[i] = worksheet[i] with {
                    operation = cols[i][0] switch {
                        '+' => Operation.Add,
                        '*' => Operation.Multiply,
                        _ => throw new UnreachableException()
                    }
                };
            }
        }

        return worksheet;
    }

    private static long Part1(List<(List<int> numbers, Operation operation)> worksheet) {
        var sum = 0L;

        foreach (var (column, operation) in worksheet) {
            switch (operation)
            {
                case Operation.Add:
                    sum += column.Sum();
                    break;
                case Operation.Multiply:
                    sum += column.Select(x => (long)x).Aggregate((a, b) => a * b);
                    break;
                default:
                    throw new UnreachableException();
            }
        }


        return sum;
    }
}