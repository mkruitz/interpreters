namespace Interpeter
{
  public class CardiacLoader
  {
    private readonly IDeviceWithInput inputDevice;
    private int startAddress = 003;

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
      EnqueueBootLoader();
      EnqueueProgramInstructions(instructions);
      EnqueueStartProgram();
    }

    private void EnqueueProgramInstructions(params int[] instructions)
    {
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
      inputDevice.Enqueue(002);
      inputDevice.Enqueue(800);
    }

    private void EnqueueStartProgram()
    {
      var startAdressJumpInstruction = 800 + startAddress;
      inputDevice.Enqueue(startAdressJumpInstruction);
    }
  }
}
