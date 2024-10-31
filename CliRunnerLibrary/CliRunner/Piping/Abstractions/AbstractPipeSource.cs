/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CliRunner.Piping.Abstractions.Options;

namespace CliRunner.Piping.Abstractions
{
    public abstract class AbstractPipeSource
    {
        
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
         protected PipeSourceOptions? options;  
#elif NETSTANDARD2_0
        protected PipeSourceOptions options;
#endif
        
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