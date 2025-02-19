/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */


using System.Diagnostics;

namespace CliRunner.Abstractions;

/// <summary>
/// An interface to enable Creating Processes from Command Configuration inputs.
/// </summary>
public interface IProcessCreator
{
    /// <summary>
    /// Creates a process with the specified process start information.
    /// </summary>
    /// <param name="processStartInfo">The process start information to be used to configure the process to be created.</param>
    /// <returns>The newly created Process with the specified start information.</returns>
    Process CreateProcess(ProcessStartInfo processStartInfo);

    /// <summary>
    /// Creates Process Start Information based on specified Command object values.
    /// </summary>
    /// <param name="commandConfiguration">The command object to specify Process info.</param>
    /// <returns>A new ProcessStartInfo object configured with the specified Command object values.</returns>
    ProcessStartInfo CreateStartInfo(ICliCommandConfiguration commandConfiguration);

    /// <summary>
    /// Creates Process Start Information based on specified parameters and Command object values.
    /// </summary>
    /// <param name="commandConfiguration">The command object to specify Process info.</param>
    /// <param name="redirectStandardOutput">Whether to redirect the Standard Output.</param>
    /// <param name="redirectStandardError">Whether to redirect the Standard Error.</param>
    /// <returns>A new ProcessStartInfo object configured with the specified parameters and Command object values.</returns>
    ProcessStartInfo CreateStartInfo(ICliCommandConfiguration commandConfiguration, bool redirectStandardOutput, bool redirectStandardError);

}