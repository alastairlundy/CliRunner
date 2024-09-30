using System.Collections.Generic;
using System.Diagnostics;

namespace CliRunner.Processes.Abstractions
{
    public interface IExecutable
    {
        string Name { get; }
        
        string FilePath { get; }
        
        IEnumerable<string> Arguments { get; }
        
        ProcessStartInfo? StartInfo { get; }
        
        
    }
}