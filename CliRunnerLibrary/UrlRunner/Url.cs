/*
    UrlRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using UrlRunner.Abstractions;

// ReSharper disable RedundantIfElseBlock

namespace UrlRunner
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Url : IUrlConfiguration, IEquatable<Url>
    {
        public string Scheme { get; protected set; }
        public string Prefix { get; protected set; }
        public string BaseUrl { get; protected set; }
        public int? PortNumber { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        public Url(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Url FromString(string url)
        {
            var newUrl = new Url(url);
            
            if (url.Contains("://"))
            {
                int schemeIndex = url.IndexOf("://", StringComparison.Ordinal);
               
                newUrl.Scheme = url.Substring(0, schemeIndex);
            }
            else
            {
                newUrl.Scheme = "https";
            }

            if (url.Contains("."))
            {
                int firstDotIndex = url.IndexOf('.');
                int lastDotIndex = url.LastIndexOf('.');

                if (firstDotIndex != lastDotIndex)
                {
                    if (url.Contains(":") == false)
                    {
                        int prefixEndIndex = url.IndexOf(".", StringComparison.Ordinal);
                        newUrl.Prefix = url.Substring(0, Math.Abs(0 - prefixEndIndex));
                        newUrl.BaseUrl = url.Substring(firstDotIndex, Math.Abs(firstDotIndex - url.Length));
                    }
                    else
                    {
                        int startingIndex = url.IndexOf("://", StringComparison.Ordinal);
                        newUrl.Prefix = url.Substring(startingIndex, Math.Abs(startingIndex - firstDotIndex));
                        newUrl.BaseUrl = url.Substring(firstDotIndex, Math.Abs(firstDotIndex - url.LastIndexOf(":", StringComparison.Ordinal)));
                    }

                    bool hasPortNumber = int.TryParse(url[url.LastIndexOf(":", StringComparison.Ordinal) + 1].ToString(), out int portNumber);
                    
                    if (hasPortNumber)
                    {
                        newUrl.PortNumber = portNumber;
                    }
                }
                else
                {
                    newUrl.Prefix = string.Empty;
                }
            }
            
            return newUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="urlPrefix"></param>
        /// <param name="urlScheme"></param>
        /// <param name="portNumber"></param>
        public Url(string baseUrl, string urlPrefix, string urlScheme, int? portNumber)
        {
            BaseUrl = baseUrl;
            Prefix = urlPrefix;
            Scheme = urlScheme;
            PortNumber = portNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">A Uri to be converted to a Url.</param>
        public Url(Uri baseUrl)
        {
           var url = FromUri(baseUrl);
           this.Scheme = url.Scheme;
           this.Prefix = url.Prefix;
           this.BaseUrl = url.BaseUrl;
           this.PortNumber = url.PortNumber;
        }


        /// <summary>
        /// Converts a Uri to a Url.
        /// </summary>
        /// <param name="uri">The Uri object to be converted.</param>
        /// <returns>The newly created Url object.</returns>
        public static Url FromUri(Uri uri)
        {
           return new Url(uri.ToString(), string.Empty, uri.Scheme, uri.Port);
        }

        /// <summary>
        /// Builds the URL that the specified UrlBuilder has configured.
        /// </summary>
        /// <param name="urlBuilder"></param>
        /// <returns></returns>
        public static Url FromUrlBuilder(UrlBuilder urlBuilder)
        {
            return urlBuilder.Build();
        }

        /// <summary>
        /// Returns a string representation of this Url object.
        /// </summary>
        /// <returns>a string representation of this Url object.</returns>
        public override string ToString()
        {
            if (PortNumber != null)
            {
                if (Prefix.Equals("https://") || Prefix.Equals("http://") || Prefix.Equals("file://") || string.IsNullOrEmpty(Prefix))
                {
                    return $"{Scheme}://{Prefix}{BaseUrl}:{PortNumber}";
                }
                else
                {
                    return $"{Scheme}://{Prefix}.{BaseUrl}:{PortNumber}";
                }
            }
            else
            {
                if (Prefix.Equals("https://") || Prefix.Equals("http://") || Prefix.Equals("file://") || string.IsNullOrEmpty(Prefix))
                {
                    return $"{Scheme}{Prefix}{BaseUrl}";
                }
                else
                {
                    return $"{Scheme}{Prefix}.{BaseUrl}";
                }
            }
        }
        
        /// <summary>
        /// Returns whether a Url is equal to this Url.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the Url is Equal to this Url; returns false otherwise.</returns>
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        public bool Equals(Url? other)
#else
        public bool Equals(Url other)
#endif
        {
            if (other is null)
            {
                return false;
            }
            
            return Scheme == other.Scheme && Prefix == other.Prefix && BaseUrl == other.BaseUrl && PortNumber == other.PortNumber;
        }

        /// <summary>
        /// Returns whether an object is equal to this Url.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if the object is a Url and is Equal to this Url; returns false otherwise.</returns>
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            
            return Equals((Url)obj);
        }

        /// <summary>
        /// Returns the hashcode for this Url.
        /// </summary>
        /// <returns>A 32-bit signed integer hashcode.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}