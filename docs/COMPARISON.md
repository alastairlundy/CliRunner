## CliRunner vs CliWrap comparisons

### Entry Point
CliWrap's intended entrypoint is the ``Cli`` static class with the ``Wrap`` static method.

CliRunner's intended entrypoint is the ``CliRunner`` static class with the ``Run`` static method.

### Command
In addition to what CliWrap provides in Command properties, CliRunner adds several properties for ``UseShellExecution``, ``ProcessorAffinity``, and ``WindowCreation``

### Credential Model
To avoid naming conflicts with naming both a variable ``Credentials`` and the ``Credentials`` class, CliRunner calls the class ``UserCredentials``.

``UserCredentials`` also implements IDisposable to handle disposing of the SecureString Password when finished.

### Piping Differences
CliWrap uses it's own Pipe Target and Pipe Source abstractions and interfaces to enable piping.

CliRunner uses C#'s Stream objects and StreamWriter and StreamReader class to handle piping.

### Command Execution
CliWrap supports both Task based Execution and an EventStream execution model.

CliRunner only supports Task based Execution.

