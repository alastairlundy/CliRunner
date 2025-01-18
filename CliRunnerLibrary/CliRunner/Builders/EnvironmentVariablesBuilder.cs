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
using System.Diagnostics.Contracts;
using System.Linq;
// ReSharper disable UseCollectionExpression
// ReSharper disable ArrangeObjectCreationWhenTypeEvident
// ReSharper disable RedundantExplicitArrayCreation

namespace CliRunner.Builders;

/// <summary>
/// A class that provides builder methods for constructing Environment Variables.
/// </summary>
public class EnvironmentVariablesBuilder
{
    private readonly Dictionary<string, string> _environmentVariables;

    public EnvironmentVariablesBuilder()
    {
      _environmentVariables  = new Dictionary<string, string>(StringComparer.Ordinal);
    }
        
    /// <summary>
    /// Initializes a new instance of the EnvironmentVariablesBuilder class.
    /// </summary>
    /// <param name="vars">The initial environment variables to use.</param>
    private EnvironmentVariablesBuilder(Dictionary<string, string> vars)
    {
        _environmentVariables = AddToExisting(vars.ToArray());
    }

    /// <summary>
    /// Adds new environment variables to the existing ones.
    /// </summary>
    /// <param name="pairs">The new environment variables to add.</param>
    /// <returns>A dictionary containing the updated environment variables.</returns>
    [Pure]
    private Dictionary<string, string> AddToExisting(IEnumerable<KeyValuePair<string, string>> pairs)
    {
#if NET8_0_OR_GREATER
        Dictionary<string, string> output = new Dictionary<string, string>(pairs);
#else
        Dictionary<string, string> output = new Dictionary<string, string>();
#endif
        
        if (_environmentVariables.Count > 0)
        {
            foreach (KeyValuePair<string, string> pair in _environmentVariables)
            {
                output.Add(pair.Key, pair.Value);
            }
        }
        
#if NETSTANDARD2_0 || NETSTANDARD2_1
        foreach (KeyValuePair<string, string> pair in pairs)
        {
            output.Add(pair.Key, pair.Value);
        }        
#endif

        return output;
    }
        
    /// <summary>
    /// Sets a single environment variable.
    /// </summary>
    /// <param name="name">The name of the environment variable to set.</param>
    /// <param name="value">The value of the environment variable to set.</param>
    /// <returns>A new instance of the EnvironmentVariablesBuilder with the updated environment variables.</returns>
    [Pure]
    public EnvironmentVariablesBuilder Set(string name, string value){
#if NET8_0_OR_GREATER
        Dictionary<string, string> vars = AddToExisting([
            new KeyValuePair<string, string>(name, value) ]);
#else
        Dictionary<string, string> vars = AddToExisting(new KeyValuePair<string, string>[]
            { new KeyValuePair<string, string>(name, value) });
#endif
        return new EnvironmentVariablesBuilder(vars);
    }

    /// <summary>
    /// Sets multiple environment variables.
    /// </summary>
    /// <param name="variables">The environment variables to set.</param>
    /// <returns>A new instance of the EnvironmentVariablesBuilder with the updated environment variables.</returns>
    [Pure]
    public EnvironmentVariablesBuilder Set(IEnumerable<KeyValuePair<string, string>> variables)
    {
        Dictionary<string, string> vars = AddToExisting(variables);

        return new EnvironmentVariablesBuilder(vars);
    }

    /// <summary>
    /// Sets multiple environment variables from a read-only dictionary.
    /// </summary>
    /// <param name="variables">The read-only dictionary of environment variables to set.</param>
    /// <returns>A new instance of the EnvironmentVariablesBuilder with the updated environment variables.</returns>
    [Pure]
    public EnvironmentVariablesBuilder Set(IReadOnlyDictionary<string, string> variables)
    {
        Dictionary<string, string> vars = AddToExisting(variables);

        return new EnvironmentVariablesBuilder(vars);
    }

    /// <summary>
    /// Builds the dictionary of configured environment variables.
    /// </summary>
    /// <returns>A read-only dictionary containing the configured environment variables.</returns>
    public IReadOnlyDictionary<string, string> Build()
    {
        return _environmentVariables;
    }

    /// <summary>
    /// Deletes the environment variable values.
    /// </summary>
    public void Clear()
    {
        _environmentVariables.Clear();
    }
}