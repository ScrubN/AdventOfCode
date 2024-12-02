using System.Runtime.InteropServices;

namespace Day1;

internal static class Program {
    internal static void Main(string[] args) {
        var (ids1, ids2) = ReadIds();

        var occurrences = new Dictionary<int, int>();
        foreach (var id in ids2) {
            CollectionsMarshal.GetValueRefOrAddDefault(occurrences, id, out _)++;
        }

        var sum = 0;
        var similarity = 0;
        for (var i = 0; i < ids1.Count; i++) {
            var id1 = ids1[i];
            var id2 = ids2[i];

            sum += Math.Abs(id1 - id2);
            if (occurrences.TryGetValue(id1, out var value)) {
                similarity += id1 * value;
            }
        }

        Console.WriteLine($"Sum: {sum} Similarity: {similarity}");
    }

    private static (IReadOnlyList<int>, IReadOnlyList<int>) ReadIds() {
        using var fs = File.OpenRead("Inputs.txt");
        using var sr = new StreamReader(fs);

        var ids1 = new List<int>();
        var ids2 = new List<int>();

        while (sr.ReadLine() is { } line) {
            ids1.Add(int.Parse(line.AsSpan(0, line.IndexOf(' '))));
            ids2.Add(int.Parse(line.AsSpan(line.LastIndexOf(' '))));
        }

        ids1.Sort();
        ids2.Sort();

        return (ids1, ids2);
    }
}