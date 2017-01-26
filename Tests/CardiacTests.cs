using System;
using Interpeter;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CardiacTests
    {
        private Cardiac computer;

        [SetUp]
        public void SetUp()
        {
            computer = new Cardiac();
        }

        [Test]
        public void Cardiac_ReadInputAndStopWithoutCrash()
        {
            computer.Input.Enqueue(900);

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(0, computer.Output.Count);
        }

        [Test]
        public void Cardiac_ReadInputAndStopWithoutCrashTwice_EndProgram()
        {
            Cardiac_ReadInputAndStopWithoutCrash();
            Cardiac_ReadInputAndStopWithoutCrash();
        }

        [Test]
        public void Cardiac_ReadInputTwiceAndStopWithoutCrash()
        {
            computer.Input.Enqueue(002);
            computer.Input.Enqueue(900);

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(0, computer.Output.Count);
        }

        [Test]
        public void Cardiac_ReadInputWriteToFirstCell_ThrowsBecauseCannotWriteToROMCell()
        {
            computer.Input.Enqueue(000);
            computer.Input.Enqueue(900);

            Assert.Throws<NotSupportedException>(() => computer.Execute());
        }

        [Test]
        public void Cardiac_ReadInputWriteToLastCell_ThrowsBecauseCannotWriteToROMCell()
        {
            computer.Input.Enqueue(099);
            computer.Input.Enqueue(900);

            Assert.Throws<NotSupportedException>(() => computer.Execute());
        }

        [Test]
        public void Cardiac_ReadInputJumpLastCell_EndProgram()
        {
            computer.Input.Enqueue(899);

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(0, computer.Output.Count);
        }

        [Test]
        public void HRS_LoadEndlessProgram_HaltsAndJumpsTo()
        {
            EnqueueProgram(
                903
            );

            computer.Execute();
            computer.Execute();
            computer.Execute();
        }

        [Test]
        public void Cardiac_LoadProgramAndStopWithoutCrash()
        {
            EnqueueProgram(
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(0, computer.Output.Count);
        }

        [Test]
        public void Cardiac_LoadProgram_OutputFirstMemoryCell()
        {
            EnqueueProgram(
                599,
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);

            Assert.AreEqual(900, computer.Output.Dequeue());
        }

        // ProgramCounter starts at 03.
        // Memory cell  Value
        // -----------  -----
        // 03           010
        // 04           011
        // 05           110
        // 06           211
        // 07           612
        // 08           512
        // 09           900
        [Test, Ignore("")]
        public void Program_AddTwoNumbers()
        {
            computer.Input.Enqueue(900);
            computer.Execute();
            // {
            //     "901", "308", "901", "309", "508", "209", "902", "000"
            // });
            // 
            // lmc.Inbox.Enqueue(10);
            // lmc.Inbox.Enqueue(1);
            // 
            // lmc.Execute();
            // 
            // var outbox = lmc.Outbox.Dequeue();
            // Assert.AreEqual(9, outbox);
        }
        
        private void EnqueueProgram(params int[] instructions)
        {
            EnqueueBootLoader();

            var addr = 003;
            foreach (var instruction in instructions)
            {
                computer.Input.Enqueue(addr);
                computer.Input.Enqueue(instruction);
                addr++;
            }

            EnqueueStartProgram();
        }

        private void EnqueueBootLoader()
        {
            computer.Input.Enqueue(002);
            computer.Input.Enqueue(800);
        }

        private void EnqueueStartProgram()
        {
            computer.Input.Enqueue(803);
        }
    }
}
