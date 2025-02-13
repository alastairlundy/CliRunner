using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using CliRunner.Piping.Abstractions;

namespace CliRunner.Piping;

/// <summary>
/// 
/// </summary>
public class ProcessPipeHandler : IProcessPipeHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif 
    public async Task PipeStandardInputAsync(StreamWriter source, Process destination)
    {
        await destination.StandardInput.FlushAsync();
        destination.StandardInput.BaseStream.Position = 0;
        await source.BaseStream.CopyToAsync(destination.StandardInput.BaseStream);  
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif 
    public async Task PipeStandardOutputAsync(Process source, StreamReader destination)
    {
        if (destination.Equals(StreamReader.Null))
        {
            destination = new StreamReader(source.StandardOutput.BaseStream);
        }
        
        destination.DiscardBufferedData();
        destination.BaseStream.Position = 0;
        await source.StandardOutput.BaseStream.CopyToAsync(destination.BaseStream);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif 
    public async Task PipeStandardErrorAsync(Process source, StreamReader destination)
    {
        if (destination.Equals(StreamReader.Null))
        {
            destination = new StreamReader(source.StandardError.BaseStream);
        }
        
        destination.DiscardBufferedData();
        destination.BaseStream.Position = 0;
        await source.StandardError.BaseStream.CopyToAsync(destination.BaseStream);
    }
}