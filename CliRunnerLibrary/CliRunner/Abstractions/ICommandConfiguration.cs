﻿/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace CliRunner.Abstractions;

/// <summary>
/// 
/// </summary>
public interface ICommandConfiguration
{
    bool RequiresAdministrator { get; }
    string TargetFilePath { get; }
    string WorkingDirectoryPath { get; }

    string Arguments { get; }

    IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

    UserCredentials Credentials { get;  } 
    CommandResultValidation ResultValidation { get;}

    StreamWriter StandardInput { get; }
    StreamReader StandardOutput { get; }
    StreamReader StandardError { get; }
        
    bool UseShellExecution { get; }

    bool WindowCreation { get; }
        
    Encoding StandardInputEncoding { get; }
    Encoding StandardOutputEncoding { get; }
    Encoding StandardErrorEncoding { get; }
        
#if NET6_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
#endif
    IntPtr ProcessorAffinity { get; }
}