/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CliRunner.Builders
{
    public class ArgumentsBuilder
    {
        private static readonly IFormatProvider DefaultFormatProvider = CultureInfo.InvariantCulture;

        private readonly StringBuilder _buffer;
        
        public ArgumentsBuilder()
        {
            _buffer = new StringBuilder();
        }
        
        private ArgumentsBuilder(StringBuilder buffer)
        {
            this._buffer = buffer;
        }

        public ArgumentsBuilder Add(string value, bool escape)
        {
            _buffer.Append(value);

            if (escape)
            {
                _buffer.Append(Escape(value));
            }
            
            return new ArgumentsBuilder(_buffer);
        }

        public ArgumentsBuilder Add(string value)
        {
            return Add(value, false);
        }

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

        public ArgumentsBuilder Add(IEnumerable<string> values)
        {
            return Add(values, false);
        }

        public ArgumentsBuilder Add(IFormattable value, IFormatProvider formatProvider, bool escape = true)
        {
           string val = (string)formatProvider.GetFormat(value.GetType());
           
           return Add(value.ToString(val, formatProvider), escape);
        }

        public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo, bool escape)
        {
            return Add(value.ToString((string)cultureInfo.GetFormat(value.GetType()), DefaultFormatProvider), escape);
        }

        public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo)
        {
            return Add(value, cultureInfo, false);
        }

        public ArgumentsBuilder Add(IFormattable value, bool escape)
        {
            return Add(value, CultureInfo.CurrentCulture, escape);
        }

        public ArgumentsBuilder Add(IFormattable value)
        {
            return Add(value, false);
        }

        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, IFormatProvider formatProvider, bool escape = true)
        {
            foreach (var val in values)
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

        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo, bool escape)
        {
            foreach (var val in values)
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

        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo)
        {
            return Add(values, cultureInfo, false);
        }

        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, bool escape)
        {
            return Add(values, CultureInfo.CurrentCulture, escape);
        }

        public ArgumentsBuilder Add(IEnumerable<IFormattable> values)
        {
            return Add(values, false);
        }

        public string Build()
        {
            return _buffer.ToString();
        }

        private static string Escape(string argument)
        {
            return argument.Replace("\\", "\\\\")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r")
                .Replace("\"", "\\\"")
                .Replace("'", "\\'");
        }
    }
}