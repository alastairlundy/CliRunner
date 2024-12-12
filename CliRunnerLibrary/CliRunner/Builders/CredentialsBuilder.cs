/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System.Security;

namespace CliRunner.Builders
{
    public class CredentialsBuilder
    {
        private string _domain;
        private string _username;
        private SecureString _password;
        private bool _loadUserProfile;

        public CredentialsBuilder SetDomain(string domain) =>
            new CredentialsBuilder()
            {
                _domain = domain,
                _loadUserProfile = _loadUserProfile,
                _password = _password,
                _username = _username,
            };

        public CredentialsBuilder SetUsername(string username) =>
            new CredentialsBuilder()
            {
                _domain = _domain,
                _loadUserProfile = _loadUserProfile,
                _password = _password,
                _username = username,
            };

        public CredentialsBuilder SetPassword(SecureString password) =>
            new CredentialsBuilder()
            {
                _domain = _domain,
                _loadUserProfile = _loadUserProfile,
                _password = password,
                _username = _username,
            };
        
        public CredentialsBuilder LoadUserProfile(bool loadUserProfile) =>
            new CredentialsBuilder()
            {
                _domain = _domain,
                _loadUserProfile = loadUserProfile,
                _password = _password,
                _username = _username,
            };
        
        public UserCredentials Build() => 
            new UserCredentials(_domain, _username, _password, _loadUserProfile);
    }
}