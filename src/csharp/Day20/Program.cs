using System.Drawing;
using System.Runtime.InteropServices;

namespace Day20;

internal static class Program {
    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private record struct BfsNode(Point Position, int Cost);

    internal static void Main(string[] args) {
        var (track, start, end) = GetInput();

        // PrintTrack(track, start, end);

        var count1 = Part1(track);
        var count2 = Part2(track);

        Console.WriteLine($"Part 1: {count1}");
        Console.WriteLine($"Part 2: {count2}");
    }

    private static (int[,] track, Point start, Point end) GetInput() {
        var input = File.ReadAllLines("Inputs.txt");
        var track = new int[input.Length, input[0].Length];

        var start = default(Point);
        var end = default(Point);
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '#') {
                    track[x, y] = -1;
                }
                else if (input[y][x] == 'S') {
                    start = new Point(x, y);
                }
                else if (input[y][x] == 'E') {
                    end = new Point(x, y);
                }
            }
        }

        var current = start;
        var picoseconds = 0;
        while (true) {
            track[current.X, current.Y] = picoseconds;
            picoseconds++;

            var newPoint = current with { Y = current.Y - 1 };
            if (newPoint != start && track[newPoint.X, newPoint.Y] == 0) {
                current = newPoint;
                continue;
            }

            newPoint = current with { X = current.X - 1 };
            if (newPoint != start && track[newPoint.X, newPoint.Y] == 0) {
                current = newPoint;
                continue;
            }

            newPoint = current with { X = current.X + 1 };
            if (newPoint != start && track[newPoint.X, newPoint.Y] == 0) {
                current = newPoint;
                continue;
            }

            newPoint = current with { Y = current.Y + 1 };
            if (newPoint != start && track[newPoint.X, newPoint.Y] == 0) {
                current = newPoint;
                continue;
            }

            break;
        }

        return (track, start, end);
    }

    private static int Part1(int[,] track) {
        var count = 0;

        var length0 = track.GetLength(0);
        var length1 = track.GetLength(1);
        for (var y = 0; y < length0; y++) {
            for (var x = 0; x < length1; x++) {
                var initialCost = track[x, y];
                if (initialCost == -1) {
                    continue;
                }

                // #
                // .
                if (track[x, y - 1] == -1) {
                    count += CountCheats1(track, new Point(x, y - 1), Direction.Up, initialCost);
                }

                // #.
                if (track[x - 1, y] == -1) {
                    count += CountCheats1(track, new Point(x - 1, y), Direction.Left, initialCost);
                }

                // .#
                if (track[x + 1, y] == -1) {
                    count += CountCheats1(track, new Point(x + 1, y), Direction.Right, initialCost);
                }

                // .
                // #
                if (track[x, y + 1] == -1) {
                    count += CountCheats1(track, new Point(x, y + 1), Direction.Down, initialCost);
                }
            }
        }

        return count;
    }

    private static int CountCheats1(int[,] track, Point point, Direction direction, int initialCost) {
        const int COST_THRESHOLD = 100;
        var cheats = 0;

        var (xDiff, yDiff) = direction switch {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        //  2
        //  1
        // #.#
        int cost;
        if ((cost = track.TryIndex(point.X + xDiff, point.Y + yDiff, -1)) != -1) {
            if (cost - 1 - initialCost >= COST_THRESHOLD) {
                cheats++;
                // PrintTrack(track, point, new Point(point.X + xDiff, point.Y + yDiff));
            }
        }

        (xDiff, yDiff) = direction switch {
            Direction.Up => (-1, 0),
            Direction.Down => (-1, 0),
            Direction.Left => (0, -1),
            Direction.Right => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        // 21
        // #.#
        if ((cost = track.TryIndex(point.X + xDiff, point.Y + yDiff, -1)) != -1) {
            if (cost - 1 - initialCost >= COST_THRESHOLD) {
                cheats++;
                // PrintTrack(track, point, new Point(point.X + xDiff, point.Y + yDiff));
            }
        }

        //  12
        // #.#
        if ((cost = track.TryIndex(point.X + xDiff * -1, point.Y + yDiff * -1, -1)) != -1) {
            if (cost - 1 - initialCost >= COST_THRESHOLD) {
                cheats++;
                // PrintTrack(track, point, new Point(point.X + xDiff * -1, point.Y + yDiff * -1));
            }
        }

        return cheats;
    }

    private static T TryIndex<T>(this T[,] arr, int x, int y, T defaultValue) {
        if (x < 0 || y < 0 || x >= arr.GetLength(0) || y >= arr.GetLength(1)) {
            return defaultValue;
        }

        return arr[x, y];
    }

    private static int Part2(int[,] track) {
        var cheats = new Dictionary<Point, HashSet<Point>>();

        var length0 = track.GetLength(0);
        var length1 = track.GetLength(1);
        for (var y = 0; y < length0; y++) {
            for (var x = 0; x < length1; x++) {
                var initialCost = track[x, y];
                if (initialCost == -1) {
                    continue;
                }

                var start = new Point(x, y);
                ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(cheats, start, out _);
                set ??= [];

                FindCheats2(track, start, initialCost, set);
            }
        }

        return cheats.Sum(x => x.Value.Count);
    }

    private static void FindCheats2(int[,] track, Point start, int initialCost, HashSet<Point> cheats) {
        const int COST_THRESHOLD = 100;
        const int CHEAT_LENGTH = 20;

        var visited = new HashSet<Point>();
        var queue = new Queue<BfsNode>();
        queue.Enqueue(new BfsNode(start, 0));

        while (queue.TryDequeue(out var node)) {
            foreach (var neighbor in GetNeighbors(node)) {
                if (neighbor.Cost > CHEAT_LENGTH) {
                    continue;
                }

                var cost = track.TryIndex(neighbor.Position.X, neighbor.Position.Y, -1);
                if (cost != -1) {
                    var deltaCost = cost - initialCost - (neighbor.Cost - 1);
                    if (deltaCost >= COST_THRESHOLD) {
                        if (cheats.Add(neighbor.Position)) {
                            // PrintTrack(track, start, neighbor.Position);
                        }
                    }
                }

                if (visited.Add(neighbor.Position)) {
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    private static IEnumerable<BfsNode> GetNeighbors(BfsNode node) {
        for (var y = -1; y < 2; y++)
        for (var x = -1; x < 2; x++) {
            // x | 0 | x
            // 0 | x | 0
            // x | 0 | x
            if (Math.Abs(x) == Math.Abs(y)) {
                continue;
            }

            var newPos = node.Position with { X = node.Position.X + x, Y = node.Position.Y + y };
            var newCost = node.Cost + 1;

            yield return new BfsNode(newPos, newCost);
        }
    }

    private static void PrintTrack(int[,] track, Point start, Point end) {
        var length0 = track.GetLength(0);
        var length1 = track.GetLength(1);
        for (var y = 0; y < length0; y++) {
            for (var x = 0; x < length1; x++) {
                var resetColor = false;
                if (start.X == x && start.Y == y) {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    resetColor = true;
                }
                else if (end.X == x && end.Y == y) {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    resetColor = true;
                }

                var val = track[x, y];
                if (val == -1) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('#');
                }
                else {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write((char)(val % 26 + 'a'));
                }

                if (resetColor) {
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }

        Console.ResetColor();
        Console.WriteLine();
    }
}