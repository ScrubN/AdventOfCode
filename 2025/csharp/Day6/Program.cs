using System.Diagnostics;

namespace Day6;

internal static class Program {
    extension(Range range) {
        private int Length => range.End.Value - range.Start.Value;
    }

    public enum Operation {
        None,
        Add,
        Multiply
    }

    public static void Main(string[] args) {
        var worksheetPart1 = GetWorksheetPart1();
        var part1 = SolveWorksheet(worksheetPart1);
        Console.WriteLine($"Part 1: {part1}");

        var worksheetPart2 = GetWorksheetPart2();
        var part2 = SolveWorksheet(worksheetPart2);
        Console.WriteLine($"Part 2: {part2}");
    }

    private static List<(List<int> numbers, Operation operation)> GetWorksheetPart1() {
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

    private static List<(List<int> numbers, Operation operation)> GetWorksheetPart2() {
        var lines = File.ReadAllLines("Inputs.txt");

        var ranges = new List<Range>();
        var start = 0;
        for (var i = 0; i < lines[0].Length; i++) {
            if (lines.Any(x => x[i] != ' ')) continue;

            ranges.Add(new Range(start, i));
            start = i + 1;
        }

        ranges.Add(new Range(start, lines[0].Length));


        var worksheet = new List<(List<int> numbers, Operation operation)>(ranges.Count);
        Span<char> numberScratch = stackalloc char[lines.Length - 1];
        foreach (var range in ranges) {
            List<int> numbers = [];
            for (var i = 0; i < range.Length; i++) {
                numberScratch.Fill(' ');
                for (var j = 0; j < lines.Length - 1; j++)
                {
                    numberScratch[j] = lines[j][range.Start.Value + i];
                }

                numbers.Add(int.Parse(numberScratch));
            }

            var operation = lines[^1].AsSpan(range).Trim()[0] switch {
                '+' => Operation.Add,
                '*' => Operation.Multiply,
                _ => throw new UnreachableException()
            };

            worksheet.Add((numbers, operation));
        }

        return worksheet;
    }

    private static long SolveWorksheet(List<(List<int> numbers, Operation operation)> worksheet) {
        var sum = 0L;

        foreach (var (column, operation) in worksheet) {
            switch (operation) {
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