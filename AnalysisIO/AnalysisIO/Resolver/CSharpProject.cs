// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace AnalysisIO_Console.Resolver
{
	/// <summary>
	/// Represents a C# project (.csproj file)
	/// </summary>
	public class CSharpProject
	{
		/// <summary>
		/// Parent solution.
		/// </summary>
		public readonly Solution Solution;
		
		/// <summary>
		/// Title is the project name as specified in the .sln file.
		/// </summary>
		public readonly string Title;
		
		/// <summary>
		/// Name of the output assembly.
		/// </summary>
		public readonly string AssemblyName;
		
		/// <summary>
		/// Full path to the .csproj file.
		/// </summary>
		public readonly string FileName;
		
		public readonly List<CSharpFile> Files = new List<CSharpFile>();
		
		public readonly CompilerSettings CompilerSettings = new CompilerSettings();
		
		/// <summary>
		/// The unresolved type system for this project.
		/// </summary>
		public readonly IProjectContent ProjectContent;
		
		/// <summary>
		/// The resolved type system for this project.
		/// This field is initialized once all projects have been loaded (in Solution constructor).
		/// </summary>
		public ICompilation Compilation;
		
		public CSharpProject(Solution solution, string title, string fileName)
		{
			// Normalize the file name
			fileName = Path.GetFullPath(fileName);
			
			Solution = solution;
			Title = title;
			FileName = fileName;
			
			// Use MSBuild to open the .csproj
			Project msbuildProject = new Project(fileName);
			// Figure out some compiler settings
			AssemblyName = msbuildProject.GetPropertyValue("AssemblyName");
			CompilerSettings.AllowUnsafeBlocks = GetBoolProperty(msbuildProject, "AllowUnsafeBlocks") ?? false;
			CompilerSettings.CheckForOverflow = GetBoolProperty(msbuildProject, "CheckForOverflowUnderflow") ?? false;
			string defineConstants = msbuildProject.GetPropertyValue("DefineConstants");
			foreach (string symbol in defineConstants.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
			    CompilerSettings.ConditionalSymbols.Add(symbol.Trim());
			}

		    // Initialize the unresolved type system
			IProjectContent pc = new CSharpProjectContent();
			pc = pc.SetAssemblyName(AssemblyName);
			pc = pc.SetProjectFileName(fileName);
			pc = pc.SetCompilerSettings(CompilerSettings);
			// Parse the C# code files
			foreach (CSharpFile file in msbuildProject.GetItems("Compile").Select(item => new CSharpFile(this, Path.Combine(msbuildProject.DirectoryPath, item.EvaluatedInclude))))
			{
			    Files.Add(file);
			}
			// Add parsed files to the type system
			pc = pc.AddOrUpdateFiles(Files.Select(f => f.UnresolvedTypeSystemForFile));
			
			// Add referenced assemblies:
		    pc = ResolveAssemblyReferences(msbuildProject).Select(solution.LoadAssembly).Aggregate(pc, (current, assembly) => current.AddAssemblyReferences(assembly));

		    // Add project references:
		    pc = msbuildProject.GetItems("ProjectReference").Select(item => Path.Combine(msbuildProject.DirectoryPath, item.EvaluatedInclude)).Select(Path.GetFullPath).Aggregate(pc, (current, referencedFileName) => current.AddAssemblyReferences(new ProjectReference(referencedFileName)));
            ProjectContent = pc;
		}

	    private IEnumerable<string> ResolveAssemblyReferences(Project project)
		{
			// Use MSBuild to figure out the full path of the referenced assemblies
			ProjectInstance projectInstance = project.CreateProjectInstance();
			projectInstance.SetProperty("BuildingProject", "false");
			project.SetProperty("DesignTimeBuild", "true");
			
			projectInstance.Build("ResolveAssemblyReferences", new [] { new ConsoleLogger(LoggerVerbosity.Minimal) });
			ICollection<ProjectItemInstance> items = projectInstance.GetItems("_ResolveAssemblyReferenceResolvedFiles");
			string baseDirectory = Path.GetDirectoryName(FileName);
			return items.Select(i => Path.Combine(baseDirectory, i.GetMetadataValue("Identity")));
		}

	    private static bool? GetBoolProperty(Project p, string propertyName)
		{
			string val = p.GetPropertyValue(propertyName);
			bool result;
		    return bool.TryParse(val, out result) ? (bool?) result : null;
		}
		
		public override string ToString() => $"[CSharpProject AssemblyName={AssemblyName}]";
	}
}
