/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

namespace CliRunner.Commands.Abstractions
{
    public interface ICommandRunner
    { 
        ProcessResult RunCommandOnMac(string command, bool runAsAdministrator = false);
        
        ProcessResult RunCommandOnMac(Command command, bool runAsAdministrator = false);

        ProcessResult RunCommandOnLinux(string command, bool runAsAdministrator = false);
        
        ProcessResult RunCommandOnLinux(Command command, bool runAsAdministrator = false);

        ProcessResult RunCommandOnFreeBsd(string command, bool runAsAdministrator = false);
        
        ProcessResult RunCommandOnFreeBsd(Command command, bool runAsAdministrator = false);
    }
}