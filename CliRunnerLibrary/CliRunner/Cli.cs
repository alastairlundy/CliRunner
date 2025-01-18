/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Cli.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Cli.cs

     Method signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace CliRunner
{
    /// <summary>
    /// A static class to enable creating a new Command object in a more pretty fashion.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    [Obsolete("This method is obsolete and will be removed in future versions. Use Command's static CreateInstance methods directly instead.")]
    public static class Cli
    {
        /// <summary>
        /// Creates a Command object with the specified target file path. 
        /// </summary>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>.WithArguments("[your args]")</code> and <code>.WithWorkingDirectory("[your directory]")</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        /// <param name="targetFilePath">The target file path of the executable to wrap.</param>
        /// <returns>A new Command object with the configured Target File Path.</returns>
        [Pure]
        [Obsolete("This method is obsolete and will be removed in future versions. Use Wrap instead.")]
        public static Command Run(string targetFilePath) => Command.CreateInstance(targetFilePath);
        
        /// <summary>
        /// Used to wrap an existing Command object when a modified version is desired.
        /// </summary>
        /// <param name="command">The command to wrap</param>
        /// <returns>A new Command object with the configured Command information passed to it.</returns>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>WithArguments("[your args]");</code> and <code>WithWorkingDirectory("[your directory]");</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        [Pure]
        [Obsolete("This method is obsolete and will be removed in future versions. Use Wrap instead.")]
        public static Command Run(Command command) => Command.CreateInstance(command);
    }
}