using System;
using Interpreter;
using NUnit.Framework;

namespace Tests
{
  [TestFixture]
  public class CardiacTests
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
    public void Cardiac_ReadInputAndStopWithoutCrash()
    {
      computer.Enqueue(900);

      computer.Execute();

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
      computer.Enqueue(002);
      computer.Enqueue(900);

      computer.Execute();

      Assert.AreEqual(0, computer.Output.Count);
    }

    [Test]
    public void Cardiac_ReadInputWriteToFirstCell_ThrowsBecauseCannotWriteToROMCell()
    {
      computer.Enqueue(000);
      computer.Enqueue(900);

      Assert.Throws<NotSupportedException>(() => computer.Execute());
    }

    [Test]
    public void Cardiac_ReadInputWriteToLastCell_ThrowsBecauseCannotWriteToROMCell()
    {
      computer.Enqueue(099);
      computer.Enqueue(900);

      Assert.Throws<NotSupportedException>(() => computer.Execute());
    }

    [Test]
    public void Cardiac_ReadInputJumpLastCell_EndProgram()
    {
      computer.Enqueue(899);

      computer.Execute();

      Assert.AreEqual(0, computer.Output.Count);
    }

    [Test]
    public void HRS_LoadEndlessProgram_HaltsAndJumpsTo()
    {
      loader.EnqueueProgram(
          903
      );

      computer.Execute();
      computer.Execute();
      computer.Execute();
    }

    [Test]
    public void Cardiac_LoadProgramAndStopWithoutCrash()
    {
      loader.EnqueueProgram(
          900
      );

      computer.Execute();

      Assert.AreEqual(0, computer.Output.Count);
    }

    [Test]
    public void Cardiac_LoadProgram_OutputFirstMemoryCell()
    {
      loader.EnqueueProgram(
          599,
          900
      );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);

      Assert.AreEqual(900, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_StoreEmptyAccumilatorIntoCell_OutputAccumilatorContent()
    {
      loader.EnqueueProgram(
          603,
          503,
          900
          );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(000, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddOne_OutputOne()
    {
      loader.EnqueueProgram(
          200,
          603,
          503,
          900
      );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(01, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddOneTwice_OutputTwo()
    {
      loader.EnqueueProgram(
          200,
          200,
          610,
          510,
          900
      );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(02, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddTen_OutputTen()
    {
      loader.EnqueueProgram(
          010,
          210,
          610,
          510,
          904
      );
      computer.Enqueue(010);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(10, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddAndClearOneTwice_OutputOne()
    {
      loader.EnqueueProgram(
          100,
          100,
          610,
          510,
          900
      );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(01, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddOneAndRemoveOne_OutputZero()
    {
      loader.EnqueueProgram(
          200,
          700,
          610,
          510,
          900
      );

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(00, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddTenAndRemoveOne_OutputNine()
    {
      loader.EnqueueProgram(
          020,
          021,
          220,
          721,
          630,
          530,
          904
      );
      computer.Enqueue(010);
      computer.Enqueue(001);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(009, computer.Output.Dequeue());
    }

    [Test]
    public void CardiacAccumilator_AddZeroAndRemoveOne_OutputMinusOne()
    {
      loader.EnqueueProgram(
        020,
        021,
        220,
        721,
        630,
        530,
        904
      );
      computer.Enqueue(000);
      computer.Enqueue(001);

      computer.Execute();

      Assert.AreEqual(1, computer.Output.Count);
      Assert.AreEqual(-001, computer.Output.Dequeue());
    }
  }
}
