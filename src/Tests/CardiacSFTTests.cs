using System;
using Interpreter;
using NUnit.Framework;

namespace Tests
{
  [TestFixture]
  public class CardiacSFTTests
  {
    private Cardiac computer;
    private CardiacLoader loader;

    [SetUp]
    public void SetUp()
    {
      computer = new Cardiac();
      loader = new CardiacLoader(computer);
    }

    [Test]
    public void ShiftLeft_Once_InputTimesTen()
    {
      TestProgram(
        410, 
        001, 010);
    }

    [Test]
    public void ShiftLeft_OutOfRange_Zero()
    {
      TestProgram(
        440,
        001, 000);
    }

    [Test]
    public void ShiftRight_OutOfRange_Zero()
    {
      TestProgram(
        401,
        001, 000);
    }

    [Test]
    public void ShiftRight_Once_DividedByTen()
    {
      TestProgram(
        401,
        010, 001);
    }

    [Test]
    public void Shift_LeftFirstThenRight_One()
    {
      TestProgram(
        411,
        001, 001);
    }

    [Test]
    public void Shift_LeftOnLastSpaceOfAccumilator_ThenRight_One()
    {
      TestProgram(
        433,
        001, 001);
    }

    [Test]
    public void Shift_LeftOutsideAccumilatorSpace_ThenRight_One()
    {
      TestProgram(
        444,
        001, 000);
    }

    private void TestProgram(int shiftOperation, int input, int expectedOutput)
    {
      loader.EnqueueProgram(
        020,
        120,
        shiftOperation,
        620,
        520,
        900
      );

      computer.Enqueue(input);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(expectedOutput, computer.Output.Dequeue());
    }
  }
}
