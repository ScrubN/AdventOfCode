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

            for (var j = 0; j < disk.Count; j++) {
                if (disk[j] != -1) {
                    continue;
                }

                disk[j] = disk[i];
                disk[i] = -1;
            }

            var done = true;
            foreach (var item in disk.SkipWhile(x => x != -1)) {
                if (item != -1) {
                    done = false;
                }
            }

            if (done) {
                break;
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