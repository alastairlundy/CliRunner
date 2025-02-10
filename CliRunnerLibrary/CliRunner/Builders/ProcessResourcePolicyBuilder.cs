/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;

using CliRunner.Builders.Abstractions;

namespace CliRunner.Builders;

/// <summary>
/// 
/// </summary>
public class ProcessResourcePolicyBuilder : IProcessResourcePolicyBuilder
{
    private readonly ProcessResourcePolicy _processResourcePolicy;

    /// <summary>
    /// 
    /// </summary>
    public ProcessResourcePolicyBuilder()
    {
        _processResourcePolicy = new ProcessResourcePolicy();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processResourcePolicy"></param>
    private ProcessResourcePolicyBuilder(ProcessResourcePolicy processResourcePolicy)
    {
        _processResourcePolicy = processResourcePolicy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processorAffinity"></param>
    /// <returns></returns>
    public IProcessResourcePolicyBuilder WithProcessorAffinity(nint processorAffinity) =>
        new ProcessResourcePolicyBuilder(new ProcessResourcePolicy(
            processorAffinity,
            _processResourcePolicy.MinWorkingSet,
            _processResourcePolicy.MaxWorkingSet,
            _processResourcePolicy.PriorityClass,
            _processResourcePolicy.EnablePriorityBoost));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minWorkingSet"></param>
    /// <returns></returns>
    public IProcessResourcePolicyBuilder WithMinWorkingSet(nint minWorkingSet) =>
        new ProcessResourcePolicyBuilder(new ProcessResourcePolicy(
            _processResourcePolicy.ProcessorAffinity,
            minWorkingSet,
            _processResourcePolicy.MaxWorkingSet,
            _processResourcePolicy.PriorityClass,
            _processResourcePolicy.EnablePriorityBoost));
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxWorkingSet"></param>
    /// <returns></returns>
    public IProcessResourcePolicyBuilder WithMaxWorkingSet(nint maxWorkingSet) =>
        new ProcessResourcePolicyBuilder(new ProcessResourcePolicy(
            _processResourcePolicy.ProcessorAffinity,
            _processResourcePolicy.MinWorkingSet,
            maxWorkingSet,
            _processResourcePolicy.PriorityClass,
            _processResourcePolicy.EnablePriorityBoost));
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="processPriorityClass"></param>
    /// <returns></returns>
    public IProcessResourcePolicyBuilder WithPriorityClass(ProcessPriorityClass processPriorityClass) =>
        new ProcessResourcePolicyBuilder(new ProcessResourcePolicy(
            _processResourcePolicy.ProcessorAffinity,
            _processResourcePolicy.MinWorkingSet,
            _processResourcePolicy.MaxWorkingSet,
            processPriorityClass,
            _processResourcePolicy.EnablePriorityBoost));
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enablePriorityBoost"></param>
    /// <returns></returns>
    public IProcessResourcePolicyBuilder WithPriorityBoost(bool enablePriorityBoost) =>
        new ProcessResourcePolicyBuilder(new ProcessResourcePolicy(
            _processResourcePolicy.ProcessorAffinity,
            _processResourcePolicy.MinWorkingSet,
            _processResourcePolicy.MaxWorkingSet,
            _processResourcePolicy.PriorityClass,
            enablePriorityBoost));
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ProcessResourcePolicy Build()
    {
        return _processResourcePolicy;
    }
}