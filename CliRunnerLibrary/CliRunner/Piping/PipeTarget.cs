using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Piping.Abstractions;
using CliRunner.Piping.Abstractions.Options;

namespace CliRunner.Piping
{
    public class PipeTarget : AbstractPipeTarget
    {
        public override void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override async Task WriteToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeTarget ToStream(Stream stream)
        {
            throw new NotImplementedException();
        }

#if NET5_0_OR_GREATER || NETSTANDARD2_1
        public override AbstractPipeTarget ToStream(Stream stream, PipeSourceOptions? options)
#else
        public override AbstractPipeTarget ToStream(Stream stream, PipeSourceOptions options)
#endif
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeTarget ToStream(Action<Stream> streamAction)
        {
            throw new NotImplementedException();
        }

#if NET5_0_OR_GREATER || NETSTANDARD2_1
        public override AbstractPipeTarget ToStream(Action<Stream> streamAction, PipeTargetOptions? options)
#else
        public override AbstractPipeTarget ToStream(Action<Stream> streamAction, PipeTargetOptions options)
#endif
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeTarget Null { get; }
    }
}