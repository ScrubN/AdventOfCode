namespace Day5;

internal static class Program {
    public readonly record struct Range(long Start, long End) {
        public long Count => End - Start + 1;

        public bool HasValue(long num) {
            return Start <= num && num <= End;
        }

        public bool TryCombine(Range other, out Range combined) {
            // S1       E1
            //    S2      E2
            if (Start <= other.Start) {
                // S1  E1
                //        S2  E2
                if (End < other.Start) {
                    combined = default;
                    return false;
                }

                combined = new Range(Start, Math.Max(End, other.End));
                return true;
            }

            //         S1    E1
            // S2   E2
            if (other.End < Start) {
                combined = default;
                return false;
            }

            //    S1    E1
            // S2    E2
            combined = new Range(other.Start, Math.Max(End, other.End));
            return true;
        }
    }

    public static void Main(string[] args) {
        var (ranges, ids) = GetIdRanges();

        var part1 = Part1(ranges, ids);
        var part2 = Part2(ranges);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
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

        var ranges2 = ranges;
        while (true) {
            var combined = CombineRanges(ranges2);
            if (combined.Count == ranges2.Count) {
                break;
            }

            ranges2 = combined;
        }

        return (ranges2, ids);
    }

    private static List<Range> CombineRanges(List<Range> source) {
        var newRanges = new List<Range>(source.Count / 2);

        foreach (var range in source) {
            for (var i = 0; i < newRanges.Count; i++) {
                if (newRanges[i].TryCombine(range, out var combined)) {
                    newRanges[i] = combined;
                    goto NextRange;
                }
            }

            newRanges.Add(range);

            NextRange: ;
        }

        return newRanges;
    }

    private static long Part1(List<Range> ranges, List<long> ids) {
        var count = 0L;

        foreach (var id in ids) {
            foreach (var range in ranges) {
                if (range.HasValue(id)) {
                    count++;
                    break;
                }
            }
        }

        return count;
    }

    private static long Part2(List<Range> ranges) {
        var sum = 0L;

        foreach (var range in ranges) {
            sum += range.Count;
        }

        return sum;
    }
}