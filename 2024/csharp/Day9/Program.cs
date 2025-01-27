using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Day9;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt").Select(x => x - '0').ToArray();
        Debug.Assert(input.All(x => x is >= 0 and <= 9));

        var disk1 = InitializeDisk(input);
        var disk2 = disk1.ToArray();

        Part1(CollectionsMarshal.AsSpan(disk1));
        Part2(disk2);

        var checksum1 = ComputeChecksum(CollectionsMarshal.AsSpan(disk1));
        var checksum2 = ComputeChecksum(disk2);

        Console.WriteLine($"Part 1: {checksum1}");
        Console.WriteLine($"Part 2: {checksum2}");
    }

    private static List<int> InitializeDisk(int[] input) {
        const int EXPECTED_VALUE = (int)4.5; // Assuming an even distribution of 0-9, the expected value is 4.5
        var disk = new List<int>(input.Length * EXPECTED_VALUE);
        for (var i = 0; i < input.Length; i++) {
            var id = i % 2 == 0
                ? i / 2
                : -1;

			var count = input[i];
            for (var j = 0; j < count; j++) {
                disk.Add(id);
            }
        }

        return disk;
    }

    private static void Part1(Span<int> disk) {
        var first = 0;
        for (var last = disk.Length - 1; last > first; last--) {
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

    private static void Part2(Span<int> disk) {
        var firstEmpty = 0;
        for (var last = disk.Length - 1; last > firstEmpty; last--) {
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

                firstEmpty = disk[firstEmpty..].IndexOf(-1) + firstEmpty;
                break;
            }

            last -= fileSize - 1;
        }
    }

    private static long ComputeChecksum(ReadOnlySpan<int> disk) {
        var checksum = 0L;
        for (var i = 0; i < disk.Length; i++) {
            var item = disk[i];
            if (item == -1) {
                continue;
            }

            checksum += item * i;
        }

        return checksum;
    }
}