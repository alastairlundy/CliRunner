using System;

namespace CliRunner.Processes
{
    [Flags]
    public enum CommandResultValidation
    {
        None = 0b0,
        ExitCodeZero = 0b1,
    }
}