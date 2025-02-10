/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;

namespace CliRunner;

/// <summary>
/// 
/// </summary>
public class ProcessResourcePolicy
{
    /// <summary>
    /// 
    /// </summary>
    public ProcessResourcePolicy(nint processorAffinity = default(nint),
        nint? minWorkingSet = null, 
        nint? maxWorkingSet = null,
        ProcessPriorityClass priorityClass = ProcessPriorityClass.Normal,
        bool enablePriorityBoost = true)
    {
        MinWorkingSet = minWorkingSet;
        MaxWorkingSet = maxWorkingSet;
        ProcessorAffinity = processorAffinity;
        PriorityClass = priorityClass;
        EnablePriorityBoost = enablePriorityBoost;
    }

    /// <summary>
    /// 
    /// </summary>
    public IntPtr ProcessorAffinity { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public ProcessPriorityClass PriorityClass { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool EnablePriorityBoost { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public nint? MinWorkingSet { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public nint? MaxWorkingSet { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public static ProcessResourcePolicy Default { get; } = new ProcessResourcePolicy();
}