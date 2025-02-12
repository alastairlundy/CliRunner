using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Xunit;

using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

namespace CliRunner.Tests.Builders;

public class CommandBuilderTests
{

        [Fact]
        public void TestDefaultConfiguration()
        {
                ICommandBuilder commandBuilder = new CommandBuilder("foo");

                var builtCommand = commandBuilder.Build();
                Assert.Equal("foo", builtCommand.TargetFilePath);
                Assert.Equal(string.Empty, builtCommand.Arguments);
                Assert.Equal(Directory.GetCurrentDirectory(), builtCommand.WorkingDirectoryPath);

                Assert.Equal(new Dictionary<string, string>(), builtCommand.EnvironmentVariables);
                Assert.True(builtCommand.StandardInputEncoding.Equals(Encoding.Default) &&
                            builtCommand.StandardOutputEncoding.Equals(Encoding.Default) &&
                            builtCommand.StandardErrorEncoding.Equals(Encoding.Default));

                Assert.Equal(builtCommand.Credential, UserCredential.Null);
                Assert.Equal(ProcessResultValidation.ExitCodeZero, builtCommand.ResultValidation);

                Assert.Equal(builtCommand.StandardInput, StreamWriter.Null);
                Assert.Equal(builtCommand.StandardOutput, StreamReader.Null);
                Assert.Equal(builtCommand.StandardError, StreamReader.Null);

                Assert.Equal(builtCommand.Credential, UserCredential.Null);
                
                Assert.Equal(ProcessResourcePolicy.Default, builtCommand.ResourcePolicy);

                Assert.False(builtCommand.WindowCreation);
                Assert.False(builtCommand.UseShellExecution);
                Assert.False(builtCommand.RequiresAdministrator);
        }

        [Fact]
        public void TestIncompatiblePipingOptionsThrowsException()
        {
                ICommandBuilder commandBuilder = new CommandBuilder("foo");

                Assert.Throws<ArgumentException>(() =>
                {
                    commandBuilder.WithShellExecution(true)
                        .WithStandardOutputPipe(new StreamReader(Console.OpenStandardOutput()));
                });
                
                Assert.Throws<ArgumentException>(() =>
                {
                        commandBuilder.WithShellExecution(true)
                                .WithStandardErrorPipe(new StreamReader(Console.OpenStandardError()));
                });
                
                Assert.Throws<ArgumentException>(() =>
                {
                        commandBuilder.WithShellExecution(true)
                                .WithStandardInputPipe(new StreamWriter(Console.OpenStandardInput()));
                });
        }
        
}