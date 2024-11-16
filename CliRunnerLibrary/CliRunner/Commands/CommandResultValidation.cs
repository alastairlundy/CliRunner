using System;

namespace CliRunner.Commands
{
    [Flags]
    public enum CommandResultValidation
    {
        None = 0b0,
        ExitCodeZero = 0b1,
    }
}