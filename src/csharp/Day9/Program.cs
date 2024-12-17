namespace Day9;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt").Select(ToInt).ToArray();

        var disk1 = InitializeDisk(input);
        var disk2 = disk1.ToList();

        Part1(disk1);
        Part2(disk2);

        var checksum1 = ComputeChecksum(disk1);
        var checksum2 = ComputeChecksum(disk2);

        Console.WriteLine($"Part 1: {checksum1}");
        Console.WriteLine($"Part 2: {checksum2}");
    }

    private static int ToInt(char arg) {
        if (arg is >= '0' and <= '9') {
            return arg - '0';
        }

        throw new ArgumentException(null, nameof(arg));
    }

    private static List<int> InitializeDisk(int[] input) {
        var disk = new List<int>(input.Length);
        for (var i = 0; i < input.Length; i++) {
            var id = i % 2 == 0
                ? i / 2
                : -1;

            for (var j = 0; j < input[i]; j++) {
                disk.Add(id);
            }
        }

        return disk;
    }

    private static void Part1(List<int> disk) {
        var first = 0;
        for (var last = disk.Count - 1; last > first; last--) {
            var id = disk[last];
            if (id == -1) {
                continue;
            }

            for (; first < last; first++) {
                if (disk[first] != -1) {
                    continue;
                }

                disk[first] = id;
                disk[last] = -1;
                break;
            }
        }
    }

    private static void Part2(List<int> disk) {
        var firstEmpty = 0;
        for (var last = disk.Count - 1; last > firstEmpty; last--) {
            var id = disk[last];
            if (id == -1) {
                continue;
            }

            // Compute file size
            var fileSize = 1;
            while (last - fileSize >= 0
                   && disk[last - fileSize] == id) {
                fileSize++;
            }

            // Search for chunk to move to
            for (var first = firstEmpty; first < last; first++) {
                if (disk[first] != -1) {
                    continue;
                }

                // Compute chunk size
                var space = 1;
                while (space < fileSize
                       && first + space < last
                       && disk[first + space] == -1) {
                    space++;
                }

                if (space < fileSize) {
                    first += space - 1;
                    continue;
                }

                // Move file
                for (var i = 0; i < fileSize; i++) {
                    disk[first + i] = disk[last - i];
                    disk[last - i] = -1;
                }

                firstEmpty = disk.IndexOf(-1, firstEmpty);
                break;
            }

            last -= fileSize - 1;
        }
    }

    private static long ComputeChecksum(List<int> disk) {
        var checksum = 0L;
        for (var i = 0; i < disk.Count; i++) {
            var item = disk[i];
            if (item == -1) {
                continue;
            }

            checksum += item * i;
        }

        return checksum;
    }
}