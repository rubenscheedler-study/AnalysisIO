using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisIO.Tree;
using AnalysisIO.Visitor;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.SharpZipLib.Zip;
using StringIndexOf;

namespace AnalysisIO.SourceImporter
{
    public class SourceImporter
    {
        public static Tree.Tree Tree = new Tree.Tree();
        public SourceImporter()
        {
            AsyncMethod();
        }

        public async void AsyncMethod()
        {
            var client = new GitHubClient(new ProductHeaderValue("rationally"));

            var releases = client.Repository.Release.GetAll("rationally", "rationally_visio");

            var latest = releases.Result[0];
            Console.WriteLine(
                "The latest release is tagged at {0} and is named {1}",
                latest.TagName,
                latest.Name);

            var archive = await client.Repository.Content.GetArchive("rationally", "rationally_visio",ArchiveFormat.Zipball);

            File.WriteAllBytes("rationally.zip", archive);

            FastZip entry = new FastZip();
            entry.ExtractZip("rationally.zip","repos/rationally/versions",null);

            string[] versions = Directory.GetDirectories("repos/rationally/versions");
            string solutionFilePath = Directory.GetFiles(versions[0]).ToList().First(f => f.EndsWith(".sln"));
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

            var y = 6;

        }
        
    }
}
