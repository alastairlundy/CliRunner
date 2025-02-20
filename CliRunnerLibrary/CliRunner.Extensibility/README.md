## CliRunner.Extensibility
This readme covers the **CliRunner.Extensibility** package. Looking for the [CliRunner Readme](https://github.com/alastairlundy/CliRunner/blob/main/README.md)?

This package adds:
* abstractions to make extending CliRunner, and it's supported types easier.

[![NuGet](https://img.shields.io/nuget/v/CliRunner.Extensibility.svg)](https://www.nuget.org/packages/CliRunner.Extensibility/)
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.Extensibility.svg)](https://www.nuget.org/packages/CliRunner.Extensibility/)

## Usage

### SpecializedCommandConfiguration
CliRunner.Extensibility provides an abstraction to make it easier to create custom Command Configurations.

This abstraction comes in the form of ``SpecializedCliCommandConfiguration``.

## Why a separate package?
There's a few different reasons:
* De-couples the extensibility related abstractions from the main library
* Not every user of CliRunner will necessarily use or need CliRunner Specializations or other extensibility powered features.

## Licensing
CliRunner.Extensibility is licensed under the MPL 2.0 license. If you modify any of the package's files then the modified files must be licensed under the MPL 2.0 .

If you use this package in your project please make an exact copy of the contents of the LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.

### Assets
CliRunner's Icon is NOT licensed under the MPL 2.0 license and are licensed under Copyright with all rights reserved to me (Alastair Lundy).

If you fork CliRunner and re-distribute it, please replace the usage of the icon unless you have prior written agreements from me.

## Acknowledgements
This project would like to thank the following projects for their work:


For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/CliRunner/blob/main/CliRunnerLibrary/CliRunner.Extensibility/THIRD_PARTY_NOTICES.txt).
