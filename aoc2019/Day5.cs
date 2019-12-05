using System;
using System.Collections.Generic;
using System.Linq;

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

        public class Computer
        {
            private Queue<int> _input;
            private int[] _memory;
            private Queue<int> _output;

            public Computer(params int[] program)
            {
                Program = program;

                Instructions = new Dictionary<OpCode, Func<int, int, int>>
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
            }

            private IDictionary<OpCode, Func<int, int, int>> Instructions { get; }

            private int[] Program { get; }

            public int[] MemoryDump => _memory.ToArray();

            public int this[int position] => _memory[position];

            public int[] Compute(params int[] input)
            {
                _memory = Program.ToArray();

                _input = new Queue<int>(input);
                _output = new Queue<int>();

                var position = 0;
                while (position < _memory.Length)
                {
                    var instruction = GetInstruction(position);
                    position = Instructions[GetOpCode(instruction)](position, instruction);
                }

                return _output.ToArray();
            }

            private int GetInstruction(int position)
            {
                return _memory[position];
            }

            private static OpCode GetOpCode(int instruction)
            {
                return (OpCode) (instruction % 100);
            }

            private int Addition(int position, int instruction)
            {
                var (first, second, third) = GetThreeParameters(position, instruction);

                _memory[third] = second + first;

                return position + 4;
            }

            private int Multiplication(int position, int instruction)
            {
                var (first, second, third) = GetThreeParameters(position, instruction);

                _memory[third] = second * first;

                return position + 4;
            }

            private int Input(int position, int instruction)
            {
                var first = ResolveAddress(GetParameter(position, instruction, Parameter.First));

                _memory[first] = _input.Dequeue();

                return position + 2;
            }

            private int Output(int position, int instruction)
            {
                var first = ResolveAddress(GetParameter(position, instruction, Parameter.First));

                _output.Enqueue(_memory[first]);

                return position + 2;
            }

            private int JumpIfTrue(int position, int instruction)
            {
                var (first, second) = GetTwoParameters(position, instruction);

                return first != 0 ? second : position + 3;
            }

            private int JumpIfFalse(int position, int instruction)
            {
                var (first, second) = GetTwoParameters(position, instruction);

                return first == 0 ? second : position + 3;
            }

            private int LessThan(int position, int instruction)
            {
                var (first, second, third) = GetThreeParameters(position, instruction);

                _memory[third] = second > first ? 1 : 0;

                return position + 4;
            }

            private int EqualsOp(int position, int instruction)
            {
                var (first, second, third) = GetThreeParameters(position, instruction);

                _memory[third] = second == first ? 1 : 0;

                return position + 4;
            }

            private int Halt(int position, int instruction)
            {
                return _memory.Length;
            }

            private (int first, int second) GetTwoParameters(int position, int instruction)
            {
                return (Resolve(GetParameter(position, instruction, Parameter.First)),
                    Resolve(GetParameter(position, instruction, Parameter.Second)));
            }

            private (int first, int second, int third) GetThreeParameters(int position, int instruction)
            {
                return (Resolve(GetParameter(position, instruction, Parameter.First)),
                    Resolve(GetParameter(position, instruction, Parameter.Second)),
                    ResolveAddress(GetParameter(position, instruction, Parameter.Third)));
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

            private int Resolve((int position, ParameterMode mode) parameter)
            {
                return _memory[ResolveAddress(parameter)];
            }

            private int ResolveAddress((int position, ParameterMode mode) parameter)
            {
                var (position, mode) = parameter;

                return mode == ParameterMode.ImmediateMode
                    ? position
                    : _memory[position];
            }
        }
    }
}
