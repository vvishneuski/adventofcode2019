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
            PositionMode = 0,
            ImmediateMode = 1
        }

        public class IntComputer
        {
            private readonly IDictionary<OpCode, Func<int, int, IntComputer, int>> _instructions
                = new Dictionary<OpCode, Func<int, int, IntComputer, int>>
                {
                    {OpCode.Addition, Addition},
                    {OpCode.Multiplication, Multiplication},
                    {OpCode.Input, Input},
                    {OpCode.Output, Output},
                    {OpCode.JumpIfTrue, JumpIfTrue},
                    {OpCode.JumpIfFalse, JumpIfFalse},
                    {OpCode.LessThan, LessThan},
                    {OpCode.Equals, EqualsOp},
                    {OpCode.Halt, Halt}
                };

            private ConcurrentQueue<int> _input;
            private ConcurrentQueue<int> _output;

            public IntComputer(params int[] program)
            {
                Program = program;
            }

            public int[] Program { get; }
            public int[] Memory { get; private set; }

            public int[] Compute(params int[] input)
            {
                Memory = Program.ToArray();

                _input = new ConcurrentQueue<int>(input);
                _output = new ConcurrentQueue<int>();

                var position = 0;
                while (position < Memory.Length)
                {
                    var instruction = GetInstruction(Memory, position);
                    position = _instructions[GetOpCode(instruction)](position, instruction, this);
                }

                return _output.ToArray();
            }

            public void Compute(ConcurrentQueue<int> input, ConcurrentQueue<int> output)
            {
                Memory = Program.ToArray();
                _input = input;
                _output = output;

                var position = 0;
                while (position < Memory.Length)
                {
                    var instruction = GetInstruction(Memory, position);
                    position = _instructions[GetOpCode(instruction)](position, instruction, this);
                }
            }

            private static int GetInstruction(int[] memory, int position)
            {
                return memory[position];
            }

            private static OpCode GetOpCode(int instruction)
            {
                return (OpCode) (instruction % 100);
            }

            private static int Addition(int position, int instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer.Memory, position, instruction);

                computer.Memory[third] = second + first;

                return position + 4;
            }

            private static int Multiplication(int position, int instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer.Memory, position, instruction);

                computer.Memory[third] = second * first;

                return position + 4;
            }

            private static int Input(int position, int instruction, IntComputer computer)
            {
                var first = ResolveAddress(computer.Memory, GetParameter(position, instruction, Parameter.First));

                do
                {
                    if (computer._input.TryDequeue(out var value))
                    {
                        computer.Memory[first] = value;
                        return position + 2;
                    }
                } while (true);
            }

            private static int Output(int position, int instruction, IntComputer computer)
            {
                var first = ResolveAddress(computer.Memory, GetParameter(position, instruction, Parameter.First));

                computer._output.Enqueue(computer.Memory[first]);

                return position + 2;
            }

            private static int JumpIfTrue(int position, int instruction, IntComputer computer)
            {
                var (first, second) = GetTwoParameters(computer.Memory, position, instruction);

                return first != 0 ? second : position + 3;
            }

            private static int JumpIfFalse(int position, int instruction, IntComputer computer)
            {
                var (first, second) = GetTwoParameters(computer.Memory, position, instruction);

                return first == 0 ? second : position + 3;
            }

            private static int LessThan(int position, int instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer.Memory, position, instruction);

                computer.Memory[third] = second > first ? 1 : 0;

                return position + 4;
            }

            private static int EqualsOp(int position, int instruction, IntComputer computer)
            {
                var (first, second, third) = GetThreeParameters(computer.Memory, position, instruction);

                computer.Memory[third] = second == first ? 1 : 0;

                return position + 4;
            }

            private static int Halt(int position, int instruction, IntComputer computer)
            {
                return computer.Memory.Length;
            }

            private static (int first, int second) GetTwoParameters(int[] memory, int position, int instruction)
            {
                return (Resolve(memory, GetParameter(position, instruction, Parameter.First)),
                    Resolve(memory, GetParameter(position, instruction, Parameter.Second)));
            }

            private static (int first, int second, int third) GetThreeParameters(int[] memory, int position,
                int instruction)
            {
                return (Resolve(memory, GetParameter(position, instruction, Parameter.First)),
                    Resolve(memory, GetParameter(position, instruction, Parameter.Second)),
                    ResolveAddress(memory, GetParameter(position, instruction, Parameter.Third)));
            }

            private static (int position, ParameterMode mode) GetParameter(int position, int instruction,
                Parameter parameter)
            {
                return (GetPosition(position, parameter), GetMode(instruction, parameter));
            }

            private static int GetPosition(int position, Parameter parameter)
            {
                return position + (int) parameter;
            }

            private static ParameterMode GetMode(int instruction, Parameter parameter)
            {
                return (ParameterMode) (instruction / (int) Math.Pow(10, (int) parameter + 1) % 10);
            }

            private static int Resolve(int[] memory, (int position, ParameterMode mode) parameter)
            {
                return memory[ResolveAddress(memory, parameter)];
            }

            private static int ResolveAddress(int[] memory, (int position, ParameterMode mode) parameter)
            {
                var (position, mode) = parameter;

                return mode == ParameterMode.ImmediateMode
                    ? position
                    : memory[position];
            }

            public static int[] LoadProgram(string name)
            {
                using (var input = AssetManager.GetAsset(name))
                {
                    return input.ReadToEnd().Split(',').Select(Int32.Parse).ToArray();
                }
            }

            public void DumpMemory(string name)
            {
                using (var fileStream = AssetManager.OpenAsset(name))
                using (var output = new StreamWriter(fileStream))
                {
                    output.WriteLine(String.Join(",", Memory));
                    output.Close();
                }
            }
        }
    }
}
