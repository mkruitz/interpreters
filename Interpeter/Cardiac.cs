using System;
using System.Collections.Generic;

namespace Interpeter
{
    public class Cardiac
    {
        public Queue<int> Input { get; }
        public Queue<int> Output { get; }

        private readonly CardiacMemoryCell[] memory;
        private int programCounter;
        private int accumilator = 0;

        public Cardiac()
        {
            Input = new Queue<int>();
            Output = new Queue<int>();

            programCounter = 0;

            memory = new CardiacMemoryCell[100];
            memory[0] = new CardiacROMCell(001);
            memory[99] = new CardiacROMCell(900);

            for (var i = 1; i < 99; i++)
            {
                memory[i] = new CardiacRAMCell();
            }
        }

        public void Execute()
        {
            while (true)
            {
                var nextCounter = programCounter + 1;
                var cell = memory[programCounter];
                switch (cell.OpCode)
                {
                    case CardiacOpcode.INP:
                        memory[cell.Address].Value = Input.Dequeue();
                        break;
                    case CardiacOpcode.CLA:
                        accumilator = memory[cell.Address].Value;
                        break;
                    case CardiacOpcode.ADD:
                        accumilator += memory[cell.Address].Value;
                        break;
                    // TAC
                    // SFT
                    case CardiacOpcode.OUT:
                        Output.Enqueue(memory[cell.Address].Value);
                        break;
                    case CardiacOpcode.STO:
                        memory[cell.Address].Value = accumilator;
                        break;
                    case CardiacOpcode.SUB:
                        accumilator -= memory[cell.Address].Value;
                        break;
                    case CardiacOpcode.JMP:
                        nextCounter = cell.Address;
                        break;
                    case CardiacOpcode.HRS:
                        programCounter = cell.Address;
                        return;
                }

                programCounter = nextCounter;
                if (programCounter >= memory.Length)
                    throw new NotImplementedException();
            }
        }
    }
}
