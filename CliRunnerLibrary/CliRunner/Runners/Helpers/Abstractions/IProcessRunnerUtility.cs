/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Runners.Helpers.Abstractions;

public interface IProcessRunnerUtility
{
    public Task ExecuteAsync(Process process, CancellationToken cancellationToken = default);

    public void DisposeOfProcess(Process process);
    
    public ProcessResult GetResult(Process process, bool disposeOfProcess); 
    public BufferedProcessResult GetBufferedResult(Process process, bool disposeOfProcess);
    
    public Task<ProcessResult> GetResultAsync(Process process, bool disposeOfProcess);
    public Task<BufferedProcessResult> GetBufferedResultAsync(Process process, bool disposeOfProcess);
    
}