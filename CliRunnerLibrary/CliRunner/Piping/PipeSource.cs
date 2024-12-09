using System;

using System.IO;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Commands;
using CliRunner.Piping.Abstractions;

namespace CliRunner.Piping
{
    public abstract class PipeSource
    {
        private class AnonymousPipeSource(Func<Stream, CancellationToken, Task> copyToAsync) : PipeSource()
        {
            public override async Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
        public abstract Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default);
            
        public static PipeSource Null { get; } = 
            Create((Stream _, CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : Task.CompletedTask);

        public static PipeSource Create(Func<Stream, CancellationToken, Task> handlePipeAsync)
        {
            
        }

        public static PipeSource Create(Action<Stream> handlePipe)
        {
            
        }

        public static PipeSource FromStream(Stream stream, bool autoFlush)
        {
            
        }

        public static PipeSource FromStream(Stream stream)
        {
            
        }

        public static PipeSource FromFile(string filePath)
        {
            
        }

        public static PipeSource FromBytes(ReadOnlyMemory<byte> data)
        {
            
        }

        public static PipeSource FromBytes(byte[] data)
        {
            
        }

        public static PipeSource FromString(string str, Encoding encoding)
        {
            
        }

        public static PipeSource FromString(string str)
        {
            
        }

        public static PipeSource FromCommand(Command command)
        {
            
        }
    }
}