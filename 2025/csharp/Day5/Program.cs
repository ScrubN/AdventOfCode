namespace Day5;

internal static class Program {
    public readonly record struct Range(long Start, long End) {
        public bool IsInRange(long num) {
            return Start <= num && num <= End;
        }
    }

    public static void Main(string[] args) {
        var (ranges, ids) = GetIdRanges();

        var part1 = Part1(ranges, ids);
        // var part2 = Part2(ranges, ids);

        Console.WriteLine($"Part 1: {part1}");
        // Console.WriteLine($"Part 2: {part2}");
    }

    private static (List<Range> idRanges, List<long> ids) GetIdRanges() {
        var ranges = new List<Range>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line && !string.IsNullOrEmpty(line)) {
            var separator = line.IndexOf('-');

            var start = long.Parse(line[..separator]);
            var end = long.Parse(line[(separator + 1)..]);

            ranges.Add(new Range(start, end));
        }

        var ids = new List<long>();
        while (sr.ReadLine() is { } line) {
            ids.Add(long.Parse(line));
        }

        return (ranges, ids);
    }

    private static long Part1(List<Range> ranges, List<long> ids) {
        var count = 0L;

        foreach (var id in ids) {
            foreach (var range in ranges) {
                if (range.IsInRange(id)) {
                    count++;
                    break;
                }
            }
        }

        return count;
    }
}