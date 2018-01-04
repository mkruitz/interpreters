using System.Collections.Generic;
using System.Linq;
using Interpeter;
using NUnit.Framework;

namespace Tests
{
  [TestFixture]
  public class CardiacLoaderTests
  {
    private static readonly int[] BootLoader = { 002, 800 };
    private static readonly int[] ProgramStart = { 803 };

    private CardiacLoader loader;
    private DeviceWithInputStub stub;

    [SetUp]
    public void SetUp()
    {
      stub = new DeviceWithInputStub();
      loader = new CardiacLoader(stub);
    }

    [Test]
    public void Loader_NoProgram_QueueWithBootLoaderAndProgramStart()
    {
      loader.EnqueueProgram();

      AssertAndDequeueProgram(stub.InputsQueued,
          BootLoader,
          ProgramStart
      );
    }

    [Test]
    public void Loader_OneInstruction_QueueWithBootLoaderAndLoadInstructionsAndProgramStart()
    {
      loader.EnqueueProgram(
          000
          );

      AssertAndDequeueProgram(stub.InputsQueued,
          BootLoader,
          new[] { 003, 000 },
          ProgramStart
      );
    }

    [Test]
    public void Loader_TwoInstructions_QueueWithBootLoaderAndLoadInstructionsAndProgramStart()
    {
      loader.EnqueueProgram(
          000,
          999
      );

      AssertAndDequeueProgram(stub.InputsQueued,
          BootLoader,
          new[] { 003, 000 },
          new[] { 004, 999 },
          ProgramStart
      );
    }

    [Test]
    public void Loader_SetStartPosition_TwoInstructions_ProgramStartsAtRequestedPosition()
    {
      loader.StartAt(050);
      loader.EnqueueProgram(
        000,
        999
      );

      AssertAndDequeueProgram(stub.InputsQueued,
        BootLoader,
        new[] { 050, 000 },
        new[] { 051, 999 },
        new [] { 850 } // ProgramStart
      );
    }

    [Test]
    public void Loader_AddSubRoutine_StartsWithBootLoader()
    {
      loader.StartAt(050);
      loader.EnqueueSubRoutine(
        999
      );

      AssertAndDequeueProgram(stub.InputsQueued,
        BootLoader,
        new[] { 050, 999 }
      );
    }

    [Test]
    public void Loader_AddTwoSubRoutines_OnlyAddBootLoaderOnce()
    {
      loader.StartAt(050);
      loader.EnqueueSubRoutine(
        999
      );
      loader.StartAt(060);
      loader.EnqueueSubRoutine(
        999
      );

      AssertAndDequeueProgram(stub.InputsQueued,
        BootLoader,
        new[] { 050, 999 },
        new[] { 060, 999 }
      );
    }

    private void AssertAndDequeueProgram(Queue<int> actualQueue, params int[][] expectedInstructionRanges)
    {
      Assert.AreEqual(expectedInstructionRanges.Sum(range => range.Length), actualQueue.Count, "Number of instruction are not equal");

      foreach (var expectedInstructionRange in expectedInstructionRanges)
      {
        AssertAndDequeueInstructions(actualQueue, expectedInstructionRange);
      }

      AssertQueueEmpty(actualQueue);
    }

    private void AssertAndDequeueInstructions(Queue<int> actualQueue, params int[] expectedInstructions)
    {
      foreach (var expectedInstruction in expectedInstructions)
      {
        Assert.AreEqual(expectedInstruction, actualQueue.Dequeue());
      }
    }

    private void AssertQueueEmpty(Queue<int> actualQueue)
    {
      Assert.AreEqual(0, actualQueue.Count);
    }

    public class DeviceWithInputStub : IDeviceWithInput
    {
      public readonly Queue<int> InputsQueued = new Queue<int>();

      public void Enqueue(int input)
      {
        InputsQueued.Enqueue(input);
      }
    }
  }
}
