﻿using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using Xunit;

namespace CliRunner.Tests.Builders;

public class ArgumentsBuilderTests
{
    [Fact]
    public void BuilderChainingTest()
    {
        IArgumentsBuilder argumentsBuilder = new ArgumentsBuilder()
            .Add("new")
            .Add(["list", "--help"]);

        string expected = "new list --help";
        
        Assert.Equal(expected, argumentsBuilder.ToString());
    }
    
     
}