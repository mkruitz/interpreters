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

        [Test]
        public void CardiacAccumilator_StoreEmptyAccumilatorIntoCell_OutputAccumilatorContent()
        {
            EnqueueProgram(
                603,
                503,
                900
                );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(000, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddOne_OutputOne()
        {
            EnqueueProgram(
                200,
                603,
                503,
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(01, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddOneTwice_OutputTwo()
        {
            EnqueueProgram(
                200,
                200,
                610,
                510,
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(02, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddTen_OutputTen()
        {
            EnqueueProgram(
                010,
                203,
                610,
                510,
                904
            );
            computer.Input.Enqueue(000);

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(10, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddAndClearOneTwice_OutputOne()
        {
            EnqueueProgram(
                100,
                100,
                610,
                510,
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(01, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddOneAndRemoveOne_OutputZero()
        {
            EnqueueProgram(
                200,
                700,
                610,
                510,
                900
            );

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(00, computer.Output.Dequeue());
        }

        [Test]
        public void CardiacAccumilator_AddTenAndRemoveOne_OutputZero()
        {
            EnqueueProgram(
                010,
                200,
                703,
                610,
                510,
                904
            );
            computer.Input.Enqueue(000);

            computer.Execute();

            Assert.AreEqual(0, computer.Input.Count);
            Assert.AreEqual(1, computer.Output.Count);
            Assert.AreEqual(09, computer.Output.Dequeue());
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
