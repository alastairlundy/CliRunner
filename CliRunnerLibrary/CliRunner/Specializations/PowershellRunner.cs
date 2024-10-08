﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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