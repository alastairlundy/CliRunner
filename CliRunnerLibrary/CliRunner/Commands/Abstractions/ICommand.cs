/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace CliRunner.Commands.Abstractions;

/// <summary>
/// An interface representing a more complete Command object without any static operators.
/// </summary>
public interface ICommand : ICommandConfiguration, ICommandConfigurationBuilder
{
    
}