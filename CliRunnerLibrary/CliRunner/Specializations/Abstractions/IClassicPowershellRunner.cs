/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;

using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner.Processes;

namespace CliRunner.Specializations.Abstractions
{
    public interface IClassicPowershellRunner
    {
        #if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        #endif
        ProcessResult Execute(string command, bool runAsAdministrator);

        string GetInstallLocation();
        
        bool IsInstalled();
        
        Version GetInstalledVersion();
    }
}