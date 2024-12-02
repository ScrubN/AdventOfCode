namespace Day1;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var (ids1, ids2) = ReadIds();

        var sum = 0;
        for (var i = 0; i < ids1.Count; i++)
        {
            sum += Math.Abs(ids1[i] - ids2[i]);
        }

        Console.WriteLine(sum);
    }

    private static (IReadOnlyList<int>, IReadOnlyList<int>) ReadIds() {
        using var fs = File.OpenRead("Inputs.txt");
        using var sr = new StreamReader(fs);

        var ids1 = new List<int>();
        var ids2 = new List<int>();

        while (sr.ReadLine() is { } line)
        {
            ids1.Add(int.Parse(line.AsSpan(0, line.IndexOf(' '))));
            ids2.Add(int.Parse(line.AsSpan(line.LastIndexOf(' '))));
        }

        ids1.Sort();
        ids2.Sort();

        return (ids1, ids2);
    }
}