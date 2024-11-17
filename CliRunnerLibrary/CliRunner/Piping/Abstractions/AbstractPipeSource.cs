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
         protected PipeSourceOptions? _options;  
#elif NETSTANDARD2_0
        protected PipeSourceOptions _options;
#endif
        
        public AbstractPipeSource()
        {
            _options = null;
        }

        public AbstractPipeSource(Func<Stream> streamFactory)
        {
            _options = null;
        }

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        public AbstractPipeSource(Func<Stream> streamFactory, PipeSourceOptions? options)
        {
            
            this._options = options;
        }
#elif NETSTANDARD2_0
        public AbstractPipeSource(Func<Stream> streamFactory, PipeSourceOptions options)
        {
            
            this._options = options;
        }
#endif
        public abstract AbstractPipeSource Null { get; }
        
       public abstract Task<Stream> CopyToAsync(CancellationToken cancellationToken = default);

       public abstract Stream GetStream();
       public abstract Task<Stream> GetStreamAsync(CancellationToken cancellationToken = default);
       
       public abstract AbstractPipeSource FromStream(Stream stream);

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        public abstract AbstractPipeSource FromStream(Stream stream, PipeSourceOptions? options);
#elif NETSTANDARD2_0
        public abstract AbstractPipeSource FromStream(Stream stream, PipeSourceOptions options);
#endif
       
        public abstract AbstractPipeSource FromStream(Func<Stream> streamFactory);
       
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        public abstract AbstractPipeSource FromStream(Func<Stream> streamFactory, PipeSourceOptions? options);
#elif NETSTANDARD2_0
        public abstract AbstractPipeSource FromStream(Func<Stream> streamFactory, PipeSourceOptions options);
#endif
    }
}