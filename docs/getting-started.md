# Getting Started

## Installing CliRunner
The main way to install CliRunner is using [nuget](https://www.nuget.org/packages/CliRunner/) directly or through your IDE or Code Editor of choice.

### Versions

### Stable Versions
Where possible you should always use a stable version of CliRunner and update to the latest minor CliRunner update within the Major.Minor.Build scheme.

### Pre-release Versions
Versions starting with ``0.`` or ending with ``-alpha.``. ``-beta.``, or ``-rc.`` are pre-release versions and may not be as stable or bug-free as stable releases. 

When configuring Nuget setup in your ``.csproj`` file, staying within a major version of CliRunner is recommended.

Assuming CliRunner version 1 has been released, the following tweaks to your ``.csproj`` file would stop version 2.0 from being installed until you are ready to migrate to it (once it is released):
```csharp
<ItemGroup>
    <PackageReference Include="CliRunner" Version="[1.0.0, 2.0.0)"/>
</ItemGroup>
```

## Setting up CliRunner

### Dependency Injection 
There's 2 main ways of setting up CliRunner with dependency injection: manually, and using CliRunner's ``UseCliRunner`` configuration extension methods.

#### Using ``UseCliRunner``
If your project doesn't already use Dependency Injection, you can set it up as follows:

```csharp
using Microsoft.Extensions.DependencyInjection;

using CliRunner.Extensions;

namespace MyApp;

    class Program
    {
      internal ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register Your other dependencies here
            
            // UseCliRunner goes here
            services.UseCliRunner();

            // Build the service provider
            serviceProvider = services.BuildServiceProvider();

            //Your other code goes here
        }
}
```

#### Manual Setup
This example manually sets up ``ICommandPipeHandler``, ``IProcessCreator``, and ``ICommandRunner`` as Singletons.

```csharp
using Microsoft.Extensions.DependencyInjection;

using CliRunner;
using CliRunner.Abstractions;

namespace MyApp;

    class Program
    {
      internal ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register Your other dependencies here
            
            // UseCliRunner goes here
            services.AddSingleton<ICommandPipeHandler, CommandPipeHandler>();
            services.AddSingleton<IProcessCreator, ProcessCreator>();
            services.AddSingleton<ICommandRunner, CommandRunner>();

            // Build the service provider
            serviceProvider = services.BuildServiceProvider();

            //Your other code goes here
        }
}
```

## Example Usage
Here's an example of a simple usage of creating a CliRunner command. For more detailed examples, see the wiki page.

```csharp
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

ICommandRunner commandRunner = serviceProvider.GetService<ICommandRunner>();

ICommandBuilder builder = new CommandBuilder("Path/To/Exe")
              .WithArguments(["arg1", "arg2"])
              .WithWorkingDirectory("/Path/To/Directory");

Command command = builder.Build();

var result = await command.ExecuteBufferedAsync(command);
```