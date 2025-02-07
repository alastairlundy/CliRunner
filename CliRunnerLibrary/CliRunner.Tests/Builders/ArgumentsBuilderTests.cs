using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

namespace CliRunner.Tests.Builders;

public class ArgumentsBuilderTests
{
    [Fact]
    public void BuilderChainingTest()
    {
        IArgumentsBuilder argumentsBuilder = new ArgumentsBuilder()
            .Add("-v");

        Assert.True(string.IsNullOrEmpty(argumentsBuilder.Build()));
    }
    
     
}