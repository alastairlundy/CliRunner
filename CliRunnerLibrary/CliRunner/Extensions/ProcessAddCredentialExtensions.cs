/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Runtime.Versioning;

namespace CliRunner.Extensions;

public static class ProcessAddCredentialExtensions
{
    
    /// <summary>
    /// Adds the specified Credential to the current Process object.
    /// </summary>
    /// <param name="process">The current Process object.</param>
    /// <param name="credential">The credential to be added.</param>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static void AddUserCredential(this Process process, UserCredential credential)
    {
#pragma warning disable CA1416
        if (credential != null)
        {
            if (credential.Domain is not null)
            {
                process.StartInfo.Domain = credential.Domain;
            }

            if (credential.UserName is not null)
            {
                process.StartInfo.UserName = credential.UserName;
            }

            if (credential.Password is not null)
            {
                process.StartInfo.Password = credential.Password;
            }

            if (credential.LoadUserProfile is not null)
            {
                process.StartInfo.LoadUserProfile = (bool)credential.LoadUserProfile;
            }
        }
#pragma warning restore CA1416
    }
    
    /// <summary>
    /// Adds the specified Credential to the current ProcessStartInfo object.
    /// </summary>
    /// <param name="processStartInfo">The current ProcessStartInfo object.</param>
    /// <param name="credential">The credential to be added.</param>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static void AddUserCredential(this ProcessStartInfo processStartInfo, UserCredential credential)
    {
#pragma warning disable CA1416
        if (credential != null)
        {
            if (credential.Domain is not null)
            {
                processStartInfo.Domain = credential.Domain;
            }

            if (credential.UserName is not null)
            {
                processStartInfo.UserName = credential.UserName;
            }

            if (credential.Password is not null)
            {
                processStartInfo.Password = credential.Password;
            }

            if (credential.LoadUserProfile is not null)
            {
                processStartInfo.LoadUserProfile = (bool)credential.LoadUserProfile;
            }
        }
#pragma warning restore CA1416
    }
}