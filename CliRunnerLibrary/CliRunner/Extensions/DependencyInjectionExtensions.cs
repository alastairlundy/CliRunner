/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System;

using CliRunner.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace CliRunner.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Sets up Dependency Injection for CliRunner's main interface-able types.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddCliRunner(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Add(lifetime, typeof(ICommand), typeof(Command));
        services.Add(lifetime, typeof(ICommandPipeHandler), typeof(CommandPipeHandler));
        services.Add(lifetime, typeof(ICommandRunner), typeof(CommandRunner));
        return services;
    }

    private static IServiceCollection Add(this IServiceCollection services, ServiceLifetime lifetime, Type serviceType, Type implementationType)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(serviceType, implementationType);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(serviceType, implementationType);
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(serviceType, implementationType);
                break;
        }
        return services;
    }
}