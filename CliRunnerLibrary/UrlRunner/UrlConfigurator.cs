/*
    UrlRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using UrlRunner.Abstractions;

namespace UrlRunner
{
    public partial class Url : IUrlConfigurationBuilder
    {
        /// <summary>
        /// Convert HTTP to HTTPS
        /// </summary>
        /// <param name="https"></param>
        /// <returns></returns>
        public Url UseHttps()
        {
            Scheme = Scheme.Replace("http", "https");

            return new Url(BaseUrl, Prefix, Scheme, PortNumber);
        }

        /// <summary>
        /// Adds HTTP(S) to a URL if it's missing.
        /// </summary>
        /// <param name="allowNonSecureHttp">Whether to use HTTP instead of HTTPS.</param>
        /// <returns>the modified URL string.</returns>
        public Url AddSchemeIfMissing()
        {
            string url = ToString();

            if (string.IsNullOrWhiteSpace(Scheme))
            {
                if ((!url.StartsWith("https://") || !url.StartsWith("www.")) && (!url.StartsWith("file://")))
                {
                    if (url.Contains("http://"))
                    {
                        Scheme = "http://";
                    }
                    else
                    {
                        Scheme = "https://";
                    }
                }
                else
                {
                    Scheme = "https://";
                }
            }

            return new Url(BaseUrl, Prefix, Scheme, PortNumber);
        }
    }
}