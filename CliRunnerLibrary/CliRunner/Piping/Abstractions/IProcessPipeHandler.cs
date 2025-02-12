/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading.Tasks;

namespace CliRunner.Piping.Abstractions;

/// <summary>
/// An interface to allow for a standardized way of Process pipe handling.
/// </summary>
public interface IProcessPipeHandler
{
    /// <summary>
    /// Asynchronously copies the Command's Standard Input to the process' standard input.
    /// </summary>
    /// <param name="source">The command to be copied from.</param>
    /// <param name="destination">The process to be copied to</param>
    Task PipeStandardInputAsync(Process source, Process destination);

    /// <summary>
    /// Asynchronously copies the process' Standard Output to the Command's Standard Output.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The command to be copied to</param>
    Task PipeStandardOutputAsync(Process source, Process destination);

    /// <summary>
    /// Asynchronously copies the process' Standard Error to the Command's Standard Error.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The command to be copied to</param>
    Task PipeStandardErrorAsync(Process source, Process destination);
}