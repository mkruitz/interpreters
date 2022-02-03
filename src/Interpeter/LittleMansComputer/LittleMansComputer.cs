using System;
using System.Collections.Generic;

namespace Interpreter.LittleMansComputer
{
  public class LittleMansComputer
  {
    public string[] Memory { get; private set; }
    public Queue<int> Inbox { get; private set; }
    public Queue<int> Outbox { get; private set; }
    public int ProgramCounter { get; private set; }

    private Accumulator accumulator = new Accumulator();

    public LittleMansComputer(string[] program)
    {
      Memory = new string[100];
      for (var i = 0; i < program.Length; ++i)
      {
        Memory[i] = program[i];
      }

      Inbox = new Queue<int>();
      Outbox = new Queue<int>();
    }

    public void Execute()
    {
      var run = true;

      while (run)
      {
        var field = Memory[ProgramCounter];
        var opCode = field.Substring(0, 1);
        var addressCode = int.Parse(field.Substring(1));
        ProgramCounter++;

        switch (opCode)
        {
          case "0":
            run = false;
            break;
          case "1":
            accumulator.Add(int.Parse(Memory[addressCode]));
            break;
          case "2":
            accumulator.Subtract(int.Parse(Memory[addressCode]));
            break;
          case "3":
            Memory[addressCode] = accumulator.Value.ToString();
            break;
          case "5":
            accumulator.Value = int.Parse(Memory[addressCode]);
            break;
          case "9":
            switch (addressCode)
            {
              case 1:
                accumulator.Value = Inbox.Dequeue();
                break;
              case 2:
                Outbox.Enqueue(accumulator.Value);
                break;
              default:
                throw new NotImplementedException();
            }
            break;
        }
      }

      // 1. Check the Program Counter for the mailbox number that contains a program instruction(i.e.zero at the start of the program)
      // 2. Fetch the instruction from the mailbox with that number.
      //     Each instruction contains two fields:
      //      - An opcode(indicating the operation to perform) 
      //      - the address field(indicating where to find the data to perform the operation on).
      // 3. Increment the Program Counter(so that it contains the mailbox number of the next instruction)
      // 4. Decode the instruction.
      //     If the instruction utilises data stored in another mailbox then use the address field to find the 
      //     mailbox number for the data it will work on, e.g. 'get data from mailbox 42')
      // 5. Fetch the data(from the input, accumulator, or mailbox with the address determined in step 4)
      // 6. Execute the instruction based on the opcode given
      // 7. Branch or store the result(in the output, accumulator, or mailbox with the address determined in step 4)
      // 8. Return to the Program Counter to repeat the cycle or halt
    }

    private class Accumulator
    {
      public int Value { get; set; }

      public int Add(int a)
      {
        return Value += a;
      }

      public int Subtract(int a)
      {
        return Value -= a;
      }
    }
  }
}
