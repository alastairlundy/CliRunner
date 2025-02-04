﻿/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Credentials.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Credentials.c

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Security;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

#nullable enable

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace CliRunner
{
    /// <summary>
    /// 
    /// </summary>
    public class UserCredential : IEquatable<UserCredential>, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public UserCredential()
        {
            Domain = null;
            UserName = null;
            Password = null;
            LoadUserProfile = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loadUserProfile"></param>
        public UserCredential(string? domain, string username, SecureString? password, bool loadUserProfile)
        {
            Domain = domain;
            UserName = username;
            Password = password;
            LoadUserProfile = loadUserProfile;
        }
        
        /// <summary>
        /// 
        /// </summary>
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public string? Domain { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? UserName { get; }
        
        /// <summary>
        /// 
        /// </summary>
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public SecureString? Password { get; }
        
        /// <summary>
        /// 
        /// </summary>
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public bool? LoadUserProfile { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public static UserCredential Null { get; } = new UserCredential();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Password?.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UserCredential? other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.UserName is null || other.Domain is null || other.Password is null ||
                other.LoadUserProfile is null)
            {
                return false;
            }
            
            return Domain == other.Domain &&
               UserName == other.UserName &&
               Password!.Equals(other.Password)
               && LoadUserProfile == other.LoadUserProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(UserCredential? left, UserCredential? right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            
            return left.Equals(right);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is UserCredential other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Domain, UserName, Password, LoadUserProfile);
        }

        /// <summary>
        /// Determines if a UserCredential is equal to another UserCredential.
        /// </summary>
        /// <param name="left">A UserCredential to be compared.</param>
        /// <param name="right">The other UserCredential to be compared.</param>
        /// <returns>True if both UserCredentials are equal to each other; false otherwise.</returns>
        public static bool operator ==(UserCredential? left, UserCredential? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if a UserCredential is not equal to another UserCredential.
        /// </summary>
        /// <param name="left">A UserCredential to be compared.</param>
        /// <param name="right">The other UserCredential to be compared.</param>
        /// <returns>True if both UserCredentials are not equal to each other; false otherwise.</returns>
        public static bool operator !=(UserCredential? left, UserCredential? right)
        {
            return Equals(left, right) == false;
        }
    }
}