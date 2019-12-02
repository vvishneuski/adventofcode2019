namespace aoc2019
{
    public static class Day2
    {
        public static int[] Memory { get; set; } = { };

        public static void Compute()
        {
            var pointer = 0;
            while (pointer < Memory.Length)
            {
                switch (GetOpcode(pointer))
                {
                    case 1:
                        pointer = AdditionInstruction(pointer);
                        break;
                    case 2:
                        pointer = MultiplicationInstruction(pointer);
                        break;
                    case 99:
                        pointer = HaltInstruction();
                        break;
                }
            }
        }

        private static int AdditionInstruction(int pointer)
        {
            SetResult(pointer + 3, GetParameter(pointer + 1) + GetParameter(pointer + 2));
            return pointer + 4;
        }

        private static int MultiplicationInstruction(int pointer)
        {
            SetResult(pointer + 3, GetParameter(pointer + 1) * GetParameter(pointer + 2));
            return pointer + 4;
        }

        private static int HaltInstruction()
        {
            return Memory.Length;
        }

        private static int GetOpcode(int address)
        {
            return Memory[address];
        }

        private static int GetAddress(int address)
        {
            return Memory[address];
        }

        private static int GetParameter(int pointer)
        {
            return Memory[GetAddress(pointer)];
        }

        private static void SetResult(int pointer, int result)
        {
            Memory[GetAddress(pointer)] = result;
        }
    }
}
