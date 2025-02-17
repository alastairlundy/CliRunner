## CliRunner.Extensions.DependencyInjection
This readme covers the **CliRunner Dependency Injections** package. Looking for the [CliRunner Readme](https://github.com/alastairlundy/CliRunner/blob/main/README.md)?

[![NuGet](https://img.shields.io/nuget/v/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)
[![NuGet](https://img.shields.io/nuget/v/CliRunner.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/CliRunner.Extensions.DependencyInjection/)
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/CliRunner.Extensions.DependencyInjection/)

## Usage
CliRunner.Extensions.DependencyInjection provides an extension method to make it easier to use CliRunner with Microsoft.Extensions.DependencyInjection.

The ``AddCliRunner`` IServiceCollection extension method adds CliRunner's interface-able services.

The services injected includes:
* ``IFilePathResolver``
* ``ICommandRunner``
* ``IProcessPipeHandler``
* ``IProcessCreator``
* ``IPipedProcessRunner``
* ``IProcessRunner``
* ``IProcessRunnerUtility``

## Why a separate package?
There's a few different reasons:
* Not everybody necessarily uses Microsoft's Dependency Injection packages.
* Helps de-couple the Dependency Injection extension functionality from the main library

## Licensing
CliRunner.Extensions.DependencyInjection is licensed under the MPL 2.0 license. If you modify any of the package's files then the modified files must be licensed under the MPL 2.0 .

If you use this package in your project please make an exact copy of the contents of the LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.

### Assets
CliRunner's Icon is NOT licensed under the MPL 2.0 license and are licensed under Copyright with all rights reserved to me (Alastair Lundy).

If you fork CliRunner and re-distribute it, please replace the usage of the icon unless you have prior written agreements from me.

## Acknowledgements
This project would like to thank the following projects for their work:
* [Microsoft.Extensions.DependencyInjection.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions) for providing Dependency Injection Abstractions for .NET .

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/CliRunner/blob/main/CliRunnerLibrary/CliRunner.Extensions.DependencyInjection/THIRD_PARTY_NOTICES.txt).
