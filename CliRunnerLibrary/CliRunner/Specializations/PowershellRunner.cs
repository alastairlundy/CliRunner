/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using CliRunner.Processes;
using CliRunner.Processes.Abstractions;
using CliRunner.Specializations.Abstractions;

namespace CliRunner.Specializations
{
    public class PowershellRunner : IPowershellRunner
    {
        public ProcessResult Execute(string commandLine, bool runAsAdministrator)
        {
            throw new NotImplementedException();
        }

        public bool IsInstalled()
        {
            throw new NotImplementedException();
        }

        public Version GetInstalledVersion()
        {
            throw new NotImplementedException();
        }
    }
}