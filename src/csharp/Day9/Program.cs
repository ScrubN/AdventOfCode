namespace Day9;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt").Select(x => int.Parse(x.ToString())).ToArray();

        var disk = new List<int>();
        for (var i = 0; i < input.Length; i++) {
            var id = i % 2 == 0
                ? i / 2
                : -1;

            for (var j = 0; j < input[i]; j++) {
                disk.Add(id);
            }
        }

        for (var i = disk.Count - 1; i >= 0; i--) {
            if (disk[i] == -1) {
                continue;
            }

            var fileSize = 1;
            while (i - fileSize >= 0
                   && disk[i - fileSize] == disk[i]) {
                fileSize++;
            }

            for (var j = 0; j < disk.Count; j++) {
                if (j >= i) {
                    i -= fileSize - 1;
                    break;
                }

                if (disk[j] != -1) {
                    continue;
                }

                var space = 1;
                while (space < fileSize
                       && j + space < i
                       && disk[j + space] == -1) {
                    space++;
                }

                if (space < fileSize) {
                    continue;
                }

                for (var h = 0; h < fileSize; h++) {
                    disk[j + h] = disk[i - h];
                    disk[i - h] = -1;
                }
            }
        }

        var checksum = ComputeChecksum(disk);

        Console.WriteLine(checksum);
    }

    private static long ComputeChecksum(IEnumerable<int> disk) {
        var checksum = 0L;
        foreach (var (i, item) in disk.Index().Where(x => x.Item != -1)) {
            var val = item * i;
            checksum += val;
        }

        return checksum;
    }
}