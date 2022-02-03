using System;
using System.Collections.Generic;

namespace Interpreter
{
  public class Cardiac : IDeviceWithInput
  {
    public Queue<int> Output { get; } = new Queue<int>();

    private readonly Queue<int> input = new Queue<int>();
    private readonly CardiacMemoryCell[] memory;
    private readonly Accumilator accumilator = new Accumilator();
    private int programCounter = 0;

    public Cardiac()
    {
      memory = new CardiacMemoryCell[100];
      memory[0] = new CardiacROMCell(001);
      memory[99] = new CardiacROMCell(900);

      for (var i = 1; i < 99; i++)
      {
        memory[i] = new CardiacRAMCell();
      }
    }

    public void Enqueue(int inputValue)
    {
      input.Enqueue(inputValue);
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
            memory[cell.Address].Value = input.Dequeue();
            break;
          case CardiacOpcode.CLA:
            accumilator.Set(memory[cell.Address].Value);
            break;
          case CardiacOpcode.ADD:
            accumilator.Add(memory[cell.Address].Value);
            break;
          case CardiacOpcode.TAC:
            if (accumilator.Value < 0)
            {
              nextCounter = cell.Address;
            }
            break;
          case CardiacOpcode.SFT:
            var timeShiftLeft = cell.Address / 10;
            var timeShiftRight = cell.Address % 10;

            accumilator.ShiftLeft(timeShiftLeft);
            accumilator.ShiftRight(timeShiftRight);
            break;
          case CardiacOpcode.OUT:
            Output.Enqueue(memory[cell.Address].Value);
            break;
          case CardiacOpcode.STO:
            memory[cell.Address].Value = accumilator.Value;
            break;
          case CardiacOpcode.SUB:
            accumilator.Subtract(memory[cell.Address].Value);
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

    private class Accumilator
    {
      private int _value;

      public int Value
      {
        get { return _value; }
        set { _value = value % 10000; }
      }

      public void Set(int val)
      {
        Value = val;
      }

      public void Add(int val)
      {
        Value += val;
      }

      public void Subtract(int val)
      {
        Value -= val;
      }

      public void ShiftLeft(int spaces)
      {
        Value *= (int)Math.Pow(10, spaces);
      }

      public void ShiftRight(int spaces)
      {
        Value /= (int)Math.Pow(10, spaces);
      }
    }
  }
}
