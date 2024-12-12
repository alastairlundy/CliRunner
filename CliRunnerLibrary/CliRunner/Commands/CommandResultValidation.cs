/*
    Based on Tyrrrz's CliWrap CommandResultValidation.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResultValidation.cs

     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

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