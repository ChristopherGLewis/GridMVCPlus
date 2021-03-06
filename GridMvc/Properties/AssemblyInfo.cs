﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("GridMvc")]
[assembly: AssemblyDescription("ASP.NET MVC Grid component")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("gridmvc.codeplex.com")]
[assembly: AssemblyProduct("GridMvc")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("7d38c093-51ea-4869-8044-0a85eda1bb6f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

#if DEBUG
[assembly: AssemblyVersion("3.2.*")]
//[assembly: AssemblyFileVersion("3.1")]
#else
[assembly: AssemblyVersion("3.1")]
[assembly: AssemblyFileVersion("3.1")]
#endif
[assembly: InternalsVisibleTo("GridMvc.Tests")]