// ReSharper disable ClassNeverInstantiated.Global

/*
   Based on Tyrrrz's CliWrap Credentials.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Credentials.cs

    Portions of this code are licensed under the MIT license.
 */

using System;
using System.Security;

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
    #nullable enable
#endif

namespace CliRunner
{
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
        
#if NET6_0_OR_GREATER || NETSTANDARD2_1
        public string? Domain { get; set; }
        public string? UserName { get; set; }
        public SecureString? Password { get; set; }        
        public bool? LoadUserProfile { get; set; }
#else
        public string Domain { get; set; }
        public string UserName { get; set; }
        public SecureString Password { get; set; }
        public bool LoadUserProfile { get; set; }
#endif
        
        public static UserCredentials Default { get; } = new UserCredentials();

        public void Dispose()
        {
            Password?.Dispose();
        }
    }
}