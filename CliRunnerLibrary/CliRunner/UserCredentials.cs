// ReSharper disable ClassNeverInstantiated.Global

/*
   Based on Tyrrrz's CliWrap Credentials.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Credentials.cs

    Portions of this code are licensed under the MIT license.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Security;

#nullable enable

namespace CliRunner
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public class UserCredentials : IDisposable
    {
        public UserCredentials()
        {
            Domain = null;
            UserName = null;
            Password = null;
        }

        public UserCredentials(string domain, string username, SecureString password, bool loadUserProfile)
        {
            Domain = domain;
            UserName = username;
            Password = password;
            LoadUserProfile = loadUserProfile;
        }
        
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public string? Domain { get; set; }
        public string? UserName { get; set; }
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public SecureString? Password { get; set; }       
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public bool? LoadUserProfile { get; set; }

        
        public static UserCredentials Default { get; } = new UserCredentials();

        public void Dispose()
        {
            Password?.Dispose();
        }
    }
}