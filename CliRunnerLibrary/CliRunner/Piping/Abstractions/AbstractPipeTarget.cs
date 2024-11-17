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
    public abstract class AbstractPipeTarget
    {
        public abstract void WriteToStream(Stream stream);

        public abstract Task WriteToStreamAsync(Stream stream, CancellationToken cancellationToken = default);

        public abstract AbstractPipeTarget ToStream(Stream stream);

#if NETSTANDARD2_1 || NET6_OR_GREATER
        public abstract AbstractPipeTarget ToStream(Stream stream, PipeSourceOptions? options);
#elif NETSTANDARD2_0
        public abstract AbstractPipeTarget ToStream(Stream stream, PipeSourceOptions options);
#endif
        public abstract AbstractPipeTarget ToStream(Action<Stream> streamAction);
#if NETSTANDARD2_1 || NET6_OR_GREATER
        public abstract AbstractPipeTarget ToStream(Action<Stream> streamAction, PipeTargetOptions? options);
#elif NETSTANDARD2_0
        public abstract AbstractPipeTarget ToStream(Action<Stream> streamAction, PipeTargetOptions options);
#endif

        public abstract AbstractPipeTarget Null { get; }
    }
}