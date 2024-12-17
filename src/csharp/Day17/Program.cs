using System.Text.RegularExpressions;

namespace Day17;

internal static partial class Program {
    /// <summary> The adv instruction (opcode 0) performs division. The numerator is the value in the A register. The denominator is found by raising 2 to the power of the instruction's combo operand. (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.) The result of the division operation is truncated to an integer and then written to the A register.</summary>
    private const int ADIV = 0;

    /// <summary> The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.</summary>
    private const int BXL = 1;

    /// <summary> The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits), then writes that value to the B register.</summary>
    private const int BST = 2;

    /// <summary> The jnz instruction (opcode 3) does nothing if the A register is 0. However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand; if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.</summary>
    private const int JNZ = 3;

    /// <summary> The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C, then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)</summary>
    private const int BXC = 4;

    /// <summary> The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value. (If a program outputs multiple values, they are separated by commas.)</summary>
    private const int OUT = 5;

    /// <summary> The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register. (The numerator is still read from the A register.)</summary>
    private const int BDV = 6;

    /// <summary> The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register. (The numerator is still read from the A register.)</summary>
    private const int CDV = 7;

    private record Computer(int A, int B, int C, int Ip) {
        public int A { get; set; } = A;
        public int B { get; set; } = B;
        public int C { get; set; } = C;
        public int Ip { get; set; } = Ip;
        public List<int> StdOut { get; } = [];

        public Computer DeepClone() => new(A, B, C, Ip);
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
        var com = computer.DeepClone();

        FdeLoop(com, program);

        return string.Join(',', com.StdOut.Select(x => x.ToString()));
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
            case ADIV:
                computer.A = DivIns(computer.A, GetComboOperand(computer, operand));
                break;
            case BXL:
                computer.B = computer.B ^ operand;
                break;
            case BST:
                computer.B = GetComboOperand(computer, operand) % 8;
                break;
            case JNZ:
                if (computer.A != 0) {
                    computer.Ip = operand;
                    return;
                }

                break;
            case BXC:
                computer.B = computer.B ^ computer.C;
                break;
                computer.StdOut.Add(GetComboOperand(computer, operand) % 8);
            case OUT:
                break;
            case BDV:
                computer.B = DivIns(computer.A, GetComboOperand(computer, operand));
                break;
            case CDV:
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