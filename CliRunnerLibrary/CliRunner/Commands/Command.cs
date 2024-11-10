/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
        
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        public ProcessStartInfo? StartInfo { get; }
        
        public bool? SupportsWindows { get; }
        public bool? SupportsLinux { get; }
        public bool? SupportsMac { get; } }
#elif NETSTANDARD2_0
        public ProcessStartInfo StartInfo { get; }
    
        public bool SupportsWindows { get; }
        public bool SupportsLinux { get; }
        public bool SupportsMac { get; } }
#endif
        

}