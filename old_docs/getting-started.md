# Getting Started

## Installing CliInvoke
The main way to install CliInvoke is using [nuget](https://www.nuget.org/packages/AlastairLundy.CliInvoke/) directly or through your IDE or Code Editor of choice.

### Versions

### Stable Versions
Where possible you should always use a stable version of CliInvoke and update to the latest minor CliInvoke update within the Major.Minor.Build scheme.

### Pre-release Versions
Versions starting with ``0.`` or ending with ``-alpha.``. ``-beta.``, or ``-rc.`` are pre-release versions and may not be as stable or bug-free as stable releases. 

When configuring Nuget setup in your ``.csproj`` file, staying within a major version of CliInvoke is recommended.

Assuming CliInvoke version 1 has been released, the following tweaks to your ``.csproj`` file would stop version 2.0 from being installed until you are ready to migrate to it (once it is released):
```csharp
<ItemGroup>
    <PackageReference Include="AlastairLundy.CliInvoke" Version="[1.0.0, 2.0.0)"/>
</ItemGroup>
```

## Setting up CliInvoke

### Dependency Injection 
There's 2 main ways of setting up CliInvoke with dependency injection: manually, and using CliInvoke's ``AddCliInvoke`` configuration extension methods with the ``CliInvoke.Extensions.DependencyInjection`` nuget package.

#### Using ``AddCliInvoke``
For this approach you'll need the ``CliInvoke.Extensions.DependencyInjection`` nuget package.

If your project doesn't already use Dependency Injection, you can set it up as follows:

```csharp
using Microsoft.Extensions.DependencyInjection;

using AlastairLundy.CliInvoke.Extensions.DependencyInjection;

namespace MyApp;

    class Program
    {
      internal ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register Your other dependencies here
            
            // AddCliInvoke goes here
            services.AddCliInvoke();

            // Build the service provider
            serviceProvider = services.BuildServiceProvider();

            //Your other code goes here
        }
}
```

#### Manual Setup
This example manually sets up ``ICommandPipeHandler``, ``IProcessFactory``, ``ICliCommandRunner``, and other dependencies as Singletons.

```csharp
using Microsoft.Extensions.DependencyInjection;

using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;

namespace MyApp;

    class Program
    {
      internal ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register Your other dependencies here
            
            services.AddSingleton<IFilePathResolver, FilePathResolver>();
            services.AddSingleton<IProcessRunnerUtility, ProcessRunnerUtility>();
            services.AddSingleton<IPipedProcessRunner, PipedProcessRunner>();
            services.AddSingleton<IProcessPipeHandler, ProcessPipeHandler>();
            services.AddSingleton<IProcessFactory, ProcessFactory>();
            services.AddSingleton<ICliCommandInvoker, CliCommandInvoker();

            // Build the service provider
            serviceProvider = services.BuildServiceProvider();

            //Your other code goes here
        }
}
```

## Example Usage
Here's an example of a simple usage of creating a CliInvoke command. For more detailed examples, see the wiki page.

```csharp
using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;
using AlastairLundy.CliInvoke.Builders;
using AlastairLundy.CliInvoke.Builders.Abstractions;

ICliCommandInvoker commandRunner = serviceProvider.GetRequiredService<ICliCommandInvoker>();

ICliCommandConfigurationBuilder builder = new CliCommandConfigurationBuilder("Path/To/Exe")
              .WithArguments(["arg1", "arg2"])
              .WithWorkingDirectory("/Path/To/Directory");

CliCommandConfiguration command = builder.Build();

var result = await commandRunner.ExecuteBufferedAsync(command);
```