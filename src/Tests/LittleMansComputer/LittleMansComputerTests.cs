using NUnit.Framework;

namespace Tests.LittleMansComputer
{
  [TestFixture]
  public class LittleMansComputerTests
  {
    [Test]
    public void FirstProgram()
    {
      var lmc = new Interpreter.LittleMansComputer.LittleMansComputer(new[]
      {
                "901", "308", "901", "309", "508", "209", "902", "000"
            });

      lmc.Inbox.Enqueue(10);
      lmc.Inbox.Enqueue(1);

      lmc.Execute();

      var outbox = lmc.Outbox.Dequeue();
      Assert.AreEqual(9, outbox);
    }
  }
}
