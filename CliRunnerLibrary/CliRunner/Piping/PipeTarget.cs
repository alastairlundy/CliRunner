using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Piping.Abstractions;

namespace CliRunner.Piping
{
    public abstract class PipeTarget
    {
        public abstract Task CopyFromAsync(Stream origin, CancellationToken cancellationToken = default);
        
        public static PipeTarget Null =>
            Create((Stream _, CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : Task.CompletedTask);
        
        
        public static PipeTarget Create(Func<Stream, CancellationToken, Task> handlePipeAsync)
        {
            
        }

        public static PipeTarget Create(Action<Stream> handlePipe)
        {
            
        }

        public static PipeTarget ToStream(Stream stream, bool autoFlush)
        {
            
        }

        public static PipeTarget ToStream(Stream stream)
        {
            
        }

        public static PipeTarget ToFile(string filePath)
        {
            
        }

        public static PipeTarget ToStringBuilder(StringBuilder builder, Encoding encoding)
        {
            
        }

        public static PipeTarget ToStringBuilder(StringBuilder builder)
        {
            return ToStringBuilder(builder, Encoding.Default);
        }

        public static PipeTarget ToDelegate(Func<string, CancellationToken, Task> handleLineAsync, Encoding encoding)
        {
            
        }
        
        public static PipeTarget ToDelegate(Func<string, CancellationToken, Task> handleLineAsync)
        {
            return ToDelegate(handleLineAsync, Encoding.Default);
        }

        public static PipeTarget ToDelegate(Action<string> handleLine, Encoding encoding)
        {
            
        }
        
        public static PipeTarget ToDelegate(Action<string> handleLine)
        {
            return ToDelegate(handleLine, Encoding.Default);
        }

        public static PipeTarget Merge(IEnumerable<PipeTarget> targets)
        {
            
        }

        public static PipeTarget Merge(params PipeTarget[] targets)
        {
            
        }
    }
}