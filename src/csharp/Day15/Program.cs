using System.Drawing;

namespace Day15;

internal static class Program {
    private record Grid(HashSet<Point> Boxes, HashSet<Point> Walls);

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    internal static void Main(string[] args) {
        var (grid, robot, directions) = GetInput();

        var sum = Part1(grid, robot, directions);

        Console.WriteLine(sum);
    }

    private static (Grid grid, Point robot, List<Direction> directions) GetInput() {
        using var sr = new StreamReader("Inputs.txt");

        HashSet<Point> boxes = [];
        HashSet<Point> walls = [];
        Point robot = default;

        var y = 0;
        while (sr.ReadLine() is { } line) {
            if (string.IsNullOrWhiteSpace(line)) {
                break;
            }

            for (var x = 0; x < line.Length; x++) {
                if (line[x] == '.') {
                    continue;
                }

                switch (line[x]) {
                    case '@':
                        robot = new Point(x, y);
                        continue;
                    case 'O':
                        boxes.Add(new Point(x, y));
                        continue;
                    case '#':
                        walls.Add(new Point(x, y));
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException($"Got char that could not be handled: {line[x]}");
                }
            }

            y++;
        }

        var directions = new List<Direction>();
        while (sr.ReadLine() is { } line) {
            foreach (var c in line) {
                var direction = c switch {
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    '>' => Direction.Right,
                    _ => throw new ArgumentOutOfRangeException()
                };

                directions.Add(direction);
            }
        }

        return (new Grid(boxes, walls), robot, directions);
    }

    private static int Part1(Grid grid, Point robot, List<Direction> directions) {
        foreach (var direction in directions) {
            MoveRobot(grid, ref robot, direction);
        }

        return SumGpsCoordinates(grid);
    }

    private static int SumGpsCoordinates(Grid grid) {
        var sum = 0;
        foreach (var box in grid.Boxes) {
            sum += box.X + box.Y * 100;
        }

        return sum;
    }

    private static void MoveRobot(Grid grid, ref Point robot, Direction direction) {
        var (xDiff, yDiff) = direction switch {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var newPos = robot with { X = robot.X + xDiff, Y = robot.Y + yDiff };

        if (grid.Walls.Contains(newPos)) {
            return;
        }

        if (grid.Boxes.Contains(newPos)) {
            var boxPos = newPos;
            do {
                boxPos = newPos with { X = boxPos.X + xDiff, Y = boxPos.Y + yDiff };
                if (!grid.Boxes.Contains(boxPos)) {
                    if (grid.Walls.Contains(boxPos)) {
                        return;
                    }

                    grid.Boxes.Add(boxPos);
                    grid.Boxes.Remove(newPos);
                    break;
                }

            } while (true);
        }

        robot = newPos;
    }
}