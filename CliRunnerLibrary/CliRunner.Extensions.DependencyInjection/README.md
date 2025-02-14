## CliRunner.Extensions.DependencyInjection
This readme covers the **CliRunner Dependency Injections** package. Looking for the [CliRunner Readme](https://github.com/alastairlundy/CliRunner/blob/main/README.md)?

## Usage
CliRunner.Extensions.DependencyInjection provides an extension method to make it easier to use CliRunner with Microsoft.Extensions.DependencyInjection.

The ``AddCliRunner`` IServiceCollection extension method adds CliRunner's interface-able services.

The services injected includes:
* ``ICommandRunner``
* ``IProcessPipeHandler``
* ``IProcessCreator``
* ``IPipedProcessRunner``
* ``IProcessRunner``
* ``IProcessRunnerUtility``

## Why a separate package?
There's a few different reasons:
* Not everybody necessarily uses Microsoft's Dependency Injection packages.


## Licensing
CliRunner.Extensions.DependencyInjection is licensed under the MPL 2.0 license. If you modify any of the package's files then the modified files must be licensed under the MPL 2.0 .

If you use this package in your project please make an exact copy of the contents of the LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.
