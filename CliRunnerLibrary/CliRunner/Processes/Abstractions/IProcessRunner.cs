/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
   */

using System.Diagnostics;

using CliRunner.Processes;

namespace CliRunner
{
    public interface IProcessRunner
    {
        ProcessResult RunProcessOnWindows(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null, bool runAsAdministrator = false,
            bool insertExeInExecutableNameIfMissing = true);
        
        ProcessResult RunProcessOnMac(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);

        ProcessResult RunProcessOnLinux(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);

        ProcessResult RunProcessOnFreeBsd(string executableLocation, string executableName, string arguments = "",
            ProcessStartInfo? processStartInfo = null);

    }
}