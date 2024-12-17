/*
    UrlRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

// ReSharper disable RedundantIfElseBlock

using System;

namespace UrlRunner
{
    /// <summary>
    /// 
    /// </summary>
    public class UrlBuilder
    {
        private Url _url;

        /// <summary>
        /// 
        /// </summary>
        public UrlBuilder()
        {
            _url = new Url(string.Empty);
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        public static bool TryCreate(Uri baseUri, Uri relativeUri, out UrlBuilder? urlBuilder)
#else
        public static bool TryCreate(Uri baseUri, Uri relativeUri, out UrlBuilder urlBuilder)
#endif
        {
            try
            {
                var builder = new UrlBuilder()
                    .WithScheme(baseUri.Scheme)
                    .WithBaseUrl(baseUri.ToString())
                    .AppendSegment(relativeUri.ToString().Replace(baseUri.ToString(), string.Empty));
                
                urlBuilder = builder;
                return true;
            }
            catch
            {
                urlBuilder = null;
                return false;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="portNumber"></param>
        public UrlBuilder(string baseUrl, int? portNumber = null)
        {
            if (portNumber != null)
            {
                _url = new Url(baseUrl, string.Empty, string.Empty, portNumber);
            }
            else
            {
                _url =  new Url(baseUrl);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        private UrlBuilder(Url url)
        {
            _url = url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="urlPrefix"></param>
        /// <param name="urlScheme"></param>
        /// <param name="portNumber"></param>
        private UrlBuilder(string baseUrl, string urlPrefix = null, string urlScheme = null, int? portNumber = null)
        {
            if (string.IsNullOrEmpty(urlPrefix) == false)
            {
                if (portNumber != null)
                {
                    _url = new Url(baseUrl, urlPrefix, urlScheme, portNumber);
                }
                else
                {
                    _url = new Url(baseUrl, urlPrefix, urlScheme, null);
                }
            }
            else
            {
                _url = new Url(baseUrl);
            }
        }

        public UrlBuilder WithBaseUrl(string baseUrl)
        {
           return new UrlBuilder(baseUrl, _url.Prefix, _url.Scheme, _url.PortNumber);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public UrlBuilder AppendSegment(string segment)
        {
            return new UrlBuilder(string.Concat(_url.BaseUrl, segment), _url.Prefix, _url.Scheme, _url.PortNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UrlBuilder AddForwardSlash()
        {
            return AppendSegment("/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public UrlBuilder WithPrefix(string prefix)
        {
            return new UrlBuilder(_url.BaseUrl, prefix, _url.Scheme, _url.PortNumber);
        }

        public UrlBuilder WithScheme(string scheme)
        {
            return new UrlBuilder(_url.BaseUrl, _url.Prefix, scheme, _url.PortNumber);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addWww"></param>
        /// <returns></returns>
        public UrlBuilder WithHttpsScheme()
        {
            return WithScheme("https");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UrlBuilder WithFileScheme()
        {
            return WithScheme("file");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UrlBuilder WithMailToScheme()
        {
            return WithScheme("mailto");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UrlBuilder WithPhoneScheme()
        {
            return WithScheme("tel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Url Build()
        {
            return _url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return _url.ToString();
        }
    }
}