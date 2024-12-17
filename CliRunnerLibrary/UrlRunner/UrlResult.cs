/*
    UrlRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using CliRunner.Commands;

namespace UrlRunner
{
    /// <summary>
    /// A class to represent a URL being opened as a result of a Command being executed.
    /// </summary>
    public class UrlResult : CommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exitCode">The exitcode of the Command that was executed.</param>
        /// <param name="startTime"></param>
        /// <param name="exitTime"></param>
        /// <param name="openedUrl"></param>
        public UrlResult(int exitCode, DateTime startTime, DateTime exitTime, Url openedUrl) : base(exitCode, startTime, exitTime)
        {
            OpenedUrl = openedUrl;
        }

        /// <summary>
        /// The URL that was opened.
        /// </summary>
        public Url OpenedUrl { get; protected set; }
    }
}