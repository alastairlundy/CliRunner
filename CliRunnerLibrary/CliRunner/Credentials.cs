// ReSharper disable ClassNeverInstantiated.Global

/*
   Based on Tyrrrz's CliWrap Credentials.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Credentials.cs

    Portions of this code are licensed under the MIT license.
 */

using System.Security;

namespace CliRunner
{
    public class Credentials
    {
        public Credentials()
        {
            Domain = null;
            Username = null;
            Password = null;
        }

        public Credentials(string domain, string username, SecureString password)
        {
            Domain = domain;
            Username = username;
            Password = password;
        }
        
#if NET6_0_OR_GREATER || NETSTANDARD2_1
        public string? Domain { get; set; }
        public string? Username { get; set; }
        public SecureString? Password { get; set; }        
#else
        public string Domain { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }
#endif
        
        public static Credentials Default { get; } = new Credentials();
    }
}