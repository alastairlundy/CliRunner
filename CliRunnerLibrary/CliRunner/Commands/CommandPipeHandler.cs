using System.Diagnostics;
using System.Threading.Tasks;
using CliRunner.Commands.Abstractions;
using CliRunner.Piping;
using CliRunner.Piping.Abstractions;

namespace CliRunner.Commands
{
    public class CommandPipeHandler : ICommandPipeHandler
    {
        public async Task<AbstractPipeSource> PipeStandardInputAsync(Process process)
        {
            PipeSource output = new PipeSource();

            byte[] buffer = await process.StandardInput.;

            await process.StandardInput.BaseStream.WriteAsync(buffer);

            return output;
        }

        public async Task<AbstractPipeTarget> PipeStandardOutputAsync(Process process)
        {
            PipeTarget output = new PipeTarget();
            
            throw new System.NotImplementedException();
        }

        public async Task<AbstractPipeTarget> PipeStandardErrorAsync(Process process)
        {
            PipeTarget output = new PipeTarget();
            
            
            throw new System.NotImplementedException();
        }
    }
}