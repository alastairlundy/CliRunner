﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Runtime.Versioning;
using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

namespace CliRunner.Specializations.Abstractions
{
    public interface IPowershellRunner
    {
        #if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        #endif
        ProcessResult Execute(string commandLine, bool runAsAdministrator);

        bool IsInstalled();
        
        Version GetInstalledVersion();
    }
}