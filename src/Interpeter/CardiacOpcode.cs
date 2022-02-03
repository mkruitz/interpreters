namespace Interpreter
{
  public enum CardiacOpcode
  {
    INP = 0, // INP Input
             //     take a number from the input card and put it in a specified memory cell.
    CLA = 1, // CLA Clear and add
             //     clear the accumulator and add the contents of a memory cell to the accumulator.
    ADD = 2, // ADD Add
             //     add the contents of a memory cell to the accumulator.
    TAC = 3, // TAC Test accumulator contents
             //     performs a sign test on the contents of the accumulator;
             //     if minus, jump to a specified memory cell.
    SFT = 4, // SFT Shift
             //     shifts the accumulator x places left, then y places right, where x is
             //     the upper address digit and y is the lower.
    OUT = 5, // OUT Output
             //     take a number from the specified memory cell and write it on the output card.
    STO = 6, // STO Store
             //     copy the contents of the accumulator into a specified memory cell.
    SUB = 7, // SUB Subtract
             //     subtract the contents of a specified memory cell from the accumulator.
    JMP = 8, // JMP Jump
             //     jump to a specified memory cell. The current cell number is written in cell 99.
             //     This allows for one level of subroutines by having the return be the instruction
             //     at cell 99 (which had '8' hardcoded as the first digit.
    HRS = 9  // HRS Halt and reset
             //     move bug to the specified cell, then stop program execution.
  }
}