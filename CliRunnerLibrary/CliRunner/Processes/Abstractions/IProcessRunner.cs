/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;

namespace CliRunner.Processes.Abstractions
{
    public interface IProcessRunner
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        ProcessResult RunProcessOnWindows(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false,
            bool insertExeInExecutableNameIfMissing = true);
        
        ProcessResult RunProcessOnMac(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);

        ProcessResult RunProcessOnLinux(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);

        ProcessResult RunProcessOnFreeBsd(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);
#elif NETSTANDARD2_0
        ProcessResult RunProcessOnWindows(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false,
            bool insertExeInExecutableNameIfMissing = true);
        
        ProcessResult RunProcessOnMac(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo processStartInfo = null);

        ProcessResult RunProcessOnLinux(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo processStartInfo = null);

        ProcessResult RunProcessOnFreeBsd(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo processStartInfo = null);
#endif
    }
}