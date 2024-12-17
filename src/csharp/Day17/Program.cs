using System.Text.RegularExpressions;

namespace Day17;

internal static partial class Program {
    private record Computer(int A, int B, int C, int Ip) {
        public int A { get; set; } = A;
        public int B { get; set; } = B;
        public int C { get; set; } = C;
        public int Ip { get; set; } = Ip;
        public List<int> StdOut { get; } = [];
    }

    [GeneratedRegex(@"^Register (\w): (\d+)")]
    private static partial Regex RegisterRegex { get; }

    [GeneratedRegex(@"^Program: ([\d,]+)")]
    private static partial Regex ProgramRegex { get; }

    internal static void Main(string[] args) {
        var (computer, program) = GetInput();

        var res = Part1(computer, program);

        Console.WriteLine(res);
    }

    private static (Computer computer, int[] program) GetInput() {
        using var sr = new StreamReader("Inputs.txt");

        var a = 0;
        var b = 0;
        var c = 0;
        int[] program = [];
        while (sr.ReadLine() is { } line) {
            if (string.IsNullOrWhiteSpace(line)) {
                continue;
            }

            var match = RegisterRegex.Match(line);
            if (match.Success) {
                switch (match.Groups[1].ValueSpan[0]) {
                    case 'A':
                        a = int.Parse(match.Groups[2].ValueSpan);
                        break;
                    case 'B':
                        b = int.Parse(match.Groups[2].ValueSpan);
                        break;
                    case 'C':
                        c = int.Parse(match.Groups[2].ValueSpan);
                        break;
                }

                continue;
            }

            match = ProgramRegex.Match(line);
            if (!match.Success) {
                throw new Exception();
            }

            program = match.Groups[1].Value.Split(',').Select(int.Parse).ToArray();
        }

        return (new Computer(a, b, c, 0), program);
    }

    private static string Part1(Computer computer, int[] program) {
        FdeLoop(computer, program);

        return string.Join(',', computer.StdOut.Select(x => x.ToString()));
    }

    private static void FdeLoop(Computer computer, int[] program) {
        while (true) {
            if (computer.Ip + 1 > program.Length) {
                break;
            }

            var opcode = program[computer.Ip];
            var operand = program[computer.Ip + 1];
            ExecuteInstruction(computer, opcode, operand);
        }
    }

    private static void ExecuteInstruction(Computer computer, int opcode, int operand) {
        switch (opcode) {
            case 0: // adiv
                computer.A = DivIns(computer.A, GetComboOperand(computer, operand));
                break;
            case 1: // bxl
                computer.B = computer.B ^ operand;
                break;
            case 2: // bst
                computer.B = GetComboOperand(computer, operand) % 8;
                break;
            case 3: // jnz
                if (computer.A != 0) {
                    computer.Ip = operand;
                    return;
                }
                break;
            case 4: // bxc
                computer.B = computer.B ^ computer.C;
                break;
            case 5: // out
                computer.StdOut.Add(GetComboOperand(computer, operand) % 8);
                break;
            case 6: // bdv
                computer.B = DivIns(computer.A, GetComboOperand(computer, operand));
                break;
            case 7: // cdv
                computer.C = DivIns(computer.A, GetComboOperand(computer, operand));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(opcode), opcode, null);
        }

        computer.Ip += 2;
    }

    private static int DivIns(int a, int b) => a / (int)Math.Pow(2, b);

    private static int GetComboOperand(Computer computer, int operand) {
        return operand switch {
            <= 3 => operand,
            4 => computer.A,
            5 => computer.B,
            6 => computer.C,
            _ => throw new ArgumentOutOfRangeException(nameof(operand), operand, null),
        };
    }
}