using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisIO.NET.Tree;
using AnalysisIO.Visitor;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.SharpZipLib.Zip;
using Resolver;

namespace AnalysisIO.NET.SourceImporter
{
    public class SourceImporter
    {
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/analysisIO/rationally/";
        public static Tree.Tree Tree = new Tree.Tree();
        public SourceImporter()
        {
        }

        public async Task<Tree.Tree> BuildTree()
        {
            //create path if needed
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            var client = new GitHubClient(new ProductHeaderValue("rationally"));

            var releases = client.Repository.Release.GetAll("rationally", "rationally_visio");

            var latest = releases.Result[0];
            Console.WriteLine(
                "The latest release is tagged at {0} and is named {1}",
                latest.TagName,
                latest.Name);

            //var a = await client.Repository.Content.GetAllContents("rationally", "rationally_visio");
            var task = client.Repository.Content.GetArchive("rationally", "rationally_visio",ArchiveFormat.Zipball);
            var archive = await task;
            File.WriteAllBytes(PATH + "rationally.zip", archive);

            FastZip entry = new FastZip();
            entry.ExtractZip(PATH + "rationally.zip",PATH,null);

            string[] versions = Directory.GetDirectories(PATH);
            string solutionFilePath = Directory.GetFiles(versions[0]).ToList().First(f => f.EndsWith(".sln"));
            //Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe");
            Environment.SetEnvironmentVariable("VSINSTALLDIR", "D:\\VisualStudio2015");
            Solution solution = new Solution(solutionFilePath);
            var x = 5;

            Tree.Identifier = solutionFilePath;

            foreach (var file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                file.SyntaxTree.AcceptVisitor(new NamespaceVisitor(), astResolver);
            }

            foreach (var file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                /*foreach (var invocation in file.SyntaxTree.Descendants.OfType<InvocationExpression>())
                {
                    // Retrieve semantics for the invocation
                    var rr = astResolver.Resolve(invocation) as InvocationResolveResult;
                    if (rr == null)
                    {
                        // Not an invocation resolve result - e.g. could be a UnknownMemberResolveResult instead
                        continue;
                    }
                    file.Invocations.Add(invocation);
                }*/
                file.SyntaxTree.AcceptVisitor(new DependencyVisitor(), astResolver);
            }

            return Tree;

        }
        
    }
}
