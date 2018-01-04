using System.Collections.Generic;
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

    private void AssertAndDequeueProgram(Queue<int> actualQueue, params int[][] expectedInstructionRanges)
    {
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
