# Introduction

Welcome to the CliRunner Docs!

## What is CliRunner?
CliRunner is a .NET library for interacting with Command Line Interfaces and wrapping around executables. 

### What is CliRunner Specializations?
CliRunner Specializations adds Command objects that wrap around Cmd (CmdCommand), Windows Powershell (ClassicPowershellCommand), and Cross-Platform Powershell (PowershellCommand). 

Comamnd usage is the same as any other Command. The main differences is that since these Commands only work on some platforms and not others, these Commands also include methods for determining if the Command is installed and what version is in use.

## Getting Started
Check out the [Getting Started page](/getting-started)

## License
CliRunner is licensed under the Mozilla Public License 2.0. The license text can be found [here](https://github.com/alastairlundy/CliRunner/blob/main/LICENSE.txt).