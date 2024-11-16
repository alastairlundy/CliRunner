/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Collections.Generic;
using System.Diagnostics;
using CliRunner.Commands;

namespace CliRunner.Processes.Abstractions
{
    public interface IProcessRunner
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        CommandResult RunProcessOnWindows(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false,
            bool insertExeInExecutableNameIfMissing = true);
        
        CommandResult RunProcessOnMac(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false);

        CommandResult RunProcessOnLinux(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false);

        CommandResult RunProcessOnFreeBsd(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false);
#elif NETSTANDARD2_0
        CommandResult RunProcessOnWindows(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false,
            bool insertExeInExecutableNameIfMissing = true);
        
        CommandResult RunProcessOnMac(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false);

        CommandResult RunProcessOnLinux(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false);

        CommandResult RunProcessOnFreeBsd(string executableLocation, string executableName, IEnumerable<string> arguments,
            ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false);
#endif
    }
}