using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Piping.Abstractions;
using CliRunner.Piping.Abstractions.Options;

namespace CliRunner.Piping
{
    public class PipeSource : AbstractPipeSource
    {
        public override AbstractPipeSource Null { get; }
        
        public override async Task<Stream> CopyToAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public override async Task<Stream> GetStreamAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeSource FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeSource FromStream(Stream stream, PipeSourceOptions? options)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeSource FromStream(Func<Stream> streamFactory)
        {
            throw new NotImplementedException();
        }

        public override AbstractPipeSource FromStream(Func<Stream> streamFactory, PipeSourceOptions? options)
        {
            throw new NotImplementedException();
        }
    }
}