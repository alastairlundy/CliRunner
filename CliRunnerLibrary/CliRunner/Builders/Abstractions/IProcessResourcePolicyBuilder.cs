/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;

namespace CliRunner.Builders.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IProcessResourcePolicyBuilder
{
    IProcessResourcePolicyBuilder WithProcessorAffinity(nint processorAffinity);
    IProcessResourcePolicyBuilder WithMinWorkingSet(nint minWorkingSet);
    IProcessResourcePolicyBuilder WithMaxWorkingSet(nint maxWorkingSet);
    IProcessResourcePolicyBuilder WithPriorityClass(ProcessPriorityClass processPriorityClass);
    IProcessResourcePolicyBuilder WithPriorityBoost(bool enablePriorityBoost);
    ProcessResourcePolicy Build();
}