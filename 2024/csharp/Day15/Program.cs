using System.Diagnostics;
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

        var sum1 = Part1(grid, robot, directions);
        var sum2 = Part2(grid, robot, directions);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
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
        var grid2 = grid with { Boxes = grid.Boxes.ToHashSet() };

        foreach (var direction in directions) {
            MoveRobot1(grid2, ref robot, direction);
        }

        return SumGpsCoordinates(grid2);
    }

    private static void MoveRobot1(Grid grid, ref Point robot, Direction direction) {
        var (xDiff, yDiff) = direction switch {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var newPos = new Point(robot.X + xDiff, robot.Y + yDiff);

        if (grid.Walls.Contains(newPos)) {
            return;
        }

        if (grid.Boxes.Contains(newPos)) {
            var boxPos = newPos;
            do {
                boxPos = new Point(boxPos.X + xDiff, boxPos.Y + yDiff);
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

    private static int SumGpsCoordinates(Grid grid) {
        var sum = 0;
        foreach (var box in grid.Boxes) {
            sum += box.X + box.Y * 100;
        }

        return sum;
    }

    private static int Part2(Grid grid, Point robot, List<Direction> directions) {
        var (grid2, robot2) = WidenInput(grid, robot);

        foreach (var direction in directions) {
            MoveRobot2(grid2, ref robot2, direction);
        }

        return SumGpsCoordinates(grid2);
    }

    private static void PrintWideGrid(Grid grid, Point robot) {
        const int WIDTH = 50 * 2;
        const int HEIGHT = 50;
        for (var y = 0; y < HEIGHT; y++) {
            for (var x = 0; x < WIDTH; x++) {
                var point = new Point(x, y);
                if (grid.Walls.Contains(point)) {
                    Console.Write('#');
                }
                else if (grid.Boxes.Contains(point)) {
                    Console.Write("[]");
                    x++;
                    Debug.Assert(!grid.Boxes.Contains(point with { X = x}));
                }
                else if (robot == point) {
                    Console.Write('@');
                }
                else {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
    }

    // ##########     ####################
    // #..O..O.O#     ##....[]....[]..[]##
    // #......O.#     ##............[]..##
    // #.OO..O.O#     ##..[][]....[]..[]##
    // #..O@..O.#     ##....[]@.....[]..##
    // #O#..O...# --> ##[]##....[]......##
    // #O..O..O.#     ##[]....[]....[]..##
    // #.OO.O.OO#     ##..[][]..[]..[][]##
    // #....O...#     ##........[]......##
    // ##########     ####################
    private static (Grid grid, Point robot) WidenInput(Grid grid, Point robot) {
        var boxes = grid.Boxes.Select(x => x with { X = x.X * 2 }).ToHashSet();
        var walls = grid.Walls.Select(x => x with { X = x.X * 2 }).ToHashSet();
        foreach (var wall in walls.ToArray()) {
            walls.Add(wall with { X = wall.X + 1 });
        }

        return (new Grid(boxes, walls), robot with { X = robot.X * 2 });
    }

    private static void MoveRobot2(Grid grid, ref Point robot, Direction direction) {
        var newPos = direction switch {
            Direction.Up => robot with { Y = robot.Y - 1 },
            Direction.Down => robot with { Y = robot.Y + 1 },
            Direction.Left => robot with { X = robot.X - 1 },
            Direction.Right => robot with { X = robot.X + 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        if (grid.Walls.Contains(newPos)) {
            return;
        }

        if (direction == Direction.Left) {
            if (!TryMoveBoxLeft(grid, robot)) {
                return;
            }
        }
        else if (direction == Direction.Right) {
            if (!TryMoveBoxRight(grid, robot)) {
                return;
            }
        }
        else {
            if (!TryMoveBoxVertical(grid, robot, direction is Direction.Up ? -1 : 1)) {
                return;
            }
        }

        robot = newPos;
    }

    private static bool TryMoveBoxVertical(Grid grid, Point robot, int direction) {
        var firstBox = robot with { Y = robot.Y + direction };

        if (grid.Boxes.Contains(firstBox)) {
            // @.
            // []
            List<Point> toMove = [];
            if (!CanMoveBoxY(grid, firstBox, direction, toMove)) {
                return false;
            }

            MoveBoxes(grid, toMove, direction);

        }
        else if (grid.Boxes.Contains(firstBox with { X = firstBox.X - 1 })) {
            // .@
            // []
            List<Point> toMove = [];
            if (!CanMoveBoxY(grid, firstBox with { X = firstBox.X - 1 }, direction, toMove)) {
                return false;
            }

            MoveBoxes(grid, toMove, direction);
        }

        return true;

        static bool CanMoveBoxY(Grid grid, Point box, int direction, List<Point> toMove) {
            // []
            // #.
            if (grid.Walls.Contains(box)) {
                return false;
            }

            if (!grid.Boxes.Contains(box)) {
                return true;
            }

            // .[].
            // []..
            var newBox = new Point(box.X - 1, box.Y + direction);
            if (!grid.Walls.Contains(newBox) && !CanMoveBoxY(grid, newBox, direction, toMove)) {
                return false;
            }

            // .[].
            // .[].
            newBox = new Point(box.X, box.Y + direction);
            if (!CanMoveBoxY(grid, newBox, direction, toMove)) {
                return false;
            }

            // .[].
            // ..[]
            newBox = new Point(box.X + 1, box.Y + direction);
            if (!CanMoveBoxY(grid, newBox, direction, toMove)) {
                return false;
            }

            toMove.Add(box);
            return true;
        }

        static void MoveBoxes(Grid grid, IEnumerable<Point> points, int direction) {
            var orderedPoints = points.OrderBy(x => x.Y * direction * -1);
            foreach (var point in orderedPoints) {
                grid.Boxes.Remove(point);
                grid.Boxes.Add(point with { Y = point.Y + direction });
            }
        }
    }

    private static bool TryMoveBoxLeft(Grid grid, Point robotBeforeMove) {
        var firstBox = robotBeforeMove with { X = robotBeforeMove.X - 2 };
        if (!grid.Boxes.Contains(firstBox)) {
            return true;
        }

        List<Point> toMove = [];
        if (!CanMoveBox(grid, robotBeforeMove, toMove)) {
            return false;
        }

        foreach (var box in toMove) {
            grid.Boxes.Remove(box);
            grid.Boxes.Add(box with { X = box.X - 1 });
        }

        return true;

        static bool CanMoveBox(Grid grid, Point robotBeforeMove, List<Point> toMove) {
            var boxPos = robotBeforeMove;
            do {
                boxPos = boxPos with { X = boxPos.X - 2 };
                if (!grid.Boxes.Contains(boxPos)) {
                    // .#[] OR ..[]
                    // ^^      ^^
                    return !grid.Walls.Contains(boxPos with { X = boxPos.X + 1 });
                }

                toMove.Add(boxPos);
            } while (true);
        }
    }

    private static bool TryMoveBoxRight(Grid grid, Point robotBeforeMove) {
        var firstBox = robotBeforeMove with { X = robotBeforeMove.X + 1 };
        if (!grid.Boxes.Contains(firstBox)) {
            return true;
        }

        List<Point> toMove = [];
        if (!CanMoveBox(grid, robotBeforeMove, toMove)) {
            return false;
        }

        foreach (var box in toMove) {
            grid.Boxes.Remove(box);
            var newBox = box with { X = box.X + 1 };
            grid.Boxes.Add(newBox);
        }

        return true;

        static bool CanMoveBox(Grid grid, Point robotBeforeMove, List<Point> toMove) {
            var boxPos = robotBeforeMove with { X = robotBeforeMove.X - 1 };
            do {
                boxPos = boxPos with { X = boxPos.X + 2 };
                if (!grid.Boxes.Contains(boxPos)) {
                    // []#. OR []..
                    //   ^^      ^^
                    return !grid.Walls.Contains(boxPos);
                }

                toMove.Add(boxPos);
            } while (true);
        }
    }
}