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
    public async Task PipeStandardInputAsync(Stream source, Process destination)
    {
        if (destination.StartInfo.RedirectStandardInput && destination.StandardInput != StreamWriter.Null)
        {
            await destination.StandardInput.FlushAsync();
            destination.StandardInput.BaseStream.Position = 0;
            await source.CopyToAsync(destination.StandardInput.BaseStream); 
        }
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
    public async Task PipeStandardOutputAsync(Process source, Stream destination)
    {
        if (source.StartInfo.RedirectStandardOutput)
        {
            if (source.StandardOutput != StreamReader.Null)
            {
                await source.StandardOutput.BaseStream.CopyToAsync(destination);
            }
        }
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
    public async Task PipeStandardErrorAsync(Process source, Stream destination)
    {
        if (source.StartInfo.RedirectStandardError)
        {
            if (source.StandardError != StreamReader.Null)
            {
                await source.StandardError.BaseStream.CopyToAsync(destination);
            }
        }
    }
}