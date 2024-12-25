namespace Day25;

internal static class Program {
    internal static void Main(string[] args) {
        var (locks, keys) = GetInput();

        var count1 = Part1(locks, keys);

        Console.WriteLine($"Part 1: {count1}");
    }

    private static (List<int[]> locks, List<int[]> keys) GetInput() {
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        using var sr = new StreamReader("Inputs.txt");
        int[]? current = null;
        bool[]? taken = null;
        var isLock = true;
        var height = 0;
        while (sr.ReadLine() is { } line) {
            if (line.Length == 0 && current != null) {
                if (isLock) {
                    locks.Add(current);
                }
                else {
                    keys.Add(current);
                }

                current = null;
                height = 0;
                continue;
            }

            if (current == null) {
                current = new int[line.Length];
                taken = new bool[line.Length];
                isLock = true;
                if (line.Any(x => x != '#')) {
                    isLock = false;
                    height = 5;
                }

                continue;
            }

            for (var i = 0; i < line.Length; i++) {
                if (taken![i]) {
                    continue;
                }

                if (!isLock) {
                    if (line[i] != '#') {
                        continue;
                    }
                }
                else {
                    if (line[i] != '.') {
                        continue;
                    }
                }

                current[i] = height;
                taken[i] = true;
            }

            if (isLock) {
                height++;
            }
            else {
                height--;
            }
        }
        
        if (isLock) {
            locks.Add(current ?? throw new InvalidOperationException());
        }
        else {
            keys.Add(current ?? throw new InvalidOperationException());
        }

        return (locks, keys);
    }

    private static int Part1(List<int[]> locks, List<int[]> keys) {
        var count = 0;

        foreach (var key in keys) {
            foreach (var @lock in locks) {
                var valid = true;
                for (var i = 0; i < key.Length; i++) {
                    if (key[i] + @lock[i] <= 5) {
                        continue;
                    }

                    valid = false;
                    break;
                }

                if (valid) {
                    count++;
                }
            }
        }

        return count;
    }
}