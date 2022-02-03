using System;

namespace Interpreter
{
  public class CardiacRAMCell : CardiacMemoryCell
  {
  }

  public class CardiacROMCell : CardiacMemoryCell
  {
    public CardiacROMCell(int defaultValue)
    {
      base.Value = defaultValue;
    }

    public override int Value
    {
      get { return base.Value; }
      set
      {
        throw new NotSupportedException("Cannot write to ROMCell");
      }
    }

  }

  public abstract class CardiacMemoryCell
  {
    private int value;

    public virtual int Value
    {
      get { return value; }
      set { this.value = value % 1000; }
    }

    public CardiacOpcode OpCode => (CardiacOpcode)Math.Abs(value / 100);
    public int Address => Math.Abs(value % 100);
  }
}