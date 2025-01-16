﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading.Tasks;

namespace CliRunner.Abstractions
{
    /// <summary>
    /// An interface to allow for a more standardized implementation of Command pipe handling.
    /// </summary>
    public interface ICommandPipeHandler
    {
        /// <summary>
        /// An interface method to asynchronously copy the process' Standard Input to the Command's Standard Input.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination"></param>
        Task PipeStandardInputAsync(Command source, Process destination);

        /// <summary>
        /// An interface method to asynchronously copy the process' Standard Output to the Command's Standard Output.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination"></param>
        Task PipeStandardOutputAsync(Process source, Command destination);

        /// <summary>
        /// An interface method to asynchronously copy the process' Standard Error to the Command's Standard Error.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination"></param>
        Task PipeStandardErrorAsync(Process source, Command destination);
    }
}