using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CliRunner.Piping.Abstractions;

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

        public override AbstractPipeTarget ToStream(Action<Stream> streamAction)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeTarget Null { get; }
    }
}