/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Globalization;

namespace CliRunner.Extensions;

public static class FormattableToStringExtensions
{
    /// <summary>
    /// Converts an IFormattable to a string.
    /// </summary>
    /// <param name="formattable">The IFormattable to be converted to a string.</param>
    /// <returns>The newly created string.</returns>
    public static string ToString(this IFormattable formattable)
    {
        return (string)CultureInfo.InvariantCulture.GetFormat(formattable.GetType());
    }
}