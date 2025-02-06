﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CliRunner.Internal.Localizations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CliRunner.Internal.Localizations.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Command&apos;s Target File Path cannot be null or empty..
        /// </summary>
        internal static string Command_TargetFilePath_Empty {
            get {
                return ResourceManager.GetString("Command.TargetFilePath.Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (Requires Administrator).
        /// </summary>
        internal static string Command_ToString_RequiresAdmin {
            get {
                return ResourceManager.GetString("Command.ToString.RequiresAdmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (Uses Shell Execution).
        /// </summary>
        internal static string Command_ToString_ShellExecution {
            get {
                return ResourceManager.GetString("Command.ToString.ShellExecution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working Directory.
        /// </summary>
        internal static string Command_ToString_WorkingDirectory {
            get {
                return ResourceManager.GetString("Command.ToString.WorkingDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UseShellExecution value of true conflicts with setting of Standard Error to a non-null value. See https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror.
        /// </summary>
        internal static string CommandBuilder_ArgumentConflict_ShellExecution_Error {
            get {
                return ResourceManager.GetString("CommandBuilder.ArgumentConflict.ShellExecution.Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UseShellExecution value of true conflicts with setting of Standard Input to a non-null value. See https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardinput.
        /// </summary>
        internal static string CommandBuilder_ArgumentConflict_ShellExecution_Input {
            get {
                return ResourceManager.GetString("CommandBuilder.ArgumentConflict.ShellExecution.Input", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UseShellExecution value of true conflicts with setting of Standard Output to a non-null value. See https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput.
        /// </summary>
        internal static string CommandBuilder_ArgumentConflict_ShellExecution_Output {
            get {
                return ResourceManager.GetString("CommandBuilder.ArgumentConflict.ShellExecution.Output", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot convert an non-exited process to a CommandResult.
        /// </summary>
        internal static string CommandResult_ToBuffered_ExitedProcess {
            get {
                return ResourceManager.GetString("CommandResult.ToBuffered.ExitedProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot convert a process without StandardOutput/StandardError to a BufferedCommandResult..
        /// </summary>
        internal static string CommandResult_ToStandardOutError {
            get {
                return ResourceManager.GetString("CommandResult.ToStandardOutError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed command was not succesful and returned an exit code of {x}..
        /// </summary>
        internal static string Exceptions_CommandNotSuccessful_Generic {
            get {
                return ResourceManager.GetString("Exceptions.CommandNotSuccessful.Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed command, with executable name of {x}, was not succesful and returned an exit code of {y}..
        /// </summary>
        internal static string Exceptions_CommandNotSuccessful_Specific {
            get {
                return ResourceManager.GetString("Exceptions.CommandNotSuccessful.Specific", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {file} does not exist in the specified path..
        /// </summary>
        internal static string Exceptions_FileNotFound {
            get {
                return ResourceManager.GetString("Exceptions.FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The process start info&apos;s file name cannot be null or empty..
        /// </summary>
        internal static string Process_FileName_Empty {
            get {
                return ResourceManager.GetString("Process.FileName.Empty", resourceCulture);
            }
        }
    }
}
