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
    public abstract class AbstractPipeTarget
    {
        public abstract void WriteToStream(Stream stream);

        public abstract ValueTask WriteToStreamAsync(Stream stream, CancellationToken cancellationToken = default);

        public abstract AbstractPipeTarget ToStream(Stream stream);

        public abstract AbstractPipeTarget ToStream(Stream stream, PipeSourceOptions? options);

        public abstract AbstractPipeTarget ToStream(Action<Stream> streamAction);
        public abstract AbstractPipeTarget ToStream(Action<Stream> streamAction, PipeTargetOptions? options);

        public abstract AbstractPipeTarget ToNull();
        public abstract AbstractPipeTarget ToNull(PipeTargetOptions? options);
    }
}