using System;
using System.Configuration;
using Interpeter;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CardiacMemoryCellTests
    {
        [Test]
        [TestCase(000, CardiacOpcode.INP)]
        [TestCase(099, CardiacOpcode.INP)]
        [TestCase(100, CardiacOpcode.CLA)]
        [TestCase(199, CardiacOpcode.CLA)]
        [TestCase(200, CardiacOpcode.ADD)]
        [TestCase(299, CardiacOpcode.ADD)]
        [TestCase(300, CardiacOpcode.TAC)]
        [TestCase(399, CardiacOpcode.TAC)]
        [TestCase(400, CardiacOpcode.SFT)]
        [TestCase(499, CardiacOpcode.SFT)]
        [TestCase(500, CardiacOpcode.OUT)]
        [TestCase(599, CardiacOpcode.OUT)]
        [TestCase(600, CardiacOpcode.STO)]
        [TestCase(699, CardiacOpcode.STO)]
        [TestCase(700, CardiacOpcode.SUB)]
        [TestCase(799, CardiacOpcode.SUB)]
        [TestCase(800, CardiacOpcode.JMP)]
        [TestCase(899, CardiacOpcode.JMP)]
        [TestCase(900, CardiacOpcode.HRS)]
        [TestCase(999, CardiacOpcode.HRS)]
        public void Cardiac_Value_OpCode(int startValue, CardiacOpcode expectedOpcode)
        {
            var cell = new CardiacRAMCell { Value = startValue };

            Assert.AreEqual(expectedOpcode, cell.OpCode);
        }

        [Test]
        [TestCase(1000, 000)]
        [TestCase(-1000, 000)]
        [TestCase(1111, 111)]
        [TestCase(-1111, 111)]
        public void Cardiac_SetIncorrectValue_ValueIs3CharacterPositiveInt(int startValue, int expectedValue)
        {
            var cell = new CardiacRAMCell { Value = startValue };

            Assert.AreEqual(expectedValue, cell.Value);
        }

        [Test]
        [TestCase(1900, CardiacOpcode.HRS)]
        public void Cardiac_SetIncorrectValue_OpcodeIsThirdCharacterFromRight(int startValue, CardiacOpcode expectedOpcode)
        {
            var cell = new CardiacRAMCell { Value = startValue };

            Assert.AreEqual(expectedOpcode, cell.OpCode);
        }

        [Test]
        [TestCase(000, 00)]
        [TestCase(100, 00)]
        [TestCase(111, 11)]
        public void Cardiac_Value_Address(int startValue, int expectedAddress)
        {
            var cell = new CardiacRAMCell { Value = startValue };

            Assert.AreEqual(expectedAddress, cell.Address);
        }

        [Test]
        public void ROMCell_SetValue_throws()
        {
            var cell = new CardiacROMCell(001);

            Assert.Throws<NotSupportedException>(() => cell.Value = 002 );
        }
    }
}
