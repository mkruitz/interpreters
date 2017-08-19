using Interpeter;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CardiacExamplePrograms_IntegrationTests
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
        public void ProgramAddTwoNumbers()
        {
            loader.EnqueueProgram(
                017,
                018,
                117,
                218,
                619,
                519,
                903
                );

            computer.Enqueue(002);
            computer.Enqueue(002);

            computer.Execute();

            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(004, computer.Output.Dequeue());
        }
    }
}
