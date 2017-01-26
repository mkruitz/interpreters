using System;

namespace Interpeter
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
            set { this.value = Math.Abs(value % 1000); }
        }

        public CardiacOpcode OpCode => (CardiacOpcode)(value / 100);
        public int Address => value % 100;
    }
}