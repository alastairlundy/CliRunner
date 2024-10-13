/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
   */

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Piping.Abstractions
{
    public abstract class AbstractPipeSource
    {
        protected PipeSourceOptions? options;

        public AbstractPipeSource()
        {
            options = null;
        }

        public AbstractPipeSource(Func<Stream> streamFactory)
        {
            options = null;
        }

        public AbstractPipeSource(Func<Stream> streamFactory, PipeSourceOptions? options)
        {
            
            this.options = options;
        }
        
       public abstract Task CopyToAsync(out Stream destination,
            CancellationToken cancellationToken = default);

       public abstract Stream GetStream();
       public abstract ValueTask<Stream> GetStreamAsync(CancellationToken cancellationToken = default);
       
       public abstract AbstractPipeSource FromStream(Stream stream);
       public abstract AbstractPipeSource FromStream(Stream stream, PipeSourceOptions? options);

       public abstract AbstractPipeSource FromStream(Func<Stream> streamFactory);
       public abstract AbstractPipeSource FromStream(Func<Stream> streamFactory, PipeSourceOptions? options);

    }
}