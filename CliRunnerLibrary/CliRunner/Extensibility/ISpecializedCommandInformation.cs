﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Threading.Tasks;

namespace CliRunner.Extensibility
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISpecializedCommandInformation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<string> GetInstallLocationAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> IsInstalledAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<Version> GetInstalledVersionAsync();
    }
}