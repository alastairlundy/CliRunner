using CliRunner.Builders;

namespace CliRunner.Tests.Builders;

public class ArgumentsBuilderTests
{
    [Fact]
    public void BuilderChainingTest()
    {
        ArgumentsBuilder argumentsBuilder = new ArgumentsBuilder()
            .Add("-v");

        Assert.True(string.IsNullOrEmpty(argumentsBuilder.Build()));
    }
    
     
}