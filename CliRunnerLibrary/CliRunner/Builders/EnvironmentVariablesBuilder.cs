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
using System.Linq;

namespace CliRunner.Builders
{
    public class EnvironmentVariablesBuilder
    {
        private readonly Dictionary<string, string> _environmentVariables = new Dictionary<string, string>(StringComparer.Ordinal);

        private EnvironmentVariablesBuilder()
        {
            
        }

        private Dictionary<string, string> AddToExisting(KeyValuePair<string, string>[] pairs)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> pair in _environmentVariables)
            {
                output.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<string, string> newPair in pairs)
            {
                output.Add(newPair.Key, newPair.Value);
            }

            return output;
        }
        
        private EnvironmentVariablesBuilder(Dictionary<string, string> vars)
        {
            foreach (KeyValuePair<string, string> pair in vars)
            {
                _environmentVariables.Add(pair.Key, pair.Value);
            }
        }
        
        public EnvironmentVariablesBuilder Set(string name, string value)
        {
            Dictionary<string, string> vars = AddToExisting(new KeyValuePair<string, string>[]
                { new KeyValuePair<string, string>(name, value) });

            return new EnvironmentVariablesBuilder(vars);
        }

        public EnvironmentVariablesBuilder Set(IEnumerable<KeyValuePair<string, string>> variables)
        {
            Dictionary<string, string> vars = AddToExisting(variables.ToArray());

            return new EnvironmentVariablesBuilder(vars);
        }

        public EnvironmentVariablesBuilder Set(IReadOnlyDictionary<string, string> variables)
        {
            Dictionary<string, string> vars = AddToExisting(variables.ToArray());

            return new EnvironmentVariablesBuilder(vars);
        }

        public IReadOnlyDictionary<string, string> Build()
        {
            return _environmentVariables;
        }
    }
}