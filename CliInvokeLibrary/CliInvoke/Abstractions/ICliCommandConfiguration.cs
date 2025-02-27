/*
    CliInvoke
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    This file also contains some code from CliWrap's ICommandConfiguration.cs that is licensed under the MIT license.
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/ICommandConfiguration.cs


    MIT License

    Copyright (c) 2017-2024 Oleksii Holub

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.Collections.Generic;
using System.IO;
using System.Text;
using AlastairLundy.Extensions.Processes;

namespace AlastairLundy.CliInvoke.Abstractions;

/// <summary>
/// This interface details the properties expected of a Command.
/// </summary>
public interface ICliCommandConfiguration
{
        /// <summary>
        /// Whether administrator privileges are required when executing the Command.
        /// </summary>
        public bool RequiresAdministrator { get; }

        /// <summary>
        /// The file path of the executable to be run and wrapped.
        /// </summary>
        public string TargetFilePath { get; }

        /// <summary>
        /// The working directory path to be used when executing the Command.
        /// </summary>
        public string WorkingDirectoryPath { get; }

        /// <summary>
        /// The arguments to be provided to the executable to be run.
        /// </summary>
        public string Arguments { get; }

        /// <summary>
        /// Whether to enable window creation or not when the Command's Process is run.
        /// </summary>
        public bool WindowCreation { get; }

        /// <summary>
        /// The environment variables to be set.
        /// </summary>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        /// <summary>
        /// The credentials to be used when executing the executable.
        /// </summary>
        public UserCredential Credential { get; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public ProcessResultValidation ResultValidation { get; }

        /// <summary>
        /// The Standard Input source.
        /// </summary>
        public StreamWriter StandardInput { get; }

        /// <summary>
        /// The Standard Output target.
        /// </summary>
        public StreamReader StandardOutput { get; }

        /// <summary>
        /// The Standard Error target.
        /// </summary>
        public StreamReader StandardError { get; }

        /// <summary>
        /// The resource policy to use for executing the Command.
        /// </summary>
        public ProcessResourcePolicy ResourcePolicy { get; }

        /// <summary>
        /// Whether to use Shell Execution or not.
        /// </summary>
        public bool UseShellExecution { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Input.
        /// </summary>
        public Encoding StandardInputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Output.
        /// </summary>
        public Encoding StandardOutputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Error.
        /// </summary>
        public Encoding StandardErrorEncoding { get;  }
}