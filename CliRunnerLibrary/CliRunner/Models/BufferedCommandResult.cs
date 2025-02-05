/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap BufferedCommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Buffered/BufferedCommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
// ReSharper disable MemberCanBePrivate.Global

namespace CliRunner
{
    /// <summary>
    /// A buffered CommandResult containing a Command's StandardOutput and StandardError information.
    /// </summary>
    public class BufferedCommandResult(
        int exitCode,
        string standardOutput,
        string standardError,
        DateTime startTime,
        DateTime exitTime)
        : CommandResult(exitCode, startTime, exitTime), IEquatable<BufferedCommandResult>
    {
        /// <summary>
        /// The Standard Output from the Command represented as a string.
        /// </summary>
        public string StandardOutput { get; } = standardOutput;

        /// <summary>
        /// The Standard Error from the Command represented as a string.
        /// </summary>
        public string StandardError { get; } = standardError;

        
        /// <summary>
        /// Determines whether this BufferedCommandResult is equal to another BufferedCommandResult object.
        /// </summary>
        /// <remarks>This method intentionally does not consider Start and Exit times of Command Results for the purposes of equality comparison.</remarks>
        /// <param name="other">The other BufferedCommandResult to compare.</param>
        /// <returns>True if this BufferedCommandResult is equal to the other BufferedCommandResult; false otherwise.</returns>
        public bool Equals(BufferedCommandResult other)
        {
            if (other is null)
            {
                return false;
            }
            
            return StandardOutput == other.StandardOutput &&
                   StandardError == other.StandardError &&
                   ExitCode == other.ExitCode;
        }

        /// <summary>
        /// Determines whether this BufferedCommandResult is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>True if the other object is a BufferedCommandResult and is equal to this BufferedCommandResult; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != typeof(BufferedCommandResult))
            {
                return false;
            }
            
            return Equals((BufferedCommandResult)obj);
        }

        /// <summary>
        /// Returns the hash code for the current BufferedCommandResult.
        /// </summary>
        /// <returns>The hash code for the current BufferedCommandResult.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(StandardOutput, StandardError, ExitCode);
        }

        /// <summary>
        /// Determines whether two BufferedCommandResults are equal.
        /// </summary>
        /// <param name="left">The first BufferedCommandResult to compare.</param>
        /// <param name="right">The second BufferedCommandResult to compare.</param>
        /// <returns>True if the two BufferedCommandResult objects are equal; false otherwise.</returns>
        public static bool Equals(BufferedCommandResult left, BufferedCommandResult right)
        {
            return left.Equals(right);
        }
        
        /// <summary>
        /// Determines if a BufferedCommandResult is equal to another BufferedCommandResult.
        /// </summary>
        /// <param name="left">A BufferedCommandResult to be compared.</param>
        /// <param name="right">The other BufferedCommandResult to be compared.</param>
        /// <returns>True if both BufferedCommandResults are equal to each other; false otherwise.</returns>
        public static bool operator ==(BufferedCommandResult left, BufferedCommandResult right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if a BufferedCommandResult is not equal to another BufferedCommandResult.
        /// </summary>
        /// <param name="left">A BufferedCommandResult to be compared.</param>
        /// <param name="right">The other BufferedCommandResult to be compared.</param>
        /// <returns>True if both BufferedCommandResults are not equal to each other; false otherwise.</returns>
        public static bool operator !=(BufferedCommandResult left, BufferedCommandResult right)
        {
            return Equals(left, right) == false;
        }
    }
}