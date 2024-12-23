using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

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
        var (track, start, end, path) = GetInput();

        // PrintTrack(track, start, end);

        var count1 = Part1(track, path);
        var count2 = Part2(track, path);

        Console.WriteLine($"Part 1: {count1}");
        Console.WriteLine($"Part 2: {count2}");
    }

    private static (short[,] track, Point start, Point end, List<Point> path) GetInput() {
        var input = File.ReadAllLines("Inputs.txt");
        var track = new short[input.Length, input[0].Length];

        var start = default(Point);
        var end = default(Point);
        for (var y = 0; y < input.Length; y++)
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

        var path = new List<Point>();
        var current = start;
        var picoseconds = (short)0;
        while (true) {
            path.Add(current);
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

        return (track, start, end, path);
    }

    private static int Part1(short[,] track, List<Point> path) {
        var count = 0;

        foreach (var point in path) {
            var x = point.X;
            var y = point.Y;
            var initialCost = track[x, y];

            // #
            // .
            if (track[x, y - 1] == -1) {
                count += CountCheats1(track, x, y - 1, Direction.Up, initialCost);
            }

            // #.
            if (track[x - 1, y] == -1) {
                count += CountCheats1(track, x - 1, y, Direction.Left, initialCost);
            }

            // .#
            if (track[x + 1, y] == -1) {
                count += CountCheats1(track, x + 1, y, Direction.Right, initialCost);
            }

            // .
            // #
            if (track[x, y + 1] == -1) {
                count += CountCheats1(track, x, y + 1, Direction.Down, initialCost);
            }
        }

        return count;
    }

    private static int CountCheats1(short[,] track, int pointX, int pointY, Direction direction, int initialCost) {
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
        if ((cost = track.TryIndex(pointX + xDiff, pointY + yDiff, (short)-1)) != -1) {
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
        if ((cost = track.TryIndex(pointX + xDiff, pointY + yDiff, (short)-1)) != -1) {
            if (cost - 1 - initialCost >= COST_THRESHOLD) {
                cheats++;
                // PrintTrack(track, point, new Point(point.X + xDiff, point.Y + yDiff));
            }
        }

        //  12
        // #.#
        if ((cost = track.TryIndex(pointX + xDiff * -1, pointY + yDiff * -1, (short)-1)) != -1) {
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

    private static int Part2(short[,] track, List<Point> path) {
        var sum = 0;

        foreach (var point in path) {
            var x = point.X;
            var y = point.Y;
            var initialCost = track[x, y];
            sum += FindCheats2Manhattan(track, x, y, initialCost);
        }

        return sum;
    }

    private static int FindCheats2Bfs(short[,] track, int startX, int startY, int initialCost) {
        const int COST_THRESHOLD = 100;
        const int CHEAT_LENGTH = 20;

        var cheats = new HashSet<long>();
        var visited = new HashSet<long>();
        var queue = new Queue<BfsNode>();
        queue.Enqueue(new BfsNode(new Point(startX, startY), 0));

        while (queue.TryDequeue(out var node)) {
            for (var y = -1; y < 2; y++)
            for (var x = -1; x < 2; x++) {
                // x | 0 | x
                // 0 | x | 0
                // x | 0 | x
                if (Math.Abs(x) == Math.Abs(y)) {
                    continue;
                }

                var newCost = node.Cost + 1;
                if (newCost > CHEAT_LENGTH) {
                    continue;
                }

                var newPos = new Point(node.Position.X + x, node.Position.Y + y);
                var cost = track.TryIndex(newPos.X, newPos.Y, (short)-1);
                if (cost != -1) {
                    var deltaCost = cost - initialCost - (newCost - 1);
                    if (deltaCost >= COST_THRESHOLD) {
                        if (cheats.Add(Unsafe.As<Point, long>(ref newPos))) {
                            // PrintTrack(track, start, newPos);
                        }
                    }
                }

                if (visited.Add(Unsafe.As<Point, long>(ref newPos))) {
                    queue.Enqueue(new BfsNode(newPos, newCost));
                }
            }
        }

        return cheats.Count;
    }

    private static int FindCheats2Manhattan(short[,] track, int startX, int startY, int initialCost) {
        const int COST_THRESHOLD = 100;
        const int CHEAT_LENGTH = 20;

        var cheatCount = 0;

        var maxY = track.GetLength(0);
        var maxX = track.GetLength(1);
        for (var y = -CHEAT_LENGTH; y <= CHEAT_LENGTH; y++) {
            var pointY = startY + y;
            if (pointY < 0 || pointY >= maxY) {
                continue;
            }

            var manhattanY = Math.Abs(y);

            var deltaX = CHEAT_LENGTH - manhattanY;
            for (var x = -deltaX; x <= deltaX; x++) {
                var pointX = startX + x;
                if (pointX < 0 || pointX >= maxX) {
                    continue;
                }

                // 5-10% faster to omit this
                // if (pointX == startX && pointY == startY) {
                //     continue;
                // }

                var cost = track[pointX, pointY];
                if (cost == -1) {
                    continue;
                }

                var manhattanDistance = Math.Abs(x) + manhattanY;
                Debug.Assert(manhattanDistance <= CHEAT_LENGTH);

                var deltaCost = cost - initialCost - manhattanDistance;
                if (deltaCost >= COST_THRESHOLD) {
                    cheatCount++;
                    // PrintTrack(track, new Point(startX, startY), new Point(pointX, pointY));
                }
            }
        }

        return cheatCount;
    }

    private static void PrintTrack(short[,] track, Point start, Point end) {
        var maxY = track.GetLength(0);
        var maxX = track.GetLength(1);
        for (var y = 0; y < maxY; y++) {
            for (var x = 0; x < maxX; x++) {
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