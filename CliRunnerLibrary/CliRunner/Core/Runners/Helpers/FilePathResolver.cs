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
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", inputFilePath));
        }
        
        if (File.Exists(inputFilePath) == false)
        {
            bool isPartOfPath = false;
            
            string? pathLocation = null;

            if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                string? path = Environment.GetEnvironmentVariable("PATH");

                if (path != null)
                {
                    char separator = OperatingSystem.IsWindows() ? ';' : ':';
                    
                    string[] lines = path.Split(separator);
                    
                    isPartOfPath = lines.Any(x => x.EndsWith(inputFilePath + Path.DirectorySeparatorChar) || x.EndsWith(inputFilePath));

                    if (isPartOfPath)
                    {
                        string actual = Path.GetFullPath(lines.First(x => x.EndsWith(inputFilePath + Path.DirectorySeparatorChar) || x.EndsWith(inputFilePath)));

                        bool exactFileExists =  Directory.GetFiles(actual).Select(x => Path.GetFileNameWithoutExtension(x))
                            .Any(x => x.Equals(inputFilePath));

                        if (exactFileExists)
                        {
                            string exactFile = Directory.GetFiles(actual).First(x => Path.GetFileNameWithoutExtension(x).Equals(inputFilePath));
                            pathLocation = Path.Combine(actual, exactFile);
                        }
                    }
                    else
                    {
                        pathLocation = null;
                    }
                }
                else
                {
                    isPartOfPath = false;
                }
            }

            if (isPartOfPath && pathLocation != null && string.IsNullOrEmpty(pathLocation) == false)
            {
                outputFilePath = pathLocation;
            }

            if (File.Exists(inputFilePath) == false)
            {
                throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", inputFilePath));
            }

            outputFilePath = inputFilePath;
        }
        else
        {
            outputFilePath = inputFilePath;
        }
    }
}