/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace CliRunner.Builders;

/// <summary>
/// A class that provides a fluent interface style builder for constructing Arguments to provide to a program.
/// </summary>
public class ArgumentsBuilder
{
    private static readonly IFormatProvider DefaultFormatProvider = CultureInfo.InvariantCulture;

    private readonly StringBuilder _buffer;
        
    public ArgumentsBuilder()
    {
        _buffer = new StringBuilder();
    }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffer"></param>
    private ArgumentsBuilder(StringBuilder buffer)
    {
        this._buffer = buffer;
    }

    /// <summary>
    /// Appends a string value to the arguments builder.
    /// </summary>
    /// <param name="value">The string value to append.</param>
    /// <param name="escape">True to escape special characters in the value, false otherwise.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(string value, bool escape)
    {
        _buffer.Append(value);

        if (escape)
        {
            _buffer.Append(Escape(value));
        }
            
        return new ArgumentsBuilder(_buffer);
    }

    /// <summary>
    /// Appends a string value to the arguments builder without escaping special characters.
    /// </summary>
    /// <param name="value">The string value to append.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(string value)
    {
        return Add(value, false);
    }

    /// <summary>
    /// Appends a collection of string values to the arguments builder.
    /// </summary>
    /// <param name="values">The collection of string values to append.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<string> values, bool escape)
    {
        string[] enumerable = values as string[] ?? values.ToArray();
            
        for(int index = 0; index < enumerable.Length; index++)
        {
            _buffer.Append(enumerable[index]);

            if (escape)
            {
                _buffer.Append(Escape(enumerable[index]));
            }
        }

        return new ArgumentsBuilder(_buffer);
    }

    /// <summary>
    /// Appends a collection of string values to the arguments builder without escaping special characters.
    /// </summary>
    /// <param name="values">The collection of string values to append.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<string> values)
    {
        return Add(values, false);
    }

    /// <summary>
    /// Appends a formattable value to the arguments builder.
    /// </summary>
    /// <param name="value">The formattable value to append.</param>
    /// <param name="formatProvider">The format provider to use for formatting the value.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IFormattable value, IFormatProvider formatProvider, bool escape = true)
    {
        string val = (string)formatProvider.GetFormat(value.GetType());
           
        return Add(value.ToString(val, formatProvider), escape);
    }

    /// <summary>
    /// Appends a formattable value to the arguments builder using the specified culture.
    /// </summary>
    /// <param name="value">The formattable value to append.</param>
    /// <param name="cultureInfo">The culture to use for formatting the value.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo, bool escape)
    {
        return Add(value.ToString((string)cultureInfo.GetFormat(value.GetType()), DefaultFormatProvider), escape);
    }
        
    /// <summary>
    /// Appends a formattable value to the arguments builder using the specified culture without escaping special characters.
    /// </summary>
    /// <param name="value">The formattable value to append.</param>
    /// <param name="cultureInfo">The culture to use for formatting the value.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo)
    {
        return Add(value, cultureInfo, false);
    }

    /// <summary>
    /// Appends a formattable value to the arguments builder.
    /// </summary>
    /// <param name="value">The formattable value to append.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IFormattable value, bool escape)
    {
        return Add(value, CultureInfo.CurrentCulture, escape);
    }

    /// <summary>
    /// Appends a formattable value to the arguments builder without specifying a culture and without escaping special characters.
    /// </summary>
    /// <param name="value">The formattable value to append.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IFormattable value)
    {
        return Add(value, false);
    }
        
    /// <summary>
    /// Appends a collection of formattable values to the arguments builder.
    /// </summary>
    /// <param name="values">The collection of formattable values to append.</param>
    /// <param name="formatProvider">The format provider to use for formatting the values.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<IFormattable> values, IFormatProvider formatProvider, bool escape = true)
    {
        foreach (IFormattable val in values)
        {
            string newVal = (string)formatProvider.GetFormat(val.GetType());
           
            newVal = val.ToString(newVal, formatProvider);
                
            _buffer.Append(newVal);

            if (escape)
            {
                _buffer.Append(Escape(newVal));
            }
        }

        return new ArgumentsBuilder(_buffer);
    }

    /// <summary>
    /// Appends a collection of formattable values to the arguments builder using the specified culture.
    /// </summary>
    /// <param name="values">The collection of formattable values to append.</param>
    /// <param name="cultureInfo">The culture to use for formatting the values.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo, bool escape)
    {
        foreach (IFormattable val in values)
        {
            string newVal = val.ToString((string)cultureInfo.GetFormat(val.GetType()), DefaultFormatProvider);
                
            _buffer.Append(newVal);

            if (escape)
            {
                _buffer.Append(Escape(newVal));
            }
        }

        return new ArgumentsBuilder(_buffer);
    }

    /// <summary>
    /// Appends a collection of formattable values to the arguments builder using the specified culture without escaping special characters.
    /// </summary>
    /// <param name="values">The collection of formattable values to append.</param>
    /// <param name="cultureInfo">The culture to use for formatting the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo)
    {
        return Add(values, cultureInfo, false);
    }
        
    /// <summary>
    /// Appends a collection of formattable values to the arguments builder without specifying a culture.
    /// </summary>
    /// <param name="values">The collection of formattable values to append.</param>
    /// <param name="escape">Whether to escape special characters in the values.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<IFormattable> values, bool escape)
    {
        return Add(values, CultureInfo.CurrentCulture, escape);
    }

    /// <summary>
    /// Appends a collection of formattable values to the arguments builder without specifying a culture and without escaping special characters.
    /// </summary>
    /// <param name="values">The collection of formattable values to append.</param>
    /// <returns>A new instance of the ArgumentsBuilder with the updated arguments.</returns>
    [Pure]
    public ArgumentsBuilder Add(IEnumerable<IFormattable> values)
    {
        return Add(values, false);
    }

    /// <summary>
    /// Builds the arguments into a string.
    /// </summary>
    /// <returns>The arguments as a string.</returns>
    public string Build()
    {
        return _buffer.ToString();
    }

    /// <summary>
    /// Escapes special characters in a string.
    /// </summary>
    /// <param name="argument">The string to escape.</param>
    /// <returns>The escaped string.</returns>
    [Pure]
    public static string Escape(string argument)
    {
        return argument.Replace("\\", "\\\\")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t")
            .Replace("\r", "\\r")
            .Replace("\"", "\\\"")
            .Replace("'", "\\'");
    }

    /// <summary>
    /// Clears the provided argument strings.
    /// </summary>
    public void Clear()
    {
        _buffer.Clear();
    }
}