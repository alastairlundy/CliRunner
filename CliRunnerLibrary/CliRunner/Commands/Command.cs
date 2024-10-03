/*
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

using System.Collections.Generic;
using System.Diagnostics;

using CliRunner.Commands.Abstractions;
using CliRunner.Processes.Abstractions;

namespace CliRunner.Commands
{
    public class Command : IExecutable
    {
        public Command(string commandName, string filePath, IEnumerable<string> arguments, bool supportsWindows, bool supportsLinux, bool supportsMac)
        {
            this.Name = commandName;
            this.FilePath = filePath;
            this.Arguments = arguments;
            this.SupportsWindows = supportsWindows;
            this.SupportsMac = supportsMac;
            this.SupportsLinux = supportsLinux;
        }

        public Command(string commandName, string filePath, IEnumerable<string> arguments, bool supportsWindows, bool supportsLinux, bool supportsMac, ProcessStartInfo processStartInfo)
        {
            this.Name = commandName;
            this.FilePath = filePath;
            this.Arguments = arguments;
            this.SupportsWindows = supportsWindows;
            this.SupportsMac = supportsMac;
            this.SupportsLinux = supportsLinux;
            this.StartInfo = processStartInfo;
        }
        
        public string Name { get; }
        public string FilePath { get; }
        public IEnumerable<string> Arguments { get; }
        public ProcessStartInfo? StartInfo { get; }
        public bool SupportsWindows { get; }
        public bool SupportsLinux { get; }
        public bool SupportsMac { get; } }
}