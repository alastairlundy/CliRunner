/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */


#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#endif

using System;
using System.Diagnostics;

namespace CliRunner.Extensions.Processes;

public static class ProcessSetResourcePolicyExtensions
{

    /// <summary>
    /// Applies a ProcessResourcePolicy to a Process.
    /// </summary>
    /// <param name="process">The process to apply the policy to.</param>
    /// <param name="policy">The process resource policy to be applied.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void SetResourcePolicy(this Process process, ProcessResourcePolicy policy)
    {
        if (process.HasStarted())
        {
            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                if (policy.ProcessorAffinity is not null)
                {
                    process.ProcessorAffinity = (IntPtr)policy.ProcessorAffinity;
                }
            }

            if (OperatingSystem.IsMacOS() ||
                OperatingSystem.IsMacCatalyst() ||
                OperatingSystem.IsFreeBSD() ||
                OperatingSystem.IsWindows())
            {
                if (policy.MinWorkingSet != null)
                {
                    process.MinWorkingSet = (nint)policy.MinWorkingSet;
                }

                if (policy.MaxWorkingSet != null)
                {
                    process.MaxWorkingSet = (nint)policy.MaxWorkingSet;
                }
            }
        
            process.PriorityClass = policy.PriorityClass;
            process.PriorityBoostEnabled = policy.EnablePriorityBoost;
        }
        else
        {
            throw new InvalidOperationException("Cannot set Resource Policy to a Process that has not already been started.");
        }
    }
}