/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System;
using AlastairLundy.Extensions.IO.Files;
using AlastairLundy.Extensions.IO.Files.Abstractions;
using AlastairLundy.Extensions.Processes;
using AlastairLundy.Extensions.Processes.Abstractions;
using AlastairLundy.Extensions.Processes.Piping;
using AlastairLundy.Extensions.Processes.Piping.Abstractions;
using AlastairLundy.Extensions.Processes.Utilities;
using AlastairLundy.Extensions.Processes.Utilities.Abstractions;

using CliRunner.Abstractions;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable RedundantAssignment
// ReSharper disable UnusedMember.Global

namespace CliRunner.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Sets up Dependency Injection for CliRunner's main interface-able types.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="lifetime">The service lifetime to use if specified; Singleton otherwise.</param>
    /// <returns>The updated service collection with the added CliRunner dependency injection.</returns>
    public static IServiceCollection AddCliRunner(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    { 
        services.Add(lifetime, typeof(IFilePathResolver), typeof(FilePathResolver));
        
        services.Add(lifetime, typeof(IProcessRunnerUtility), typeof(ProcessRunnerUtility));
        services.Add(lifetime, typeof(IPipedProcessRunner), typeof(PipedProcessRunner));
        services.Add(lifetime, typeof(IProcessRunner), typeof(ProcessRunner));
        services.Add(lifetime, typeof(IProcessCreator), typeof(ProcessCreator));
        services.Add(lifetime, typeof(IProcessPipeHandler), typeof(ProcessPipeHandler));
        
        services.Add(lifetime, typeof(ICliCommandRunner), typeof(CliCommandRunner));
        return services;
    }

    private static void Add(this IServiceCollection services, ServiceLifetime lifetime, Type serviceType,
        Type implementationType)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services = services.AddSingleton(serviceType, implementationType);
                break;
            case ServiceLifetime.Scoped:
                services = services.AddScoped(serviceType, implementationType);
                break;
            case ServiceLifetime.Transient:
                services = services.AddTransient(serviceType, implementationType);
                break;
            default:
                services = services.AddSingleton(serviceType, implementationType);
                break;
        }
    }
}