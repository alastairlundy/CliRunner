/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace CliRunner.Commands.Extensions
{
    public static class CommandRunExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Command Run(this Command command)
        {
            return command;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static Command Run(this Command command, string targetFilePath)
        {
            return new Command(targetFilePath);
        }
    }
}