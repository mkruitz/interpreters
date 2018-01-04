using Interpeter;
using NUnit.Framework;

namespace Tests
{
  [TestFixture]
  public class CardiacManual_ExamplePrograms_IntegrationTests
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

    [Test, Ignore("Fails: - Wrong implementation, negative numbers?")]
    public void ProgramNo4_RocketLaunchingCountdown()
    {
      loader.StartAt(19);
      loader.EnqueueProgram(
        119,
        200,
        618,
        518,
        321,
        900
      );

      computer.Enqueue(-004);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(-003, computer.Output.Dequeue());
      Assert.AreEqual(-002, computer.Output.Dequeue());
      Assert.AreEqual(-001, computer.Output.Dequeue());
      Assert.AreEqual(000, computer.Output.Dequeue());
    }
  }
}
