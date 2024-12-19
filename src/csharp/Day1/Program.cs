using System.Runtime.InteropServices;

namespace Day1;

internal static class Program {
    internal static void Main(string[] args) {
        var (ids1, ids2) = GetIds();

        var occurrences = new Dictionary<int, int>();
        foreach (var id in ids2) {
            CollectionsMarshal.GetValueRefOrAddDefault(occurrences, id, out _)++;
        }

        var (distance, similarity) = GetIdMetrics(CollectionsMarshal.AsSpan(ids1), CollectionsMarshal.AsSpan(ids2), occurrences);

        Console.WriteLine($"Part 1: {distance}");
        Console.WriteLine($"Part 2: {similarity}");
    }

    private static (List<int>, List<int>) GetIds() {
        var ids1 = new List<int>();
        var ids2 = new List<int>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            ids1.Add(int.Parse(line.AsSpan(0, line.IndexOf(' '))));
            ids2.Add(int.Parse(line.AsSpan(line.LastIndexOf(' '))));
        }

        ids1.Sort();
        ids2.Sort();

        return (ids1, ids2);
    }

    private static (int distance, int similarity) GetIdMetrics(ReadOnlySpan<int> ids1, ReadOnlySpan<int> ids2, Dictionary<int, int> occurrences) {
        var distance = 0;
        var similarity = 0;
        for (var i = 0; i < ids1.Length; i++) {
            var id1 = ids1[i];
            var id2 = ids2[i];

            distance += Math.Abs(id1 - id2);
            if (occurrences.TryGetValue(id1, out var value)) {
                similarity += id1 * value;
            }
        }

        return (distance, similarity);
    }
}