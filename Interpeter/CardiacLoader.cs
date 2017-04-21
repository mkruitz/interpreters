using System.Linq;

namespace Interpeter
{
    public class CardiacLoader
    {
        private readonly IDeviceWithInput inputDevice;

        public CardiacLoader(IDeviceWithInput inputDevice)
        {
            this.inputDevice = inputDevice;
        }

        public void EnqueueProgram(params int[] instructions)
        {
            EnqueueBootLoader();
            if (instructions.Any())
            {
                EnqueueProgramInstructions(instructions);
            }
            EnqueueStartProgram();
        }

        private void EnqueueProgramInstructions(params int[] instructions)
        {
            var addr = 003;
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
            inputDevice.Enqueue(803);
        }
    }
}
