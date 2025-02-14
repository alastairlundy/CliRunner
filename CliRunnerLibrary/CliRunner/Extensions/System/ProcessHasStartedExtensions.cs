/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */


using System;
using System.Diagnostics;

namespace CliRunner.Extensions;

public static class ProcessHasStartedExtensions
{
    /// <summary>
    /// Determines whether a process has started or not.
    /// </summary>
    /// <param name="process">The process to be checked.</param>
    /// <returns>True if it has started; false otherwise.</returns>
    public static bool HasStarted(this Process process)
    {
        try
        {
            var startTime = process.StartTime.ToUniversalTime();

            return startTime < DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }
}