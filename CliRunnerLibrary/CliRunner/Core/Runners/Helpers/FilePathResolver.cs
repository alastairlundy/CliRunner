/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */


#nullable enable
using System;
using System.IO;
using System.Linq;

using CliRunner.Internal.Localizations;
using CliRunner.Runners.Helpers.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace CliRunner.Runners.Helpers;

public class FilePathResolver : IFilePathResolver
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="outputFilePath"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void ResolveFilePath(string inputFilePath, out string outputFilePath)
    {
        if (string.IsNullOrEmpty(inputFilePath))
        {
            throw new ArgumentNullException(Resources.Exceptions_TargetFile_NullOrEmpty);
        }

        if (File.Exists(inputFilePath))
        {
            outputFilePath = inputFilePath;
            return;
        }
        
        string? path = Environment.GetEnvironmentVariable("PATH");
            
        char pathSeparator = OperatingSystem.IsWindows() ? ';' : ':';

        if (path == null)
        {
            outputFilePath = inputFilePath;
            return;
        }
            
        string[] paths = path.Split(pathSeparator);

        foreach (string pathLine in paths)
        {
            string[] files = Directory.EnumerateFiles(pathLine,
                $"{Path.GetFileName(inputFilePath)}.*",
                SearchOption.TopDirectoryOnly).ToArray();
            
            if (files.Length > 0)
            {
                outputFilePath = files.First();
                return;
            }
        }
        
        throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", inputFilePath));
    }
}