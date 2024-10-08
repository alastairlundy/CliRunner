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