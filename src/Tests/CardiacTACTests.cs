using System;
using Interpeter;
using NUnit.Framework;

namespace Tests
{
  [TestFixture]
  public class CardiacTACTests
  {
    private Cardiac computer;
    private CardiacLoader loader;

    private const int ResultWhen_Negative = 999;
    private const int ResultWhen_Positive = 333;

    [SetUp]
    public void SetUp()
    {
      computer = new Cardiac();
      loader = new CardiacLoader(computer);
    }

    [Test]
    public void TAC_TestNegativeNumber_OutputOne()
    {
      EnqueTestProgram_Return999WhenNegative();
      computer.Enqueue(-001);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(ResultWhen_Negative, computer.Output.Dequeue());
    }

    [Test]
    public void TAC_TestPostiveNumber_OutputZero()
    {
      EnqueTestProgram_Return999WhenNegative();
      computer.Enqueue(001);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(ResultWhen_Positive, computer.Output.Dequeue());
    }

    private void EnqueTestProgram_Return999WhenNegative()
    {
      loader.StartAt(050);
      loader.EnqueueSubRoutine(
        560,
        900
      );
      
      loader.StartAt(060);
      loader.EnqueueSubRoutine(
        ResultWhen_Negative,
        ResultWhen_Positive
      );

      loader.StartAt(003);
      loader.EnqueueProgram(
        020,
        120,
        350,
        561,
        900
      );
    }
  }
}
