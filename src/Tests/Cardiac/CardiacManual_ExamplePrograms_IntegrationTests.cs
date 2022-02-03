using Interpreter.Cardiac;
using NUnit.Framework;

namespace Tests.Cardiac
{
  [TestFixture]
  public class CardiacManual_ExamplePrograms_IntegrationTests
  {
    private Interpreter.Cardiac.Cardiac computer;
    private CardiacLoader loader;

    [SetUp]
    public void SetUp()
    {
      computer = new Interpreter.Cardiac.Cardiac();
      loader = new CardiacLoader(computer);
    }

    [Test]
    public void ProgramNo1_AddNumberA_toNumberB_toProduceSumS()
    {
      loader.EnqueueProgram(
          034,
          035,
          134,
          235,
          636,
          536,
          900
          );

      computer.Enqueue(032);
      computer.Enqueue(109);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(141, computer.Output.Dequeue());
    }

    [Test]
    public void ProgramNo2_Counting()
    {
      loader.EnqueueProgram(
        100,
        603,
        503,
        200,
        603,
        503,
        200,
        603,
        503,
        200,
        603,
        503,
        900
      );

      computer.Execute();

      Assert.AreEqual(4, computer.Output.Count);
      Assert.AreEqual(001, computer.Output.Dequeue());
      Assert.AreEqual(002, computer.Output.Dequeue());
      Assert.AreEqual(003, computer.Output.Dequeue());
      Assert.AreEqual(004, computer.Output.Dequeue());
    }

    [Test, Ignore("Program never ends")]
    public void ProgramNo3_Loop()
    {
      loader.EnqueueProgram(
        100,
        603,
        503,
        200,
        822
      );

      computer.Execute();

      //Results in infinite loop
    }

    [Test]
    public void ProgramNo4_RocketLaunchingCountdown()
    {
      loader.StartAt(019);
      loader.EnqueueSubRoutine(
        -004
      );
      loader.StartAt(020);
      loader.EnqueueProgram(
        119,
        200,
        618,
        518,
        321,
        900
      );

      computer.Execute();

      Assert.AreEqual(4, computer.Output.Count);
      Assert.AreEqual(-003, computer.Output.Dequeue());
      Assert.AreEqual(-002, computer.Output.Dequeue());
      Assert.AreEqual(-001, computer.Output.Dequeue());
      Assert.AreEqual(000, computer.Output.Dequeue());
    }

    [Test]
    public void ProgramNo5_MultiplicationBySingleDigitMultiplier()
    {
      loader.StartAt(007);
      loader.EnqueueProgram(
        068,
        404,
        669,
        070,
        170,
        700,
        670,
        319,
        169,
        268,
        669,
        811,
        569,
        900
      );
      computer.Enqueue(12);
      computer.Enqueue(3);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(12 * 3, computer.Output.Dequeue());
    }

    [Test]
    public void ProgramNo6_ReversingTheOrderOfANumber()
    {
      loader.StartAt(015);
      loader.EnqueueProgram(
        039,
        139,
        431,
        240,
        640,
        139,
        413,
        240,
        640,
        139,
        423,
        410,
        240,
        640,
        540,
        900
      );
      computer.Enqueue(123);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(321, computer.Output.Dequeue());
    }
  }
}
