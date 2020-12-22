using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day8
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var instructions = File
                .ReadAllLines("input.txt")
                .SelectMany(s => Instruction.TryParse(s, out var result) ? new[] { result } : Enumerable.Empty<Instruction>() )
                .ToList();
            Console.WriteLine("{0} instructions.", instructions.Count);

            TryRunToEnd(instructions, out var stateBeforeLoop);
            Console.WriteLine("Acc before loop: {0}", stateBeforeLoop.Accumulator);

            for (var i = 0; i < instructions.Count; i++)
            {
                var originalInstruction = instructions[i];
                var newCommand = originalInstruction.Command switch
                {
                    Command.Acc => Command.Acc,
                    Command.Jmp => Command.Nop,
                    _ => Command.Jmp
                };

                var newInstruction = new Instruction(newCommand, originalInstruction.Value);
                instructions[i] = newInstruction;
                if (TryRunToEnd(instructions, out var endState))
                {
                    Console.WriteLine("Accumulator on exit: {0}", endState.Accumulator);
                    break;
                }

                instructions[i] = originalInstruction;
            }
        }

        private static bool TryRunToEnd(IList<Instruction> instructions, out State finalState)
        {
            var seenInstructions = new HashSet<int>();
            var state = new State(0, 0);
            while (true)
            {
                var instruction = instructions[state.InstructionPointer];
                var (deltaIp, deltaAcc) = instruction.Command switch
                {
                    Command.Acc => (1, instruction.Value),
                    Command.Jmp => (instruction.Value, 0),
                    _ => (1, 0)
                };

                var newState = new State(state.InstructionPointer + deltaIp, state.Accumulator + deltaAcc);
                if (newState.InstructionPointer == instructions.Count)
                {
                    finalState = newState;
                    return true;
                }

                if (!seenInstructions.Add(newState.InstructionPointer))
                {
                    finalState = state;
                    return false;
                }

                state = newState;
            }
        }
    }

    enum Command
    {
        Acc,
        Jmp,
        Nop
    }

    internal readonly struct State
    {
        internal State(int instructionPointer, int accumulator)
        {
            InstructionPointer = instructionPointer;
            Accumulator = accumulator;
        }

        internal int InstructionPointer { get; }

        internal int Accumulator { get; }

        public override string ToString()
        {
            return $"State({InstructionPointer}, {Accumulator})";
        }
    }

    internal readonly struct Instruction
    {
        internal Instruction(Command command, int value)
        {
            Command = command;
            Value = value;
        }

        internal Command Command { get; }

        internal int Value { get; }

        internal static bool TryParse(string s, out Instruction result)
        {
            result = default;
            if (s is null || s.Length < 5)
            {
                return false;
            }

            Command command;
            if (s.StartsWith("acc "))
            {
                command = Command.Acc;
            }
            else if (s.StartsWith("jmp "))
            {
                command = Command.Jmp;
            }
            else if (s.StartsWith("nop "))
            {
                command = Command.Nop;
            }
            else
            {
                return false;
            }

            result = new Instruction(command, int.Parse(s.AsSpan(4)));
            return true;
        }
    }

    internal record Acc(int Value);

    internal record Jmp(int Value);

    internal record Nop();
}
