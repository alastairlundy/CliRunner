/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace UrlRunner.Abstractions
{
    public interface IUrlConfiguration
    {
         string Scheme { get;  }
         string Prefix { get;  }
         string BaseUrl { get; }
         int? PortNumber { get; }
    }
}