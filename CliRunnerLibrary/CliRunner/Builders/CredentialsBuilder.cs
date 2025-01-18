/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security;
// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace CliRunner.Builders;

/// <summary>
/// A class that provides builder methods for constructing UserCredentials.
/// </summary>
[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class CredentialsBuilder : IDisposable
{
    private string _domain;
    private string _username;
    private SecureString _password;
    private bool _loadUserProfile;

    /// <summary>
    /// 
    /// </summary>
    public CredentialsBuilder()
    {
        _domain = string.Empty;
        _username = string.Empty;
        _password = new SecureString();
        _loadUserProfile = false;
    }
        
    /// <summary>
    /// Sets the domain for the credential to be created.
    /// </summary>
    /// <param name="domain">The domain to set.</param>
    /// <returns>A new instance of the CredentialsBuilder with the updated domain.</returns>
    [Pure]
    public CredentialsBuilder SetDomain(string domain) =>
        new CredentialsBuilder
        {
            _domain = domain,
            _loadUserProfile = _loadUserProfile,
            _password = _password,
            _username = _username,
        };

    /// <summary>
    /// Sets the username for the credential to be created.
    /// </summary>
    /// <param name="username">The username to set.</param>
    /// <returns>A new instance of the CredentialsBuilder with the updated username.</returns>
    [Pure]
    public CredentialsBuilder SetUsername(string username) =>
        new CredentialsBuilder
        {
            _domain = _domain,
            _loadUserProfile = _loadUserProfile,
            _password = _password,
            _username = username,
        };

    /// <summary>
    /// Sets the password for the credential to be created.
    /// </summary>
    /// <param name="password">The password to set, as a SecureString.</param>
    /// <returns>A new instance of the CredentialsBuilder with the updated password.</returns>
    [Pure]
    public CredentialsBuilder SetPassword(SecureString password) =>
        new CredentialsBuilder
        {
            _domain = _domain,
            _loadUserProfile = _loadUserProfile,
            _password = password,
            _username = _username,
        };
        
    /// <summary>
    /// Specifies whether to load the user profile.
    /// </summary>
    /// <param name="loadUserProfile">True to load the user profile, false otherwise.</param>
    /// <returns>A new instance of the CredentialsBuilder with the updated load user profile setting.</returns>
    [Pure]
    public CredentialsBuilder LoadUserProfile(bool loadUserProfile) =>
        new CredentialsBuilder
        {
            _domain = _domain,
            _loadUserProfile = loadUserProfile,
            _password = _password,
            _username = _username,
        };
        
    /// <summary>
    /// Builds a new instance of UserCredentials using the current settings.
    /// </summary>
    /// <returns>The built UserCredentials.</returns>
    [Pure]
    public UserCredentials Build() => 
        new UserCredentials(_domain, _username, _password, _loadUserProfile);

    /// <summary>
    /// Deletes the values of the provided settings.
    /// </summary>
    public void Clear()
    {
        _domain = string.Empty;
        _username = string.Empty;
        _password.Clear();
    }
        
    /// <summary>
    /// Disposes of the provided settings.
    /// </summary>
    public void Dispose()
    {
        Clear();
        _password?.Dispose();
    }
}