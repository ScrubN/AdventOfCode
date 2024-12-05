using System.Runtime.InteropServices;

namespace Day5;

internal static class Program {
    internal static void Main(string[] args) {
        var (pageOrdering, manuals) = GetInput();
        List<byte[]> validManuals = [];

        foreach (var pageNumbers in manuals) {
            var numbers = new HashSet<byte>(pageNumbers);
            var orderingSlice = pageOrdering
                .Where(x => numbers.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            var isValid = true;
            HashSet<byte> validated = [];
            foreach (var number in pageNumbers) {
                if (orderingSlice
                    .Where(x => !validated.Contains(x.Key))
                    .All(x => !x.Value.Contains(number))) {
                    validated.Add(number);
                }
                else {
                    isValid = false;
                    break;
                }
            }

            if (isValid) {
                validManuals.Add(pageNumbers);
            }
        }

        var sum = 0;
        foreach (var manual in validManuals) {
            sum += manual[manual.Length / 2];
        }

        Console.WriteLine(sum);
    }

    private static (Dictionary<byte, HashSet<byte>> pageOrdering, List<byte[]> manuals) GetInput() {
        using var reader = new StreamReader("Inputs.txt");

        var pageOrdering = new Dictionary<byte, HashSet<byte>>();
        while (reader.ReadLine() is { } line) {
            if (line.Length == 0) {
                break;
            }

            var a = byte.Parse(line.AsSpan(0, line.IndexOf('|')));
            var b = byte.Parse(line.AsSpan(line.IndexOf('|') + 1));
            ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(pageOrdering, a, out _);
            set ??= [];
            set.Add(b);
        }

        var manuals = new List<byte[]>();
        while (reader.ReadLine() is { } line) {
            manuals.Add(line.Split(',').Select(byte.Parse).ToArray());
        }

        return (pageOrdering, manuals);
    }
}