using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2019.Utils;

namespace aoc2019
{
    public static class Day5
    {
        private enum OpCode
        {
            Addition = 1,
            Multiplication = 2,
            Input = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            AdjustRelativeBase = 9,
            Halt = 99
        }

        private enum Parameter
        {
            First = 1,
            Second = 2,
            Third = 3
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2
        }

        public class IntComputer
        {
            private readonly IDictionary<OpCode, Func<long, long, IntComputer, long>> _instructions
                = new Dictionary<OpCode, Func<long, long, IntComputer, long>>
                {
                    {OpCode.Addition, Addition},
                    {OpCode.Multiplication, Multiplication},
                    {OpCode.Input, Input},
                    {OpCode.Output, Output},
                    {OpCode.JumpIfTrue, JumpIfTrue},
                    {OpCode.JumpIfFalse, JumpIfFalse},
                    {OpCode.LessThan, LessThan},
                    {OpCode.Equals, EqualsOp},
                    {OpCode.AdjustRelativeBase, AdjustRelativeBase},
                    {OpCode.Halt, Halt}
                };


            private ConcurrentQueue<long> _input;
            private ConcurrentQueue<long> _output;

            public IntComputer(params long[] program)
            {
                Program = program;
            }

            public long[] Program { get; }
            public ConcurrentDictionary<int, long> Memory { get; private set; }
            private long RelativeBase { get; set; }

            private ConcurrentDictionary<int, long> InitProgram()
            {
                var memory = Program.Select((value, address) => (value, address))
                                    .ToDictionary(cell => cell.address, cell => cell.value);
                return new ConcurrentDictionary<int, long>(memory);
            }

            public long[] Compute(params long[] input)
            {
                Memory = InitProgram();
                RelativeBase = 0L;
                
                _input = new ConcurrentQueue<long>(input);
                _output = new ConcurrentQueue<long>();

                var position = 0L;
                while (position < Memory.Count)
                {
                    var instruction = GetInstruction(Memory, position);
                    position = _instructions[GetOpCode(instruction)](position, instruction, this);
                }

                return _output.ToArray();
            }

            public void Compute(ConcurrentQueue<long> input, ConcurrentQueue<long> output)
            {
                Memory = InitProgram();
                RelativeBase = 0L;
                
                _input = input;
                _output = output;

                var position = 0L;
                while (position < Memory.Count)
                {
                    var instruction = GetInstruction(Memory, position);
                    position = _instructions[GetOpCode(instruction)](position, instruction, this);
                }
            }

            private static long GetInstruction(IDictionary<int, long> memory, long position)
            {
                return memory[(int) position];
            }

            private static OpCode GetOpCode(long instruction)
            {
                return (OpCode) (instruction % 100);
            }

            private static long Addition(long position, long instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer, position, instruction);

                computer.Memory[(int) third] = second + first;

                return position + 4;
            }

            private static long Multiplication(long position, long instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer, position, instruction);

                computer.Memory[(int) third] = second * first;

                return position + 4;
            }

            private static long Input(long position, long instruction, IntComputer computer)
            {
                var first = ResolveAddress(computer, GetParameter(position, instruction, Parameter.First));

                do
                {
                    if (computer._input.TryDequeue(out var value))
                    {
                        computer.Memory[(int) first] = value;
                        return position + 2;
                    }
                } while (true);
            }

            private static long Output(long position, long instruction, IntComputer computer)
            {
                var first = ResolveAddress(computer, GetParameter(position, instruction, Parameter.First));

                computer._output.Enqueue(computer.Memory[(int) first]);

                return position + 2;
            }

            private static long JumpIfTrue(long position, long instruction, IntComputer computer)
            {
                var (first, second) = GetTwoParameters(computer, position, instruction);

                return first != 0 ? second : position + 3;
            }

            private static long JumpIfFalse(long position, long instruction, IntComputer computer)
            {
                var (first, second) = GetTwoParameters(computer, position, instruction);

                return first == 0 ? second : position + 3;
            }

            private static long LessThan(long position, long instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer, position, instruction);

                computer.Memory[(int) third] = second > first ? 1 : 0;

                return position + 4;
            }

            private static long EqualsOp(long position, long instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer, position, instruction);

                computer.Memory[(int) third] = second == first ? 1 : 0;

                return position + 4;
            }

            private static long AdjustRelativeBase(long position, long instruction, IntComputer computer)
            {
                var first = Resolve(computer, GetParameter(position, instruction, Parameter.First));

                computer.RelativeBase += first;

                return position + 2;
            }

            private static long Halt(long position, long instruction, IntComputer computer)
            {
                return computer.Memory.Count;
            }

            private static (long first, long second) GetTwoParameters(IntComputer computer, long position, long instruction)
            {
                var first = Resolve(computer, GetParameter(position, instruction, Parameter.First));
                var second = Resolve(computer, GetParameter(position, instruction, Parameter.Second));
                return (first, second);
            }

            private static (long first, long second, long third) GetThreeParameters(IntComputer computer, long position, long instruction)
            {
                var first = Resolve(computer, GetParameter(position, instruction, Parameter.First));
                var second = Resolve(computer, GetParameter(position, instruction, Parameter.Second));
                var third = ResolveAddress(computer, GetParameter(position, instruction, Parameter.Third));
                return (first, second, third);
            }

            private static (long position, ParameterMode mode) GetParameter(long position, long instruction,
                Parameter parameter)
            {
                return (GetPosition(position, parameter), GetMode(instruction, parameter));
            }

            private static long GetPosition(long position, Parameter parameter)
            {
                return position + (long) parameter;
            }

            private static ParameterMode GetMode(long instruction, Parameter parameter)
            {
                return (ParameterMode) (instruction / (long) Math.Pow(10, (long) parameter + 1) % 10);
            }

            private static long Resolve(IntComputer computer, (long position, ParameterMode mode) parameter)
            {
                return computer.Memory.GetOrAdd((int) ResolveAddress(computer, parameter), 0);
            }

            private static long ResolveAddress(IntComputer computer, (long position, ParameterMode mode) parameter)
            {
                var (position, mode) = parameter;

                switch (mode)
                {
                    case ParameterMode.Immediate:
                        return position;
                    case ParameterMode.Relative:
                        return computer.RelativeBase + computer.Memory[(int) position];
                    case ParameterMode.Position:
                    default:
                        return computer.Memory[(int) position];
                }
            }

            public static long[] LoadProgram(string name)
            {
                using (var input = AssetManager.GetAsset(name))
                {
                    return input.ReadToEnd().Split(',').Select(long.Parse).ToArray();
                }
            }

            public void DumpMemory(string name)
            {
                using (var fileStream = AssetManager.OpenAsset(name))
                using (var output = new StreamWriter(fileStream))
                {
                    output.WriteLine(string.Join(",", Memory));
                    output.Close();
                }
            }
        }
    }
}
