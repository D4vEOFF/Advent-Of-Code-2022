using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day10
    {
        enum InstructionType
        {
            ADDR, NOOP
        }
        class Instruction
        {
            public InstructionType Type { get; private set; }
            public int Value { get; private set; }
            public Instruction(InstructionType type = InstructionType.NOOP, int value = 0)
            {
                Type = type;
                Value = value;
            }
            public override string ToString()
            {
                return (Type == InstructionType.ADDR ? "ADDR" : "NOOP") + $" {Value}";
            }
        }
        static Instruction[] ParseInput(string code)
        {
            return code
                .Split(Environment.NewLine)
                .Select(x => x.Split(' '))
                .Select(x =>
                {
                    InstructionType type = InstructionType.NOOP;
                    int value = 0;
                    switch (x.First())
                    {
                        case "addx": 
                            type = InstructionType.ADDR; 
                            value = int.Parse(x.Last());
                            break;
                        case "noop": type = InstructionType.NOOP; break;
                    }
                    return new Instruction(type, value);
                })
                .ToArray();
        }
        internal static int GetSignalStrength(string code, int startCheckCycle, int difference, int maxCycles)
        {
            Instruction[] instructions = ParseInput(code);

            int signalStrength = 0;
            int registerValue = 1;
            int cycle = 0;

            Dictionary<InstructionType, int> instructionWaitingTime = new Dictionary<InstructionType, int>()
            {
                {InstructionType.ADDR, 2},
                {InstructionType.NOOP, 1},
            };

            int cycleMultiple = 0;
            foreach (var instruction in instructions)
            {
                for (int _ = 0; _ < instructionWaitingTime[instruction.Type]; _++)
                {
                    cycle++;
                    if (cycle == cycleMultiple * difference + startCheckCycle)
                    {
                        signalStrength += cycle * registerValue;
                        cycleMultiple++;
                    }
                }
                registerValue += instruction.Value;

                if (cycle == maxCycles)
                    break;
            }

            return signalStrength;
        }
        internal static string GetScreen(string code)
        {
            Queue<Instruction> instructions = new Queue<Instruction>(ParseInput(code));
            StringBuilder screen = new StringBuilder("");
            screen.Append('.', 240);

            int registerValue = 1;

            Dictionary<InstructionType, int> instructionWaitingTime = new Dictionary<InstructionType, int>()
            {
                {InstructionType.ADDR, 2},
                {InstructionType.NOOP, 1}
            };

            int wait = 0;
            Instruction instruction = new Instruction();
            bool instructionProcessed = false;
            for (int cycle = 0; cycle < screen.Length; cycle++)
            {
                if (instructions.Count > 0 && !instructionProcessed)
                {
                    instruction = instructions.Dequeue();
                    instructionProcessed = true;
                    wait = instructionWaitingTime[instruction.Type] - 1;
                }

                int position = cycle % 40;
                if (registerValue - 1 <= position && position <= registerValue + 1)
                    screen[cycle] = '#';

                if (wait == 0 && instructionProcessed)
                {
                    registerValue += instruction.Value;
                    instructionProcessed = false;
                }
                wait--;
            }

            for (int i = 40; i < screen.Length; i += 41)
                screen.Insert(i, '\n');

            return screen.ToString();
        }
    }
}
