namespace Interpreter
{
  public class CardiacLoader
  {
    private readonly IDeviceWithInput inputDevice;
    private int startAddress = 003;
    private bool isBootLoaderEnqueed;

    public CardiacLoader(IDeviceWithInput inputDevice)
    {
      this.inputDevice = inputDevice;
    }

    public void StartAt(int address)
    {
      startAddress = address;
    }

    public void EnqueueProgram(params int[] instructions)
    {
      EnqueueSubRoutine(instructions);
      EnqueueStartProgram();
    }

    public void EnqueueSubRoutine(params int[] instructions)
    {
      EnqueueBootLoader();
      var addr = startAddress;
      foreach (var instruction in instructions)
      {
        inputDevice.Enqueue(addr);
        inputDevice.Enqueue(instruction);
        addr++;
      }
    }

    private void EnqueueBootLoader()
    {
      if (isBootLoaderEnqueed)
      {
        return;
      }

      inputDevice.Enqueue(002);
      inputDevice.Enqueue(800);

      isBootLoaderEnqueed = true;
    }

    private void EnqueueStartProgram()
    {
      var startAdressJumpInstruction = 800 + startAddress;
      inputDevice.Enqueue(startAdressJumpInstruction);
    }
  }
}
